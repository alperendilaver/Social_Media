using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data;
using BlogApp.Repositories.Abstract;
using BlogApp.Repositories.Concreate.EfCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{

    public class GroupMemberShipRequest : Controller
    {
        private BlogContext _blogContext;
        private readonly IGroupService _groupService;

        private UserManager<AppUser> _userManager;
        private IGroupRepository _groupRepository;
        RoleManager<IdentityRole> _roleManager;
        public GroupMemberShipRequest(IGroupService group,BlogContext blogContext,UserManager<AppUser> userManager,IGroupRepository groupRepository,RoleManager<IdentityRole> roleManager)
        {
            _groupService=group;
            _blogContext=blogContext;
            _userManager =userManager;
            _groupRepository=groupRepository;
            _roleManager=roleManager;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create( GroupMemberShipRequests request)
        {
            
            var membershipRequest = new GroupMemberShipRequests
            {
                GroupId = request.GroupId,
                UserId = request.UserId,
                date = DateTime.UtcNow,
                stat = status.waiting
            };
            await _groupService.CreateMembershipRequest(membershipRequest); 
            return RedirectToAction("Index","Group");

        }
        [HttpPost]
        public async Task <IActionResult> AcceptRequest(int reqId,int grpId){
            var request= _blogContext.GroupMemberShipRequests.Include(x=>x.user).Include(x=>x.group).FirstOrDefault(x=>x.requestId==reqId);
            if(request!=null){
            await _groupService.
            ApproveMembershipRequest(reqId);
            
            if(!_blogContext.groupMembers.Any(x=>x.groupId==grpId && x.userId==request.user.Id) && grpId!=null)
            {
                _blogContext.groupMembers.Add(new GroupMembers{
                    groupId=grpId,
                    userId=request.user.Id
                });
                _blogContext.SaveChanges();
            _blogContext.Entry(request.user).State = EntityState.Detached;

             if(!await _roleManager.RoleExistsAsync(request.group.GroupName+"üye")){
                    await _roleManager.CreateAsync(new IdentityRole{Name=request.group.GroupName+"üye"});
            }      
        
                return RedirectToAction("AddRole","Roles",new{
                    appUserId = request.user.Id,
                    name=request.group.GroupName+"üye",
                    groupId= grpId
                });
            }
                return NotFound();
            }
            else{
                return NotFound();}
        }
        [HttpPost]
        public async Task< IActionResult> RejectRequest(int reqId){
            var request =_blogContext.GroupMemberShipRequests.FirstOrDefault(x=>x.requestId==reqId);
            if(request!=null){
                await _groupService.RejectMembershipRequest(reqId);
                return RedirectToAction("Index","Group");
            }
            return NotFound();
        }
        
    }
}
