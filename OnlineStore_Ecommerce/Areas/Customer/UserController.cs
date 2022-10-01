using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineStore_Ecommerce.Data;
using OnlineStore_Ecommerce.Models;

namespace OnlineStore_Ecommerce.Areas.Customer
{
    [Area("Customer")]
    public class UserController : Controller
    {
        UserManager<IdentityUser> userManeger;
        ApplicationDbContext context;
        public UserController(UserManager<IdentityUser> userManeger, ApplicationDbContext context)
        {
            this.userManeger = userManeger;
            this.context = context;
        }
        public IActionResult Index()
        {
            return View(context.ApplicationUsers.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser model)
        {
            if(ModelState.IsValid)
            {
                var result = await userManeger.CreateAsync(model, model.PasswordHash);
                if (result.Succeeded)
                {
                    var isSavedRole = await userManeger.AddToRoleAsync(model, "User");
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string?id)
        {
            var user = await userManeger.FindByIdAsync(id);
            if (id == null)
                return NotFound();
            if (user == null)
                return NotFound();
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            var user = await context.ApplicationUsers.FindAsync(model.Id);
            if (user == null)
                return NotFound();
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            var result = await userManeger.UpdateAsync(user);
            if (result.Succeeded)
            {
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> Detatils(string? id)
        {
            var user = await userManeger.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            return View(user);
        }
        public async Task<IActionResult> Locout(string? id)
        {
            var user = await context.ApplicationUsers.FindAsync(id);
            if (user == null)
                return NotFound();
            user.LockoutEnd = DateTime.Now.AddYears(100);
            int rowAffected = context.SaveChanges();
            if(rowAffected > 0)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }
        public async Task<IActionResult> Activate(string? id)
        {
            var user = await context.ApplicationUsers.FindAsync(id);
            if (user == null)
                return NotFound();
            user.LockoutEnd = DateTime.Now.AddDays(-1);
            int rowAffected = context.SaveChanges();
            if (rowAffected > 0)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }
        public async Task<IActionResult> Delete(string? id)
        {
            var user = await context.ApplicationUsers.FindAsync(id);
            if (user == null)
                return NotFound();
            context.ApplicationUsers.Remove(user);
            int rowAffected = context.SaveChanges();
            if (rowAffected > 0)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }
    }
}
