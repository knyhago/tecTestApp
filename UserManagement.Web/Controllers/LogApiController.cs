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

         public async Task<IActionResult> GetAllLogs()
        {
            var users =await _logService.GetLogsAsync();

            return Ok(users); 
        }

        [HttpGet("{id}")]
         public async Task<IActionResult> GetLogs(int id)
        {
            var items =await _logService.GetLogsAsync();

            var users=items.Where(x=>x.UserId==id);

            return Ok(users);
        }

         [HttpPost("addLog")]
        public async Task<IActionResult> Create([FromBody] Log log)
        {
            await _logService.AddLogAsync(log);
            return Ok(log);
        }

    }
}
