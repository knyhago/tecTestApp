using System;
using System.Collections;
using System.Collections.Generic;
using UserManagement.Data.Entities;

namespace UserManagement.Services.Domain.Interfaces;

public interface ILogService
{
    void AddLog(Log log);

    IEnumerable<Log> GetLogs();

}
