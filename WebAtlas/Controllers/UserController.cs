using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using WebAtlas.Data.Enum;
using WebAtlas.Interfaces;
using WebAtlas.Models;
using WebAtlas.Repository;
using WebAtlas.ViewModels;

namespace WebAtlas.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPhotoService _photoService;
        private readonly IAddressRepository _addressRepository;

        public UserController(IUserRepository userRepository, UserManager<AppUser> userManager, IPhotoService photoService, IAddressRepository addressRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _photoService = photoService;
            _addressRepository = addressRepository;
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index( string city = "Все", string searchString = "")
        {

            var users = await _userRepository.GetAllUsers();
            var addresses = await _addressRepository.GetAll();
            var cities = new List<string>();
            foreach (var address in addresses)
            {
                cities.Add(address.City);
            }
            cities = cities.Distinct().OrderBy(x => x).ToList();


            if (!String.IsNullOrEmpty(searchString))
            {
                users = await _userRepository.GetBySearch(searchString);
            }

            if (city != "Все")
            {
                users = await _userRepository.GetByCity(city);
            }

            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userViewModel = new UserViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    NameLita = user.NameLita,
                    Description = user.Description,
                    City = user.City,
                    ProfileImageUrl = user.ProfileImageUrl ?? "/img/avatar-male-4.jpg",
                    Сities = cities
                };
                result.Add(userViewModel);
            }
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Users");
            }

            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                NameLita = user.NameLita,
                Description = user.Description,
                City = user.City,
                Street = user.Street,
                HouseNumber = user.HouseNumber,
                ContactEmail = user.ContactEmail,
                ContactPhone = user.ContactPhone,
                AddressLink = user.AddressLink,
                ProfileImageUrl = user.ProfileImageUrl ?? "/img/avatar-male-4.jpg",
            };
            return View(userDetailViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return View("Error");
            }

            var editMV = new EditProfileViewModel()
            {
                NameLita = user.NameLita,
                Description = user.Description,
                City = user.City,
                Street = user.Street,
                HouseNumber = user.HouseNumber,
                AddressLink = user.AddressLink,
                ContactEmail = user.ContactEmail,
                ContactPhone = user.ContactPhone,
                ProfileImageUrl = user.ProfileImageUrl ?? "/img/avatar-male-4.jpg",
            };
            return View(editMV);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditProfileViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Не удалось изменить профиль");
                return View("EditProfile", editVM);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return View("Error");
            }

            if (editVM.Image != null) // only update profile image
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                if (photoResult.Error != null)
                {
                    ModelState.AddModelError("Image", "Не удалось загрузить изображение");
                    return View("EditProfile", editVM);
                }

                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    _ = _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }

                user.ProfileImageUrl = photoResult.Url.ToString();
                editVM.ProfileImageUrl = user.ProfileImageUrl;

                await _userManager.UpdateAsync(user);

                return RedirectToAction("Detail", "User", new { user.Id });
            }

            user.NameLita = editVM.NameLita;
            user.Description = editVM.Description;
            user.City = editVM.City;
            user.Street = editVM.Street;
            user.AddressLink = editVM.AddressLink;
            user.HouseNumber = editVM.HouseNumber;
            user.ContactEmail = editVM.ContactEmail;
            user.ContactPhone = editVM.ContactPhone;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Detail", "User", new { user.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userRepository.GetUserById(id);


            if (!string.IsNullOrEmpty(user.ProfileImageUrl))
            {
                _ = _photoService.DeletePhotoAsync(user.ProfileImageUrl);
            }

            _userRepository.Delete(user);
            return RedirectToAction("Index");
        }
    }
}
