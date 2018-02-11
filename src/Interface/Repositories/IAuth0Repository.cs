namespace Interface.Repositories
{
    public interface IAuth0Repository
    {
        (bool condition, string message, string token) CreateToken (
            string username, string password);
    }
}