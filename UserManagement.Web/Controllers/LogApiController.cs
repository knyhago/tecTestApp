using System.Linq;
using UserManagement.Data.Entities;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Web.Controllers
{
    [Route("api/logs")]
    [ApiController]
    public class LogApiController : ControllerBase
    {
         private readonly ILogService _logService;

        public LogApiController(
        ILogService logService)
        {
            _logService = logService;
        }

         public IActionResult GetAllLogs()
        {
            var users = _logService.GetLogs();

            return Ok(users);  // returns JSON for Blazor
        }

        [HttpGet("{id}")]
         public IActionResult GetLogs(int id)
        {
            var users = _logService.GetLogs()
            .Where(x=>x.UserId==id);

            return Ok(users);  // returns JSON for Blazor
        }

         [HttpPost("addLog")]
        public IActionResult Create([FromBody] Log log)
        {
            _logService.AddLog(log);
            return Ok(log);
        }

    }
}
