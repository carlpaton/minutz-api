using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interface.Repositories;
using Interface.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers {
  public class LogController : Controller {
    private readonly ILogRepository _logrepository;
    private readonly IApplicationSetting _applicationSettings;
    public LogController (ILogRepository logRepository,
      IApplicationSetting applicationSettings) {
      this._logrepository = logRepository;
      this._applicationSettings = applicationSettings;
    }

    [HttpGet ("api/Logs")]
    public IActionResult Get () {
      return new ObjectResult (_logrepository.Logs (
        _applicationSettings.Schema,
        _applicationSettings.CreateConnectionString ()));
    }

    [Route ("Home/Error")]
    public IActionResult Error () {
      // Get the details of the exception that occurred
      var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature> ();

      if (exceptionFeature != null) {
        // Get which route the exception occurred at
        string routeWhereExceptionOccurred = exceptionFeature.Path;

        // Get the exception that occurred
        Exception exceptionThatOccurred = exceptionFeature.Error;

        // TODO: Do something with the exception
        // Log it with Serilog?
        // Send an e-mail, text, fax, or carrier pidgeon?  Maybe all of the above?
        // Whatever you do, be careful to catch any exceptions, otherwise you'll end up with a blank page and throwing a 500
        var notify = new Notifications.Notify ();
        var error = new Notifications.ErrorNotification (notify);
        error.SendErrorMail (exceptionThatOccurred);
      }

      return View ();
    }
  }
}