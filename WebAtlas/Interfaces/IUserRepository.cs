using WebAtlas.Data.Enum;
using WebAtlas.Models;

namespace WebAtlas.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(string id);
        bool Add(AppUser user);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();

        Task<IEnumerable<AppUser>> GetByCity(string city);
        Task<IEnumerable<AppUser>> GetBySearch(string searchString);
    }
}
