using System.ComponentModel.DataAnnotations;
using WebAtlas.Models;

namespace WebAtlas.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Не правильная почта")]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Требуется подтверждение пароля")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль не подходит")]
        public string ConfirmPassword { get; set; }
        public string NameLita { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public string? AddressLink { get; set; }
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
    }
}
