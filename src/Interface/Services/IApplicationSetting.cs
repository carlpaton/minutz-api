namespace Interface.Services
{
    public interface IApplicationSetting
    {
        string ClientId { get; }
        
        string ClientSecret { get; }
        
        string Catalogue { get; }
        
        string Schema { get; }
        
        string Username { get; }
        
        string Password { get; }
        
        string Server { get; }
        
        string AuthorityDomain { get; }
        
        string Authority { get; }
        
        string AuthorityConnection { get; }
        
        string AuthorityManagmentToken { get; }
        
        string AuthorityManagementClientId { get; }
        
        string AuthorityManagementClientSecret { get; }
        
        string ReportUrl { get; }
        
        string ReportUsername { get; }
    
        string ReportPassword { get; }

        string GetReportTemplateKey();

        string CreateMasterConnectionString();
            
        string CreateConnectionString(
            string server,string catalogue, string username,string password);

        string CreateConnectionString();

        string GetInstancePassword(string instance);
    }
}