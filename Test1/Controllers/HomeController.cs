using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Test1.CustomPolicy;
using Test1.Models;
using Test1.ViewModels;

namespace Test1.Controllers
{
    [CheckIsAllData]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ApplicationContext _context;

        public HomeController(ApplicationContext _context, ILogger<HomeController> logger, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this._context = _context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Index()
        {

            if(_context.Mythologies.ToList().Count > 4)
            {
                return View(_context.Mythologies.ToList().Take(4));
            }
            return View(_context.Mythologies.ToList());
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Main(List<string> Filters = null, int page = 1, string searchresult = null)
        {
            var Articles_ = _context.СreatureArticles.ToList();
            List<string> MythologiesList = new List<string>();
            List<string> DangreTypesList = new List<string>();
            List<string> CreatureTypesList = new List<string>();
            List<string> SexList = new List<string>();
            string[] str = new string[1];

            if(searchresult != null)
            {
                Filters.Clear();

                Articles_ = _context.СreatureArticles.Where(t => t.Name.ToLower().Contains(searchresult.ToLower())).ToList();
            }

            if(Filters.Count == 1)
            {
                string[] tempListFilters = Filters[0].Split("&Filters=");

                if(tempListFilters.Length > 1)
                {
                    Filters = tempListFilters.ToList();
                }

            }

            if (Filters.Count() != 0)
            {

                var ResFilters = new List<СreatureArticle>();
                var TempFilters = new List<СreatureArticle>();

                foreach (var item in _context.Mythologies)
                {
                    if (Filters.Contains(item.Name))
                        MythologiesList.Add(item.Name);
                }
                foreach (var item in _context.DangerTypes)
                {
                    if (Filters.Contains(item.Name))
                        DangreTypesList.Add(item.Name);
                }
                foreach (var item in _context.CreatureTypes)
                {
                    if (Filters.Contains(item.Name))
                        CreatureTypesList.Add(item.Name);
                }
                foreach (var item in _context.Sexes)
                {
                    if (Filters.Contains(item.Name))
                        SexList.Add(item.Name);
                }

                if (MythologiesList.Count() == 0)
                {
                    foreach (var item in _context.Mythologies.ToList())
                    {
                        MythologiesList.Add(item.Name);
                    }
                }

                foreach (string item in MythologiesList)
                {
                    ResFilters.AddRange(Articles_.Where(i => i.Mythology.Name == item));
                }

                if (MythologiesList.Count() != 0 && ResFilters.Count != 0)
                {

                    Articles_ = ResFilters;
                    if (DangreTypesList.Count() != 0)
                    {
                        if (ResFilters.Count() == 0)
                        {
                            foreach (string item in DangreTypesList)
                            {
                                TempFilters.AddRange(Articles_.Where(i => i.dangerType.Name == item));
                            }
                        }
                        else
                        {
                            foreach (string item in DangreTypesList)
                            {
                                TempFilters.AddRange(ResFilters.Where(i => i.dangerType.Name == item));
                            }
                        }
                        ResFilters = TempFilters;
                        TempFilters = new List<СreatureArticle>();
                    }
                    if (CreatureTypesList.Count() != 0)
                    {
                        if (ResFilters.Count() == 0)
                        {
                            foreach (string item in CreatureTypesList)
                            {
                                TempFilters.AddRange(Articles_.Where(i => i.CreatureType.Name == item));
                            }
                        }
                        else
                        {
                            foreach (string item in CreatureTypesList)
                            {
                                TempFilters.AddRange(ResFilters.Where(i => i.CreatureType.Name == item));
                            }
                        }
                        ResFilters = TempFilters;
                        TempFilters = new List<СreatureArticle>();
                    }
                    if (SexList.Count() != 0)
                    {
                        if (ResFilters.Count() == 0)
                        {
                            foreach (string item in SexList)
                            {
                                TempFilters.AddRange(Articles_.Where(i => i.Sex.Name == item));
                            }
                        }
                        else
                        {
                            foreach (string item in SexList)
                            {
                                TempFilters.AddRange(ResFilters.Where(i => i.Sex.Name == item));
                            }
                        }
                        ResFilters = TempFilters;
                        TempFilters = new List<СreatureArticle>();
                    }
                }
                Articles_ = ResFilters.ToList();
            }


            _context.СreatureArticles.Include(prop => prop.Pictures).ToList();
            int count = Articles_.Count();
            int itemsPerPage = 6;
            Articles_ = Articles_.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();





            FiltersViewModel filtersViewModel = new FiltersViewModel();
            filtersViewModel.creatureTypes = _context.CreatureTypes.ToList();
            filtersViewModel.dangerTypes = _context.DangerTypes.ToList();
            filtersViewModel.mythologies = _context.Mythologies.ToList();
            filtersViewModel.sexes = _context.Sexes.ToList();
            filtersViewModel.Cheked = Filters;

            CreatureListViewModel viewModel = new CreatureListViewModel()
            {
                Articles = Articles_,
                DangerType = new SelectList(_context.DangerTypes.ToList(), nameof(DangerType.Id), nameof(DangerType.Name)),
                Sex = new SelectList(_context.Sexes.ToList(), nameof(Sex.Id), nameof(Sex.Name)),
                Mythology = new SelectList(_context.Mythologies.ToList(), nameof(Mythology.Id), nameof(Mythology.Name)),
                articleState = new SelectList(_context.ArticleStates.ToList(), nameof(ArticleState.Id), nameof(ArticleState.Name)),
                CreatureType = new SelectList(_context.CreatureTypes.ToList(), nameof(CreatureType.Id), nameof(CreatureType.Name)),
                CreaturePageViewModel = new CreaturePageViewModel(count, itemsPerPage) { pageNumber = page },
                filtersMain = filtersViewModel
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Article(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            СreatureArticle article = await _context.СreatureArticles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            if (!signInManager.Context.User.IsInRole("Admin") && article.articleStateId != 3)
            {
                return NotFound();
            }


            await _context.Entry(article).Reference(s => s.Sex).LoadAsync();
            await _context.Entry(article).Reference(d => d.dangerType).LoadAsync();
            await _context.Entry(article).Reference(m => m.Mythology).LoadAsync();
            await _context.Entry(article).Reference(c => c.CreatureType).LoadAsync();
            await _context.Entry(article).Collection(p => p.Pictures).LoadAsync();

            return View(article);
            //return View(_context.СreatureArticles.Where(t=> t.).ToList());
        }

        public IActionResult Categories()
        {
            FiltersViewModel filtersViewModel = new FiltersViewModel();
            filtersViewModel.creatureTypes = _context.CreatureTypes.ToList();
            filtersViewModel.dangerTypes = _context.DangerTypes.ToList();
            filtersViewModel.mythologies = _context.Mythologies.ToList();
            filtersViewModel.sexes = _context.Sexes.ToList();
            filtersViewModel.Cheked = null;
            return View(filtersViewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}