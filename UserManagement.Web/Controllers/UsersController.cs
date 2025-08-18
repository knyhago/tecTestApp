using System.Linq;
using Microsoft.Extensions.Logging;
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
    public ViewResult List()
    {
        var items = _userService.GetAll().Select(p => new UserListItemViewModel
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
        _logger.LogInformation("Displayed whole List");

        return View(model);
    }
    
    [HttpGet("FilterList")]
     public ViewResult FilterList(bool isActive)
    {
        var items = _userService.GetAll().Where(user=>user.IsActive==isActive)
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
    public ViewResult AddEditUserForm(int? id)
    {

        if(id is not null)
        {
            IEnumerable<UserListItemViewModel> users = _userService.GetAll().
            Select(p => new UserListItemViewModel
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
    public IActionResult SubmitForm(UserDto userDTO)
    {
        var newUser = _userService.ToEntity(userDTO);

        if (newUser.Id == 0)
        {
            _userService.Add(newUser);
        }
        else
        {
            newUser.Id = userDTO.Id;
            _userService.UpdateUser(newUser);
        }

        return RedirectToAction("List");
    }

     [HttpGet("delete/{id}")]
    public IActionResult DeleteUser(int id)
    {
        var users = _userService.GetAll();

        var user = users.First(usr=>usr.Id==id);

        _userService.DeleteUser(user);

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
