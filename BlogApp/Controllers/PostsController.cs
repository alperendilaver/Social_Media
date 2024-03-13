using System.Security.Claims;
using BlogApp.Data;
using BlogApp.Models;
using BlogApp.Repositories.Abstract;
using BlogApp.Repositories.Concreate.EfCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApp.Controllers
{
    public class PostsController:Controller{
        private readonly BlogContext _blogContext;
         private UserManager<AppUser> _userManager;
               
        private IPostRepository _postRepository;
        private ICommentRepository _commentRepository;
        public PostsController(BlogContext blogContext,IPostRepository postRepository,UserManager<AppUser> userManager,ICommentRepository commentRepository)
        {
            _blogContext=blogContext;
            _postRepository=postRepository;
            _userManager = userManager;
            _commentRepository=commentRepository;
        }
          

      
        public IActionResult CreatePost(string? id,bool? isGrupPost,int? grpId ){
            //yönlendirme gruptan geldiyse grup seçtirme
            if(!User.Identity.IsAuthenticated){
                return RedirectToAction("Login","Users");
            }     
            var user= _userManager.Users.FirstOrDefault(u=>u.Id==User.FindFirstValue(ClaimTypes.NameIdentifier));
            var groups=_blogContext.Groups;
            foreach (var item in _blogContext.groupMembers)
            {
                if(item.userId==user.Id){
                    var group=groups.Find(item.groupId);
                    user.UserGroups.Add(group);
                }
            }
            ViewBag.isGroupPost= isGrupPost;
            if(id!=null)
                ViewBag.groupId=grpId;
            ViewBag.UserGroups = new SelectList(user.UserGroups.ToList(),"GroupId","GroupName");
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> CreatePost(PostCreateViewModel model){
            ModelState.Remove("GroupId");
            if(ModelState.IsValid){
                var currentUserName = User.FindFirstValue(ClaimTypes.Name);
                var currentUser = await _userManager.FindByNameAsync(currentUserName ?? "");
                if(currentUser!=null){
                    if(model.imageFile!=null){
                        var extension = Path.GetExtension(model.imageFile.FileName);
                        var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                        var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",randomFileName);
                        using (var stream = new FileStream(path,FileMode.Create))
                        {
                            await model.imageFile.CopyToAsync(stream);
                        }
                        model.image=randomFileName;
                    }
                    var post=new Post{
                    UserId = currentUser.Id,
                    Context = model.Context,
                    Published = DateTime.Now,
                    image = model.image,
                    GroupId=model.GroupId
                    };
                _postRepository.CreatePost(post);
                if(post.GroupId!=null)
                    return  RedirectToAction("Detail","Group",new {id=post.GroupId});
                else
                    return RedirectToAction("Index","Home");
                }
                
            }
            return View(model);
        }
        public async Task< IActionResult> Detail(int id){
            var post = await _postRepository.posts.
            FirstOrDefaultAsync(x=>x.PostId == id);
            if(post!=null){
                var userId=post.UserId;
                var user = await _userManager.FindByIdAsync(userId??"");
                if(user!=null){
                var comment=_commentRepository.comments.Include(x=>x.user).Include(x=>x.post).ToList();
                var reactions = _blogContext.reactions.Where(x=>x.PostId==id).ToList();
                ViewBag.likeCount = reactions.Where(x=>x.reaction==Reactions.like).Count();
                
                ViewBag.unlikeCount = reactions.Where(x=>x.reaction==Reactions.unlike).Count();
                
                ViewBag.angryCount = reactions.Where(x=>x.reaction==Reactions.angry).Count();
                
                ViewBag.congrCount = reactions.Where(x=>x.reaction==Reactions.congr).Count();

               var currUserId= User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userReaction = _blogContext.reactions.Where(x=>x.UserId==currUserId && x.PostId==post.PostId).FirstOrDefault();

                var viewModel = new PostDetailViewModel{
                    userId=userId,
                    user = user,
                    post = post,
                    postId=id,
                    comments=comment,
                    reactions = reactions,
                    userReaction = userReaction
                };
                return View(viewModel);
                }
            }
            return RedirectToAction("Index",
            "Home");
        }
        [Authorize]
        public async Task< IActionResult> Edit(PostEditViewModel model,int id){
            var post = await _postRepository.posts.Include(x=>x.user).
            FirstOrDefaultAsync(x=>x.PostId == id);
            var claimuser=User.FindFirstValue(ClaimTypes.Name);
            if(post.user.UserName!=claimuser){
                return RedirectToAction("Index","Home");
            }
            if(post!=null){
                var userId=post.UserId;
                var user = await _userManager.FindByIdAsync(userId??"");
                if(user!=null){

                var viewModel = new PostEditViewModel{
                    Context =post.Context,
                    PostId = post.PostId,
                    image=post.image,
                    GroupId = post.GroupId
                };
                return View(viewModel);
                }
            }
            return RedirectToAction("Index",
            "Home");
              
        }
        [HttpPost]
        public async Task< IActionResult> Edit(PostEditViewModel model){
            if(model!=null && _postRepository.posts.Any(x=>x.PostId==model.PostId)){
                    var oldPost= await _postRepository.posts.FirstOrDefaultAsync(x=>x.PostId==model.PostId); 
                    if(oldPost!=null){
                      if(model.imageFile!=null && model.deletePhoto==false){
                        if(oldPost.image!=null && oldPost.image !=model.image){
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", oldPost.image);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        var extension = Path.GetExtension(model.imageFile.FileName);
                        var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                        var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",randomFileName);
                        using (var stream = new FileStream(path,FileMode.Create))
                        {
                            await model.imageFile.CopyToAsync(stream);
                        }
                        model.image=randomFileName;
                        }else if(model.deletePhoto==false&&model.imageFile==null ){//foto güncellememe
                            model.image=oldPost.image;
                        }
                        else if(model.deletePhoto==true &&model.imageFile==null){//foto kaldırma
                            model.image=null;
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", oldPost.image);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        else if(model.deletePhoto==true&& model.imageFile!=null){
                        ModelState.AddModelError("","Geçersiz işlem");
                        model.image=oldPost.image;
                        model.PostId = oldPost.PostId;
                        return View(model);
                        }
                    }
                    _postRepository.EditPost(model);
                    if(model.GroupId!=null)
                        return RedirectToAction("Detail","Group",new{id=model.GroupId});
                    return RedirectToAction("Profile","Users");
                }
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(PostEditViewModel model){
            _postRepository.DeletePost(model);
            if(model.GroupId!=null)
                return RedirectToAction("Detail","Group",new {id = model.GroupId});
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public JsonResult AddComment(int postId,string userId,string text){

            var userName = User.FindFirstValue(ClaimTypes.Name);
            var entity = new Comment{
                postId=postId,
                userId=userId,
                Text = text
            };
            _commentRepository.createComment(entity);
                return  Json(new{
                    userName,
                    text,
                    postId,
                    userId
                });
            }
            [HttpPost]
            public async Task< IActionResult> AddReaction(string userId,int postId,int reactionId){
                await _postRepository.AddReaction(userId,
                postId,reactionId);
                return RedirectToAction("Index","Home"); 
            }
            [HttpPost]
            public async Task< JsonResult> RemoveReaction(string UserId,int? postId){
                if(UserId !=null && postId !=null){
                    var reaction = _blogContext.reactions.Where(x=>x.PostId==postId & x.UserId == UserId).FirstOrDefault();
                    await _postRepository.RemoveReaction(reaction);

                }
                return Json(new {});

            }
            [HttpPost]
            public IActionResult RemoveComment(int id){
                var comm =_blogContext.comments.Find(id);
                if(comm!=null){
                    _postRepository.DeleteComment(comm);
                    return RedirectToAction("Detail","Posts", new {id = comm.postId});
                }
                else return NoContent();
            }
        }   
        

    }
