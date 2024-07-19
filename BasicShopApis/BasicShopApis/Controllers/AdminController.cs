using BasicShopApis.Data;
using BasicShopApis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicShopApis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Admin/Products
        [HttpGet("Products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // POST: api/Admin/Product
        [HttpPost("Product")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
        }

        // PUT: api/Admin/Product/{id}
        [HttpPut("Product/{id}")]
        public async Task<IActionResult> EditProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id || product == null)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Admin/Product/{id}
        [HttpDelete("Product/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Admin/SearchProducts/{query}
        [HttpGet("SearchProducts/{query}")]
        public async Task<IActionResult> SearchProducts(string query)
        {
            var products = await _context.Products
                .Where(p => p.Name.Contains(query))
                .ToListAsync();

            return Ok(products);
        }
    }
}
