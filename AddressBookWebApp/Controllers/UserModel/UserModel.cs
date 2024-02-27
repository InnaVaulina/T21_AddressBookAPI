using System.ComponentModel.DataAnnotations;

namespace AddressBookWebApp.Controllers.UserModel
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "LoginProp")]
        public string LoginProp { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterViewModelForAdmin : RegisterViewModel
    {

        public string UserRole { get; set; }
    }



    public class EntryViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string LoginProp { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }


    }


    public class UserRole
    { 
        public string Role { get; set; }

    }


    public class UserModelForAdmin
    {

        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
    }

}
