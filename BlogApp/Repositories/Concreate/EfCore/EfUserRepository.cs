using BlogApp.Data;
using BlogApp.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories.Concreate.EfCore
{
    public class EfUserRepository : IUserRepository
    {
        BlogContext _blogContext;
        public EfUserRepository(BlogContext blogContext)
        {
            _blogContext =blogContext;
        }
        public IQueryable<AppUser> users => _blogContext.AppUser;

        public void addGroup(AppUser user, Group group)
        {
           var curuser = users.FirstOrDefault(x=>x.Id==user.Id);
           if(curuser!=null){
             user.UserGroups.Add(group);
             _blogContext.SaveChanges();
           }
        }
    }
}