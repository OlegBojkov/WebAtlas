using WebAtlas.Data.Enum;

namespace WebAtlas.ViewModels
{
    public class EditNewsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public Category Category { get; set; }
    }
}
