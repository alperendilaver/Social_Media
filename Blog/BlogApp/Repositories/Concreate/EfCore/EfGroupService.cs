using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlogApp.Controllers;
using BlogApp.Data;
using BlogApp.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories.Concreate.EfCore
{
    public class EfGroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMembershipRequestRepository _membershipRequestRepository;
        private BlogContext _blogContext;
        public EfGroupService(IGroupRepository groupRepository,IMembershipRequestRepository membershipRequestRepository,BlogContext blogContext)
        {
            _groupRepository=groupRepository;
            _membershipRequestRepository = membershipRequestRepository;
            _blogContext=blogContext;
        }
        public async Task ApproveMembershipRequest(int requestId)
        {
            await _membershipRequestRepository.ApproveRequestAsync(requestId);
        }

        public async Task CreateMembershipRequest(GroupMemberShipRequests request)
        {
            await _membershipRequestRepository.CreateAsync(request);
        }

        public Task<Data.Group> GetGroupById(int groupId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Data.Group>> GetGroups()
        {
            return await _blogContext.Groups.ToListAsync();
        }

        public async Task<List<Data.Group>> GetGroupsforUser(string userId)
        {
            var query = await _blogContext.groupMembers.Where(x => x.userId == userId).Select(x=>x.group).ToListAsync();
            return query;
        }

        public async Task<List<GroupMembers>> GetMembers()
        {
            return await _blogContext.groupMembers.ToListAsync();
        }

        public Task<bool> IsMember(int userId, int groupId)
        {
            throw new NotImplementedException();
        }

        public async Task RejectMembershipRequest(int requestId)
        {
           await _membershipRequestRepository.RejectRequestAsync(requestId);
        }

        async Task<List<GroupMemberShipRequests>> IGroupService.GetMembershipRequests(int groupId)
        {
            return await _membershipRequestRepository.GetRequestsAsync(groupId);
        }
    }
}