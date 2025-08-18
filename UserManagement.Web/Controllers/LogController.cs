using System.Linq;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Web.Controllers
{
    [Route("logs")]
    public class LogController : Controller
    {
        private ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;

        }
    [HttpGet("LogDetails")] //gets log details for specific log id
    public ViewResult LogView(int id)
    {
        var logs=_logService.GetLogs();

        var log =logs.First(item=>item.Id==id);
        return View(log);
       
    }

     [HttpGet("AllLogs")]//This gets all the logs irrespective of the user
    public ViewResult ViewAllLogs(int id)
    {
        var logs=_logService.GetLogs();

        return View("ViewLogs",logs);
       
    }

    [HttpGet("Logs")] //gets logs for the specific user
    public ViewResult ViewLogs(int id)
    {
        var logs=_logService.GetLogs();

        var log =logs.Where(item=>item.UserId==id);
        return View(log);
       
    }
    }


}
