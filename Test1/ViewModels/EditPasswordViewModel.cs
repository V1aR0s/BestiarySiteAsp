using System.ComponentModel.DataAnnotations;

namespace Test1.ViewModels
{
    public class EditPasswordViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }


        [Required, DataType(DataType.Password), Display(Name = "Старый ароль")]
        public string OldPassword { get; set; }


        [Required, Display(Name = "Введите новый пароль"), DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
