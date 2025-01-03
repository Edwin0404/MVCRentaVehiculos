using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCRentaVehiculos.Models;

namespace MVCRentaVehiculos.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Marca> Marca { get; set; }
        public DbSet<Tipo> Tipo { get; set; }
        public DbSet<Vehiculo> Vehiculo { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Alquiler> Alquiler { get; set; }
    }
}