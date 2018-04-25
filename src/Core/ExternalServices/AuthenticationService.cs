using System;
using System.Collections.Generic;
using System.Linq;
using Interface.Repositories;
using Interface.Services;
using Microsoft.Extensions.Caching.Memory;
using Minutz.Models;
using Minutz.Models.Entities;
using Minutz.Models.Extensions;
using Minutz.Models.Message;
using Models.Auth0Models;

namespace Core.ExternalServices
{
  public class AuthenticationService : IAuthenticationService
  {
    private readonly IMeetingAttendeeRepository _meetingAttendeeRepository;
    private readonly IApplicationSetupRepository _applicationSetupRepository;
    private readonly IApplicationSetting _applicationSetting;
    private readonly IAuth0Repository _auth0Repository;
    private readonly IUserRepository _userDatabaseRepository;
    private readonly IInstanceRepository _instanceRepository;
    private readonly ILogService _logService;
    private readonly IMemoryCache _cache;

    public AuthenticationService(
      IApplicationSetting applicationSetting, IMemoryCache memoryCache,
      IAuth0Repository auth0Repository, ILogService logService,
      IUserRepository userDatabaseRepository, IApplicationSetupRepository applicationSetupRepository,
      IInstanceRepository instanceRepository, IMeetingAttendeeRepository meetingAttendeeRepository)
    {
      _applicationSetting = applicationSetting;
      _cache = memoryCache;
      _auth0Repository = auth0Repository;
      _logService = logService;
      _userDatabaseRepository = userDatabaseRepository;
      _applicationSetupRepository = applicationSetupRepository;
      _instanceRepository = instanceRepository;
      _meetingAttendeeRepository = meetingAttendeeRepository;
    }

    public AuthRestModelResponse LoginFromFromToken
      (string accessToken, string idToken,string expiresIn,string instanceId = null)
    {
      var result = new AuthRestModelResponse { Condition = false, Message =  string.Empty, InfoResponse = new AuthRestModel()};
      var userInfoResult = GetUserInfo (accessToken);
      if (!userInfoResult.Condition)
      {
        result.Message = userInfoResult.Message;
        return result;
      }

      userInfoResult.InfoResponse.IdToken = idToken;
      userInfoResult.InfoResponse.AccessToken = accessToken;
      userInfoResult.InfoResponse.TokenExpire = expiresIn;

      result = Login(userInfoResult, instanceId);

      return result;
    }

    public AuthRestModelResponse LoginFromLoginForm
      (string username, string password, string instanceId = null)
    {
      var result = new AuthRestModelResponse { Condition = false, Message =  string.Empty, InfoResponse = new AuthRestModel()};
      var valid =  ValidateStringUsernameAndPassword(username, password);
      if (!valid.Condition)
      {
        result.Message = valid.Message;
        return result;
      }

      var tokenCreateResult = _auth0Repository.CreateToken (username, password);
      if (!tokenCreateResult.Condition)
      {
        result.Message = tokenCreateResult.Message;
        return result;
      }
      var userInfoResult = _auth0Repository.GetUserInfo(tokenCreateResult.AuthTokenResponse.access_token);
      if (!userInfoResult.Condition)
      {
        result.Message = userInfoResult.Message;
        return result;
      }
      userInfoResult.InfoResponse.UpdateTokenInfo(tokenCreateResult.AuthTokenResponse);
      result = Login(userInfoResult, instanceId);
     
      return result;
    }

