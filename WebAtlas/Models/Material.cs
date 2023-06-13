using System.ComponentModel.DataAnnotations;

namespace WebAtlas.Models
{
    public class Material
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public long? FileLength { get; set; }
    }
}
