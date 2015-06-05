using PontoRemoto.Application.Domain;

namespace PontoRemoto.Web.Api.V1.Models
{
    public class ClientViewModel
    {
        public ClientViewModel()
        {
        }

        public ClientViewModel(Client client)
        {
            Id = client.Id;
            Identification = client.Identification;
            Name = client.Name;
            WorkerIdentificationLabel = client.WorkerIdentificationLabel;
        }

        public int Id { get; set; }

        public string Identification { get; set; }

        public string Name { get; set; }

        public string WorkerIdentificationLabel { get; set; }
    }
}