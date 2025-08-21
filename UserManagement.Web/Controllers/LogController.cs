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

    [HttpGet("AllLogs")]
    public async Task<ViewResult> ViewAllLogs(int page = 1, int pageSize = 10)
    {
        var logs = await _logService.GetLogsAsync();

        // Order by timestamp descending
        var orderedLogs = logs.OrderByDescending(l => l.Timestamp);

        // Pagination
        var pagedLogs = orderedLogs
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

        // Pass total count for page links
        ViewBag.TotalPages = (int)Math.Ceiling((double)logs.Count() / pageSize);
        ViewBag.CurrentPage = page;

        return View("ViewLogs", pagedLogs);
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
