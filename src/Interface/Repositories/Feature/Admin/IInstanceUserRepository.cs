using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Admin
{
    public interface IInstanceUserRepository
    {
        PersonResponse GetInstancePeople(string schema, string connectionString);

        PersonResponse AddInstancePerson(Person person, string schema, string connectionString);

        PersonResponse UpdateInstancePerson(Person person, string schema, string connectionString);
    }
}