using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_DataAccess;
using Shop_Models;
using Shop_Models.ViewModels;
using Shop_Utility;
using System.Diagnostics;

namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Products = _db.Product.Include(a => a.Category).Include(a => a.TestModel),
                Categories = _db.Category
            };
            return View(homeVM);
        }
        public IActionResult Details(int id)
        {
            var ShoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) != null && HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                ShoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }          
            DetailsVM DetailsVM = new DetailsVM()
            {
                Product = _db.Product.Include(a => a.Category).Include(a => a.TestModel)
                .Where(a => a.Id == id).FirstOrDefault(),
                ExistInCart = false
            };
            foreach (var item in ShoppingCartList)
            {
                if (item.ProductId == id)
                {
                    DetailsVM.ExistInCart = true;
                }
            }
            return View(DetailsVM);
        }
        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            var ShoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) != null && HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                ShoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            ShoppingCartList.Add(new ShoppingCart() { ProductId = id });
            HttpContext.Session.Set(WC.SessionCart, ShoppingCartList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            var ShoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) != null && HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                ShoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            var itemForRemove = ShoppingCartList.SingleOrDefault(a => a.ProductId ==  id);
            if (itemForRemove != null)
            {
                ShoppingCartList.Remove(itemForRemove);
            }
            HttpContext.Session.Set(WC.SessionCart, ShoppingCartList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}