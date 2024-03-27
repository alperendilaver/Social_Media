using BlogApp.Data;

namespace BlogApp.Repositories.Abstract{
    public interface IUserRepository{
        IQueryable<AppUser> users {get;}

        void addGroup(AppUser user,Group group);
    }
}