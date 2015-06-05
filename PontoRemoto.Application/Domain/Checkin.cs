using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PontoRemoto.Application.Domain
{
    public class Checkin
    {
        public Checkin()
        {
            Date = DateTime.Now;
        }

        public long Id { get; set; }

        [Required]
        public long WorkerId { get; set; }

        public Worker Worker { get; set; }

        [Required]
        public DateTime Date { get; private set; }

        [Required]
        public CheckinType Type { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        public string Hash { get; set; }

        public bool Notified { get; set; }
    }
}