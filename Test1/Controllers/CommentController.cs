using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using Test1.Models;
using Test1.ViewModels;

namespace Test1.Controllers
{

    public class CommentController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ApplicationContext DbContext;

        public CommentController(ApplicationContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            this.DbContext = context;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> CommentList(int id)
        {
            return PartialView(await DbContext.Commetns.Where(t => t.ArticleId == id).ToListAsync());
        }

        public async Task<IActionResult> CommentListUser(string id)
        {
            return PartialView(await DbContext.Commetns.Where(t => t.UserId == id).ToListAsync());
        }

        [HttpPost]
        public async Task<string> Create(CreateCommentViewModel model)
        {
            if (model != null)
            {
                СreatureArticle art = await DbContext.СreatureArticles.FindAsync(model.Id);
                User user = await userManager.FindByIdAsync(GetUserId());
                if(art == null || user == null || model.Text == null)
                {
                    return "Коментарий не может быть создан";
                }
                Comment com = new Comment()
                {
                    Text = model.Text,
                    ArticleId = model.Id,
                    article = art,
                    UserId = GetUserId(),
                    user = user,
                    DataTime = DateTime.Now.ToString()
                };


                DbContext.Commetns.Add(com);
                await DbContext.SaveChangesAsync();
            }
            return "Коментарий создан";
        }
        [HttpPost]
        public async Task<string> DeleteComment(int id)
        {
            if (id != null)
            {
                Comment com = await DbContext.Commetns.FindAsync(id);

                DbContext.Commetns.Remove(com);
                await DbContext.SaveChangesAsync();
                return "Коментарий удален";
            }
            return "False";
        }

        public string GetUserId()
        {
            return httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }
 }
