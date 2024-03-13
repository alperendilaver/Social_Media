using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Data
{
    public class Reaction
    {
        public int Id { get; set; }

        public required string UserId { get; set; } 
        public AppUser user { get; set; }
    
        public required int PostId { get; set; }
        public Post post { get; set; }

        public Reactions reaction { get; set; }

    }
}