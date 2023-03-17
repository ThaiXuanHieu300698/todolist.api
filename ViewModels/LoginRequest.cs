using System.ComponentModel.DataAnnotations;

namespace TodoList.Api.ViewModels
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        [Required(ErrorMessage="Mật khẩu không được để trống")]
        public string Password { get; set; }
    }
}