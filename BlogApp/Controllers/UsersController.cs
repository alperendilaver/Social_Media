using System.IO.Compression;
using System.Security.Claims;
using BlogApp.Data;
using BlogApp.Models;
using BlogApp.Repositories.Abstract;
using BlogApp.Repositories.Concreate.EfCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogApp.Controllers
{
    public class UsersController : Controller{
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IPostRepository _postRepository;
        private SignInManager<AppUser> _signInManager;
        private BlogContext _context;
        public UsersController(UserManager<AppUser> userManager , RoleManager<IdentityRole> roleManager,SignInManager<AppUser> signInManager,IPostRepository postRepository,BlogContext blogContext)
        {_userManager = userManager;
            _roleManager = roleManager;
            _signInManager =signInManager;
            _postRepository = postRepository;
            _context=blogContext;
        }
        
        [HttpGet]
        public IActionResult Create(){
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateViewModel model){
            if(ModelState.IsValid && model.Password!=null){
                var user =  new AppUser{
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                };
                IdentityResult result = await _userManager.CreateAsync(user,model.Password);
                if(result.Succeeded){
                    _context.AppUser.Add(user);
                    _context.SaveChanges();
                    return RedirectToAction("Index","Home");
                }
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError("",item.Description);  
                }

            }
            return View(model);
        }
        public IActionResult Login(){
            if(User.Identity.IsAuthenticated)
                return RedirectToAction("Profile");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel model){
            if(ModelState.IsValid){
                var user=await _userManager.FindByEmailAsync(model.Email ?? "");
                if(user!=null)
                {
                await _signInManager.SignOutAsync();
                var result = await _signInManager.PasswordSignInAsync(user,model.Password,isPersistent:true,lockoutOnFailure:true);
                if(result.Succeeded){
                    
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ModelState.AddModelError("","Hatalı Giriş");
                    return View(model);
                }
                }
                else{
                    ModelState.AddModelError("","Hatalı E-posta veya parola girişi");
                    return View(model);
                }
            }
            else 
                return View(model);
        }
        public async Task<IActionResult> Profile(string? id){
            if(!User.Identity.IsAuthenticated)
                return RedirectToAction("Login");
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userManager.FindByNameAsync(userName);
            if(id!=null){
                user = await _userManager.FindByIdAsync(id);
            }

            if(user!=null){
                var uposts = await _postRepository.GetPostsByUserIdAsync(user.Id);
          
                var ProfileModel = new ProfileViewModel{
                user = user,
                posts = (List<Post>)uposts,
            };
                return View(ProfileModel);
            }
            return RedirectToAction("Login");
        }
        public async Task<IActionResult>Logout(){
            if(User.Identity.IsAuthenticated){
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("Index","Home");
        }
    }
}