using System.Linq;
using Microsoft.Extensions.Logging;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.DTOS;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

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
    [HttpGet("Logs")]
    public ViewResult ViewLogs(int id)
    {
        var logs=_logService.GetLogs();

        var log =logs.Where(item=>item.UserId==id);
        return View(log);
       
    }
    [HttpGet("AllLogs")]
    public ViewResult ViewAllLogs(int id)
    {
        var logs=_logService.GetLogs();

        return View("ViewLogs",logs);
       
    }

     [HttpGet("LogDetails")]
    public ViewResult LogView(int id)
    {
        var logs=_logService.GetLogs();

        var log =logs.First(item=>item.Id==id);
        return View(log);
       
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
            IEnumerable<User> users = _userService.GetAll();
            User user =users.First(usr=>usr.Id == id);

            var data = new AddUserDTO(
                user.Id,
                user.Forename,
                user.Surname,
                user.DateOfBirth,
                user.Email,
                user.IsActive
            );
            
            return View(data);

        }
        return View();
    }
    //submit the form after add/edit
    [HttpPost("SubmitForm")]
    public IActionResult SubmitForm(AddUserDTO userDTO)
    {
        
        User newUser= new(){
            Id=userDTO.Id,
            Forename=userDTO.Forename,
            Surname=userDTO.Surname,
            DateOfBirth=userDTO.DateOfBirth,
            Email=userDTO.Email,
            IsActive=true
        };

        if(newUser.Id==0)
        {
            _userService.Add(newUser);
        }
        else{
            newUser.Id=userDTO.Id;
            _userService.UpdateUser(newUser);
        }
        
        return RedirectToAction("List");
    }

    [HttpGet("delete")]
    public IActionResult DeleteUser(int id)
    {
        var users = _userService.GetAll();

        var user = users.First(usr=>usr.Id==id);

        _userService.DeleteUser(user);

        return RedirectToAction("List");

    }
}
