using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Controllers;
using BlogApp.Data;

namespace BlogApp.Models
{
    public class GroupRequestsViewModel
    {
        public List<GroupMemberShipRequests> request =new List<GroupMemberShipRequests>();
        public AppUser user { get; set; } = null!;

    }
}