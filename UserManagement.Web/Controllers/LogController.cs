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
    public async Task<ViewResult> LogView(int id)
    {
        var logs=await _logService.GetLogsAsync();

        var log =logs.First(item=>item.Id==id);
        return View(log);
       
    }

     [HttpGet("AllLogs")]//This gets all the logs irrespective of the user
    public async Task<ViewResult> ViewAllLogs(int id)
    {
        var logs=await _logService.GetLogsAsync();

        return View("ViewLogs",logs);
       
    }

    [HttpGet("Logs")] //gets logs for the specific user
    public async Task <ViewResult> ViewLogs(int id)
    {
        var logs=await _logService.GetLogsAsync();

        var log =logs.Where(item=>item.UserId==id);
        return View(log);
       
    }
    }


}
