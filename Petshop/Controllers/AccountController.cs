using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Petshop.DTOs.Account;

namespace Petshop.Controllers
{
    public class AccountController : Controller
    {
        UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> CreateRole()
        {
            IdentityRole role1 = new IdentityRole("Admin");
            IdentityRole role2 = new IdentityRole("Member");
            await _roleManager.CreateAsync(role1);
            await _roleManager.CreateAsync(role2);
            return Ok("Salammmmm");
        }

        public async Task<IActionResult> CreateAdmin()
        {
            AppUser admin = new AppUser()
            {
                FullName = "Rashid Babazada",
                UserName = "Admin",
                Email = "rashid@gmail.com"
            };
            var user = await _userManager.CreateAsync(admin,"Admin123@");
            if (user.Succeeded)
            {
                var result = await _userManager.AddToRoleAsync(admin, "Admin");
                return Ok(result);
            }
            return BadRequest();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new AppUser()
            {
                FullName = register.FullName,
                UserName = register.UserName,
                Email = register.Email
            };
            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, "Member");
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if(loginDto == null)
            {
                ModelState.AddModelError("", "Login Data is not valid!");
                return View();
            }
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if(user == null)
            {
                ModelState.AddModelError("", "Username or Password is not valid!");
                return View();
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is not valid!");
                return View();
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Banlanmisiniz! daha sonra yeniden yoxlayin!");
                return View();
            }
            var signInResult =  await _signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, false);

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Xeta bas verdi Yeniden Cehd edin!");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
