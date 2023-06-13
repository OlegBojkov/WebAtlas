using Microsoft.EntityFrameworkCore;
using WebAtlas.Data;
using WebAtlas.Data.Enum;
using WebAtlas.Interfaces;
using WebAtlas.Models;

namespace WebAtlas.Repository
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly ApplicationDbContext _context;
        public MaterialRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Material material)
        {
            _context.Add(material);
            return Save();
        }

        public bool Delete(Material material)
        {
            _context.Remove(material);
            return Save();
        }
        public async Task<Material> GetByIdAsync(int id)
        {
            return await _context.Materials.Include(m => m.AppUser).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Material>> GetAll()
        {
            return await _context.Materials.Include(m => m.AppUser).ToListAsync();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Material material)
        {
            _context.Update(material);
            return Save();
        }

        public async Task<IEnumerable<Material>> GetOrderByDate()
        {
            return await _context.Materials.Include(m => m.AppUser).OrderByDescending(m => m.DateCreated).ToListAsync();
        }
        public async Task<IEnumerable<Material>> GetBySearch(string searchString)
        {
            return await _context.Materials.Include(m => m.AppUser).Where(m => m.Description.Contains(searchString) || m.Title.Contains(searchString)).Distinct().ToListAsync();
        }
        public async Task<IEnumerable<Material>> GetByUser(string name)
        {
            return await _context.Materials.Include(m => m.AppUser).Where(m => m.AppUser.NameLita.Contains(name)).ToListAsync();
        }
    }
}
