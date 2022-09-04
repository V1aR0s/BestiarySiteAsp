using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Test1.Models;
using Test1.ViewModels;

namespace Test1.Controllers
{

    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> roleManager;
        UserManager<User> userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        public async Task<IActionResult> Index() => View(await roleManager.Roles.ToListAsync());

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Name)
        {
            if (string.IsNullOrEmpty(Name)) {
                ModelState.AddModelError(string.Empty, "Название роли не может быть пустое");
                return View();
            }
            if(await roleManager.RoleExistsAsync(Name))
            {
                ModelState.AddModelError(string.Empty, "Роль уже есть");
                return View();
            }

            await roleManager.CreateAsync(new IdentityRole(Name));


            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            await roleManager.DeleteAsync(role);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UserList(string id)
        {
            return View(await userManager.Users.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> EditUserRoles(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            User user = await userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            ChangeUserRolesDTO viewModelRolesList = new ChangeUserRolesDTO
            {
                Id = user.Id,
                UserRoles = await userManager.GetRolesAsync(user),
                AllRoles = await roleManager.Roles.ToListAsync(),
                Name = user.UserName,
                Email = user.Email
            };
            return View(viewModelRolesList);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserRoles(ChangeUserRolesDTO viewModel)
        {
            if (ModelState.IsValid)
            {

                User user = await userManager.FindByIdAsync(viewModel.Id);

                if (user == null)
                    return NotFound();

                //Роли польователя, и все роли
                var userRolesOld = await userManager.GetRolesAsync(user);
                var AddedRoles = viewModel.UserRoles.Except(userRolesOld);


                var deletedRoles = userRolesOld.Except(viewModel.UserRoles);

                await userManager.AddToRolesAsync(user, AddedRoles);
                await userManager.RemoveFromRolesAsync(user, deletedRoles);

                return RedirectToAction("UserList");
            }
            return View(viewModel);
        }
    }
}
