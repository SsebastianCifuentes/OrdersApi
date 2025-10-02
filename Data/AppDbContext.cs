using Microsoft.EntityFrameworkCore;
using OrdersApi.Models;

namespace OrdersApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Producto> Productos { get; set; }  // Tabla Productos
    public DbSet<Orden> Ordenes { get; set; }       // Tabla Ordenes
    public DbSet<OrdenProducto> OrdenProductos { get; set; } // Tabla intermedia
}
