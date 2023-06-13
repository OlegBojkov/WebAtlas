using System.ComponentModel.DataAnnotations.Schema;
using WebAtlas.Data.Enum;
using WebAtlas.Models;

namespace WebAtlas.ViewModels
{
    public class EditEventViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        [ForeignKey("Adress")]
        public int AdressId { get; set; }
        public Address Address { get; set; }
        public DateTime DateEvent { get; set; }
        public Category Category { get; set; }
    }
}
