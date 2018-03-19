namespace Interface.Repositories
{
    public interface IReportRepository
    {
        byte[] CreateMinutesReport
        (dynamic meeting);
    }
}