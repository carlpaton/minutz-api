using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspnetAuthenticationRespository
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private string Server => Environment.GetEnvironmentVariable("SERVER_ADDRESS");

        private string Catalogue => Environment.GetEnvironmentVariable("DEFAULT_CATALOGUE");

        private string Username => Environment.GetEnvironmentVariable("DEFAULT_USER");

        private string Password => Environment.GetEnvironmentVariable("DEFAULT_PASSWORD");
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }
        
        private string GetConnectionString()
        {   
            return $"Server={Server}; database={Catalogue};uid={Username};pwd={Password};pooling=true;";
        }
    }
}