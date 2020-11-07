using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using dotnetcondapackage.Services;
using dotnetcondapackage.Models;
using dotnetcondapackage.Entities;
using System.Security.Claims;

namespace dotnetcondapackage.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        //[Authorize(Roles = Role.Admin)]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var currentUserId = int.Parse(User.Identity.Name);
            
            if (!_userService.AuthenrticateAdmin(currentUserId))
                return Forbid();
            var users = _userService.GetAllUser();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            // only allow admins to access other user records
            var currentUserId = int.Parse(User.Identity.Name);

            //if (id != currentUserId && !User.IsInRole(Role.Admin))
            //if (id != currentUserId)
            if (id != currentUserId && !_userService.AuthenrticateAdmin(id))
                return Forbid();

            var user = _userService.GetUserById(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }


    

}
}
