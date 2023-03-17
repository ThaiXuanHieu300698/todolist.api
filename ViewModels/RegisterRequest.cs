using System.ComponentModel.DataAnnotations;

namespace TodoList.Api.ViewModels
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Họ không được để trống")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        public string LastName { get; set; }
    }
}