using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data
{
    public enum status{
        accepted,rejected,waiting
    }
    public class GroupMemberShipRequests
    {
        [Key]
        public int requestId { get; set; }
        
        [Required]
        public string? UserId { get; set; }
        public AppUser user { get; set; }

        public Group group { get; set; }
        [Required]
        public int GroupId { get; set; }

        public DateTime date { get; set; }

        public status stat { get; set; }
    }
}