using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace OnlineStore_Ecommerce.Models
{
    public class SpecialTags
    {
        public int Id { get; set; }
        [Required, Display(Name = "Tage")]
        public string Name { get; set; }
    }
}
