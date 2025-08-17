using System;
using System.Collections.Generic;
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
    public void AddLog(Log log){
        _dataContext.Create(log);
    }

    public IEnumerable<Log> GetLogs()
    {
       return _dataContext.GetAll<Log>();
    }

}
