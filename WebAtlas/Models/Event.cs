using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebAtlas.Data.Enum;

namespace WebAtlas.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        [ForeignKey("Adress")]
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateEvent { get; set; }
        public Category Category { get; set; }

        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
