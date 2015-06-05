using PontoRemoto.Application.Domain;
using PontoRemoto.Application.Resources;
using System.ComponentModel.DataAnnotations;

namespace PontoRemoto.Web.Models
{
    public class ClientConfigurationViewModel
    {
        public ClientConfigurationViewModel()
        {
        }

        public ClientConfigurationViewModel(Client client)
        {
            if (client == null) return;

            Id = client.Id;
            WorkerIdentificationLabel = client.WorkerIdentificationLabel;
            UrlCheckinNotification = client.UrlCheckinNotification;
            AppCode = client.AppCode;
            AppSecret = client.AppSecret;
        }

        public int Id { get; set; }

        [StringLength(100, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "StringLengthError")]
        [Display(ResourceType = typeof(Labels), Name = "WorkerIdentificationLabel")]
        public string WorkerIdentificationLabel { get; set; }

        [StringLength(1024, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "StringLengthError")]
        [Display(ResourceType = typeof(Labels), Name = "UrlCheckinNotification")]
        public string UrlCheckinNotification { get; set; }

        public string AppCode { get; set; }

        public string AppSecret { get; set; }
    }
}