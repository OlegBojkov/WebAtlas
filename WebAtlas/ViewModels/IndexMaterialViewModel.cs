using WebAtlas.Models;

namespace WebAtlas.ViewModels
{
    public class IndexMaterialViewModel
    {
        public IEnumerable<Material> Materials { get; set; }
        public List<String> NameLita { get; set; } = null;
    }
}
