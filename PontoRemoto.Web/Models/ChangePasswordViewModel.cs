using PontoRemoto.Application.Resources;
using System.ComponentModel.DataAnnotations;

namespace PontoRemoto.Web.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MandatoryFieldError")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Labels), Name = "CurrentPassword")]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MandatoryFieldError")]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "StringLengthError")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Labels), Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "PasswordNotMatch")]
        [Display(ResourceType = typeof(Labels), Name = "NewPasswordConfirm")]
        public string ConfirmPassword { get; set; }
    }
}