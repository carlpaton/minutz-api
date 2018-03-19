using System;
using System.Net.Http;
using System.Text;
using Interface.Repositories;
using Interface.Services;
using Newtonsoft.Json;

namespace Reports
{
  public class JsReportRepository : IReportRepository
  {
    private readonly IApplicationSetting _applicationSetting;
    private readonly IHttpService _httpService;

    public JsReportRepository(
      IApplicationSetting applicationSetting, IHttpService httpService)
    {
      _applicationSetting = applicationSetting;
      _httpService = httpService;
    }

    public (bool condition, string message, byte[] file) CreateMinutesReport
      (dynamic meeting)
    {
      var payload = new
      {
        template = new { shortid = _applicationSetting.GetReportTemplateKey()},
        data = meeting
      };
      
      var token = CreateAuthHeader();
      var body = MinutesRequestBody(payload);
      var result = _httpService.PostReport(_applicationSetting.ReportUrl,body ,token);
       return (result.condition,result.message, result.result);
    }

    private string CreateAuthHeader()
    {
      var username = _applicationSetting.ReportUsername;
      var password = _applicationSetting.ReportPassword;
      var hash = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password)));
      return $"Basic {hash}";
    }

    private static StringContent MinutesRequestBody(dynamic model)
    {
      return new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
    }
  }
}