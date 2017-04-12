using tzatziki.minutz.interfaces.Repositories;

namespace tzatziki.minutz.mysqlrepository
{
    public class PersonRepository : IPersonRepository
    {
        public bool IsPerson(string email)
        {
            return false;
        }
    }
}