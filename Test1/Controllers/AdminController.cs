using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test1.Models;

namespace Test1.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationContext _context;

        public AdminController(ApplicationContext _context)
        {
            this._context = _context;
        }

        public IActionResult Index()
        {

            var Comments = _context.Commetns.ToList();
            return View(Comments);
        }

        public async Task<IActionResult> DeleteComment(int? id)
        {
            var comment = await _context.Commetns.FindAsync(id);

            if(comment != null)
            {
                _context.Remove(comment);
                await _context.SaveChangesAsync();
            }


            return RedirectToAction("Index");
        }
    }
}
