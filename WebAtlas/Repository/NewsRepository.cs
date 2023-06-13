using Microsoft.EntityFrameworkCore;
using WebAtlas.Data;
using WebAtlas.Data.Enum;
using WebAtlas.Interfaces;
using WebAtlas.Models;

namespace WebAtlas.Repository
{
    public class NewsRepository : INewsRepository
    {
        private readonly ApplicationDbContext _context;
        public NewsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(News news)
        {
            _context.Add(news);
            return Save();
        }

        public bool Delete(News news)
        {
            _context.Remove(news);
            return Save();
        }
        public async Task<News> GetByIdAsync(int id)
        {
            return await _context.News.Include(n => n.AppUser).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<News>> GetAll()
        {
            return await _context.News.Include(n => n.AppUser).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(News news)
        {
            _context.Update(news);
            return Save();
        }

        public async Task<IEnumerable<News>> GetOrderByDate()
        {
            return await _context.News.Include(n => n.AppUser).OrderByDescending(n => n.DateCreated).ToListAsync();
        }
        public async Task<IEnumerable<News>> GetByCategory(Category category)
        {
            return await _context.News.Include(n => n.AppUser).Where(n => n.Category == category).ToListAsync();
        }
        public async Task<IEnumerable<News>> GetByCity(string city)
        {
            return await _context.News.Include(n => n.AppUser).Where(n => n.AppUser.City.Contains(city)).Distinct().ToListAsync();
        }
        public async Task<IEnumerable<News>> GetByUser(string name)
        {
            return await _context.News.Include(n => n.AppUser).Where(n => n.AppUser.NameLita.Contains(name)).ToListAsync();
        }

        public async Task<IEnumerable<News>> GetBySearch(string searchString)
        {
            return await _context.News.Include(n => n.AppUser).Where(n => n.Description.Contains(searchString) || n.Title.Contains(searchString)).Distinct().ToListAsync();
        }
    }
}
