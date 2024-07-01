using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlogApp.Controllers;
using BlogApp.Data;

namespace BlogApp.Repositories.Abstract
{
    public interface IGroupService
    {
        Task<Data.Group> GetGroupById(int groupId);
        Task<bool> IsMember(int userId, int groupId);
        Task CreateMembershipRequest(GroupMemberShipRequests request);
        Task<List<GroupMemberShipRequests>> GetMembershipRequests(int groupId);
        Task ApproveMembershipRequest(int requestId);
        Task RejectMembershipRequest(int requestId);

        Task<List<BlogApp.Data.Group>> GetGroups();
        Task<List<GroupMembers>> GetMembers();

        Task<List<Data.Group>> GetGroupsforUser(string userId);
        
    }
}