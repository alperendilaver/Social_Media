using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data
{
    public class AppUser:IdentityUser{

        
        public List<Group>? UserGroups = new List<Group>();
    }
}