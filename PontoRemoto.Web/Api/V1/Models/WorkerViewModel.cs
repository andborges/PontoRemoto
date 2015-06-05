using PontoRemoto.Application.Domain;
using System.ComponentModel.DataAnnotations;

namespace PontoRemoto.Web.Api.V1.Models
{
    public class WorkerViewModel
    {
        public WorkerViewModel()
        {
        }

        public WorkerViewModel(Worker worker)
        {
            Id = worker.Id;
            DeviceId = worker.DeviceId;
            DeviceModel = worker.DeviceModel;
            DevicePlatform = worker.DevicePlatform;
            DeviceAlias = worker.DeviceAlias;
            Name = worker.Name;
            Identification = worker.Identification;
            Status = worker.Status.ToString();
        }

        public long Id { get; set; }

        [Required]
        [StringLength(256)]
        public string DeviceId { get; set; }

        [Required]
        [StringLength(256)]
        public string DeviceModel { get; set; }

        [Required]
        [StringLength(256)]
        public string DevicePlatform { get; set; }

        [Required]
        [StringLength(256)]
        public string DeviceAlias { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        public string Identification { get; set; }

        public string Status { get; set; }
    }
}