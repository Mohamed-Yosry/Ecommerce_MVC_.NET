using System.ComponentModel.DataAnnotations;

namespace OnlineStore_Ecommerce.Models
{
    public class ProductTypes
    {
        public int Id { get; set; }
        [Required,Display(Name = "Product Type")]
        public string ProductType { get; set; }
    }
}
