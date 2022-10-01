using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore_Ecommerce.Data;
using OnlineStore_Ecommerce.Models;
using OnlineStore_Ecommerce.Utility;

namespace OnlineStore_Ecommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext context;
        public OrderController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        [ActionName("CheckOut")]
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                foreach(var product in products)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.PorductId = product.Id;
                    order.OrderDetails.Add(orderDetails);
                }
            }
            order.OrderNo = GetOrderCount();
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            HttpContext.Session.Set("products", new List<Products>());
            return View();
        }
        public string GetOrderCount()
        {
            int count = context.Orders.ToList().Count() + 1;
            return count.ToString("000");
        }
    }
}
