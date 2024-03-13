using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Controllers;
using BlogApp.Data;

namespace BlogApp.Repositories.Abstract
{
    public interface IMembershipRequestRepository
    {
        Task CreateAsync(GroupMemberShipRequests request);
        Task<List<GroupMemberShipRequests>> GetRequestsAsync(int groupId);
        Task ApproveRequestAsync(int requestId);
        Task RejectRequestAsync(int requestId);
    }
}