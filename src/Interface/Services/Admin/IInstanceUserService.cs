using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Admin
{
    public interface IInstanceUserService
    {
        PersonResponse GetInstancePeople(AuthRestModel user);
        
        PersonResponse AddInstancePerson(Person person, AuthRestModel user);
    }
}