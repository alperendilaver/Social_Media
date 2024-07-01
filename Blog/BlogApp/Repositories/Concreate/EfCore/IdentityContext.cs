using BlogApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories.Concreate.EfCore
{
    public class IdentityContext:IdentityDbContext<AppUser,IdentityRole,string>{
        public IdentityContext  (DbContextOptions<IdentityContext> options):base(options)
        {
            
        }
    }
}