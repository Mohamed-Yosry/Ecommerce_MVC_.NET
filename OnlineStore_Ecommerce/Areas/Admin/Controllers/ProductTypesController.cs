using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore_Ecommerce.Data;
using OnlineStore_Ecommerce.Models;

namespace OnlineStore_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext context;

        public ProductTypesController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(context.ProductTypes.ToList());
        }

        //Create GET Action method
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //Create Post Action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductTypes model)
        {
            if (ModelState.IsValid)
            {
                context.ProductTypes.Add(model);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        //Edit GET Action method
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var productType = context.ProductTypes.Find(id);
            if (productType == null)
                return NotFound();
            return View(productType);
        }

        //Edit Post Action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductTypes model)
        {
            if (ModelState.IsValid)
            {
                var productType = context.ProductTypes.Find(model.Id);
                if (productType != null)
                    productType.ProductType = model.ProductType;
                else
                    return View(productType);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        //Details GET Action method
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();
            var productType = context.ProductTypes.Find(id);
            if (productType == null)
                return NotFound();
            return View(productType);
        }

        //Details Post Action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ProductTypes model)
        {
            return RedirectToAction(nameof(Index));
        }

        //Delete GET Action method
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var productType = context.ProductTypes.Find(id);
            if (productType == null)
                return NotFound();
            context.ProductTypes.Remove(productType);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
