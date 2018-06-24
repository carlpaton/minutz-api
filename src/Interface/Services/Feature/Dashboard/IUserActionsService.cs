using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Dashboard
{
    public interface IUserActionsService
    {
        ActionMessage Actions(AuthRestModel user);
    }
}