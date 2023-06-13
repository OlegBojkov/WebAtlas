using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAtlas.Interfaces;
using WebAtlas.ViewModels;

namespace WebAtlas.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRespository;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashboardRepository dashboardRespository, IPhotoService photoService)
        {
            _dashboardRespository = dashboardRespository;
            _photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            var userNews = await _dashboardRespository.GetAllUserNews();
            var userEvents= await _dashboardRespository.GetAllUserEvents();
            var userMaterials = await _dashboardRespository.GetAllUserMaterials();

            var dashboardViewModel = new DashboardViewModel()
            {
                News = userNews,
                Events = userEvents,
                Materials = userMaterials
            };
            return View(dashboardViewModel);
        }
    }
}
