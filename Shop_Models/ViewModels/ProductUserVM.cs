namespace Shop_Models.ViewModels
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            ProductList = new List<Product>();
        }
        public AppUser AppUser { get; set; }
        public IList<Product> ProductList { get; set; }
    }
}
