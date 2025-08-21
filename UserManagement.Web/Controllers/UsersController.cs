using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Contracts.DTOS;
namespace UserManagement.WebMS.Controllers;

//[ApiController]
[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly ILogService _logService;
     private readonly ILogger<User> _logger;
    public UsersController(IUserService userService,
                            ILogger<User> logger,
                            ILogService logService)
    {
        _userService = userService;
        _logger=logger;
        _logService=logService;

    } 

    [HttpGet]
    public async Task<ViewResult> List()//changed this function to display dto instead of  IEnumerable<UserListItemViewModel>
    {
        List<User> users = await _userService.GetAllAsync();
        List<UserDto> items = _userService.ToDtoList(users);// Move manual list mapping to ToDtolist
        

        _logger.LogInformation("Displayed whole List");

        return View(items);
    }

    [HttpGet("FilterList")]
     public async Task<ViewResult> FilterList(bool isActive)
    {
        List<User> users = await _userService.FilterByActiveAsync(isActive);

        var items = _userService.ToDtoList(users);

        _logger.LogInformation("Displayed {isActive} List",isActive);

        return View("List",items);
    }

    //Form to Add a New User
    [HttpGet("AddForm")]
    public async Task<ViewResult> AddEditUserForm(long? id)
    {

        if (id.HasValue && id.Value > 0)
    {
        // Edit scenario
        List<User> items = await _userService.GetAllAsync();
        User? user = items.FirstOrDefault(usr => usr.Id == id.Value);

        if (user is null)
            throw new Exception("User Page can be found to edit");

        UserDto data = _userService.ToDto(user);
        return View(data);
    }

    // Add scenario - pass empty model
    return View(new UserDto());
    }


    //submit the form after add/edit
    [HttpPost("SubmitForm")]
    public async Task<IActionResult> SubmitForm(UserDto userDTO)
    {
        User newUser = _userService.ToEntity(userDTO);

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
        List<User> users =await _userService.GetAllAsync();

        User user = users.First(usr=>usr.Id==id);

        await _userService.DeleteUserAsync(user);

        return RedirectToAction("List");

    }

    

}