    private AuthRestModelResponse Login
      (AuthRestModelResponse userInfoResult, string instanceId)
    {
      var result = new AuthRestModelResponse { Condition = false, Message =  string.Empty, InfoResponse = new AuthRestModel()};

      var userExistsByEmail = _userDatabaseRepository.MinutzPersonCheckIfUserExistsByEmail (userInfoResult.InfoResponse.Email, _applicationSetting.CreateConnectionString ());

      if (!userExistsByEmail.Condition)
      {
        var userSearchResult = _auth0Repository.SearchUserByEmail(userInfoResult.InfoResponse.Email);
        if (userSearchResult.Condition)
        {
          var newInsertUser = new AuthRestModel
          {
            Email = userSearchResult.User.email,
            Name = userSearchResult.User.name,
            Picture = userSearchResult.User.picture,
            Sub = userSearchResult.User.user_id,
            InstanceId = userSearchResult.User.user_metadata.instance,
            Role = userSearchResult.User.user_metadata.role
          };

          if (userSearchResult.User.user_metadata != null)
          {
            newInsertUser.Role = !string.IsNullOrEmpty(userSearchResult.User.user_metadata.role) 
              ? userSearchResult.User.user_metadata.role 
              : RoleTypes.Guest;
          }

          var createUserResult = _userDatabaseRepository.CreateNewUser
            (newInsertUser, _applicationSetting.CreateConnectionString());
          
          if (!createUserResult.Condition)
          {
            _logService.Log(LogLevel.Error, $"Login => CreateUser  => error: {createUserResult.Message}.");
            result.Message = createUserResult.Message;
            return result;
          }
          userExistsByEmail = _userDatabaseRepository.MinutzPersonCheckIfUserExistsByEmail (userInfoResult.InfoResponse.Email, _applicationSetting.CreateConnectionString ());
        }
      }
      
      if (string.IsNullOrEmpty (userExistsByEmail.Person.Related))
      {
        if (string.IsNullOrEmpty (userExistsByEmail.Person.InstanceId))
        {
          if (string.IsNullOrEmpty (instanceId))
          {
            instanceId = userInfoResult.InfoResponse.ToInstanceString();
          }
        }
        else
        {
          instanceId = userExistsByEmail.Person.InstanceId;
        }
      }
      else
      {
        var relatedInstances = userExistsByEmail.Person.RelatedItems();
        instanceId = relatedInstances.FirstOrDefault ().instanceId;
      }

      var instanceResponse = _instanceRepository.GetByUsername (instanceId, _applicationSetting.CreateConnectionString ());
      if (instanceResponse == null)
      {
        userInfoResult.InfoResponse.Role = RoleTypes.User;
        userInfoResult.InfoResponse.Related = string.Empty;
          _userDatabaseRepository.CreateNewSchema(userInfoResult.InfoResponse, _applicationSetting.Schema, _applicationSetting.CreateConnectionString());
        instanceResponse = _instanceRepository.GetByUsername (instanceId, _applicationSetting.CreateConnectionString ());
      }

      userInfoResult.InfoResponse.UpdateFromInstance(userExistsByEmail.Person, instanceResponse.Instance);
      result.InfoResponse = userInfoResult.InfoResponse;
      result.Condition = true;
      return result;
    }

