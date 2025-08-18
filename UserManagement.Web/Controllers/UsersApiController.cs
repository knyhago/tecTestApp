using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    

     public IActionResult GetAll()
        {
            var users = _userService.GetAll()
                .Select(u => new UserDto(
                    u.Id, u.Forename, u.Surname, u.DateOfBirth,
                     u.Email, u.IsActive))
                .ToList();

            return Ok(users);  // returns JSON for Blazor
        }
         // GET: api/users/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var user = _userService.GetUserById(id);
            if (user == null) return NotFound();

            var dto = new UserDto(
                user.Id, user.Forename, user.Surname, user.DateOfBirth, user.Email, user.IsActive);

            return Ok(dto);
        }
          [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var user = _userService.GetUserById(id);
            if (user == null) return NotFound();

            _userService.DeleteUser(user);
            return NoContent();  // 204, perfect for HttpClient.DeleteAsync
        }

         // PUT: api/users/{id}
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] User user)
        {
            var existingUser = _userService.GetUserById(id);
            if (existingUser == null) return NotFound();

            existingUser.Forename = user.Forename;
            existingUser.Surname = user.Surname;
            existingUser.Email = user.Email;
            existingUser.IsActive = user.IsActive;
            existingUser.DateOfBirth = user.DateOfBirth;

            _userService.UpdateUser(existingUser);
            return NoContent();
        }

        // POST: api/users
        [HttpPost("add")]
        public IActionResult Create( User user)
        {
            _userService.Add(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }




    }

    
}
