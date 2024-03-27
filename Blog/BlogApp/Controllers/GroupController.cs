using System.Security.Claims;
using BlogApp.Data;
using BlogApp.Models;
using BlogApp.Repositories.Abstract;
using BlogApp.Repositories.Concreate.EfCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{   
    
    public class GroupController:Controller{
        IGroupRepository _groupRepository;
        UserManager<AppUser> _userManager;
        BlogContext _blogContext;
        IPostRepository _postRepository;
        IUserRepository _userRepository;
        RoleManager<IdentityRole> _roleManager;
        IGroupService _groupService;
        public GroupController(IGroupRepository groupRepository,UserManager<AppUser> userManager,BlogContext blogContext,IUserRepository userRepository,IPostRepository postRepository,RoleManager<IdentityRole> roleManager,IGroupService groupService)
        {
            _groupService=groupService;
            _groupRepository = groupRepository;
            _userManager = userManager;
            _blogContext=blogContext;
            _userRepository = userRepository;
            _postRepository=postRepository;
            _roleManager=roleManager;
        }
       [Authorize]
        public async Task<IActionResult> Index(){
            var groups = _groupRepository.groups.ToList();
            var currentUser=await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)??"");
            //üye olunan grupların listesi
            var jndgrp = _blogContext.groupMembers.Include(x=>x.group).Where(x=>x.userId==currentUser.Id).Select(x=>x.group).ToList();
            //current kullanıcıya ait istekleri getirir
            var requests = _blogContext.GroupMemberShipRequests.Where(x=>x.UserId==currentUser.Id && x.stat==status.waiting).ToList();
            ViewBag.requests = requests ;
            ViewBag.joinedGroups= jndgrp;
             ViewBag.CurrentUser= currentUser.Id;
            var a = requests.Find(x=>x.UserId==currentUser.Id && x.UserId==currentUser.Id);
          
            return View(groups);
        
        }
        
        
        public IActionResult Create(){
         if(!User.Identity.IsAuthenticated){
                return RedirectToAction("Login","Users");
            }    
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Create(CreateGroupViewModel model){
             if(!User.Identity.IsAuthenticated){
                return RedirectToAction("Login","Users");
            }    

            if(ModelState.IsValid){
                if(model.groupPhoto!=null){
                    var extension = Path.GetExtension(model.groupPhoto.FileName);
                        var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                        var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",randomFileName);
                        using (var stream = new FileStream(path,FileMode.Create))
                        {
                            await model.groupPhoto.CopyToAsync(stream);
                        }
                        model.image=randomFileName;
                }
                var user = await _userManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Name)??"");
                if(user!=null){
                    await _roleManager.CreateAsync(new IdentityRole{
                        Name=model.name+"admin"
                    });
                    await _userManager.AddToRoleAsync(user,model.name+"admin");
                    _groupRepository.createGroup(new Group{
                        GroupName=model.name,
                        image=model.image,
                        userId=user.Id,
                        });
                    var groupId=_groupRepository.groups.OrderByDescending(g => g.GroupId).FirstOrDefault().GroupId;
                  
                    if(!_blogContext.groupMembers.Any(x=>x.groupId==groupId && x.userId==user.Id) && groupId!=null)
                    {
                        _blogContext.groupMembers.Add(new GroupMembers{
                            groupId=groupId,
                            userId=user.Id
                            });
                    _blogContext.SaveChanges();
                   
                    
                    }
                return RedirectToAction("Detail","Group",new {
                    id=groupId
                });

                }else{
                    return View();
                }
            }
            else{
                ModelState.AddModelError("","Hata oluştu");
                return View(model);
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddMember(string userName,int groupId){
            var user = await _userManager.Users.FirstOrDefaultAsync(u=>u.UserName==userName);
            var group = await _groupRepository.groups.FirstOrDefaultAsync(x=>x.GroupId==groupId);
            if(user!=null && group!=null){
                if(!_blogContext.groupMembers.Any(x=>x.groupId==groupId && x.userId==user.Id))
                {
                    _blogContext.groupMembers.Add(new GroupMembers{
                    groupId=group.GroupId,
                    userId=user.Id,
                });
                }
                else{
                    ModelState.AddModelError("","Kullanıcı zaten gruba üye");
                    return View();
                }
                await _roleManager.CreateAsync(new IdentityRole{
                    Name=group.GroupName+"üye"
                });
                await _userManager.AddToRoleAsync(user,group.GroupName+"üye");
                _blogContext.SaveChanges();
                return RedirectToAction("Detail","Group",new{id=groupId});
            }
            else{
                ModelState.AddModelError("","Sisteme kayıtlı kullanıcı bulunamadı");
                return View();
            }
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult AddMember(int id){
            ViewBag.GroupId = id;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Detail(int id){
            //gruba üye olmayan erişemesin
            var group = _groupRepository.groups.FirstOrDefault(x=>x.GroupId==id);
            var currentUser=await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)??"");
           
            if(await _userManager.IsInRoleAsync(currentUser,group.GroupName+"üye") == true || await _userManager.IsInRoleAsync(currentUser,group.GroupName+"admin")==true){
           
            var users=_userManager.Users;
            var groupposts= _postRepository.posts.Include(x=>x.user).Where(x=>x.GroupId==id).ToList();
            foreach(var i in _blogContext.groupMembers){
                //gruba ait üyeleri ekler
                if(id==i.groupId){
                    var user= users.FirstOrDefault(x=>x.Id==i.userId);
                    group.GroupUsers.Add(user);
                }
            }
            var viewModel = new GroupDetailViewModel{
                group = group,
                posts=groupposts,
                users = group.GroupUsers,
            };

           
            var adminUser =await _userManager.GetUsersInRoleAsync(group.GroupName+"admin");
            if(adminUser.FirstOrDefault() == currentUser){
                viewModel.isCurrUserA_Admin=true;
            }
            return View(viewModel);
            }
            else{
                return RedirectToAction("Index","Group");
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int groupId){
            var group =_groupRepository.groups.FirstOrDefault(x=>x.GroupId==groupId);
            if(group !=null){
                var posts= _postRepository.posts.Where(x=>x.GroupId==group.GroupId);
                foreach (var post in posts)
                {
                    _postRepository.DeletePost(new PostEditViewModel{
                        PostId =post.PostId,
                    });
                }
                _groupRepository.deleteGroup(group);
                var roles= _roleManager.Roles.Where(x=>x.Name.Contains(group.GroupName));
                foreach (var role in roles)
                {
                    var result = await _roleManager.DeleteAsync(role);
                    if(!result.Succeeded){
                        return NotFound();
                    }
                }
                return RedirectToAction("Index","Group");
            }
            else
                return RedirectToAction("Detail","Group",new{id=group.GroupId});

        }
        [Authorize]
        public IActionResult Members(int id){
            var members = _blogContext.groupMembers.Include(x=>x.user).Where(x=>x.groupId==id).ToList();
             ViewBag.admin=_groupRepository.groups.FirstOrDefault(x=>x.GroupId==id)?.user.Id;
            return View(members);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveMember(string id,int grupId,string grupName){
            var member= await _blogContext.groupMembers.Where(x=>x.userId==id && x.groupId==grupId).FirstOrDefaultAsync();
            if(member !=null){
                _blogContext.groupMembers.Remove(member);
                _blogContext.SaveChanges();
                var posts= _postRepository.posts.Include(x=>x.grup).Include(x=>x.user).Where(x=>x.GroupId==grupId).ToList();
                foreach (var item in posts)
                {
                    if(item.UserId==id){
                        _postRepository.DeletePost(new PostEditViewModel{GroupId=item.GroupId,PostId=item.PostId});

                    }
                }
                
                var role= _roleManager.Roles.Where(x=>x.Name.Contains(grupName+"üye")).FirstOrDefault();
                if(role!=null){
                var result = await _roleManager.DeleteAsync(role);
                    
                }
                    
            
            }
            return RedirectToAction("Detail","Group",new{id=grupId});
        }
        public async Task< IActionResult> Requests(int id){
            var requests = await _groupService.GetMembershipRequests(id);
            return View(requests);
        }
    }
}