using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using UserManagement.Contracts.DTOS;
using UserManagement.Contracts.Models.Users;
using UserManagement.Data;
using UserManagement.Data.Entities;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
   private readonly ILogger<User> _logger;

   private readonly ILogService _logService;
    public UserService(IDataContext dataAccess,
    ILogger<User> logger,ILogService logService)
    {
        _dataAccess = dataAccess;
        _logger=logger;
        _logService=logService;
    } 
    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public IEnumerable<User> FilterByActive(bool isActive)
    {
        IQueryable<User> data = _dataAccess.GetAll<User>();

        IQueryable<User> users = data.Where(user=>user.IsActive==isActive);

        return users;
       
        //throw new NotImplementedException();
    }

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();

    public User GetUserById(long id)
    {
        var users = _dataAccess.GetAll<User>();

        var user = users.First(usr=>usr.Id==id);

        if(user is null) throw new Exception("User not found");

        return user;


    }

    public void Add(User user)
    {
        _dataAccess.Create(user);
        _logger.LogInformation("Added User {Name} Id {id}",user.Forename,user.Id);

        _logService.AddLog(new Log
        {
            UserId = user.Id,
            Action = "Added",
            Details = $"Created user {user.Forename} {user.Surname}",
            PerformedBy = user.Forename
        });


    }

    public void UpdateUser(User user)
    {
      
      _dataAccess.Update(user);
        _logService.AddLog(new Log
        {
            UserId = user.Id,
            Action = "Added",
            Details = $"Edited user {user.Forename} {user.Surname}",
            PerformedBy = user.Forename
        });

        
        _logger.LogInformation("Updated User {Name} Id {id}",user.Forename,user.Id);
    }

    public void DeleteUser(User user)
    {
        _dataAccess.Delete(user);

          _logService.AddLog(new Log
        {
            UserId = user.Id,
            Action = "Added",
            Details = $"Deleted user {user.Forename} {user.Surname}",
            PerformedBy = user.Forename
        });
       _logger.LogInformation("Deleted User {Name} Id {id}",user.Forename,user.Id);

    }

     public  UserDto ToDto(UserListItemViewModel user) =>
                new UserDto(
                    user.Id,
                    user.Forename!,
                    user.Surname!,
                    user.DateOfBirth,
                    user.Email!,
                    user.IsActive
                );
     public  User ToEntity(UserDto userDTO) =>
            new()
            {
                Id = userDTO.Id,
                Forename = userDTO.Forename,
                Surname = userDTO.Surname,
                DateOfBirth = userDTO.DateOfBirth,
                Email = userDTO.Email,
                IsActive = true
            };


    
}
