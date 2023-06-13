using WebAtlas.Data.Enum;
using WebAtlas.Models;

namespace WebAtlas.Interfaces
{
    public interface INewsRepository
    {
        Task<IEnumerable<News>> GetAll();
        Task<News> GetByIdAsync(int id);
        bool Add(News news);
        bool Update(News news);
        bool Delete(News news);
        bool Save();

        Task<IEnumerable<News>> GetOrderByDate();

        Task<IEnumerable<News>> GetByCategory(Category category);

        Task<IEnumerable<News>> GetByCity(string city);

        Task<IEnumerable<News>> GetByUser(string name);

        Task<IEnumerable<News>> GetBySearch(string searchString);
    }
}
