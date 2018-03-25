﻿using System;
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
    private readonly IUserRepository _userRepository;
    private readonly IInstanceRepository _instanceRepository;
    private readonly ILogService _logService;
    private IMemoryCache _cache;

    public AuthenticationService(
      IApplicationSetting applicationSetting, IMemoryCache memoryCache,
      IAuth0Repository auth0Repository, ILogService logService,
      IUserRepository userRepository, IApplicationSetupRepository applicationSetupRepository,
      IInstanceRepository instanceRepository, IMeetingAttendeeRepository meetingAttendeeRepository)
    {
      this._applicationSetting = applicationSetting;
      this._cache = memoryCache;
      this._auth0Repository = auth0Repository;
      this._logService = logService;
      this._userRepository = userRepository;
      this._applicationSetupRepository = applicationSetupRepository;
      this._instanceRepository = instanceRepository;
      this._meetingAttendeeRepository = meetingAttendeeRepository;
    }

    public (bool condition, string message, AuthRestModel infoResponse) Login (
      string access_token, string id_token,string expires_in,string instanceId = null)
    {
      var userInfo = GetUserInfo (access_token);
      
      userInfo.IdToken = id_token;
      userInfo.AccessToken = access_token;
      userInfo.TokenExpire = expires_in;

      (bool condition, string message, Person person) existsResult =
        this._userRepository.GetUserByEmail (userInfo.Email, this._applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
      if (string.IsNullOrEmpty (existsResult.person.Related))
      {
        if (string.IsNullOrEmpty (existsResult.person.InstanceId))
        {
          if (string.IsNullOrEmpty (instanceId))
          {
            instanceId = $"A_{userInfo.Sub.Split ('|')[1]}";
          }
        }
        else
        {
          instanceId = existsResult.person.InstanceId;
        }

      }
      else
      {
        List<(string instanceId, string meetingId)> relatedInstances =
          existsResult.person.Related.SplitToList (Minutz.Models.StringDeviders.InstanceStringDevider, Minutz.Models.StringDeviders.MeetingStringDevider);
        instanceId = relatedInstances.FirstOrDefault ().instanceId;
      }

      if (string.IsNullOrEmpty(existsResult.person.FullName))
      {
        if (string.IsNullOrEmpty(existsResult.person.FirstName))
        {
          var emailSplit = existsResult.person.Email.Split('@');
          existsResult.person.FullName = emailSplit[0];
          try
          {
            _userRepository.UpdatePerson( _applicationSetting.CreateConnectionString(),this._applicationSetting.Schema, existsResult.person);
          }
          catch (Exception e)
          {
            _logService.Log(LogLevel.Exception, e.Message);
          }  
        }
      }

      Instance instance = this._instanceRepository.GetByUsername (instanceId, this._applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
      userInfo.InstanceId = instance.Username;
      userInfo.Company = instance.Company;
      userInfo.Related = existsResult.person.Related;
      userInfo.Role = existsResult.person.Role;
      userInfo.FirstName = existsResult.person.FirstName;
      userInfo.LastName = existsResult.person.LastName;
      userInfo.Email = existsResult.person.Email;
      return (true, "Success", userInfo);
    }


    public (bool condition, string message, AuthRestModel infoResponse) Login (
      string username, string password, string instanceId = null)
    {
      if (string.IsNullOrEmpty (username))
      {
        this._logService.Log (Minutz.Models.LogLevel.Error, $"[(bool condition, string message, AuthRestModel infoResponse)] There was a issue logging in for user {username}");
        return (false, "Please provide a valid username or password", null);
      }
      if (string.IsNullOrEmpty (password))
      {
        this._logService.Log (Minutz.Models.LogLevel.Error, $"[(bool condition, string message, AuthRestModel infoResponse)] There was a issue logging in for user {username}");
        return (false, "Please provide a valid username or password", null);
      }

      (bool condition, string message, UserResponseModel tokenResponse) tokenResult =
        this._auth0Repository.CreateToken (username, password);

      (bool condition, string message, AuthRestModel infoResponse) userInfo =
        this._auth0Repository.GetUserInfo (tokenResult.tokenResponse.access_token);

      userInfo.infoResponse.IdToken = tokenResult.tokenResponse.id_token;
      userInfo.infoResponse.AccessToken = tokenResult.tokenResponse.access_token;
      userInfo.infoResponse.TokenExpire = tokenResult.tokenResponse.expires_in;

      (bool condition, string message, Person person) existsResult =
        this._userRepository.GetUserByEmail (userInfo.infoResponse.Email, this._applicationSetting.Schema, _applicationSetting.CreateConnectionString ());

      if (string.IsNullOrEmpty (existsResult.person.Related))
      {
        if (string.IsNullOrEmpty (existsResult.person.InstanceId))
        {
          if (string.IsNullOrEmpty (instanceId))
          {
            instanceId = $"A_{userInfo.infoResponse.Sub.Split ('|')[1]}";
          }
        }
        else
        {
          instanceId = existsResult.person.InstanceId;
        }

      }
      else
      {
        List<(string instanceId, string meetingId)> relatedInstances =
          existsResult.person.Related.SplitToList (Minutz.Models.StringDeviders.InstanceStringDevider, Minutz.Models.StringDeviders.MeetingStringDevider);
        instanceId = relatedInstances.FirstOrDefault ().instanceId;
      }

      Instance instance = this._instanceRepository.GetByUsername (instanceId, this._applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
      userInfo.infoResponse.InstanceId = instance.Username;
      userInfo.infoResponse.Company = instance.Company;
      userInfo.infoResponse.Related = existsResult.person.Related;
      userInfo.infoResponse.Role = existsResult.person.Role;
      userInfo.infoResponse.FirstName = existsResult.person.FirstName;
      userInfo.infoResponse.LastName = existsResult.person.LastName;
      userInfo.infoResponse.Email = existsResult.person.Email;
      return (userInfo.condition, userInfo.message, userInfo.infoResponse);
    }

    /// <summary>
    /// This is used to check if the user is in the database
    /// * If the user is in the database then get the information
    /// * If the user does not exist then create the entry in the person table and create the supporting tables
    /// </summary>
    /// <param name="name" typeof="string">This is the nickname that is created by auth0</param>
    /// <param name="username" typeof="string">This is the username that the user entered</param>
    /// <param name="email" typeof="string">This is the email address that the user entered</param>
    /// <param name="password" typeof="string">This is the password that the user entered</param>
    /// <returns typeof="Tuple(bool condition, string message, AuthRestModel tokenResponse)">The overall result, condition can be used to verify if the result is what is required.</returns>
    public (bool condition, string message, AuthRestModel tokenResponse) CreateUser (
      string name, string username, string email, string password, string role, string invitationInstanceId, string meetingId)
    {
      // first check if user is not in the db in the person table;
      (bool condition, string message, Person person) existsResult =
        this._userRepository.GetUserByEmail (email, this._applicationSetting.Schema,_applicationSetting.CreateConnectionString());

      if (!existsResult.condition)
      {
        var instanceId = Guid.NewGuid ().ToString ();
        
        // Create the user in Auth0
        (bool condition, string message, AuthRestModel tokenResponse) createNewAuth0Response =
          this._auth0Repository.CreateUser (name, username, email, password, role, $"A_{instanceId}");
        
        if (!createNewAuth0Response.condition)
        {
          this._logService.Log (Minutz.Models.LogLevel.Error, $"[(bool condition, string message, UserResponseModel tokenResponse) tokenResponse] There was a issue getting the token info for user {email}");
          return (createNewAuth0Response.condition, createNewAuth0Response.message, null);
        }

        // check if there are any references
        if (!string.IsNullOrEmpty (invitationInstanceId) && !string.IsNullOrEmpty (meetingId))
        {
          List<(string instanceId, string meetingId)> relatedInstances = new List<(string instanceId, string meetingId)> ();
          relatedInstances.Add ((invitationInstanceId, meetingId));
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
        var createNewUserResult = this._userRepository.CreateNewUser (createNewAuth0Response.tokenResponse, this._applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
        if (!createNewUserResult.condition)
        {
          this._logService.Log (Minutz.Models.LogLevel.Error, $"[(bool condition, string message, UserResponseModel tokenResponse) tokenResponse] There was a issue getting the token info for user {email}");
          return (createNewUserResult.condition, createNewUserResult.message, null);
        }

        // Default the company name
        if (!string.IsNullOrEmpty (invitationInstanceId))
        {
          //Create entry into the refernvce instance Availible person table
          this._meetingAttendeeRepository.Add(new MeetingAttendee(), invitationInstanceId,
            _applicationSetting.CreateConnectionString(_applicationSetting.Server,_applicationSetting.Catalogue,invitationInstanceId, _applicationSetting.GetInstancePassword(invitationInstanceId)));

        }
        
        
        existsResult =
          this._userRepository.GetUserByEmail (email, this._applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
      }

      switch (role)
      {
        case RoleTypes.User:
          if (!string.IsNullOrEmpty (existsResult.person.InstanceId))
          {
            //Get the token from auth0 by logging in
            (bool condition, string message, UserResponseModel tokenResponse) tokenResponse =
              this._auth0Repository.CreateToken (username, password);
            
            if (!tokenResponse.condition)
            {
              this._logService.Log (Minutz.Models.LogLevel.Error, $"[(bool condition, string message, UserResponseModel tokenResponse) tokenResponse] There was a issue getting the token info for user {email}");
              return (tokenResponse.condition, tokenResponse.message, null);
            }
            
            AuthRestModel authRestResult = new AuthRestModel { };
            if (tokenResponse.condition)
            {
              //Get users info from auth0
              (bool condition, string message, AuthRestModel infoResponse) infoResponseResult =
                this._auth0Repository.GetUserInfo (tokenResponse.tokenResponse.access_token);
              
              if (!infoResponseResult.condition)
              {
                this._logService.Log (Minutz.Models.LogLevel.Error, $"[(bool condition, string message, AuthRestModel infoResponse) infoResponseResult] There was a issue getting the information info for user {email}");
                return (tokenResponse.condition, tokenResponse.message, null);
              }

              Instance instance;
              if (!string.IsNullOrEmpty(existsResult.person.Related))
              {
                var relatedInstance = existsResult.person.Related.SplitToList(Minutz.Models.StringDeviders.InstanceStringDevider, Minutz.Models.StringDeviders.MeetingStringDevider).First();
                
                instance = this._instanceRepository.GetByUsername (
                  relatedInstance.instanceId,
                  this._applicationSetting.Schema,
                  _applicationSetting.CreateConnectionString (
                    _applicationSetting.Server,
                    _applicationSetting.Catalogue,
                    relatedInstance.instanceId,
                    _applicationSetting.GetInstancePassword(relatedInstance.instanceId)));
              }
              else
              {
                instance = this._instanceRepository.GetByUsername (existsResult.person.InstanceId, this._applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
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
            this._auth0Repository.CreateToken (username, password);
          
          if (!newUserTokenResponse.condition)
          {
            this._logService.Log (Minutz.Models.LogLevel.Error, $"[(bool condition, string message, UserResponseModel tokenResponse) newUserTokenResponse] There was a issue getting the token for user {email}");
            return (newUserTokenResponse.condition, newUserTokenResponse.message, null);
          }
          (bool condition, string message, AuthRestModel infoResponse) newInfoResponseResult =
            this._auth0Repository.GetUserInfo (newUserTokenResponse.tokenResponse.access_token);
          
          if (!newInfoResponseResult.condition)
          {
            this._logService.Log (Minutz.Models.LogLevel.Error, $"[(bool condition, string message, AuthRestModel infoResponse) newInfoResponseResult] There was a issue getting the token info for user {email}");
            return (newInfoResponseResult.condition, newInfoResponseResult.message, null);
          }
          // create the schema as the user is trial user;
          (string userConnectionString, string masterConnectionString) connectionStrings = this.GetConnectionStrings ();
          newInfoResponseResult.infoResponse.Role = role;
          newInfoResponseResult.infoResponse.FirstName = name;
          string schemaCreateResult = _userRepository.CreateNewSchema (
            newInfoResponseResult.infoResponse, connectionStrings.userConnectionString, connectionStrings.masterConnectionString);

          // create the tables as the user is trial user;
          bool tablesCreateResult = this._applicationSetupRepository.CreateSchemaTables (
            _applicationSetting.Schema, schemaCreateResult, _applicationSetting.CreateConnectionString ());
          
          if (!tablesCreateResult)
          {
            this._logService.Log (Minutz.Models.LogLevel.Error, $"[(bool condition, string message, AuthRestModel infoResponse) newInfoResponseResult] There was a issue creating the tables for user {email}");
            return (tablesCreateResult, "There was a problem creating the records.", null);
          }
          Instance newInstance;
          if (!string.IsNullOrEmpty (invitationInstanceId))
          {
            if (schemaCreateResult != invitationInstanceId)
            {
              var related = existsResult.person.Related.SplitToList (Minutz.Models.StringDeviders.InstanceStringDevider, Minutz.Models.StringDeviders.MeetingStringDevider);
              if (related.Any (i => i.instanceId == invitationInstanceId))
              {
                newInstance =
                  this._instanceRepository.GetByUsername (invitationInstanceId, this._applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
              }
              else
              {
                newInstance =
                  this._instanceRepository.GetByUsername (schemaCreateResult, this._applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
              }
            }
            else
            {
              newInstance =
                this._instanceRepository.GetByUsername (schemaCreateResult, this._applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
            }
          }
          else
          {
            newInstance =
              this._instanceRepository.GetByUsername (schemaCreateResult, this._applicationSetting.Schema, _applicationSetting.CreateConnectionString ());
          }

          AuthRestModel createAuthRestResult = new AuthRestModel
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
          return (tablesCreateResult, "Success", createAuthRestResult);
        case RoleTypes.Guest:
          this._logService.Log (Minutz.Models.LogLevel.Error, $"Starting Guest signup {existsResult.person.Email}");
          (bool condition, string message, UserResponseModel tokenResponse) guestTokenResponse = this._auth0Repository.CreateToken (username, password);
          if (!guestTokenResponse.condition)
          {
            this._logService.Log (Minutz.Models.LogLevel.Error, $"[(bool condition, string message, UserResponseModel tokenResponse) guestTokenResponse] There was a issue getting the token info for user {email}");
            return (guestTokenResponse.condition, guestTokenResponse.message, null);
          }
          this._logService.Log (Minutz.Models.LogLevel.Error, $"getting Guest info from auth0 {existsResult.person.Email}");
          (bool condition, string message, AuthRestModel infoResponse) guestinfoResponseResult = this._auth0Repository.GetUserInfo (guestTokenResponse.tokenResponse.access_token);

          if (!guestinfoResponseResult.condition)
          {
            this._logService.Log (Minutz.Models.LogLevel.Error, $"[(bool condition, string message, AuthRestModel infoResponse) guestinfoResponseResult] There was a issue getting the token info for user {email}");
            return (guestinfoResponseResult.condition, guestinfoResponseResult.message, null);
          }

          if (string.IsNullOrEmpty (invitationInstanceId))
          {
            this._logService.Log (Minutz.Models.LogLevel.Error, $"[(string.IsNullOrEmpty(invitationInstanceId))] There was with the invitationInstanceId for user {email}");
            return (false, "For a guest a invitation is required", null);
          }

          if (string.IsNullOrEmpty (meetingId))
          {
            this._logService.Log (Minutz.Models.LogLevel.Error, $"[(string.IsNullOrEmpty(meetingId))] There was with the meetingId for user {email}");
            return (false, "For a guest a invitation is required", null);
          }

          this._logService.Log (Minutz.Models.LogLevel.Error, $"getting Guest instance {existsResult.person.Email}");
          Instance guestInstance = this._instanceRepository.GetByUsername (invitationInstanceId, this._applicationSetting.Schema, _applicationSetting.CreateConnectionString ());

          if (!string.IsNullOrEmpty (existsResult.person.Related))
          {
            List<(string instanceId, string meetingId)> relatedInstances =
              existsResult.person.Related.SplitToList (Minutz.Models.StringDeviders.InstanceStringDevider, Minutz.Models.StringDeviders.MeetingStringDevider);
            var relatedInstance = relatedInstances.Where (i => i.instanceId == invitationInstanceId);
            if (!relatedInstance.Any ())
            {
              string updatedRelatedString = relatedInstances.ToRelatedString ();
              existsResult.person.Related = updatedRelatedString;
              (bool condition, string message) updatedPerson = this._userRepository.UpdatePerson (this._applicationSetting.CreateConnectionString (), this._applicationSetting.Schema, existsResult.person);
              if (!updatedPerson.condition)
              {
                this._logService.Log (Minutz.Models.LogLevel.Error, $"[(bool condition, string message) updatedPerson] There was with updating the person record for user {email}");
                return (false, "There was a problem updating the person information to add to existing invatations", null);
              }
            }
          }
          else
          {
            List<(string instanceId, string meetingId)> relatedInstances = new List<(string instanceId, string meetingId)> ();
            relatedInstances.Add ((invitationInstanceId, meetingId));
            string updatedRelatedString = relatedInstances.ToRelatedString ();
            existsResult.person.Related = updatedRelatedString;
            existsResult.person.Active = true;
            existsResult.person.ProfilePicture = guestinfoResponseResult.infoResponse.Picture;
            existsResult.person.Role = RoleTypes.Guest;
            
            this._logService.Log (Minutz.Models.LogLevel.Error, $"updating Guest info {existsResult.person.Email}");
            (bool condition, string message) updatedPerson = this._userRepository.UpdatePerson (this._applicationSetting.CreateConnectionString (), this._applicationSetting.Schema, existsResult.person);
            if (!updatedPerson.condition)
            {
              this._logService.Log (Minutz.Models.LogLevel.Error, $"[(bool condition, string message) updatedPerson] There was with updating the person record for user {email}");
              return (false, "There was a problem updating the person information for a new invatation", null);
            }
          }
          this._logService.Log (Minutz.Models.LogLevel.Error, $"update Guest invite status {existsResult.person.Email}");
          (bool condition, string message) meetingResult = this._meetingAttendeeRepository.UpdateInviteeStatus (
            existsResult.person.Email, existsResult.person.Identityid, "Accepted", invitationInstanceId, this._applicationSetting.CreateConnectionString (
              this._applicationSetting.Server, this._applicationSetting.Catalogue, guestInstance.Username, guestInstance.Password));

          AuthRestModel guestAuthRestResult = new AuthRestModel
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
      this._cache.Remove (token);
      return this.GetUserInfo (token);
    }

    public AuthRestModel GetUserInfo (string token)
    {
      AuthRestModel result;
      if (!_cache.TryGetValue (token, out result))
      {
        var httpResult = Helper.HttpService.Get ($"{_applicationSetting.Authority}userinfo", token);
        result = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthRestModel> (httpResult);
        // Set cache options.
        var cacheEntryOptions = new MemoryCacheEntryOptions ()
          // Keep in cache for this time, reset time if accessed.
          .SetSlidingExpiration (TimeSpan.FromMinutes (10));
        // Save data in cache.
        _cache.Set (token, result, cacheEntryOptions);
      }
      return result;
    }

    internal (string userConnectionString, string masterConnectionString) GetConnectionStrings ()
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