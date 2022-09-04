#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Test1.Models;
using Test1.ViewModels;

namespace Test1.Controllers
{
    public class MythologiesController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public MythologiesController(ApplicationContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: Mythologies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Mythologies.ToListAsync());
        }

        // GET: Mythologies/Details/5
        public async Task<IActionResult> Details(int? id, int page = 1)
        {
            var Articles_ = _context.СreatureArticles.Where(t => t.MythologyId == id).ToList();

            int count = Articles_.Count();
            int itemsPerPage = 10;
            Articles_ = Articles_.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

            FiltersViewModel filters = new FiltersViewModel();
            filters.Cheked.Add(Convert.ToString(id));
            filters.Cheked.Add(_context.Mythologies.Find(id).Name);

            CreatureListViewModel viewModel = new CreatureListViewModel()
            {
                Articles = Articles_,
                DangerType = new SelectList(_context.DangerTypes.ToList(), nameof(DangerType.Id), nameof(DangerType.Name)),
                Sex = new SelectList(_context.Sexes.ToList(), nameof(Sex.Id), nameof(Sex.Name)),
                Mythology = new SelectList(_context.Mythologies.ToList(), nameof(Mythology.Id), nameof(Mythology.Name)),
                articleState = new SelectList(_context.ArticleStates.ToList(), nameof(ArticleState.Id), nameof(ArticleState.Name)),
                CreatureType = new SelectList(_context.CreatureTypes.ToList(), nameof(CreatureType.Id), nameof(CreatureType.Name)),
                CreaturePageViewModel = new CreaturePageViewModel(count, itemsPerPage) { pageNumber = page },
                filtersMain = filters

            };

            return View(viewModel);
        }

        // GET: Mythologies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mythologies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MythologyCreateViewModel model)
        {
            Mythology mythology = new Mythology();
            if (ModelState.IsValid)
            {
                mythology.Name = model.Name;


                IFormFile picture = model.MythologyPicture;
                string PicturePath = "material/CategoryPicture/" + picture.FileName;
                using (FileStream fs = new FileStream(Path.Combine(webHostEnvironment.WebRootPath, PicturePath), FileMode.Create))
                {
                    await picture.CopyToAsync(fs);
                    mythology.NameFile = picture.FileName;
                    mythology.ContentType = picture.ContentType;
                    mythology.PhotoUrl = @"/" + PicturePath;

                }

                _context.Add(mythology);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mythology);
        }

        // GET: Mythologies/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mythology = await _context.Mythologies.FindAsync(id);
            if (mythology == null)
            {
                return NotFound();
            }
            MythologyCreateViewModel model = new MythologyCreateViewModel();
            model.Id = mythology.Id;
            model.Name = mythology.Name;

            return View(model);
        }

        // POST: Mythologies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MythologyCreateViewModel model)
        {
            Mythology mythology = await _context.Mythologies.FindAsync(model.Id);

            if(mythology == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    mythology.Name = model.Name;
                    if(model.MythologyPicture != null)
                    {
                        IFormFile picture = model.MythologyPicture;
                        string PicturePath = "material/CategoryPicture/" + picture.FileName;
                        using (FileStream fs = new FileStream(Path.Combine(webHostEnvironment.WebRootPath, PicturePath), FileMode.Create))
                        {
                            await picture.CopyToAsync(fs);
                            mythology.NameFile = picture.FileName;
                            mythology.ContentType = picture.ContentType;
                            mythology.PhotoUrl = @"/" + PicturePath;

                        }
                    }
                    _context.Update(mythology);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MythologyExists(mythology.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mythology);
        }

        // GET: Mythologies/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mythology = await _context.Mythologies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mythology == null)
            {
                return NotFound();
            }

            return View(mythology);
        }

        // POST: Mythologies/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var mythology = await _context.Mythologies.FindAsync(id);
            System.IO.File.Delete(webHostEnvironment.WebRootPath + mythology.PhotoUrl);
            _context.Mythologies.Remove(mythology);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MythologyExists(int id)
        {
            return _context.Mythologies.Any(e => e.Id == id);
        }
    }
}
