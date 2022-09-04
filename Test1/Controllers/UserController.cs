using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using Test1.CustomPolicy;
using Test1.Models;
using Test1.ViewModels;


namespace Test1.Controllers
{
    public class UserController : Controller
    {

        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ApplicationContext _context;

        public UserController(ApplicationContext _context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.webHostEnvironment = webHostEnvironment;
            this._context = _context;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index() => View(await userManager.Users.ToListAsync());

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { AboutMe = null, UserName = model.Name, Email = model.Email, Year = model.Year };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound();

            }
            EditUserViewModel userModel = new EditUserViewModel { Id = user.Id, Name = user.UserName, Email = user.Email, Year = user.Year };
            return View(userModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByIdAsync(model.Id);

                user.Email = model.Email;
                user.UserName = model.Name;
                user.Year = model.Year;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            User user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction("Index");

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                if(GetUserId() != id)
                {
                    return BadRequest();
                }
                EditPasswordViewModel model = new EditPasswordViewModel { Id = id, Name = user.UserName };
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(EditPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByIdAsync(model.Id);

                var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);


                if (result.Succeeded)
                {
                    var useres = await userManager.UpdateAsync(user);
                    if (useres.Succeeded)
                    {
                        return RedirectToAction("Index","Home");
                    }

                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }

            return View(model);
        }


        [HttpPost]
        [Authorize]
        public async Task<string> ChangeYear(ChangeYearViewModel model)
        {


            User user = await userManager.FindByIdAsync(model.id);


            if(model.year >= 1950 && model.year <= 2022)
            {
                user.Year = model.year;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return model.year.ToString();
                }
            }

            return "0";
        }


        [HttpPost]
        [Authorize]
        public async Task<string> ChangeAbout(ChangeAboutMeViewModel model)
        {


            User user = await userManager.FindByIdAsync(model.id);


            if (model.aboutme.Length <= 500)
            {
                user.AboutMe = model.aboutme;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return model.aboutme;
                }
            }

            return "0";
        }

        [HttpPost]
        [Authorize]
        public async Task<string> ChangeAvatar(ChangeuserAvatarViewModel model)
        {
            string id = model.id;
            if(id == null)
            {
                return "0";
            }
            User user = await userManager.FindByIdAsync(id);
            if(user == null)
            {
                return "0";
            }
            string LasrAvatar = user.PhotoUrl;


            IFormFile img = model.file;

            string picturePath = "material/Avatars/Avatar" + img.FileName;
            using(FileStream fs = new FileStream(Path.Combine(webHostEnvironment.WebRootPath, picturePath), FileMode.Create))
            {
                await img.CopyToAsync(fs);
                user.NameFile = img.FileName;
                user.PhotoUrl = "/" + picturePath;
            }
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                if (LasrAvatar != null)
                {
                    System.IO.File.Delete(webHostEnvironment.WebRootPath + LasrAvatar);
                }
                return user.PhotoUrl;
            }

            return "0";
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInfo(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await userManager.FindByIdAsync(id);

            if(user == null)
            {
                return BadRequest();
            }
            return View(user);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserSettings(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (GetUserId() != id)
            {
                return BadRequest();
            }
            User user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return BadRequest();
            }
            return View(user);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserLikes(int page = 1)
        {
            var Like = _context.UserLikes.Where(t => t.UserId == GetUserId()).ToList();

            var Articles_ = _context.СreatureArticles.ToList();
            var list = new List<СreatureArticle>();

            foreach(var Article in Like)
            {
                foreach(var item in Articles_)
                {
                    if(item.Id == Article.ArticleId)
                    {
                        list.Add(item);
                        break;
                    }
                }
            }


            Articles_ = list;
            _context.СreatureArticles.Include(prop => prop.Pictures).ToList();
            int count = Articles_.Count();
            int itemsPerPage = 8;
            Articles_ = Articles_.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();



            CreatureListViewModel viewModel = new CreatureListViewModel()
            {
                Articles = Articles_,
                DangerType = new SelectList(_context.DangerTypes.ToList(), nameof(DangerType.Id), nameof(DangerType.Name)),
                Sex = new SelectList(_context.Sexes.ToList(), nameof(Sex.Id), nameof(Sex.Name)),
                Mythology = new SelectList(_context.Mythologies.ToList(), nameof(Mythology.Id), nameof(Mythology.Name)),
                articleState = new SelectList(_context.ArticleStates.ToList(), nameof(ArticleState.Id), nameof(ArticleState.Name)),
                CreatureType = new SelectList(_context.CreatureTypes.ToList(), nameof(CreatureType.Id), nameof(CreatureType.Name)),
                CreaturePageViewModel = new CreaturePageViewModel(count, itemsPerPage) { pageNumber = page },
                filtersMain = null
            };

            return View(viewModel);
        }




        [HttpPost]
        [Authorize]
        public async Task<string> LikeArticle(int Id)
        {
            if (Id != null)
            {
                СreatureArticle art = await _context.СreatureArticles.FindAsync(Id);
                User user = await userManager.FindByIdAsync(GetUserId());
                if (art == null || user == null)
                {
                    return "";
                }
                UserLikes com = new UserLikes()
                {
                    ArticleId = Id,
                    article = art,
                    UserId = GetUserId(),
                    user = user
                };

                _context.UserLikes.Add(com);
                await _context.SaveChangesAsync();
            }
            return "true";
        }


        [HttpPost]
        [Authorize]
        public async Task<string> RemoveLikeArticle(int Id)
        {
            if (Id != null)
            {
                UserLikes art = _context.UserLikes.Where(t=> t.ArticleId == Id && t.UserId == GetUserId()).FirstOrDefault();
                if(art != null)
                {
                    _context.UserLikes.Remove(art);
                }
                await _context.SaveChangesAsync();
            }
            return "true";
        }
        public string GetUserId()
        {
            return httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }



    }
}
