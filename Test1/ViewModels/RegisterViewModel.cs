using System.ComponentModel.DataAnnotations;

namespace Test1.ViewModels
{
    public class RegisterViewModel
    {
        [Required, Display(Name = "Почта")]
        public string Email { get; set; }

        [Required, Display(Name = "Имя")]
        public string Name { get; set; }


        [Required, Display(Name = "Год рождения")]
        public int Year { get; set; }


        [Required, DataType(DataType.Password), Display(Name ="Пароль")]
        public string Password { get; set; }


        [Required,Display(Name = "Подтвердите пароль"), DataType(DataType.Password), Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }


    }
}
