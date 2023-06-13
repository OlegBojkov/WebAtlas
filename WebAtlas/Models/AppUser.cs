using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace WebAtlas.Models
{
    public class AppUser : IdentityUser
    {
        public string? NameLita { get; set; }
        public string? Description { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }

        [ForeignKey("Adress")]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? AddressLink { get; set; }
        public ICollection<Event>? Events { get; set; }
        public ICollection<Material>? Materials { get; set; }
        public ICollection<News>? News { get; set; }
    }
}
