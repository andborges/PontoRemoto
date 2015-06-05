using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace PontoRemoto.Domain
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
    }
}