using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Test1.Models;
using Test1.ViewModels;

namespace Test1.Controllers
{
    public class CreatureController : Controller
    {
        private readonly ApplicationContext DbContext;
        private readonly IWebHostEnvironment hostEnvironment;

        public CreatureController(ApplicationContext context, IWebHostEnvironment hostEnvironment) {
            this.DbContext = context;
            this.hostEnvironment = hostEnvironment;
        }
        public IActionResult Index(int page = 1)
        {
            var Articles_ = DbContext.СreatureArticles.ToList();




            int count = Articles_.Count();
            int itemsPerPage = 10;
            Articles_ = Articles_.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

            CreatureListViewModel viewModel = new CreatureListViewModel()
            {
                Articles = Articles_,
                DangerType = new SelectList(DbContext.DangerTypes.ToList(), nameof(DangerType.Id), nameof(DangerType.Name)),
                Sex = new SelectList(DbContext.Sexes.ToList(), nameof(Sex.Id), nameof(Sex.Name)),
                Mythology = new SelectList(DbContext.Mythologies.ToList(), nameof(Mythology.Id), nameof(Mythology.Name)),
                articleState = new SelectList(DbContext.ArticleStates.ToList(), nameof(ArticleState.Id), nameof(ArticleState.Name)),
                CreatureType = new SelectList(DbContext.CreatureTypes.ToList(), nameof(CreatureType.Id), nameof(CreatureType.Name)),
                CreaturePageViewModel = new CreaturePageViewModel(count, itemsPerPage) { pageNumber = page }
            };

            return View(viewModel);
        }