    public (bool condition, string message, AuthRestModel tokenResponse) CreateUser
      (string name, string email, string password, string role, string invitationInstanceId, string meetingId)
    {
      // first check if user is not in the db in the person table;
      var existsResult = _userDatabaseRepository.MinutzPersonCheckIfUserExistsByEmail (email,_applicationSetting.CreateConnectionString());

      if (!existsResult.Condition)
      {
        var instanceId = Guid.NewGuid ().ToString ();
        
        // Create the user in Auth0
        (bool condition, string message, AuthRestModel tokenResponse) createNewAuth0Response = _auth0Repository.CreateUser (name, email, password, role, $"A_{instanceId}");
        
        if (!createNewAuth0Response.condition)
        {
          _logService.Log (LogLevel.Error, $"[(bool condition, string message, UserResponseModel tokenResponse) tokenResponse] There was a issue getting the token info for user {email}");
          return (createNewAuth0Response.condition, createNewAuth0Response.message, null);
        }

        // check if there are any references
        if (!string.IsNullOrEmpty (invitationInstanceId) && !string.IsNullOrEmpty (meetingId))
        {
          var relatedInstances = new List<(string instanceId, string meetingId)> {(invitationInstanceId, meetingId)};
          string updatedRelatedString = relatedInstances.ToRelatedString ();
          createNewAuth0Response.tokenResponse.Related = updatedRelatedString;
        }
        
        createNewAuth0Response.tokenResponse.Role = role;
        createNewAuth0Response.tokenResponse.FirstName = name;
        createNewAuth0Response.tokenResponse.Nickname = name;
        
        // Default the company name
        if (string.IsNullOrEmpty (invitationInstanceId))
        {
          createNewAuth0Response.tokenResponse.Company = $"{name}'s Company";
          
        }
        
        //Create the user in sql in the person table
        var createNewUserResult = _userDatabaseRepository.CreateNewUser (createNewAuth0Response.tokenResponse, _applicationSetting.CreateConnectionString ());
        if (!createNewUserResult.Condition)
        {
          _logService.Log (LogLevel.Error, $"[(bool condition, string message, UserResponseModel tokenResponse) tokenResponse] There was a issue getting the token info for user {email}");
          return (createNewUserResult.Condition, createNewUserResult.Message, null);
        }

        if (!string.IsNullOrEmpty (invitationInstanceId))
        {
          //Create entry into the refference instance Availible person table
          _meetingAttendeeRepository.Add(new MeetingAttendee(), invitationInstanceId,
            _applicationSetting.CreateConnectionString(_applicationSetting.Server,_applicationSetting.Catalogue,invitationInstanceId, _applicationSetting.GetInstancePassword(invitationInstanceId)));
        }
  
        existsResult =
          _userDatabaseRepository.MinutzPersonCheckIfUserExistsByEmail (email, _applicationSetting.CreateConnectionString ());
      }

      switch (role)
      {
        case RoleTypes.User:
          if (!string.IsNullOrEmpty (existsResult.Person.InstanceId))
          {
            //Get the token from auth0 by logging in
            var tokenResponse =
              _auth0Repository.CreateToken (email, password);
            
            if (!tokenResponse.Condition)
            {
              _logService.Log (LogLevel.Error, $"[(bool condition, string message, UserResponseModel tokenResponse) tokenResponse] There was a issue getting the token info for user {email}");
              return (tokenResponse.Condition, tokenResponse.Message, null);
            }
            
            var authRestResult = new AuthRestModel();
            if (tokenResponse.Condition)
            {
              //Get users info from auth0
              var infoResponseResult =
                _auth0Repository.GetUserInfo (tokenResponse.AuthTokenResponse.access_token);
              
              if (!infoResponseResult.Condition)
              {
                _logService.Log (LogLevel.Error, $"[(bool condition, string message, AuthRestModel infoResponse) infoResponseResult] There was a issue getting the information info for user {email}");
                return (tokenResponse.Condition, tokenResponse.Message, null);
              }

              Instance instance = new Instance();
              if (!string.IsNullOrEmpty(existsResult.Person.Related))
              {
                var relatedInstance = existsResult.Person.Related.SplitToList(StringDeviders.InstanceStringDevider, StringDeviders.MeetingStringDevider).First();

                var instanceResult = _instanceRepository.GetByUsername(
                  relatedInstance.instanceId,
                  _applicationSetting.CreateConnectionString());
                if (instanceResult.Condition)
                {
                  instance = instanceResult.Instance;
                }
              }
              else
              {
                var instanceResult = _instanceRepository.GetByUsername(
                  existsResult.Person.InstanceId,
                  _applicationSetting.CreateConnectionString());
                if (instanceResult.Condition)
                {
                  instance = instanceResult.Instance;
                }
              }
              
              authRestResult = new AuthRestModel
              {
                Company = instance.Company,
                InstanceId = existsResult.Person.InstanceId,
                FirstName = existsResult.Person.FirstName,
                LastName = existsResult.Person.LastName,
                Role = existsResult.Person.Role,
                Email = existsResult.Person.Email,
                TokenExpire = tokenResponse.AuthTokenResponse.expires_in,
                AccessToken = tokenResponse.AuthTokenResponse.access_token,
                Picture = infoResponseResult.InfoResponse.Picture,
                IsVerified = infoResponseResult.InfoResponse.IsVerified,
                Sub = infoResponseResult.InfoResponse.Sub,
                Name = infoResponseResult.InfoResponse.Name,
                Related = existsResult.Person.Related
              };
            }
            return (tokenResponse.Condition, tokenResponse.Message, authRestResult);
          }
          
          var newUserTokenResponse =
            _auth0Repository.CreateToken (email, password);
          
          if (!newUserTokenResponse.Condition)
          {
            _logService.Log (LogLevel.Error, $"[(bool condition, string message, UserResponseModel tokenResponse) newUserTokenResponse] There was a issue getting the token for user {email}");
            return (newUserTokenResponse.Condition, newUserTokenResponse.Message, null);
          }
          var newInfoResponseResult =
            _auth0Repository.GetUserInfo (newUserTokenResponse.AuthTokenResponse.access_token);
          
          if (!newInfoResponseResult.Condition)
          {
            _logService.Log (LogLevel.Error, $"[(bool condition, string message, AuthRestModel infoResponse) newInfoResponseResult] There was a issue getting the token info for user {email}");
            return (newInfoResponseResult.Condition, newInfoResponseResult.Message, null);
          }
          // create the schema as the user is trial user;
          (string userConnectionString, string masterConnectionString) connectionStrings = GetConnectionStrings ();
          newInfoResponseResult.InfoResponse.Role = role;
          newInfoResponseResult.InfoResponse.FirstName = name;
          var schemaCreateResult = _userDatabaseRepository.CreateNewSchema (
            newInfoResponseResult.InfoResponse, connectionStrings.userConnectionString, connectionStrings.masterConnectionString);

          // create the tables as the user is trial user;
          var tablesCreateResult = _applicationSetupRepository.CreateSchemaTables (
            _applicationSetting.Schema, schemaCreateResult, _applicationSetting.CreateConnectionString ());
          
          if (!tablesCreateResult)
          {
            _logService.Log (LogLevel.Error, $"[(bool condition, string message, AuthRestModel infoResponse) newInfoResponseResult] There was a issue creating the tables for user {email}");
            return (false, "There was a problem creating the records.", null);
          }
          Instance newInstance;
          if (!string.IsNullOrEmpty (invitationInstanceId))
          {
            if (schemaCreateResult != invitationInstanceId)
            {
              var related = existsResult.Person.Related.SplitToList (StringDeviders.InstanceStringDevider, StringDeviders.MeetingStringDevider);
              if (related.Any (i => i.instanceId == invitationInstanceId))
              {
                newInstance =
                  _instanceRepository.GetByUsername (invitationInstanceId, _applicationSetting.CreateConnectionString ()).Instance;
              }
              else
              {
                newInstance =
                  _instanceRepository.GetByUsername (schemaCreateResult, _applicationSetting.CreateConnectionString ()).Instance;
              }
            }
            else
            {
              newInstance =
                _instanceRepository.GetByUsername (schemaCreateResult, _applicationSetting.CreateConnectionString ()).Instance;
            }
          }
          else
          {
            newInstance =
              _instanceRepository.GetByUsername (schemaCreateResult, _applicationSetting.CreateConnectionString ()).Instance;
          }

          var createAuthRestResult = new AuthRestModel
          {
            Company = newInstance.Company,
            InstanceId = newInstance.Username,
            FirstName = existsResult.Person.FirstName,
            LastName = existsResult.Person.LastName,
            Role = existsResult.Person.Role,
            Email = existsResult.Person.Email,
            TokenExpire = newUserTokenResponse.AuthTokenResponse.expires_in,
            AccessToken = newUserTokenResponse.AuthTokenResponse.access_token,
            Picture = newInfoResponseResult.InfoResponse.Picture,
            IsVerified = newInfoResponseResult.InfoResponse.IsVerified,
            Sub = newInfoResponseResult.InfoResponse.Sub,
            Name = newInfoResponseResult.InfoResponse.Name,
            Related = existsResult.Person.Related
          };
          return (true, "Success", createAuthRestResult);
        case RoleTypes.Guest:
          _logService.Log (LogLevel.Error, $"Starting Guest signup {existsResult.Person.Email}");
          var guestTokenResponse = _auth0Repository.CreateToken (email, password);
          if (!guestTokenResponse.Condition)
          {
            _logService.Log (LogLevel.Error, $"[(bool condition, string message, UserResponseModel tokenResponse) guestTokenResponse] There was a issue getting the token info for user {email}");
            return (guestTokenResponse.Condition, guestTokenResponse.Message, null);
          }
          _logService.Log (LogLevel.Error, $"getting Guest info from auth0 {existsResult.Person.Email}");
          var guestinfoResponseResult = _auth0Repository.GetUserInfo (guestTokenResponse.AuthTokenResponse.access_token);

          if (!guestinfoResponseResult.Condition)
          {
            _logService.Log (LogLevel.Error, $"[(bool condition, string message, AuthRestModel infoResponse) guestinfoResponseResult] There was a issue getting the token info for user {email}");
            return (guestinfoResponseResult.Condition, guestinfoResponseResult.Message, null);
          }

          if (string.IsNullOrEmpty (invitationInstanceId))
          {
            _logService.Log (LogLevel.Error, $"[(string.IsNullOrEmpty(invitationInstanceId))] There was with the invitationInstanceId for user {email}");
            return (false, "For a guest a invitation is required", null);
          }

          if (string.IsNullOrEmpty (meetingId))
          {
            _logService.Log (LogLevel.Error, $"[(string.IsNullOrEmpty(meetingId))] There was with the meetingId for user {email}");
            return (false, "For a guest a invitation is required", null);
          }

          _logService.Log (LogLevel.Error, $"getting Guest instance {existsResult.Person.Email}");
          var guestInstance = _instanceRepository.GetByUsername (invitationInstanceId, _applicationSetting.CreateConnectionString ());

          if (!string.IsNullOrEmpty (existsResult.Person.Related))
          {
            var relatedInstances = existsResult.Person.Related.SplitToList (StringDeviders.InstanceStringDevider, StringDeviders.MeetingStringDevider);
            var relatedInstance = relatedInstances.Where (i => i.instanceId == invitationInstanceId);
            if (!relatedInstance.Any ())
            {
              var updatedRelatedString = relatedInstances.ToRelatedString ();
              existsResult.Person.Related = updatedRelatedString;
              var updatedPerson = _userDatabaseRepository.UpdatePerson (_applicationSetting.CreateConnectionString (), _applicationSetting.Schema, existsResult.Person);
              if (!updatedPerson.condition)
              {
                _logService.Log (LogLevel.Error, $"[(bool condition, string message) updatedPerson] There was with updating the person record for user {email}");
                return (false, "There was a problem updating the person information to add to existing invatations", null);
              }
            }
          }
          else
          {
            var relatedInstances = new List<(string instanceId, string meetingId)> {(invitationInstanceId, meetingId)};
            var updatedRelatedString = relatedInstances.ToRelatedString ();
            existsResult.Person.Related = updatedRelatedString;
            existsResult.Person.Active = true;
            existsResult.Person.ProfilePicture = guestinfoResponseResult.InfoResponse.Picture;
            existsResult.Person.Role = RoleTypes.Guest;
            
            _logService.Log (LogLevel.Error, $"updating Guest info {existsResult.Person.Email}");
            var updatedPerson = _userDatabaseRepository.UpdatePerson (_applicationSetting.CreateConnectionString (), _applicationSetting.Schema, existsResult.Person);
            if (!updatedPerson.condition)
            {
              _logService.Log (LogLevel.Error, $"[(bool condition, string message) updatedPerson] There was with updating the person record for user {email}");
              return (false, "There was a problem updating the person information for a new invatation", null);
            }
          }
          _logService.Log (LogLevel.Error, $"update Guest invite status {existsResult.Person.Email}");
          _meetingAttendeeRepository.UpdateInviteeStatus (
            existsResult.Person.Email, existsResult.Person.Identityid, "Accepted", invitationInstanceId, _applicationSetting.CreateConnectionString (
              _applicationSetting.Server, _applicationSetting.Catalogue, guestInstance.Instance.Username, guestInstance.Instance.Password));

          var guestAuthRestResult = new AuthRestModel
          {
            Company = guestInstance.Instance.Company,
            InstanceId = invitationInstanceId,
            FirstName = existsResult.Person.FirstName,
            LastName = existsResult.Person.LastName,
            Role = existsResult.Person.Role,
            Email = existsResult.Person.Email,
            TokenExpire = guestTokenResponse.AuthTokenResponse.expires_in,
            AccessToken = guestTokenResponse.AuthTokenResponse.access_token,
            Picture = guestinfoResponseResult.InfoResponse.Picture,
            IsVerified = guestinfoResponseResult.InfoResponse.IsVerified,
            Sub = guestinfoResponseResult.InfoResponse.Sub,
            Name = guestinfoResponseResult.InfoResponse.Name,
            Related = existsResult.Person.Related
          };
          return (true, "Success", guestAuthRestResult);
        case RoleTypes.Admin:
          return (false, "", null);
      }

      return (false, "Not a valid role was supplied.", null);
    }

