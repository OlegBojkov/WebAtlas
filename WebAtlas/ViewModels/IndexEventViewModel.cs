using Microsoft.AspNetCore.Mvc.Rendering;
using WebAtlas.Data.Enum;
using WebAtlas.Models;

namespace WebAtlas.ViewModels
{
    public class IndexEventViewModel
    {
        public IEnumerable<Event> Events { get; set; }
        public int Category { get; set; }

        public List<String> Cities { get; set; } = null;

        public List<String> NameLita { get; set; } = null;

    }
}
