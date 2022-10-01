using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace OnlineStore_Ecommerce.Areas.Admin.Models
{
    public class UserRoleMaping
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
    }
}
