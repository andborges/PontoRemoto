using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PontoRemoto.Application.Domain
{
    public class Client
    {
        public Client()
        {
            AppCode = Guid.NewGuid().ToString();
            AppSecret = Guid.NewGuid().ToString().Replace("-", "");
        }

        public int Id { get; set; }

        [Required]
        [Index("IX_Client_Identification", 1, IsUnique = true)]
        [StringLength(256, MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        [Column(TypeName = "VARCHAR")]
        public string Identification { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 5)]
        [Column(TypeName = "VARCHAR")]
        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        [Index("IX_Client_AppCode", 1, IsUnique = true)]
        [StringLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string AppCode { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string AppSecret { get; set; }

        [StringLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string WorkerIdentificationLabel { get; set; }

        [StringLength(1024)]
        [Column(TypeName = "VARCHAR")]
        public string UrlCheckinNotification { get; set; }
    }
}