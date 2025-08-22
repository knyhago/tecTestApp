using UserManagement.Contracts.DTOS;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Web.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogService _logService;

        public UsersApiController(IUserService userService,
        ILogService logService)
        {
            _userService = userService;
            _logService = logService;
        }
    

     public async Task<IActionResult> GetAll()
        {
            var items =await _userService.GetAllAsync();

             var users=items.Select(u => new UserDto(
                    u.Id, u.Forename, u.Surname, u.DateOfBirth,
                     u.Email, u.IsActive))
                .ToList();

            return Ok(users); 
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var user =await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            var dto = new UserDto(
                user.Id, user.Forename, user.Surname, user.DateOfBirth, user.Email, user.IsActive);

            return Ok(dto);
        }
          [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var user =await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            await _userService.DeleteUserAsync(user);
            return NoContent(); 
        }

    
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] User user)
        {
        var existingUser = await _userService.GetUserByIdAsync(id);
        if (existingUser == null) return NotFound();

        // Only check uniqueness if email is changed
        if (!string.Equals(existingUser.Email, user.Email, StringComparison.OrdinalIgnoreCase))
        {
            var exists = (await _userService.GetAllAsync())
                        .Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                return BadRequest("Email already exists.");
            }
        }

        existingUser.Forename = user.Forename;
        existingUser.Surname = user.Surname;
        existingUser.Email = user.Email;
        existingUser.IsActive = user.IsActive;
        existingUser.DateOfBirth = user.DateOfBirth;

        await _userService.UpdateUserAsync(existingUser);
        return NoContent();
    }

        
    [HttpPost("add")]
        
    public async Task<IActionResult> Create(User user)
    {
        //if email already exists
        var exists = (await _userService.GetAllAsync())
                    .Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            return BadRequest("Email already exists.");
        }

        await _userService.AddAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }
    }

    
}
