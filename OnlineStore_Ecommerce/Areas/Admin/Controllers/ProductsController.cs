using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineStore_Ecommerce.Data;
using OnlineStore_Ecommerce.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace OnlineStore_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IHostingEnvironment hostingEnvironment;
        public ProductsController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            this.context = context;
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(context.Products.Include(P=>P.ProductType).Include(S=>S.SpecialTag).ToList());
        }
        [HttpPost]
        public IActionResult Index(decimal? lowAmount,decimal? largeAmount)
        {
            if(lowAmount == null || largeAmount == null)
            {
                return View(context.Products.Include(P=>P.ProductType).Include(S => S.SpecialTag).ToList());
            }
            return View(context.Products.Include(P => P.ProductType).Include(S => S.SpecialTag)
                .Where(c=>c.Price>=lowAmount && c.Price<=largeAmount).ToList());
        }

        //Create GET Action method
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["productTypeId"] = new SelectList(context.ProductTypes.ToList(),"Id","ProductType");
            ViewData["TagsId"] = new SelectList(context.SpecialTags.ToList(), "Id", "Name");
            return View();
        }

        //Create Post Action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products model,IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if(image != null)
                {
                    var path = Path.Combine(hostingEnvironment.WebRootPath + "\\Images\\" + Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(path,FileMode.Create));
                    model.Image = "Images\\" +  image.FileName;
                }
                context.Products.Add(model);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            var errors = ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .Select(x => new { x.Key, x.Value.Errors })
            .ToArray();
            ViewData["productTypeId"] = new SelectList(context.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagsId"] = new SelectList(context.SpecialTags.ToList(), "Id", "Name");
            return View(model);
        }

        //Edit GET Action method
        [HttpGet]
        public IActionResult Edit(int ?id)
        {
            if(id == null)
                return NotFound();
            //var model = context.Products.Find(id);
            var model = context.Products.Include(P => P.ProductType).Include(S => S.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (model == null)
                return NotFound();
            ViewData["productTypeId"] = new SelectList(context.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagsId"] = new SelectList(context.SpecialTags.ToList(), "Id", "Name");
            return View(model);
        }
        //EDit Post Action method
        [HttpPost]
        public async Task<IActionResult> Edit(Products model, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var path = Path.Combine(hostingEnvironment.WebRootPath + "\\Images\\" + Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(path, FileMode.Create));
                    model.Image = "Images\\" + image.FileName;
                }
                var products = context.Products.Find(model.Id);

                products.Name = model.Name;
                products.Price = model.Price;
                products.Image = model.Image;
                products.IsAvailable = model.IsAvailable;
                products.ProductColor = model.ProductColor;
                products.ProductType = model.ProductType;
                products.ProductTypeId = model.ProductTypeId;
                products.SpecialTag = model.SpecialTag;
                products.SpecialTagId = model.SpecialTagId;

                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            var errors = ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .Select(x => new { x.Key, x.Value.Errors })
            .ToArray();
            ViewData["productTypeId"] = new SelectList(context.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagsId"] = new SelectList(context.SpecialTags.ToList(), "Id", "Name");
            return View(model);
        }
        //Details GET Action method
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();
            var model = context.Products.Include(P => P.ProductType).Include(S => S.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (model == null)
                return NotFound();
            ViewData["productTypeId"] = new SelectList(context.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagsId"] = new SelectList(context.SpecialTags.ToList(), "Id", "Name");
            return View(model);
        }
        //Details Post Action method
        [HttpPost]
        public IActionResult Details(ProductTypes model)
        {
            return RedirectToAction(nameof(Index));
        }
        //Delete
        public IActionResult Delete(int? id)
        {
            var products = context.Products.Find(id);
            if(products != null)
            {
                context.Products.Remove(products);
                context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
