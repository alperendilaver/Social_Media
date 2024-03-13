using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlogApp.Models;
using BlogApp.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using BlogApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers;

public class HomeController : Controller
{
    private IPostRepository _postRepository;
    private UserManager<AppUser> _userManager;
    private ICommentRepository _commentRepository;
    public HomeController(IPostRepository postRepository,UserManager<AppUser> userManager,ICommentRepository commentRepository)
    {
        _postRepository=postRepository;
        _userManager = userManager;
        _commentRepository = commentRepository;
    }
    public IActionResult Index()
    {
        var user= _userManager.Users.ToList();
        var post=_postRepository.posts.Include(x=>x.user).ToList();
        var comment=_commentRepository.comments.Include(x=>x.user).Include(x=>x.post).ToList();
        return View(new PostIndexViewModel{
            posts=post,
            users=user,
            comments=comment
        });
    }
}
