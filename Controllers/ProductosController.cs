using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersApi.Data;
using OrdersApi.Models;

namespace OrdersApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly AppDbContext _context;
    public ProductosController(AppDbContext context)
    {
        _context = context;
    }

    // GET /api/productos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
    {
        var productos = await _context.Productos.ToListAsync();
        return Ok(productos);
    }

    // GET /api/productos/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Producto>> GetProducto(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
            return NotFound();
        return Ok(producto);
    }

    // POST /api/productos
    [HttpPost]
    public async Task<ActionResult<Producto>> CrearProducto(Producto producto)
    {
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
    }

    // PUT /api/productos/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarProducto(int id, Producto productoActualizado)
    {
        if (id != productoActualizado.Id)
            return BadRequest();

        _context.Entry(productoActualizado).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Productos.Any(p => p.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE /api/productos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> BorrarProducto(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
            return NotFound();

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
