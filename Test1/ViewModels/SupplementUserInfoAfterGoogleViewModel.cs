using System.ComponentModel.DataAnnotations;

namespace Test1.ViewModels
{
    public class SupplementUserInfoAfterGoogleViewModel
    {
        public string Id { get; set; }

        [Required, Display(Name = "Логин")]
        public string Name { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Пароль")]
        public string Password { get; set; }


        [Required, Display(Name = "Подтвердите пароль"), DataType(DataType.Password), Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
