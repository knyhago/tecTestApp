using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Data.Entities;

namespace UserManagement.Services.Domain.Interfaces;

public interface ILogService
{
    Task AddLogAsync(Log log);

    Task<List<Log>> GetLogsAsync();

}
