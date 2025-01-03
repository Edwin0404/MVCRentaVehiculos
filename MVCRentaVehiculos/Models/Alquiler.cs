using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCRentaVehiculos.Models
{
    public class Alquiler
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime FechaAlquiler { get; set; }

        [Required]
        public DateTime FechaDevolucion {get; set; }

        [Required]
        [ForeignKey("VehiculoId")]
        public int VehiculoId { get; set; }
        public virtual Vehiculo Vehiculo { get; set; }

        [Required]
        [ForeignKey("ClienteId")]
        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }

        public string Observaciones { get; set; }

        public double Importe { get; set; }

    }
}
