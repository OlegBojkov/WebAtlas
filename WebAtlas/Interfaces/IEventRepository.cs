using Microsoft.EntityFrameworkCore;
using WebAtlas.Data.Enum;
using WebAtlas.Models;

namespace WebAtlas.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAll();
        Task<Event> GetByIdAsync(int id);
        bool Add(Event litEvent);
        bool Update(Event litEvent);
        bool Delete(Event litEvent);
        bool Save();

        Task<IEnumerable<Event>> GetOrderByDate();

        Task<IEnumerable<Event>> GetByCategory(Category category);

        Task<IEnumerable<Event>> GetByCity(string city);

        Task<IEnumerable<Event>> GetByUser(string name);

        Task<IEnumerable<Event>> GetBySearch(string searchString);

    }
}
