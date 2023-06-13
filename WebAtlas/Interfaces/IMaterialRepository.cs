using WebAtlas.Data.Enum;
using WebAtlas.Models;

namespace WebAtlas.Interfaces
{
    public interface IMaterialRepository
    {
        Task<IEnumerable<Material>> GetAll();
        Task<Material> GetByIdAsync(int id);
        bool Add(Material material);
        bool Update(Material material);
        bool Delete(Material material);
        bool Save();

        Task<IEnumerable<Material>> GetOrderByDate();
        Task<IEnumerable<Material>> GetBySearch(string searchString);
        Task<IEnumerable<Material>> GetByUser(string name);

    }
}