    public AuthRestModelResponse ResetUserInfo (string token)
    {
      _cache.Remove (token);
      return GetUserInfo (token);
    }

    public AuthRestModelResponse GetUserInfo (string token)
    {
      var result = new AuthRestModelResponse { Condition = false, Message = string.Empty, InfoResponse = new AuthRestModel()};
      
      if (!_cache.TryGetValue (token, out AuthRestModel userInfo))
      {
        var httpResult = Helper.HttpService.Get ($"{_applicationSetting.Authority}userinfo", token);
        userInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthRestModel> (httpResult);
        var cacheEntryOptions = new MemoryCacheEntryOptions ().SetSlidingExpiration (TimeSpan.FromMinutes (10));
        _cache.Set (token, result, cacheEntryOptions);
      }

      result.Condition = true;
      result.InfoResponse = userInfo;
      return result;
    }
    
    private MessageBase ValidateStringUsernameAndPassword
      (string username, string password)
    {
      var result = new MessageBase { Condition = false, Message = string.Empty };
      
      if (string.IsNullOrEmpty(username))
      {
        _logService.Log
          (LogLevel.Error, $"[(bool condition, string message, AuthRestModel infoResponse)] There was a issue logging in for user {username}");
        result.Message = "Please provide a valid username or password";
        return result;
      }
      if (string.IsNullOrEmpty(password))
      {
        _logService.Log
          (LogLevel.Error,$"[(bool condition, string message, AuthRestModel infoResponse)] There was a issue logging in for user {username}");
        result.Message = "Please provide a valid username or password";
        return result;
      }

      result.Condition = true;
      return result;
    }
    
