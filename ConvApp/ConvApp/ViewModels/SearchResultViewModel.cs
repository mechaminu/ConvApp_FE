using System.Collections.Generic;
using System.Linq;

namespace ConvApp.ViewModels
{
    public class SearchResultViewModel : ViewModelBase
    {
        public byte Type { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
