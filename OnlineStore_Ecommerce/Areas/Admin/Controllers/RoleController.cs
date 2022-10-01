using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore_Ecommerce.Areas.Admin.Models;
using OnlineStore_Ecommerce.Data;
using System.Data;
using System.Runtime.InteropServices;

namespace OnlineStore_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> roleManeger;
        UserManager<IdentityUser> userManeger;
        private readonly ApplicationDbContext context;

        public RoleController(RoleManager<IdentityRole> roleManeger, ApplicationDbContext context, UserManager<IdentityUser> userManeger)
        {
            this.roleManeger = roleManeger;
            this.context = context;
            this.userManeger = userManeger;
        }
        public IActionResult Index()
        {
            var roles = roleManeger.Roles.ToList();
            ViewBag.Roles = roles;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            IdentityRole role = new IdentityRole();
            role.Name = name;
            bool isExist = await roleManeger.RoleExistsAsync(role.Name);
            if (isExist)
            {
                ViewBag.message = "This role Exist";
                return View();
            }
            var result = await roleManeger.CreateAsync(role);
            if (result.Succeeded)
                return RedirectToAction("Index");
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await roleManeger.FindByIdAsync(id);
            if (role == null)
                return NotFound();
            ViewBag.Id = role.Id;
            ViewBag.Name = role.Name;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, string name)
        {
            var role = await roleManeger.FindByIdAsync(id);
            if (role == null)
                return NotFound();
            role.Name = name;
            bool isExist = await roleManeger.RoleExistsAsync(role.Name);
            if (isExist)
            {
                ViewBag.message = "This role Exist";
                ViewBag.Id = role.Id;
                ViewBag.Name = role.Name;
                return View();
            }
            var result = await roleManeger.UpdateAsync(role);
            if (result.Succeeded)
                return RedirectToAction("Index");
            return View();
        }
        public async Task<IActionResult> Delete(string id)
        {
            var role = await roleManeger.FindByIdAsync(id);
            if (role == null)
                return NotFound();
            var result = await roleManeger.DeleteAsync(role);
            if (result.Succeeded)
                return RedirectToAction("Index");
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Assign()
        {
            ViewData["UserId"] = new SelectList(context.ApplicationUsers.Where(f=>f.LockoutEnd<DateTime.Now||f.LockoutEnd==null).ToList(),"Id", "UserName");
            ViewData["RoleId"] = new SelectList(roleManeger.Roles.ToList(), "Name", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Assign(RoleUserVm model)
        {
            var user = context.ApplicationUsers.FirstOrDefault(c=>c.Id==model.UserId);
            var isRoleAssign = await userManeger.IsInRoleAsync(user, model.RoleId);
            if(isRoleAssign)
            {
                ViewData["UserId"] = new SelectList(context.ApplicationUsers.Where(f => f.LockoutEnd < DateTime.Now || f.LockoutEnd == null).ToList(), "Id", "UserName");
                ViewData["RoleId"] = new SelectList(roleManeger.Roles.ToList(), "Name", "Name");
                ViewBag.message = "the user already is assigned";
                return View();
            }
            var role = await userManeger.AddToRoleAsync(user, model.RoleId);
            if(role.Succeeded)
            {
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = new SelectList(context.ApplicationUsers.Where(f => f.LockoutEnd < DateTime.Now || f.LockoutEnd == null).ToList(), "Id", "UserName");
            ViewData["RoleId"] = new SelectList(roleManeger.Roles.ToList(), "Name", "Name");
            return View();
        }
        public ActionResult AssignUserRole()
        {
            var result = from ur in context.UserRoles
                         join r in context.Roles on ur.RoleId equals r.Id
                         join a in context.ApplicationUsers on ur.UserId equals a.Id
                         select new UserRoleMaping { 
                             RoleId = ur.RoleId,
                             UserId = ur.UserId,
                             UserName = a.UserName,
                             RoleName = r.Name
                         };
            ViewBag.UserRoles = result;
            return View();
        }
    }
}