    private static string GetInstanceId
      (string instanceId, (bool condition, string message, Person person) existsResult, AuthRestModel userInfo)
    {
      if (string.IsNullOrEmpty(existsResult.person.Related))
      {
        if (string.IsNullOrEmpty(existsResult.person.InstanceId))
        {
          if (string.IsNullOrEmpty(instanceId))
          {
            instanceId = userInfo.ToInstanceString();
          }
        }
        else
        {
          instanceId = existsResult.person.InstanceId;
        }
      }
      else
      {
        var relatedInstances = existsResult.person.RelatedItems();
        instanceId = relatedInstances.FirstOrDefault().instanceId;
      }
      return instanceId;
    }
    
    private void UpdatePersonName((bool condition, string message, Person person) existsResult)
    {
      if (string.IsNullOrEmpty(existsResult.person.FullName))
      {
        existsResult.person.FullName = existsResult.person.Email.NamedFromEmail();
        try
        {
          _userDatabaseRepository.UpdatePerson(_applicationSetting.CreateConnectionString(), _applicationSetting.Schema,
            existsResult.person);
        }
        catch (Exception e)
        {
          _logService.Log(LogLevel.Exception, e.Message);
        }
      }
    }

    private (string userConnectionString, string masterConnectionString) GetConnectionStrings ()
    {
      var masterConnectionString = _applicationSetting.CreateConnectionString (
        _applicationSetting.Server,
        "master",
        _applicationSetting.Username,
        _applicationSetting.Password);
      var userConnectionString = _applicationSetting.CreateConnectionString (
        _applicationSetting.Server,
        _applicationSetting.Catalogue,
        _applicationSetting.Username,
        _applicationSetting.Password);
      return (userConnectionString, masterConnectionString);
    }
  }
}