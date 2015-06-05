using PontoRemoto.Application.Domain;
using System.ComponentModel.DataAnnotations;

namespace PontoRemoto.Web.Api.V1.Models
{
    public class CreateCheckinViewModel
    {
        public int ClientId { get; set; }

        [Required]
        public string DeviceId { get; set; }

        [Required]
        public CheckinType Type { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}