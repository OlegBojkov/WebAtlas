namespace WebAtlas.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string? NameLita { get; set; }
        public string? Description { get; set; }
        public string? City { get; set; }
        public string ProfileImageUrl { get; set; }
        public List<String> Сities { get; set; } = null;
    }
}
