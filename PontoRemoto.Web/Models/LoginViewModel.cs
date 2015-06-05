using PontoRemoto.Application.Resources;
using System.ComponentModel.DataAnnotations;

namespace PontoRemoto.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Labels), Name = "Email")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Labels), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}