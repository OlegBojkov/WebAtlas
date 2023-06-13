namespace WebAtlas.ViewModels
{
    public class CreateMaterialViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string AppUserId { get; set; }
        public IFormFile UploadedFile { get; set; }
    }
}
