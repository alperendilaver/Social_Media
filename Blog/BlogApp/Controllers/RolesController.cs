using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlogApp.Controllers
{
    public class RolesController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<AppUser> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager=userManager;
        }   
            
        [Authorize]
        public IActionResult AddRole(string appUserId , string name,int groupId){
            var user = _userManager.Users.FirstOrDefault(x=>x.Id==appUserId);
            _userManager.AddToRoleAsync(user,name);
            return RedirectToAction("Detail","Group",new{
                id = groupId
            });
        }
    }
}