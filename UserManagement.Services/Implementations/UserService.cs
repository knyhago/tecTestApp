using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UserManagement.Contracts.DTOS;
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
    public async Task<List<User>> FilterByActiveAsync(bool isActive)
    {
        var data =await _dataAccess.GetAll<User>();

        var users =data.Where(user=>user.IsActive==isActive).ToList();

        return users;
       
    }

    public async Task<User> GetById(int id)
    {
       var user = await _dataAccess.GetById<User>(id);

    if (user is null)
        throw new Exception("User can't be found to track changes");

    return user;
    }

    public Task<List<User>> GetAllAsync() => _dataAccess.GetAll<User>();

    public async Task<User> GetUserByIdAsync(long id)
    {
        var users =await _dataAccess.GetAll<User>();

        var user = users.First(usr=>usr.Id==id);

        if(user is null) throw new Exception("User not found");

        return user;

    }

    public async Task AddAsync(User user)
    {
        await _dataAccess.Create(user);
        _logger.LogInformation("Added User {Name} Id {id}",user.Forename,user.Id);

       await _logService.AddLogAsync(new Log
        {
            UserId = user.Id,
            Action = "Added",
            Details = $"Created user {user.Forename} {user.Surname}",
            PerformedBy = user.Forename
        });


    }

    public async Task UpdateUserAsync(User user)
    {
        var olduser = await _dataAccess.GetById<User>(user.Id);
        if(olduser is null)
        {
            throw new Exception(" old user cant be found");
        }

        List<string> changes = new();

        if(string.Equals(olduser.Forename, user.Forename))
        {
            changes.Add($"{olduser.Forename} Changed to {user.Forename}");
        }
        if(string.Equals(olduser.Surname, user.Surname))
        {
            changes.Add($"{olduser.Surname} Changed to {user.Surname}");
        }
        if(olduser.DateOfBirth!= user.DateOfBirth)
        {
            changes.Add($"{olduser.DateOfBirth} Changed to {user.DateOfBirth}");
        }
        if(olduser.Email!= user.Email)
        {
            changes.Add($"{olduser.Email} Changed to {user.Email}");
        }
      
      await _dataAccess.Update(user);

      if(changes.Count != 0)
      {
         await  _logService.AddLogAsync(new Log
        {
            UserId = user.Id,
            Action = "Edited",
            Details = string.Join("; ", changes),
            PerformedBy = user.Forename
        });

      }
        
        _logger.LogInformation("Updated User {Name} Id {id}",user.Forename,user.Id);
    }

    public async Task DeleteUserAsync(User user)
    {
        await _dataAccess.Delete(user);

        await  _logService.AddLogAsync(new Log
        {
            UserId = user.Id,
            Action = "Deleted",
            Details = $"Deleted user {user.Forename} {user.Surname}",
            PerformedBy = user.Forename
        });
       _logger.LogInformation("Deleted User {Name} Id {id}",user.Forename,user.Id);

    }

    //  public  UserDto ToDto(UserListItemViewModel user) =>
    //             new UserDto(
    //                 user.Id,
    //                 user.Forename!,
    //                 user.Surname!,
    //                 user.DateOfBirth,
    //                 user.Email!,
    //                 user.IsActive
    //             );
    public  UserDto ToDto(User user) =>
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

     public  List<UserDto> ToDtoList(List<User> users) =>
            users.Select(p => new UserDto
            (
                p.Id,
                p.Forename,
                p.Surname,
                p.DateOfBirth,
                p.Email,
                p.IsActive
            )).ToList();
    

    
}
