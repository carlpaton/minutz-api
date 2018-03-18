using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;
using Minutz.Models.ViewModels;

namespace Reports
{
    public class JsReportRepository:  IReportRepository
    {
        private readonly IApplicationSetting _applicationSetting;

        public JsReportRepository(IApplicationSetting applicationSetting)
        {
            _applicationSetting = applicationSetting;
        }

        public byte[] GetMinutes(Meeting meeting)
        {
            var payload = new JsReportBase();
            return null;
        }

        // minutes id :SJKYFyoYM
        // https://minutz.jsreportonline.net/api/report
    }
}
