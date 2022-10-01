using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore_Ecommerce.Data;
using OnlineStore_Ecommerce.Models;
using OnlineStore_Ecommerce.Utility;
using System.Diagnostics;

namespace OnlineStore_Ecommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;
        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View(context.Products.Include(P => P.ProductType).Include(S => S.SpecialTag).ToList());
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
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return View("Error");
            var product = context.Products.Include(p=>p.ProductType).FirstOrDefault(p => p.Id == id);
            if(product == null)
                return NotFound();
            return View(product);
        }
        [HttpPost]
        [ActionName("Details")]
        public IActionResult ProductDetails(int? id)
        {
            List<Products> products = new List<Products>();
            if (id == null)
                return View("Error");
            var product = context.Products.Include(p => p.ProductType).FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            products = HttpContext.Session.Get<List<Products>>("products");
            if(products == null)
            {
                products = new List<Products>();
            }
            products.Add(product);
            HttpContext.Session.Set("products", products);
            return View(product);
        }
        [HttpPost]
        public IActionResult Remove(int? id)
        {
            List<Products> products = new List<Products>();
            if (id == null)
                return View("Error");
            var product = context.Products.FirstOrDefault(c=>c.Id==id);
            if (product == null)
                return NotFound();
            products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                products.RemoveAll(c=>c.Id == product.Id);
                HttpContext.Session.Set("products", products);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Cart()
        {
            List<Products> products = new List<Products>();
            products = HttpContext.Session.Get<List<Products>>("products");
            if (products == null)
            {
                products = new List<Products>();
            }
            return View(products);
        }
        [ActionName("Remove")]
        public IActionResult RemoveFromCart(int? id)
        {
            List<Products> products = new List<Products>();
            if (id == null)
                return View("Error");
            var product = context.Products.FirstOrDefault(c => c.Id == id);
            if (product == null)
                return NotFound();
            products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                products.RemoveAll(c => c.Id == product.Id);
                HttpContext.Session.Set("products", products);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}