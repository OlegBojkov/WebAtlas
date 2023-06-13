using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAtlas.Data.Enum;
using WebAtlas.Interfaces;
using WebAtlas.Models;
using WebAtlas.Repository;
using WebAtlas.ViewModels;

namespace WebAtlas.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;
        private readonly IPhotoService _photoService;
        private readonly IUserRepository _userRepository;
        private readonly IAddressRepository _addressRepository;
        public NewsController(INewsRepository newsRepository, IPhotoService photoService, IUserRepository userRepository, IAddressRepository addressRepository)
        {
            _newsRepository = newsRepository;
            _photoService = photoService;
            _userRepository = userRepository;
            _addressRepository = addressRepository;
        }

        // GET: NewsController1
        public async Task<IActionResult> Index(int category = -1, string city = "Все", string lito = "Все", string sort = "Все", string searchString = "")
        {
            var users = await _userRepository.GetAllUsers();
            var usersName = new List<string>();

            foreach (var user in users)
            {
                usersName.Add(user.NameLita);
            }
            usersName = usersName.Distinct().OrderBy(x => x).ToList();

            var news = await _newsRepository.GetAll();

            if (city != "Все")
            {
                news = await _newsRepository.GetByCity(city);
            }
            if (category != -1)
            {
                news = await _newsRepository.GetByCategory((Category)category);
            }
            if (lito != "Все")
            {
                news = await _newsRepository.GetByUser(lito);
            }
            if (sort != "Все")
            {
                news = await _newsRepository.GetOrderByDate();
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                news = await _newsRepository.GetBySearch(searchString);
            }

            var addresses = await _addressRepository.GetAll();
            var cities = new List<string>();
            foreach (var address in addresses)
            {
                cities.Add(address.City);
            }
            cities = cities.Distinct().OrderBy(x => x).ToList();

            var eventViewModel = new IndexNewsViewModel
            {
                News = news,
                Category = category,
                Cities = cities,
                NameLita = usersName
            };
            return View(eventViewModel);
        }

        // GET: NewsController1/Details/5
        public async Task<IActionResult> Details(int id)
        {
            News news = await _newsRepository.GetByIdAsync(id);
            return View(news);
        }

        // GET: NewsController1/Create
        public IActionResult Create()
        {
            var curUserId = HttpContext.User.GetUserId();
            var createNewsViewModel = new CreateNewsViewModel { AppUserId = curUserId };
            return View(createNewsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNewsViewModel newsVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(newsVM.Image);

                var news = new News
                {
                    Title = newsVM.Title,
                    Description = newsVM.Description,
                    Content = newsVM.Content,
                    Image = result.Url.ToString(),
                    DateCreated = DateTime.Now,
                    AppUserId = newsVM.AppUserId,
                    Category = newsVM.Category,
                };
                _newsRepository.Add(news);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Не удалось загрузить фото");
            }

            return View(newsVM);
        }

        // GET: NewsController1/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var news = await _newsRepository.GetByIdAsync(id);
            if (news != null)
            {
                var newsVM = new EditNewsViewModel
                {
                    Title = news.Title,
                    Description = news.Description,
                    Content = news.Content,
                    ImageUrl = news.Image,
                    Category = news.Category
                };
                return View(newsVM);
            }
            else
            {
                return View("Error");
            }
        }

        // POST: NewsController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditNewsViewModel newsVM)
        {
            if (ModelState.IsValid)
            {
                var userNews = await _newsRepository.GetByIdAsync(id);

                if (userNews != null)
                {
                    var photoResultUrl = userNews.Image;
                    if (newsVM.Image != null)
                    {
                        try
                        {
                            await _photoService.DeletePhotoAsync(userNews.Image);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Не удалось удалить фото");
                            return View(newsVM);
                        }
                        photoResultUrl = (await _photoService.AddPhotoAsync(newsVM.Image)).Url.ToString();
                    }

                    userNews.Title = newsVM.Title;
                    userNews.Description = newsVM.Description;
                    userNews.Content = newsVM.Content;
                    userNews.Image = photoResultUrl;
                    userNews.Category = newsVM.Category;

                    _newsRepository.Update(userNews);

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newsVM);
                }
            }
            else
            {
                ModelState.AddModelError("", "Не удалось отредактировать новость");
                return View("Edit", newsVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var newsDetails = await _newsRepository.GetByIdAsync(id);
            if (newsDetails == null) return View("Error");
            return View(newsDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var newsDetails = await _newsRepository.GetByIdAsync(id);

            if (newsDetails == null)
            {
                return View("Error");
            }

            if (!string.IsNullOrEmpty(newsDetails.Image))
            {
                _ = _photoService.DeletePhotoAsync(newsDetails.Image);
            }

            _newsRepository.Delete(newsDetails);
            return RedirectToAction("Index");
        }
    }
}
