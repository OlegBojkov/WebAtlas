using WebAtlas.Data.Enum;
using WebAtlas.Models;

namespace WebAtlas.ViewModels
{
    public class CreateNewsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public IFormFile Image { get; set; }
        public Category Category { get; set; }
        public string AppUserId { get; set; }
    }
}
