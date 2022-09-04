using Microsoft.AspNetCore.Identity;

namespace Test1.ViewModels
{
    //МОдель предназначенная для изменения ролей у пользвоателя
    public class ChangeUserRolesDTO
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<IdentityRole> AllRoles { get; set; }

        public IList<string> UserRoles { get; set; }

        public ChangeUserRolesDTO()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
