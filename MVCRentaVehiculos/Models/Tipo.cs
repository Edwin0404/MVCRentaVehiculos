using System.ComponentModel.DataAnnotations;

namespace MVCRentaVehiculos.Models
{
    public class Tipo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre requerido")]
        public string Nombre { get; set; }
    }
}
