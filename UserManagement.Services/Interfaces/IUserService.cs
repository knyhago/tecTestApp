using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Contracts.DTOS;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    Task<List<User>> FilterByActiveAsync(bool isActive);
    Task<List<User>> GetAllAsync();

    Task<User?> GetUserByIdAsync(long id);

    Task AddAsync(User user);

    Task UpdateUserAsync(User user);

    Task DeleteUserAsync (User user);

    User ToEntity(UserDto userDto);

    // UserDto ToDto (UserListItemViewModel user);
    List<UserDto> ToDtoList(List<User> users);

    UserDto ToDto (User user);


}
