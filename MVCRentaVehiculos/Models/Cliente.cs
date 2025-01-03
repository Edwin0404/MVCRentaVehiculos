using System.ComponentModel.DataAnnotations;

namespace MVCRentaVehiculos.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombres Requerido")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Apellidos Requerido")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "DNI Requerido")]
        [Display(Name = "DNI")]
        [MaxLength(8)]
        public string Dni { get; set; }

        [Required(ErrorMessage = "Telefono Requerido")]
        [MaxLength(9)]
        public string Telefono { get; set; }
        
        [Required(ErrorMessage = "Direccion Requerido")]
        public string Direccion { get; set; }
        public string NombreCompleto
        {
            get
            {
                return this.Apellidos + " " + this.Nombres;
            }
        }
    }
}
