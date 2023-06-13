using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAtlas.Data;
using WebAtlas.Interfaces;
using WebAtlas.Models;
using WebAtlas.ViewModels;

namespace WebAtlas.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        [HttpGet]
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                //Пользователь найден, проверка пароля
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    //Пароль верный, sign in
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                //Неверный пароль
                TempData["Error"] = "Неправильные учетные данные. Пожалуйста, попробуйте еще раз";
                return View(loginViewModel);
            }
            //Пользователь не найден
            TempData["Error"] = "Неправильные учетные данные. Пожалуйста, попробуйте еще раз";
            return View(loginViewModel);

        }

        [HttpGet]
        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "Данный email уже используется";
                return View(registerViewModel);
            }

            var newUser = new AppUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress,
                NameLita = registerViewModel.NameLita,
                Description = registerViewModel.Description,
                Address = new Address
                {
                    City = registerViewModel.Address.City,
                    Street = registerViewModel.Address.Street,
                    HouseNumber = registerViewModel.Address.HouseNumber
                },
                City = registerViewModel.Address.City,
                Street = registerViewModel.Address.Street,
                HouseNumber = registerViewModel.Address.HouseNumber,
                AddressLink = registerViewModel.AddressLink,
                ContactPhone = registerViewModel.ContactPhone

            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "News");
        }
    }
}
