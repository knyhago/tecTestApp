using System.Collections.Generic;
using UserManagement.Contracts.DTOS;
using UserManagement.Contracts.Models.Users;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    IEnumerable<User> FilterByActive(bool isActive);
    IEnumerable<User> GetAll();

    User GetUserById(long id);

    void Add(User user);

    void UpdateUser(User user);

    void DeleteUser (User user);

    User ToEntity(UserDto userDto);

    UserDto ToDto (UserListItemViewModel user);


}
