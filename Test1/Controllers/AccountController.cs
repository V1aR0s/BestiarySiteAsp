using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Test1.Models;
using Test1.ViewModels;

namespace Test1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Name, Year = model.Year, RegDate = DateTime.Now.ToShortDateString()};

                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");


                    User UserClaimSuer = await userManager.FindByEmailAsync(user.Email);
                    Claim claim = new Claim("IsAllData", "Yes", ClaimValueTypes.String);
                    IdentityResult resultClaim = await userManager.AddClaimAsync(UserClaimSuer, claim);

                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var error in resultClaim.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? ReturnUrl = null)
        {
            LoginViewModel loginViewModel = new LoginViewModel { ReturnUrl = ReturnUrl };
            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Name);

            var result = await signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Логин и (или) пароль введены неверно");
            }

            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("ServiceLoginCallBack", "Account");

            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);

            return new ChallengeResult("Google", properties);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult FacebookLogin()
        {
            var redirectUrl = Url.Action("ServiceLoginCallBack", "Account");

            var properties = signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);

            return new ChallengeResult("Facebook", properties);
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult TwitterLogin()
        {
            var redirectUrl = Url.Action("ServiceLoginCallBack", "Account");

            var properties = signInManager.ConfigureExternalAuthenticationProperties("Twitter", redirectUrl);

            return new ChallengeResult("Twitter", properties);
        }

        public async Task<IActionResult> ServiceLoginCallBack(string remoteError)
        {
            //Проверяем есть ли записи в ошибке
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, "Ошибка получения информации");
                return View("Login");
            }
            var info = await signInManager.GetExternalLoginInfoAsync();

            //Поулчаем информации об аккаунте
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Ошибка загрузка информации");
                return View("Login");
            }

            //Пробуем зайти в аккаунт
            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    User user = await userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        string[] parts = email.Split('@');
                        string USerName = parts[0];
                        user = new User
                        {
                            UserName = USerName,
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            RegDate = DateTime.Now.ToShortDateString()
                        };

                        await userManager.CreateAsync(user);
                    }

                    await userManager.AddLoginAsync(user, info);

                    await signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("SupplementUserInfo");
                }

            }

            return View("Login");
        }


        [HttpGet]
        [Authorize]
        public IActionResult SupplementUserInfo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SupplementUserInfo(SupplementUserInfoAfterGoogleViewModel userInfo)
        {

            if (userInfo == null)
            {
                return View(userInfo);
            }
            if(userInfo.Id == null)
            {
                Logout();
                return RedirectToAction("Index", "Home");
            }

            User user = await userManager.FindByIdAsync(userInfo.Id);

            user.UserName = userInfo.Name;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                if(userInfo.Password == userInfo.ConfirmPassword)
                {
                    IPasswordValidator<User> passwordValidator = HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    IPasswordHasher<User> passwordHasher = HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;
                    var validationResult = await passwordValidator.ValidateAsync(userManager, user, userInfo.Password);
                    if (validationResult.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, userInfo.Password);
                        await userManager.UpdateAsync(user);





                        User UserClaimSuer = await userManager.FindByEmailAsync(user.Email);
                        Claim claim = new Claim("IsAllData", "Yes", ClaimValueTypes.String);
                        await userManager.AddClaimAsync(UserClaimSuer, claim);



                        Logout();
                        await signInManager.PasswordSignInAsync(user.UserName, userInfo.Password, false, false);


                        return RedirectToAction("Index", "Home");
                    }
                    else
                        ModelState.AddModelError(string.Empty, "Некоректний пароль!");
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(userInfo);
        }
    }
}
