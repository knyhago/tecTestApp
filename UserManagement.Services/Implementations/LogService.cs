using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Data.Entities;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class LogService:ILogService
{
    private readonly IDataContext _dataContext;

    public LogService(IDataContext dataContext)
    {
        _dataContext=dataContext;

    }
    public async Task AddLogAsync(Log log){
        await _dataContext.Create(log);
    }

    public  async Task<List<Log>> GetLogsAsync()
    {
       return await _dataContext.GetAll<Log>();
    }

}
