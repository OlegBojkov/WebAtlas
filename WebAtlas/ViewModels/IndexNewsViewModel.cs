using WebAtlas.Models;

namespace WebAtlas.ViewModels
{
    public class IndexNewsViewModel
    {
        public IEnumerable<News> News { get; set; }
        public int Category { get; set; }

        public List<String> Cities { get; set; } = null;

        public List<String> NameLita { get; set; } = null;
    }
}
