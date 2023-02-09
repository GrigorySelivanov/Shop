using Microsoft.AspNetCore.Mvc.Rendering;

namespace Shop_Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
        public IEnumerable<SelectListItem> TestModelSelectList { get; set; }
    }
}
