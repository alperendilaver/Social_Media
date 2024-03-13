using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Controllers;
using BlogApp.Data;
using BlogApp.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories.Concreate.EfCore
{
    public class EfMembershipRequestRepository : IMembershipRequestRepository
    {

        private readonly BlogContext _context;
        public EfMembershipRequestRepository(BlogContext context)
        {
            _context=context;
        }

        public async Task ApproveRequestAsync(int requestId)
        {
            var req = await _context.GroupMemberShipRequests.FirstOrDefaultAsync(x=>x.requestId==requestId);
            if(req!=null){
                _context.Remove(req);
                _context.SaveChanges();
            }
        }

        public async Task CreateAsync(GroupMemberShipRequests request)
        {
            await _context.GroupMemberShipRequests.AddAsync(request);
            _context.SaveChanges();
        }

        public async Task<List<GroupMemberShipRequests>> GetRequestsAsync(int groupId)
        {
           return await _context.GroupMemberShipRequests.Include(x=>x.user).Where(x=>x.GroupId==groupId).ToListAsync();
        }

        public async Task RejectRequestAsync(int requestId)
        {
            var request=_context.GroupMemberShipRequests.FirstOrDefault(x=>x.requestId==requestId);
            _context.Remove(request);
            await _context.SaveChangesAsync();
        }
    }
}