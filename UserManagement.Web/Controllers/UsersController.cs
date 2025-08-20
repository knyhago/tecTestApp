using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Contracts.DTOS;
using UserManagement.Contracts.Models.Users;

namespace UserManagement.WebMS.Controllers;

//[ApiController]
[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly ILogService _logService;

     private readonly ILogger<User> _logger;
    public UsersController(IUserService userService,ILogger<User> logger,ILogService logService)
    {
        _userService = userService;
        _logger=logger;
        _logService=logService;

    } 

    [HttpGet]
    public async Task<ViewResult> List()
    {
    
        var users = await _userService.GetAllAsync();

        var items = users.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        }).ToList(); // Move ToList here

        var model = new UserListViewModel
        {
            Items = items
        };

        _logger.LogInformation("Displayed whole List");

        return View(model);
    }
    
    [HttpGet("FilterList")]
     public async Task<ViewResult> FilterList(bool isActive)
    {
        var users = await _userService.GetAllAsync();

        var items = users.Where(user=>user.IsActive==isActive)
        .Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        _logger.LogInformation("Displayed {isActive} List",isActive);

        return View("List",model);
    }
    //Form to Add a New User
    [HttpGet("AddForm")]
    public async Task<ViewResult> AddEditUserForm(int? id)
    {

        if(id is not null)
        {
            var items = await _userService.GetAllAsync();
            var users =items
            .Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        });;
            UserListItemViewModel user = users.First(usr => usr.Id == id);
            var data = _userService.ToDto(user);
            return View(data);

        }
        return View();
    }


    //submit the form after add/edit
    [HttpPost("SubmitForm")]
    public async Task<IActionResult> SubmitForm(UserDto userDTO)
    {
        var newUser = _userService.ToEntity(userDTO);

        if (newUser.Id == 0)
        {
           await _userService.AddAsync(newUser);
        }
        else
        {
            newUser.Id = userDTO.Id;
           await _userService.UpdateUserAsync(newUser);
        }

        return RedirectToAction("List");
    }

     [HttpGet("delete/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var users =await _userService.GetAllAsync();

        var user = users.First(usr=>usr.Id==id);

        await _userService.DeleteUserAsync(user);

        return RedirectToAction("List");

    }

    // private static User ToEntity(UserDto userDTO) =>
    //         new()
    //         {
    //             Id = userDTO.Id,
    //             Forename = userDTO.Forename,
    //             Surname = userDTO.Surname,
    //             DateOfBirth = userDTO.DateOfBirth,
    //             Email = userDTO.Email,
    //             IsActive = true
    //         };

    //  private static UserDto ToDto(UserListItemViewModel user) =>
    //             new UserDto(
    //                 user.Id,
    //                 user.Forename!,
    //                 user.Surname!,
    //                 user.DateOfBirth,
    //                 user.Email!,
    //                 user.IsActive
    //             );
}
