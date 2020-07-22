using System.ComponentModel.DataAnnotations;

namespace TinyURl.MVC.Models.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage ="Не указанo Имя")]
        public string Name { get; set; }
         
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}