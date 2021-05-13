using Microsoft.AspNetCore.Identity;

namespace PoliceWebApplication.Models
{
    public class User : IdentityUser
    {
        public string Code { get; set; }
    }
}
