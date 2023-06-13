using WebAtlas.Models;

namespace WebAtlas.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<News>> GetAllUserNews();
        Task<List<Material>> GetAllUserMaterials();
        Task<List<Event>> GetAllUserEvents();
        Task<AppUser> GetUserById(string id);
        //Task<AppUser> GetByIdNoTracking(string id);
        bool Update(AppUser user);
        bool Save();
    }
}
