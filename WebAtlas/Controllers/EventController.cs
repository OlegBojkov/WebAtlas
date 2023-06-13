using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Linq;
using WebAtlas.Data.Enum;
using WebAtlas.Interfaces;
using WebAtlas.Models;
using WebAtlas.Repository;
using WebAtlas.ViewModels;

namespace WebAtlas.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPhotoService _photoService;
        private readonly IAddressRepository _addressRepository;

        public EventController(IEventRepository eventRepository, IPhotoService photoService, IUserRepository userRepository, IAddressRepository addressRepository)
        {
            _eventRepository = eventRepository;
            _photoService = photoService;
            _userRepository = userRepository;
            _addressRepository = addressRepository;
        }
        public async Task<IActionResult> Index(int category = -1, string city = "Все", string lito = "Все", string sort = "Все", string searchString = "")
        {
            var users = await _userRepository.GetAllUsers();
            var usersName = new List<string>();

            foreach (var user in users)
            {
                usersName.Add(user.NameLita);
            }
            usersName = usersName.Distinct().OrderBy(x => x).ToList();


            var cities = new List<string>();

            var events = await _eventRepository.GetAll();

            if (city != "Все")
            {
                events = await _eventRepository.GetByCity(city);
            }
            if (category != -1)
            {
                events = await _eventRepository.GetByCategory((Category)category);
            }
            if (lito != "Все")
            {
                events = await _eventRepository.GetByUser(lito);
            }
            if (sort != "Все")
            {
                events = await _eventRepository.GetOrderByDate();
            }

            var addresses = await _addressRepository.GetAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                events = await _eventRepository.GetBySearch(searchString);
            }

            foreach (var address in addresses)
            {
                cities.Add(address.City);
            }
            cities = cities.Distinct().OrderBy(x => x).ToList();

            var eventViewModel = new IndexEventViewModel
            {
                Events = events,
                Category = category,
                Cities = cities,
                NameLita = usersName
            };
            return View(eventViewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            Event litEvent = await _eventRepository.GetByIdAsync(id);
            return View(litEvent);
        }

        public IActionResult Create()
        {
            var curUserId = HttpContext.User.GetUserId();
            var createEventViewModel = new CreateEventViewModel { AppUserId = curUserId };
            return View(createEventViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEventViewModel eventVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(eventVM.Image);

                var litEvent = new Event
                {
                    Title = eventVM.Title,
                    Description = eventVM.Description,
                    Content = eventVM.Content,
                    Image = result.Url.ToString(),
                    AppUserId = eventVM.AppUserId,
                    Address = new Address
                    {
                        City = eventVM.Address.City,
                        Street = eventVM.Address.Street,
                        HouseNumber = eventVM.Address.HouseNumber
                    },
                    DateCreated = DateTime.Now,
                    DateEvent = eventVM.DateEvent,
                    Category = eventVM.Category
                };
                _eventRepository.Add(litEvent);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Не удалось загрузить фото");
            }

            return View(eventVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var litEvent = await _eventRepository.GetByIdAsync(id);
            if (litEvent != null)
            {
                var eventVM = new EditEventViewModel
                {
                    Title = litEvent.Title,
                    Description = litEvent.Description,
                    Content = litEvent.Content,
                    ImageUrl = litEvent.Image,
                    Address = litEvent.Address,
                    DateEvent = litEvent.DateEvent,
                    Category = litEvent.Category
                };
                return View(eventVM);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditEventViewModel eventVM)
        {
            if (ModelState.IsValid)
            {
                var userEvent = await _eventRepository.GetByIdAsync(id);

                if (userEvent != null)
                {
                    var photoResultUrl = userEvent.Image;
                    if (eventVM.Image != null)
                    {
                        try
                        {
                            await _photoService.DeletePhotoAsync(userEvent.Image);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Не удалось удалить фото");
                            return View(eventVM);
                        }
                        photoResultUrl = (await _photoService.AddPhotoAsync(eventVM.Image)).Url.ToString();
                    }

                    userEvent.Title = eventVM.Title;
                    userEvent.Description = eventVM.Description;
                    userEvent.Content = eventVM.Content;
                    userEvent.Image = photoResultUrl;
                    userEvent.Address = eventVM.Address;
                    userEvent.DateEvent = eventVM.DateEvent;
                    userEvent.Category = eventVM.Category;

                    _eventRepository.Update(userEvent);

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(eventVM);
                }
            }
            else
            {
                ModelState.AddModelError("", "Не удалось отредактировать Lita");
                return View("Edit", eventVM);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var eventDetails = await _eventRepository.GetByIdAsync(id);
            if (eventDetails == null) return View("Error");
            return View(eventDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventDetails = await _eventRepository.GetByIdAsync(id);

            if (eventDetails == null)
            {
                return View("Error");
            }

            if (!string.IsNullOrEmpty(eventDetails.Image))
            {
                _ = _photoService.DeletePhotoAsync(eventDetails.Image);
            }

            _eventRepository.Delete(eventDetails);
            return RedirectToAction("Index");

        }
    }
}
