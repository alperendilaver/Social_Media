using BlogApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlogApp.Repositories.Concreate.EfCore
{
    public class BlogContext:DbContext{
        public BlogContext(DbContextOptions<BlogContext> options):base(options)
        {
            
        }
        
        public DbSet<AppUser> AppUser => Set<AppUser>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> comments =>Set<Comment>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<GroupMembers> groupMembers =>Set<GroupMembers>();
        public DbSet<Reaction> reactions =>Set<Reaction>();
        public DbSet<GroupMemberShipRequests> GroupMemberShipRequests => Set<GroupMemberShipRequests>();
    }
}