using PontoRemoto.Application.Domain;
using PontoRemoto.Application.Resources;
using System.ComponentModel.DataAnnotations;

namespace PontoRemoto.Web.Models
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
            Status = worker.Status;
        }

        public long Id { get; set; }

        [Required]
        [StringLength(256)]
        [Display(ResourceType = typeof(Labels), Name = "DeviceId")]
        public string DeviceId { get; set; }

        [Required]
        [StringLength(256)]
        [Display(ResourceType = typeof(Labels), Name = "DeviceModel")]
        public string DeviceModel { get; set; }

        [Required]
        [StringLength(256)]
        [Display(ResourceType = typeof(Labels), Name = "DevicePlatform")]
        public string DevicePlatform { get; set; }

        [Required]
        [StringLength(256)]
        [Display(ResourceType = typeof(Labels), Name = "DeviceAlias")]
        public string DeviceAlias { get; set; }

        [Required]
        [StringLength(256)]
        [Display(ResourceType = typeof(Labels), Name = "Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        [Display(ResourceType = typeof(Labels), Name = "Identification")]
        public string Identification { get; set; }

        public WorkerStatus Status { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "Status")]
        public string StatusDescription
        {
            get
            {
                switch (this.Status)
                {
                    case WorkerStatus.New:
                        return Labels.WorkerStatus_New;
                    case WorkerStatus.Granted:
                        return Labels.WorkerStatus_Granted;
                    case WorkerStatus.Revoked:
                        return Labels.WorkerStatus_Revoked;
                }

                return null;
            }
        }
    }
}