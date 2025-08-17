using System;

namespace UserManagement.Data.Entities;

public class Log
{
    public int Id { get; set; }
    public long UserId { get; set; }          
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Action { get; set; }      
    public string? Details { get; set; }      
    public string? PerformedBy { get; set; }

}
