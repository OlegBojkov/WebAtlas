using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebAtlas.Data.Enum;
using WebAtlas.Interfaces;
using WebAtlas.Models;
using WebAtlas.Repository;
using WebAtlas.ViewModels;

namespace WebAtlas.Controllers
{
    public class MaterialController : Controller
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IUserRepository _userRepository;
        public MaterialController(IMaterialRepository materialRepository, IWebHostEnvironment appEnvironment, IUserRepository userRepository)
        {
            _materialRepository = materialRepository;
            _appEnvironment = appEnvironment;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index(string sort = "Все", string searchString = "", string lito = "Все")
        {
            var users = await _userRepository.GetAllUsers();
            var usersName = new List<string>();

            foreach (var user in users)
            {
                usersName.Add(user.NameLita);
            }
            usersName = usersName.Distinct().OrderBy(x => x).ToList();


            var materials = await _materialRepository.GetAll();

            if (lito != "Все")
            {
                materials = await _materialRepository.GetByUser(lito);
            }
            if (sort != "Все")
            {
                materials = await _materialRepository.GetOrderByDate();
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                materials = await _materialRepository.GetBySearch(searchString);
            }

            var eventViewModel = new IndexMaterialViewModel
            {
                Materials = materials,
                NameLita = usersName
            };
            return View(eventViewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            Material material = await _materialRepository.GetByIdAsync(id);
            return View(material);
        }

        public IActionResult Create()
        {
            var curUserId = HttpContext.User.GetUserId();
            var createMaterialViewModel = new CreateMaterialViewModel { AppUserId = curUserId };
            return View(createMaterialViewModel);
        }

        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMaterialViewModel materialVM)
        {
            if (ModelState.IsValid)
            {
                var uploadedFile = materialVM.UploadedFile;
                if (uploadedFile != null)
                {
                    // путь к папке Files
                    string path = "/Files/" + uploadedFile.FileName;
                    // сохраняем файл в папку Files в каталоге wwwroot
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        var l = fileStream.Length;
                        await uploadedFile.CopyToAsync(fileStream);

                    }

                    FileInfo fileinfo = new FileInfo(_appEnvironment.WebRootPath + path);

                    var material = new Material
                    {
                        Title = materialVM.Title,
                        Description = materialVM.Description,
                        Content = materialVM.Content,
                        AppUserId = materialVM.AppUserId,
                        DateCreated = DateTime.Now,
                        FileName = uploadedFile.FileName,
                        FilePath = path,
                        FileLength = fileinfo.Length / 1000,
                    };
                    _materialRepository.Add(material);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Загрузка файла не удалась");
                }
            }
            else
            {
                ModelState.AddModelError("", "Неправильная форма");
            }

            return View(materialVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var material = await _materialRepository.GetByIdAsync(id);
            if (material != null)
            {
                var materialVM = new EditMaterialViewModel
                {
                    Title = material.Title,
                    Description = material.Description,
                    Content = material.Content
                };
                return View(materialVM);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditMaterialViewModel materialVM)
        {
            if (ModelState.IsValid)
            {
                var userMaterial = await _materialRepository.GetByIdAsync(id);

                if (userMaterial != null)
                {
                    userMaterial.Title = materialVM.Title;
                    userMaterial.Description = materialVM.Description;
                    userMaterial.Content = materialVM.Content;

                    _materialRepository.Update(userMaterial);

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(materialVM);
                }
            }
            else
            {
                ModelState.AddModelError("", "Не удалось отредактировать материал");
                return View("Edit", materialVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var materialDetails = await _materialRepository.GetByIdAsync(id);
            if (materialDetails == null) return View("Error");
            return View(materialDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var materialDetails = await _materialRepository.GetByIdAsync(id);

            if (materialDetails == null)
            {
                return View("Error");
            }

            _materialRepository.Delete(materialDetails);
            return RedirectToAction("Index");
        }
    }
}
