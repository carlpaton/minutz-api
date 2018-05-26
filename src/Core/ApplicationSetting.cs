using System;
using Interface;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;

namespace Core
{
  public class ApplicationSetting : IApplicationSetting
  {
    private readonly IInstanceRepository _instanceRepository;
    private readonly IEncryptor _encryptor;

    public ApplicationSetting
      (IInstanceRepository instanceRepository, IEncryptor encryptor)
    {
      _instanceRepository = instanceRepository;
      _encryptor = encryptor;
    }

    public string ClientId => Environment.GetEnvironmentVariable("CLIENTID");

    public string ClientSecret => Environment.GetEnvironmentVariable("CLIENTSECRET");
    
    public string Server => Environment.GetEnvironmentVariable("SERVER_ADDRESS");

    public string Catalogue => Environment.GetEnvironmentVariable("DEFAULT_CATALOGUE");

    public string Schema => Environment.GetEnvironmentVariable("DEFAULT_SCHEMA");

    public string Username => Environment.GetEnvironmentVariable("DEFAULT_USER");

    public string Password => Environment.GetEnvironmentVariable("DEFAULT_PASSWORD");

    public string AuthorityDomain => Environment.GetEnvironmentVariable("DOMAIN");
    
    public string Authority => Environment.GetEnvironmentVariable("AUTHORITY");
    
    public string AuthorityConnection => Environment.GetEnvironmentVariable("CONNECTION");

    public string AuthorityManagmentToken => Environment.GetEnvironmentVariable("AuthorityManagmentToken");
    
    public string AuthorityManagementClientId => Environment.GetEnvironmentVariable("MANAGEMENT_CLIENTID");
    
    public string AuthorityManagementClientSecret => Environment.GetEnvironmentVariable("MANAGEMENT_CLIENT_SECRETE");

    public string ReportUrl => Environment.GetEnvironmentVariable("ReportUrl");

    public string ReportUsername => Environment.GetEnvironmentVariable("ReportUsername");
    
    public string ReportPassword => Environment.GetEnvironmentVariable("ReportPassword");

    public string GetReportTemplateKey()
    {
      return Environment.GetEnvironmentVariable("ReportMinutesKey");
    }

    public string CreateConnectionString
      (string server, string catalogue, string username, string password)
    {
      var unecryptedPassword = _encryptor.DecryptString(password);
      return $"Server={server};User ID={username};pwd={unecryptedPassword};database={catalogue};";
    }

    public string CreateConnectionString()
    {
      return $"Server={Server};User ID={Username};pwd={Password};database={Catalogue};";
    }

    public string GetInstancePassword
      (string instance)
    {
      var instanceObject = _instanceRepository.GetByUsername(instance, CreateConnectionString());  
      return instanceObject.Instance.Password;
    }
  }
}