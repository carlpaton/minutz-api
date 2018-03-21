namespace Interface.Repositories
{
    public interface IReportRepository
    {
        (bool condition, string message, byte[] file) CreateMinutesReport
        (dynamic meeting);
    }
}