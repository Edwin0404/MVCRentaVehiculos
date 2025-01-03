using System.ComponentModel.DataAnnotations;

namespace MVCRentaVehiculos.Models
{
    public class Marca
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre Requerido")]
        public string Nombre { get; set; }
    }
}
