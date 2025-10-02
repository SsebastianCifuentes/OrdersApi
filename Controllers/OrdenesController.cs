using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersApi.Data;
using OrdersApi.Models;

namespace OrdersApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdenesController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdenesController(AppDbContext context)
    {
        _context = context;
    }

    // GET /api/ordenes?pageNumber=1&pageSize=5
    [HttpGet]
    public async Task<ActionResult> GetOrdenes(int pageNumber = 1, int pageSize = 5)
    {
        // Total de órdenes para calcular total de páginas
        var totalOrdenes = await _context.Ordenes.CountAsync();

        var totalPages = (int)Math.Ceiling(totalOrdenes / (double)pageSize);

        // Obtener órdenes con productos paginadas
        var ordenes = await _context.Ordenes
            .Include(o => o.OrdenProductos)
            .ThenInclude(op => op.Producto)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new 
        { 
            TotalCount = totalOrdenes, 
            PageNumber = pageNumber, 
            PageSize = pageSize, 
            TotalPages = totalPages, 
            Data = ordenes 
        });
    }

    // GET /api/ordenes/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Orden>> GetOrden(int id)
    {
        var orden = await _context.Ordenes
            .Include(o => o.OrdenProductos)
            .ThenInclude(op => op.Producto)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (orden == null)
            return NotFound();

        return Ok(orden);
    }

    // POST /api/ordenes
    [HttpPost]
    public async Task<ActionResult<Orden>> CrearOrden(Orden nuevaOrden)
    {
        // Calcular total
        decimal total = 0;
        foreach (var op in nuevaOrden.OrdenProductos)
        {
            var producto = await _context.Productos.FindAsync(op.ProductoId);
            if (producto == null)
                return BadRequest($"Producto con Id {op.ProductoId} no existe.");

            op.Producto = producto;
            total += producto.Precio * op.Cantidad;
        }

        // Aplicar descuentos
        if (total > 500) total *= 0.9m; // 10% si supera 500
        if (nuevaOrden.OrdenProductos.Select(op => op.ProductoId).Distinct().Count() > 5) 
            total *= 0.95m; // 5% adicional si > 5 productos distintos

        nuevaOrden.Total = total;
        nuevaOrden.FechaCreacion = DateTime.UtcNow;

        _context.Ordenes.Add(nuevaOrden);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrden), new { id = nuevaOrden.Id }, nuevaOrden);
    }

    // PUT /api/ordenes/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarOrden(int id, Orden ordenActualizada)
    {
        if (id != ordenActualizada.Id)
            return BadRequest();

        _context.Entry(ordenActualizada).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Ordenes.Any(o => o.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE /api/ordenes/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> BorrarOrden(int id)
    {
        var orden = await _context.Ordenes
            .Include(o => o.OrdenProductos)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (orden == null)
            return NotFound();

        _context.OrdenProductos.RemoveRange(orden.OrdenProductos);
        _context.Ordenes.Remove(orden);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
