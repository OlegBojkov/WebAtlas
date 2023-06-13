using WebAtlas.Models;

namespace WebAtlas.Interfaces
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetAll();
    }
}
