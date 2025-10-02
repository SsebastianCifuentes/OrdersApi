using System.ComponentModel.DataAnnotations;

namespace OrdersApi.Models;

public class Orden
{
    public int Id { get; set; }

    [Required]
    public string Cliente { get; set; } = null!;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public decimal Total { get; set; }

    public List<OrdenProducto> OrdenProductos { get; set; } = new();
}
