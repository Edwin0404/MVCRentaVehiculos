using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCRentaVehiculos.Models
{
    public class Vehiculo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Color Requerido")]
        public string Color { get; set; }

        [Required(ErrorMessage = "Placa Requerido")]
        //[MaxLength(7)]
        public string Placa { get; set; }

        [Required(ErrorMessage = "Tipo Requerido")]
        public int TipoId { get; set; }
        [ForeignKey("TipoId")]

        public virtual Tipo Tipo { get; set; }

        [Required(ErrorMessage = "Marca Requerido")]
        public int MarcaId { get; set; }
        [ForeignKey("MarcaId")]

        public virtual Marca Marca { get; set; }

        public string? ImagenUrl { get; set; }
    }
}
