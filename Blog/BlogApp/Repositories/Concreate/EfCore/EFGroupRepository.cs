using BlogApp.Data;
using BlogApp.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories.Concreate.EfCore
{
    public class EFGroupRepository : IGroupRepository
    {
        BlogContext _context;
        public EFGroupRepository(BlogContext blogContext)
        {
            _context=blogContext;
        }
        public IQueryable<Group> groups => _context.Groups;
        public IQueryable<GroupMembers> groupMembers  => _context.groupMembers;

        public void createGroup(Group group)
        {
            _context.Add(group);
            _context.SaveChanges();
        }

        void IGroupRepository.deleteGroup(Group group)
        {
            _context.Remove(group);
            _context.SaveChanges();
            if(group.image!=null){
                var imagePath=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",group.image);
                if(File.Exists(imagePath))
                    File.Delete(imagePath);
                }
            }
        }
    }
