using PontoRemoto.Application.Resources;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PontoRemoto.Web.Models
{
    public class CreateClientViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MandatoryFieldError")]
        [StringLength(256, MinimumLength = 5, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "StringLengthError")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "ClientIdentificationInvalid")]
        [Remote("UniqueIdentification", "Client", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "ClientIdentificationAlreadyExists")]
        [Display(ResourceType = typeof(Labels), Name = "Identification")]
        public string Identification { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MandatoryFieldError")]
        [StringLength(256, MinimumLength = 5, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "StringLengthError")]
        [Display(ResourceType = typeof(Labels), Name = "FullName")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MandatoryFieldError")]
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "EmailAddressInvalidError")]
        [Display(ResourceType = typeof(Labels), Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MandatoryFieldError")]
        [Display(ResourceType = typeof(Labels), Name = "Company")]
        public string ClientName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MandatoryFieldError")]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "StringLengthError")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Labels), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Labels), Name = "PasswordConfirm")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "PasswordNotMatch")]
        public string ConfirmPassword { get; set; }
    }
}
