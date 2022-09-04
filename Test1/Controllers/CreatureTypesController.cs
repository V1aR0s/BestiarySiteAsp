#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Test1.Models;

namespace Test1.Controllers
{
    public class CreatureTypesController : Controller
    {
        private readonly ApplicationContext _context;

        public CreatureTypesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: CreatureTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.CreatureTypes.ToListAsync());
        }


        public async Task<IActionResult> ListTypes()
        {
            return PartialView(await _context.CreatureTypes.ToListAsync());
        }


        // GET: CreatureTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creatureType = await _context.CreatureTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (creatureType == null)
            {
                return NotFound();
            }

            return View(creatureType);
        }

        // GET: CreatureTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CreatureTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<JsonResult> Create(CreatureType creatureType)
        {
            if (creatureType.Name != null)
            {
                CreatureType typ = new CreatureType()
                {
                    Name = creatureType.Name
                };
                _context.CreatureTypes.Add(typ);
                await _context.SaveChangesAsync();
            }
            return Json(JsonConvert.SerializeObject(creatureType));
        }

        // GET: CreatureTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creatureType = await _context.CreatureTypes.FindAsync(id);
            if (creatureType == null)
            {
                return NotFound();
            }
            return View(creatureType);
        }


        public async Task<JsonResult> GetCreatureTypeRequest(int? id)
        {
            CreatureType typ = new CreatureType()
            {
                Id = 0,
                Name = ""
            };
            if (id == null)
            {

                return Json(typ);
            }

            var creatureType = await _context.CreatureTypes.FindAsync(id);
            if (creatureType == null)
            {
                return Json(typ);
            }
            var json = JsonConvert.SerializeObject(creatureType);


            return Json(json);
        }

        // POST: CreatureTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<JsonResult> Edit(CreatureType creatureType)
        {
            if (creatureType.Id != 0 && creatureType.Name != null)
            {
                CreatureType obj = await _context.CreatureTypes.FindAsync(creatureType.Id);

                obj.Name = creatureType.Name;

                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            return Json(JsonConvert.SerializeObject(creatureType));
        }

        // GET: CreatureTypes/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creatureType = await _context.CreatureTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (creatureType == null)
            {
                return NotFound();
            }

            return PartialView(creatureType);
        }

        // POST: CreatureTypes/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var creatureType = await _context.CreatureTypes.FindAsync(id);
            _context.CreatureTypes.Remove(creatureType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CreatureTypeExists(int id)
        {
            return _context.CreatureTypes.Any(e => e.Id == id);
        }
    }
}
