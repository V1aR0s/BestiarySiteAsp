using Microsoft.AspNetCore.Identity;

namespace Test1.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
        public string? AboutMe { get; set; }

        public string? RegDate { get; set; }


        public string? NameFile { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