        [HttpGet]
        public IActionResult Create()
        {
            СreatureArticle article = new СreatureArticle { };
            ViewBag.SexType = new SelectList(DbContext.Sexes, "Id", "Name", article.SexId);
            ViewBag.DangerType = new SelectList(DbContext.DangerTypes, "Id", "Name", article.dangerTypeId);
            ViewBag.MythologyType = new SelectList(DbContext.Mythologies, "Id", "Name", article.MythologyId);
            ViewBag.ArticleState = new SelectList(DbContext.ArticleStates, "Id", "Name", article.articleStateId);
            ViewBag.CreatureType = new SelectList(DbContext.CreatureTypes, "Id", "Name", article.CreatureTypeId);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatureViewModel model)
        {
            СreatureArticle article = new СreatureArticle { };
            if (ModelState.IsValid)
            {
                article.Name = model.Name;
                article.Content = model.Content;
                article.Etymology = model.Etymology;
                article.AuthorComment = model.AuthorComment;
                article.articleStateId = model.articleStateId;
                article.Literature = model.Literature;
                article.SexId = model.SexId;
                article.dangerTypeId = model.dangerTypeId;
                article.MythologyId = model.MythologyId;
                article.CreatureTypeId = model.CreatureTypeId;
                article.dataCreate = DateTime.Now.Date;
                DbContext.СreatureArticles.Add(article);
                List<Picture> picturesToArticle = new List<Picture>();
                if(model.Pictures != null)
                {
                    foreach (IFormFile img in model.Pictures)
                    {
                        string path = Path.Combine("material/pictures", img.FileName);
                        using (FileStream fs = new FileStream(Path.Combine(hostEnvironment.WebRootPath, path), FileMode.Create))
                        {
                            await img.CopyToAsync(fs);
                            Picture pic = new Picture
                            {
                                NameFile = img.FileName,
                                ContentType = img.ContentType,
                                PhotoUrl = @"/" + path,
                                CreatureArticle = article
                            };
                            DbContext.Pictures.Add(pic);
                        }

                    }
                }



                await DbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ViewBag.SexType = new SelectList(DbContext.Sexes, "Id", "Name", article.SexId);
            ViewBag.DangerType = new SelectList(DbContext.DangerTypes, "Id", "Name", article.dangerTypeId);
            ViewBag.MythologyType = new SelectList(DbContext.Mythologies, "Id", "Name", article.MythologyId);
            ViewBag.ArticleState = new SelectList(DbContext.ArticleStates, "Id", "Name", article.articleStateId);
            ViewBag.CreatureType = new SelectList(DbContext.CreatureTypes, "Id", "Name", article.CreatureTypeId);
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if(id == null)
            {
                return NotFound();
            }
            СreatureArticle model = await DbContext.СreatureArticles.FindAsync(id);
            if(model == null)
            {
                return NotFound();
            }

            CreatureViewModel article = new CreatureViewModel();
            article.Id = model.Id;
            article.Name = model.Name;
            article.Content = model.Content;
            article.Etymology = model.Etymology;
            article.AuthorComment = model.AuthorComment;
            article.articleStateId = model.articleStateId;
            article.Literature = model.Literature;
            article.SexId = model.SexId;
            article.dangerTypeId = model.dangerTypeId;
            article.MythologyId = model.MythologyId;
            article.CreatureTypeId = model.CreatureTypeId;
            article.PicturesDeleted = await DbContext.Pictures.Where(i => i.CreaturArticleId == article.Id).ToListAsync();

            ViewBag.SexType = new SelectList(DbContext.Sexes, "Id", "Name", article.SexId);
            ViewBag.DangerType = new SelectList(DbContext.DangerTypes, "Id", "Name", article.dangerTypeId);
            ViewBag.MythologyType = new SelectList(DbContext.Mythologies, "Id", "Name", article.MythologyId);
            ViewBag.ArticleState = new SelectList(DbContext.ArticleStates, "Id", "Name", article.articleStateId);
            ViewBag.CreatureType = new SelectList(DbContext.CreatureTypes, "Id", "Name", article.CreatureTypeId);


            return View(article);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(CreatureViewModel model)
        {
            СreatureArticle article = new СreatureArticle { };
            if (ModelState.IsValid)
            {
                article = await DbContext.СreatureArticles.FindAsync(model.Id);


                article.Name = model.Name;
                article.Content = model.Content;
                article.Etymology = model.Etymology;
                article.AuthorComment = model.AuthorComment;
                article.articleStateId = model.articleStateId;
                article.Literature = model.Literature;
                article.SexId = model.SexId;
                article.dangerTypeId = model.dangerTypeId;
                article.MythologyId = model.MythologyId;
                article.CreatureTypeId = model.CreatureTypeId;
                DbContext.Update(article);
                List<Picture> picturesToArticle = new List<Picture>();
                if(model.Pictures != null)
                {
                    foreach (IFormFile img in model.Pictures)
                    {
                        string path = Path.Combine("material/pictures", img.FileName);
                        using (FileStream fs = new FileStream(Path.Combine(hostEnvironment.WebRootPath, path), FileMode.Create))
                        {
                            await img.CopyToAsync(fs);
                            Picture pic = new Picture
                            {
                                NameFile = img.FileName,
                                ContentType = img.ContentType,
                                PhotoUrl = @"/" + path,
                                CreatureArticle = article
                            };
                            DbContext.Pictures.Add(pic);
                        }

                    }
                }
                await DbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ViewBag.SexType = new SelectList(DbContext.Sexes, "Id", "Name", article.SexId);
            ViewBag.DangerType = new SelectList(DbContext.DangerTypes, "Id", "Name", article.dangerTypeId);
            ViewBag.MythologyType = new SelectList(DbContext.Mythologies, "Id", "Name", article.MythologyId);
            ViewBag.ArticleState = new SelectList(DbContext.ArticleStates, "Id", "Name", article.articleStateId);
            ViewBag.CreatureType = new SelectList(DbContext.CreatureTypes, "Id", "Name", article.CreatureTypeId);
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            СreatureArticle art = DbContext.СreatureArticles.Find(id);
            if (art == null)
            {
                return NotFound();
            }
            return View(art);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            СreatureArticle art = DbContext.СreatureArticles.Find(id);
            if (art == null)
            {
                return NotFound();
            }
            foreach(Picture pic in DbContext.Pictures.ToList())
            {
                if(pic.CreaturArticleId == art.Id)
                {
                    System.IO.File.Delete(hostEnvironment.WebRootPath + pic.PhotoUrl);
                }
            }
            DbContext.Pictures.RemoveRange(DbContext.Pictures.Where(x => x.CreaturArticleId == art.Id));
            DbContext.СreatureArticles.Remove(art);
            await DbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<string> DeletePicture(int? id)
        {
            if(id == null)
            {
                return "false";
            }

            Picture pic = await DbContext.Pictures.FindAsync(id);
            if(pic == null)
            {
                return "false";
            }

            string path = hostEnvironment.WebRootPath + pic.PhotoUrl;
            System.IO.File.Delete(path);
            DbContext.Pictures.Remove(pic);
            await DbContext.SaveChangesAsync();

            return "true";
        }
    }
}
