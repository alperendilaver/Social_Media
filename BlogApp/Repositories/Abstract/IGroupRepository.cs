using BlogApp.Data;

namespace BlogApp.Repositories.Abstract
{
    public interface IGroupRepository{
        IQueryable<Group> groups {get;}
        IQueryable<GroupMembers> groupMembers {get;}
        void createGroup(Group group);
        void deleteGroup(Group group);
    }
}