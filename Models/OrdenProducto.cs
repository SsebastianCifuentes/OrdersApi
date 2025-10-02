namespace OrdersApi.Models;

public class OrdenProducto
{
    public int Id { get; set; }

    public int OrdenId { get; set; }

    public Orden? Orden { get; set; }

    public int ProductoId { get; set; }

    public Producto? Producto { get; set; }

    public int Cantidad { get; set; } = 1;
}

