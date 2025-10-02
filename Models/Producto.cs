using System.ComponentModel.DataAnnotations;

namespace OrdersApi.Models;

public class Producto
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } = null!;

    [Required]
    public decimal Precio { get; set; }
}
