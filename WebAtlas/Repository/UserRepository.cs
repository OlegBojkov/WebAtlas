using Microsoft.EntityFrameworkCore;
using WebAtlas.Data;
using WebAtlas.Data.Enum;
using WebAtlas.Interfaces;
using WebAtlas.Models;

namespace WebAtlas.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(AppUser user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(AppUser user)
        {
            _context.Remove(user);
            return Save();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser user)
        {
            _context.Update(user);
            return Save();
        }

        public async Task<IEnumerable<AppUser>> GetByCity(string city)
        {
            return await _context.Users.Where(a => a.City.Contains(city)).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<AppUser>> GetBySearch(string searchString)
        {
            return await _context.Users.Where(a => a.Description.Contains(searchString) || a.NameLita.Contains(searchString)).Distinct().ToListAsync();
        }
    }
}
