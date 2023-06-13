using Microsoft.EntityFrameworkCore;
using WebAtlas.Data;
using WebAtlas.Data.Enum;
using WebAtlas.Interfaces;
using WebAtlas.Models;

namespace WebAtlas.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;
        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Event litEvent)
        {
            _context.Add(litEvent);
            return Save();
        }
        public bool Delete(Event litEvent)
        {
            _context.Remove(litEvent);
            return Save();
        }
        public async Task<Event> GetByIdAsync(int id)
        {
            return await _context.Events.Include(i => i.Address).Include(i => i.AppUser).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Event>> GetAll()
        {
            return await _context.Events.Include(i => i.Address).Include(i => i.AppUser).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Event litEvent)
        {
            _context.Update(litEvent);
            return Save();
        }

        public async Task<IEnumerable<Event>> GetOrderByDate()
        {
            return await _context.Events.Include(i => i.Address).Include(i => i.AppUser).OrderByDescending(e => e.DateEvent).ToListAsync();
        }
        public async Task<IEnumerable<Event>> GetByCategory(Category category)
        {
            return await _context.Events.Include(i => i.Address).Include(i => i.AppUser).Where(e => e.Category == category).ToListAsync();
        }
        public async Task<IEnumerable<Event>> GetByCity(string city)
        {
            return await _context.Events.Include(i => i.Address).Include(i => i.AppUser).Where(e => e.Address.City.Contains(city)).Distinct().ToListAsync();
        }
        public async Task<IEnumerable<Event>> GetByUser(string name)
        {
            return await _context.Events.Include(i => i.Address).Include(i => i.AppUser).Where(e => e.AppUser.NameLita.Contains(name)).ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetBySearch(string searchString)
        {
            return await _context.Events.Include(i => i.Address).Include(i => i.AppUser).Where(e => e.Description.Contains(searchString) || e.Title.Contains(searchString)).Distinct().ToListAsync();
        }
    }
}
