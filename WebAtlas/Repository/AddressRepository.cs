using Microsoft.EntityFrameworkCore;
using WebAtlas.Data;
using WebAtlas.Interfaces;
using WebAtlas.Models;

namespace WebAtlas.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public AddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Address>> GetAll()
        {
            var addreses = _context.Adresses;
            return addreses.ToList();
        }
    }
}
