using System;
using System.Collections.Generic;
using System.Linq;
using Interface.Repositories;
using Interface.Services;
using Microsoft.Extensions.Caching.Memory;
using Minutz.Models;
using Minutz.Models.Entities;
using Minutz.Models.Extensions;
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

    public (bool condition, string message, AuthRestModel infoResponse) LoginFromFromToken
      (string accessToken, string idToken,string expiresIn,string instanceId = null)
    {
      var userInfo = GetUserInfo (accessToken);
      
      userInfo.IdToken = idToken;
      userInfo.AccessToken = accessToken;
      userInfo.TokenExpire = expiresIn;

      var existsResult = _userDatabaseRepository.GetUserByEmail (userInfo.Email, _applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
      
      instanceId = GetInstanceId(instanceId, existsResult, userInfo);

      UpdatePersonName(existsResult);

      var instance = _instanceRepository.GetByUsername (instanceId, _applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
      return (true, "Success", userInfo.UpdateFromInstance(existsResult.person,instance));
    }


    public (bool condition, string message, AuthRestModel infoResponse) Login
      (string username, string password, string instanceId = null)
    {
      var valid =  ValidateStringUsernameAndPassword(username, password);
      if (!valid.condition)
      {
        return valid;
      }

      var tokenResult = _auth0Repository.CreateToken (username, password);

      var userInfo = _auth0Repository.GetUserInfo(tokenResult.tokenResponse.access_token);
    
      userInfo.infoResponse.UpdateTokenInfo(tokenResult.tokenResponse);

      var existsResult = _userDatabaseRepository.GetUserByEmail (userInfo.infoResponse.Email, _applicationSetting.Schema, _applicationSetting.CreateConnectionString ());

      // if existsResult = null check if there is one in auth0
      if (existsResult.person == null)
      {
        var user = _auth0Repository.SearchUserByEmail(userInfo.infoResponse.Email);
        if (user.condition)
        {
          if (user.value != null)
          {
            if (string.IsNullOrEmpty(instanceId))
            {
              if (user.value.user_metadata.role == RoleTypes.User)
              {
                instanceId = Guid.NewGuid().ToString();
              }
            }
            var newUser = new AuthRestModel
            {
              Email = userInfo.infoResponse.Email,
              Name = user.value.name,
              Picture = user.value.picture,
              Sub = user.value.user_id,
              InstanceId = user.value.user_metadata.instance,
              Role = user.value.user_metadata.role
            };
            
            var createUserResponse = _userDatabaseRepository.CreateNewUser(newUser, _applicationSetting.Schema, _applicationSetting.CreateConnectionString());
            
            if (createUserResponse.condition)
            {
              var createInstance = CreateUser(newUser.Name, newUser.Email, password, newUser.Role,instanceId,null);
              if (!createInstance.condition)
               {
                 _logService.Log(LogLevel.Error, $"Login => CreateUser  => error: {createInstance.message}.");
               }
            }
          }
        }
      }
      
      existsResult = _userDatabaseRepository.GetUserByEmail (userInfo.infoResponse.Email, _applicationSetting.Schema, _applicationSetting.CreateConnectionString ());

      if (string.IsNullOrEmpty (existsResult.person.Related))
      {
        if (string.IsNullOrEmpty (existsResult.person.InstanceId))
        {
          if (string.IsNullOrEmpty (instanceId))
          {
            instanceId = userInfo.infoResponse.ToInstanceString();
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
        instanceId = relatedInstances.FirstOrDefault ().instanceId;
      }

      var instance = _instanceRepository.GetByUsername (instanceId, _applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
      userInfo.infoResponse.UpdateFromInstance(existsResult.person, instance);
      return (userInfo.condition, userInfo.message, userInfo.infoResponse);
    }

    public (bool condition, string message, AuthRestModel tokenResponse) CreateUser
      (string name, string email, string password, string role, string invitationInstanceId, string meetingId)
    {
      // first check if user is not in the db in the person table;
      var existsResult = _userDatabaseRepository.GetUserByEmail (email, _applicationSetting.Schema,_applicationSetting.CreateConnectionString());

      if (!existsResult.condition)
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
        var createNewUserResult = _userDatabaseRepository.CreateNewUser (createNewAuth0Response.tokenResponse, _applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
        if (!createNewUserResult.condition)
        {
          _logService.Log (LogLevel.Error, $"[(bool condition, string message, UserResponseModel tokenResponse) tokenResponse] There was a issue getting the token info for user {email}");
          return (createNewUserResult.condition, createNewUserResult.message, null);
        }

        if (!string.IsNullOrEmpty (invitationInstanceId))
        {
          //Create entry into the refernvce instance Availible person table
          _meetingAttendeeRepository.Add(new MeetingAttendee(), invitationInstanceId,
            _applicationSetting.CreateConnectionString(_applicationSetting.Server,_applicationSetting.Catalogue,invitationInstanceId, _applicationSetting.GetInstancePassword(invitationInstanceId)));

        }
  
        existsResult =
          _userDatabaseRepository.GetUserByEmail (email, _applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
      }

      switch (role)
      {
        case RoleTypes.User:
          if (!string.IsNullOrEmpty (existsResult.person.InstanceId))
          {
            //Get the token from auth0 by logging in
            (bool condition, string message, UserResponseModel tokenResponse) tokenResponse =
              _auth0Repository.CreateToken (email, password);
            
            if (!tokenResponse.condition)
            {
              _logService.Log (LogLevel.Error, $"[(bool condition, string message, UserResponseModel tokenResponse) tokenResponse] There was a issue getting the token info for user {email}");
              return (tokenResponse.condition, tokenResponse.message, null);
            }
            
            var authRestResult = new AuthRestModel();
            if (tokenResponse.condition)
            {
              //Get users info from auth0
              (bool condition, string message, AuthRestModel infoResponse) infoResponseResult =
                _auth0Repository.GetUserInfo (tokenResponse.tokenResponse.access_token);
              
              if (!infoResponseResult.condition)
              {
                _logService.Log (LogLevel.Error, $"[(bool condition, string message, AuthRestModel infoResponse) infoResponseResult] There was a issue getting the information info for user {email}");
                return (tokenResponse.condition, tokenResponse.message, null);
              }

              Instance instance;
              if (!string.IsNullOrEmpty(existsResult.person.Related))
              {
                var relatedInstance = existsResult.person.Related.SplitToList(StringDeviders.InstanceStringDevider, StringDeviders.MeetingStringDevider).First();
                
                instance = _instanceRepository.GetByUsername (
                  relatedInstance.instanceId,
                  _applicationSetting.Schema,
                  _applicationSetting.CreateConnectionString (
                    _applicationSetting.Server,
                    _applicationSetting.Catalogue,
                    relatedInstance.instanceId,
                    _applicationSetting.GetInstancePassword(relatedInstance.instanceId)));
              }
              else
              {
                instance = _instanceRepository.GetByUsername (existsResult.person.InstanceId, _applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
              }
              
              authRestResult = new AuthRestModel
              {
                Company = instance.Company,
                InstanceId = existsResult.person.InstanceId,
                FirstName = existsResult.person.FirstName,
                LastName = existsResult.person.LastName,
                Role = existsResult.person.Role,
                Email = existsResult.person.Email,
                TokenExpire = tokenResponse.tokenResponse.expires_in,
                AccessToken = tokenResponse.tokenResponse.access_token,
                Picture = infoResponseResult.infoResponse.Picture,
                IsVerified = infoResponseResult.infoResponse.IsVerified,
                Sub = infoResponseResult.infoResponse.Sub,
                Name = infoResponseResult.infoResponse.Name,
                Related = existsResult.person.Related
              };
            }
            return (tokenResponse.condition, tokenResponse.message, authRestResult);
          }
          (bool condition, string message, UserResponseModel tokenResponse) newUserTokenResponse =
            _auth0Repository.CreateToken (email, password);
          
          if (!newUserTokenResponse.condition)
          {
            _logService.Log (LogLevel.Error, $"[(bool condition, string message, UserResponseModel tokenResponse) newUserTokenResponse] There was a issue getting the token for user {email}");
            return (newUserTokenResponse.condition, newUserTokenResponse.message, null);
          }
          (bool condition, string message, AuthRestModel infoResponse) newInfoResponseResult =
            _auth0Repository.GetUserInfo (newUserTokenResponse.tokenResponse.access_token);
          
          if (!newInfoResponseResult.condition)
          {
            _logService.Log (LogLevel.Error, $"[(bool condition, string message, AuthRestModel infoResponse) newInfoResponseResult] There was a issue getting the token info for user {email}");
            return (newInfoResponseResult.condition, newInfoResponseResult.message, null);
          }
          // create the schema as the user is trial user;
          (string userConnectionString, string masterConnectionString) connectionStrings = this.GetConnectionStrings ();
          newInfoResponseResult.infoResponse.Role = role;
          newInfoResponseResult.infoResponse.FirstName = name;
          var schemaCreateResult = _userDatabaseRepository.CreateNewSchema (
            newInfoResponseResult.infoResponse, connectionStrings.userConnectionString, connectionStrings.masterConnectionString);

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
              var related = existsResult.person.Related.SplitToList (StringDeviders.InstanceStringDevider, StringDeviders.MeetingStringDevider);
              if (related.Any (i => i.instanceId == invitationInstanceId))
              {
                newInstance =
                  _instanceRepository.GetByUsername (invitationInstanceId, _applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
              }
              else
              {
                newInstance =
                  _instanceRepository.GetByUsername (schemaCreateResult, _applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
              }
            }
            else
            {
              newInstance =
                _instanceRepository.GetByUsername (schemaCreateResult, _applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
            }
          }
          else
          {
            newInstance =
              _instanceRepository.GetByUsername (schemaCreateResult, _applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
          }

          var createAuthRestResult = new AuthRestModel
          {
            Company = newInstance.Company,
            InstanceId = newInstance.Username,
            FirstName = existsResult.person.FirstName,
            LastName = existsResult.person.LastName,
            Role = existsResult.person.Role,
            Email = existsResult.person.Email,
            TokenExpire = newUserTokenResponse.tokenResponse.expires_in,
            AccessToken = newUserTokenResponse.tokenResponse.access_token,
            Picture = newInfoResponseResult.infoResponse.Picture,
            IsVerified = newInfoResponseResult.infoResponse.IsVerified,
            Sub = newInfoResponseResult.infoResponse.Sub,
            Name = newInfoResponseResult.infoResponse.Name,
            Related = existsResult.person.Related
          };
          return (true, "Success", createAuthRestResult);
        case RoleTypes.Guest:
          _logService.Log (LogLevel.Error, $"Starting Guest signup {existsResult.person.Email}");
          var guestTokenResponse = _auth0Repository.CreateToken (email, password);
          if (!guestTokenResponse.condition)
          {
            _logService.Log (LogLevel.Error, $"[(bool condition, string message, UserResponseModel tokenResponse) guestTokenResponse] There was a issue getting the token info for user {email}");
            return (guestTokenResponse.condition, guestTokenResponse.message, null);
          }
          _logService.Log (LogLevel.Error, $"getting Guest info from auth0 {existsResult.person.Email}");
          var guestinfoResponseResult = _auth0Repository.GetUserInfo (guestTokenResponse.tokenResponse.access_token);

          if (!guestinfoResponseResult.condition)
          {
            _logService.Log (LogLevel.Error, $"[(bool condition, string message, AuthRestModel infoResponse) guestinfoResponseResult] There was a issue getting the token info for user {email}");
            return (guestinfoResponseResult.condition, guestinfoResponseResult.message, null);
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

          _logService.Log (LogLevel.Error, $"getting Guest instance {existsResult.person.Email}");
          var guestInstance = _instanceRepository.GetByUsername (invitationInstanceId, _applicationSetting.Schema, _applicationSetting.CreateConnectionString ());

          if (!string.IsNullOrEmpty (existsResult.person.Related))
          {
            var relatedInstances = existsResult.person.Related.SplitToList (StringDeviders.InstanceStringDevider, StringDeviders.MeetingStringDevider);
            var relatedInstance = relatedInstances.Where (i => i.instanceId == invitationInstanceId);
            if (!relatedInstance.Any ())
            {
              var updatedRelatedString = relatedInstances.ToRelatedString ();
              existsResult.person.Related = updatedRelatedString;
              var updatedPerson = _userDatabaseRepository.UpdatePerson (_applicationSetting.CreateConnectionString (), _applicationSetting.Schema, existsResult.person);
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
            existsResult.person.Related = updatedRelatedString;
            existsResult.person.Active = true;
            existsResult.person.ProfilePicture = guestinfoResponseResult.infoResponse.Picture;
            existsResult.person.Role = RoleTypes.Guest;
            
            _logService.Log (LogLevel.Error, $"updating Guest info {existsResult.person.Email}");
            var updatedPerson = _userDatabaseRepository.UpdatePerson (_applicationSetting.CreateConnectionString (), _applicationSetting.Schema, existsResult.person);
            if (!updatedPerson.condition)
            {
              _logService.Log (LogLevel.Error, $"[(bool condition, string message) updatedPerson] There was with updating the person record for user {email}");
              return (false, "There was a problem updating the person information for a new invatation", null);
            }
          }
          _logService.Log (LogLevel.Error, $"update Guest invite status {existsResult.person.Email}");
          _meetingAttendeeRepository.UpdateInviteeStatus (
            existsResult.person.Email, existsResult.person.Identityid, "Accepted", invitationInstanceId, _applicationSetting.CreateConnectionString (
              _applicationSetting.Server, _applicationSetting.Catalogue, guestInstance.Username, guestInstance.Password));

          var guestAuthRestResult = new AuthRestModel
          {
            Company = guestInstance.Company,
            InstanceId = invitationInstanceId,
            FirstName = existsResult.person.FirstName,
            LastName = existsResult.person.LastName,
            Role = existsResult.person.Role,
            Email = existsResult.person.Email,
            TokenExpire = guestTokenResponse.tokenResponse.expires_in,
            AccessToken = guestTokenResponse.tokenResponse.access_token,
            Picture = guestinfoResponseResult.infoResponse.Picture,
            IsVerified = guestinfoResponseResult.infoResponse.IsVerified,
            Sub = guestinfoResponseResult.infoResponse.Sub,
            Name = guestinfoResponseResult.infoResponse.Name,
            Related = existsResult.person.Related
          };
          return (true, "Success", guestAuthRestResult);
        case RoleTypes.Admin:
          return (false, "", null);
      }

      return (false, "Not a valid role was supplied.", null);
    }

    public AuthRestModel ResetUserInfo (string token)
    {
      _cache.Remove (token);
      return GetUserInfo (token);
    }

    public AuthRestModel GetUserInfo (string token)
    {
      if (!_cache.TryGetValue (token, out AuthRestModel result))
      {
        var httpResult = Helper.HttpService.Get ($"{_applicationSetting.Authority}userinfo", token);
        result = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthRestModel> (httpResult);
        var cacheEntryOptions = new MemoryCacheEntryOptions ()
          .SetSlidingExpiration (TimeSpan.FromMinutes (10));
        _cache.Set (token, result, cacheEntryOptions);
      }
      return result;
    }
    
    private (bool condition, string message, AuthRestModel infoResponse) ValidateStringUsernameAndPassword
      (string username, string password)
    {
      if (string.IsNullOrEmpty(username))
      {
        _logService.Log
          (LogLevel.Error, $"[(bool condition, string message, AuthRestModel infoResponse)] There was a issue logging in for user {username}");
        return (false, "Please provide a valid username or password", null);
      }

      if (!string.IsNullOrEmpty(password)) return (true, "Valid", null);
      _logService.Log
        (LogLevel.Error,$"[(bool condition, string message, AuthRestModel infoResponse)] There was a issue logging in for user {username}");
      return (false, "Please provide a valid username or password", null);
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