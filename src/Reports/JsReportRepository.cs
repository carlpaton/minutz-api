using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;
using Minutz.Models.ViewModels;
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

    public byte[] CreateMinutesReport(
      dynamic meeting)
    {
      var payload = new
      {
        template = new { shortid = "SJKYFyoYM"},
        data = meeting
      };
      
      var token = createAuthHeader();
      var body = MinutesRequestBody(payload);
      var result = _httpService.Post(_applicationSetting.ReportUrl,body ,token);
      return null;
    }

    internal string createAuthHeader()
    {
      var username = "leeroya@gmail.com";
      var password = "@nathan01";
      var hash = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password)));
      return $"Basic {hash}";
    }

    internal StringContent MinutesRequestBody(dynamic model)
    {
      return new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
    }

    // minutes id :SJKYFyoYM
    // https://minutz.jsreportonline.net/api/report
  }
}