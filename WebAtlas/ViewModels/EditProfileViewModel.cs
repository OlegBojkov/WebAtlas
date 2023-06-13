using System.ComponentModel.DataAnnotations.Schema;
using WebAtlas.Models;

namespace WebAtlas.ViewModels
{
    public class EditProfileViewModel
    {
        public string? NameLita { get; set; }
        public string? Description { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        public string? AddressLink { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public IFormFile? Image { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
