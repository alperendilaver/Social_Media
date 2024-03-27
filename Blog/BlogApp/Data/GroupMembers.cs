using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data
{
    public class GroupMembers{
        [Key]
        public int memberId { get; set; }
        public string? userId { get; set; }
        public AppUser user { get; set; }
        
        public int groupId { get; set; }
        public Group group { get; set; }
    }
}