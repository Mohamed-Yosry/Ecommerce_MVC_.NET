using Microsoft.AspNetCore.Mvc;
using OnlineStore_Ecommerce.Data;
using OnlineStore_Ecommerce.Models;

namespace OnlineStore_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialTagsController : Controller
    {
        private readonly ApplicationDbContext context;

        public SpecialTagsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View(context.SpecialTags.ToList());
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
        public IActionResult Create(SpecialTags model)
        {
            if (ModelState.IsValid)
            {
                context.SpecialTags.Add(model);
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
            var SpecialTag = context.SpecialTags.Find(id);
            if (SpecialTag == null)
                return NotFound();
            return View(SpecialTag);
        }

        //Edit Post Action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SpecialTags model)
        {
            if (ModelState.IsValid)
            {
                var SpecialTag = context.SpecialTags.Find(model.Id);
                if (SpecialTag != null)
                    SpecialTag.Name = model.Name;
                else
                    return View(SpecialTag);
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
            var SpecialTag = context.SpecialTags.Find(id);
            if (SpecialTag == null)
                return NotFound();
            return View(SpecialTag);
        }

        //Details Post Action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(SpecialTags model)
        {
            return RedirectToAction(nameof(Index));
        }

        //Delete GET Action method
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var SpecialTag = context.SpecialTags.Find(id);
            if (SpecialTag == null)
                return NotFound();
            context.SpecialTags.Remove(SpecialTag);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

