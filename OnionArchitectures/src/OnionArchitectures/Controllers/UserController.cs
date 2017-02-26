using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnionArchitectures.Data;
using OnionArchitectures.Models;
using OnionArchitectures.Service;
using System.Linq;
using OnionArchitectures.Repository;

namespace OnionArchitectures.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserProfileService _userProfileService;

        public UserController(IUserService userService, IUserProfileService userProfileService)
        {
            _userService = userService;
            _userProfileService = userProfileService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new List<UserViewModel>();
            _userService.GetUsers().ToList().ForEach(u =>
            {
                var userProfile = _userProfileService.GetUserProfile(u.Id);
                var user = new UserViewModel
                {
                    Id = u.Id,
                    Name = $"{userProfile.FirstName} {userProfile.LastName}",
                    Email = u.Email,
                    Address = userProfile.Address
                };
                model.Add(user);
            });

            return Ok(model);
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] UserViewModel model)
        {
            var userEntity = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                AddedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                UserProfile = new UserProfile
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                }
            };
            _userService.InsertUser(userEntity);
            
            return Ok(model);
        }

        [HttpGet("{id}")]
        public ActionResult EditUser(int id, [FromBody] UserViewModel model)
        {
            if (id != 0 && !string.IsNullOrEmpty(Convert.ToString(id)))
            {
                var userEntity = _userService.GetUser(model.Id);
                var userProfileEntity = _userProfileService.GetUserProfile(model.Id);
                model.FirstName = userProfileEntity.FirstName;
                model.LastName = userProfileEntity.LastName;
                model.Address = userProfileEntity.Address;
                model.Email = userEntity.Email;
            }
            return Ok(model);
        }

      
        [HttpPut]
        public ActionResult EditUser([FromBody] UserViewModel model)
        {
            var userEntity = _userService.GetUser(model.Id);
            userEntity.Email = model.Email;
            userEntity.ModifiedDate = DateTime.UtcNow;
            userEntity.IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var userProfileEntity = _userProfileService.GetUserProfile(model.Id);
            userProfileEntity.FirstName = model.FirstName;
            userProfileEntity.LastName = model.LastName;
            userProfileEntity.Address = model.Address;
            userProfileEntity.ModifiedDate = DateTime.UtcNow;
            userProfileEntity.IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            userEntity.UserProfile = userProfileEntity;
            _userService.UpdateUser(userEntity);
            return Ok(model);
        }


        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id, [FromBody] UserViewModel model)
        {
            _userService.DeleteUser(model.Id);
            return Ok(id);
        }
    }
}
