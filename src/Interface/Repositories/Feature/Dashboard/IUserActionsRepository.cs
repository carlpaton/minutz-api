using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Dashboard
{
    public interface IUserActionsRepository
    {
        ActionMessage Actions(string email, string schema, string connectionString);
    }
}