using WebAtlas.Data;
using WebAtlas.Interfaces;
using WebAtlas.Models;

namespace WebAtlas.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<News>> GetAllUserNews()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userNews = _context.News.Where(r => r.AppUser.Id == curUser);
            return userNews.ToList();
        }

        public async Task<List<Material>> GetAllUserMaterials()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userMaterials = _context.Materials.Where(r => r.AppUser.Id == curUser);
            return userMaterials.ToList();
        }

        public async Task<List<Event>> GetAllUserEvents()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userEvents = _context.Events.Where(r => r.AppUser.Id == curUser);
            return userEvents.ToList();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}
