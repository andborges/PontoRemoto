using PontoRemoto.Application.Resources;
using System.ComponentModel.DataAnnotations;

namespace PontoRemoto.Web.Api.V1.Models
{
    public class CreateWorkerViewModel
    {
        public int ClientId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MandatoryFieldError")]
        [StringLength(256, MinimumLength = 1, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "StringLengthError")]
        public string DeviceId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MandatoryFieldError")]
        [StringLength(256, MinimumLength = 1, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "StringLengthError")]
        public string DeviceModel { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MandatoryFieldError")]
        [StringLength(256, MinimumLength = 1, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "StringLengthError")]
        public string DevicePlatform { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MandatoryFieldError")]
        [StringLength(256, MinimumLength = 5, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "StringLengthError")]
        public string DeviceAlias { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MandatoryFieldError")]
        [StringLength(256, MinimumLength = 5, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "StringLengthError")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MandatoryFieldError")]
        [StringLength(256, MinimumLength = 1, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "StringLengthError")]
        public string Identification { get; set; }
    }
}