using Interface.Repositories;
using Interface.Services;

namespace Core
{
  public class JsReportService : IReportService
  {
    private readonly IReportRepository _reportRepository;

    public JsReportService(IReportRepository reportRepository)
    {
      _reportRepository = reportRepository;
    }
  }
}