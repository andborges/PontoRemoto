using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PontoRemoto.Application.Domain
{
    public class Worker
    {
        public long Id { get; set; }

        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        [Required]
        [StringLength(256)]
        [Column(TypeName = "VARCHAR")]
        public string DeviceId { get; set; }

        [Required]
        [StringLength(256)]
        [Column(TypeName = "VARCHAR")]
        public string DeviceModel { get; set; }

        [Required]
        [StringLength(256)]
        [Column(TypeName = "VARCHAR")]
        public string DevicePlatform { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 5)]
        [Column(TypeName = "VARCHAR")]
        public string DeviceAlias { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 5)]
        [Column(TypeName = "VARCHAR")]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        [Column(TypeName = "VARCHAR")]
        public string Identification { get; set; }

        public WorkerStatus Status { get; set; }
    }
}