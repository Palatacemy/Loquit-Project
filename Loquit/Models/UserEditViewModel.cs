using Loquit.Data.Entities;

namespace Loquit.Web.Models
{
    public class UserEditViewModel : AppUser
    {
        public IFormFile? Picture { get; set; }
    }
}
