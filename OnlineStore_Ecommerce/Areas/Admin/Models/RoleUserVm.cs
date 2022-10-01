using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace OnlineStore_Ecommerce.Areas.Admin.Models
{
    public class RoleUserVm
    {
        [Required,Display(Name ="User")]
        public string UserId { get; set; }
        [Required, Display(Name = "Role")]
        public string RoleId { get; set; }
    }
}
