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
        private IEmailSender _emailSender;
        public UsersController(UserManager<AppUser> userManager , RoleManager<IdentityRole> roleManager,SignInManager<AppUser> signInManager,IPostRepository postRepository,BlogContext blogContext,IEmailSender emailSender)
        {_userManager = userManager;
            _roleManager = roleManager;
            _signInManager =signInManager;
            _postRepository = postRepository;
            _context=blogContext;
            _emailSender = emailSender;
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
                     var token = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                    var url = Url.Action("ConfirmEmail","Users",new{user.Id,token});
                    await _emailSender.SendEmailAsync(user.Email,"Hesap Onayı",$"Lütfen hesabınızı onaylamak için <a href='http://localhost:5294{url}'>tıklayın.</a>");
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
        public async Task<IActionResult> ConfirmEmail(string Id, string token){
            if(Id==null || token == null){
                TempData["message"] = "Geçersiz token bilgisi";
                return View();
            }
            var user = await _userManager.FindByIdAsync(Id);
            if(user !=null){
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if(result.Succeeded){
                    TempData["message"] = "Hesabınız Onaylandı";
                    return View();
                }
            }
            TempData["message"] = "Kullanıcı Bulunamadı";
            return View();
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
                if(!await _userManager.IsEmailConfirmedAsync(user)){
                        ModelState.AddModelError("","Hesabınızı onaylayın");
                        return View();
                }
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
        public IActionResult ForgotPassword(){
            return View();
        }
        [HttpPost]
          public async Task<IActionResult> ForgotPassword(string email){
            if(string.IsNullOrEmpty(email)){
                return View();
            }
            var user = await _userManager.FindByEmailAsync(email);
            if(user!=null){
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var url = Url.Action("ResetPassword", "Users",new {user.Id,token});//en sondaki obje urlde olması gereken kısımları belirtiyo
                await _emailSender.SendEmailAsync(email,"Parola Sıfırlama",$"Parolanızı sıfırlamak için linke <a href='http://localhost:5294{url}'>tıklayın.</a> tıklayınız.");
                TempData["message"] ="Eposta adresinize yönlendirilen link ile şifrenizi yenileyebilirsiniz";
                return View();
            }
            else
                return View();
        }

        public IActionResult ResetPassword(string Id, string token){
            var model = new ResetPasswordViewModel {token=token,Id=Id};
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model){
            if(ModelState.IsValid){
                var user = await _userManager.FindByIdAsync(model.Id);
                if(user!=null){
                    var result = await _userManager.ResetPasswordAsync(user,model.token,model.password);
                    if(result.Succeeded){
                        TempData["message"] ="Şifreniz Değiştirildi";
                        return RedirectToAction("Login");
                    }
                    else{
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("",item.Description);
                        }
                        return View(model);
                    }
                }
                else {
                    ModelState.AddModelError("","Kullanıcı Bulunamadı");
                    return View(model);
                }
            }
            else{
                return View(model);
            }
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