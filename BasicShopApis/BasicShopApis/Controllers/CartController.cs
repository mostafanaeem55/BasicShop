using BasicShopApis.Data;
using BasicShopApis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicShopApis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> CreateCart()
        {
            var cart = new Cart();
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCartById), new { id = cart.Id }, cart);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCartById(int id)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }
        // GET: api/Cart/VisibleProducts
        [HttpGet("VisibleProducts")]
        public async Task<IActionResult> GetVisibleProducts()
        {
            var products = await _context.Products
                .Where(p => p.Visible)
                .ToListAsync();

            return Ok(products);
        }

        // POST: api/Cart/AddToCart
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] CartItem cartItem)
        {
            if(cartItem.Id == -1)
            {
                cartItem.Id= null;
            }
            var product = await _context.Products.FindAsync(cartItem.ProductId);
            if (product == null || product.Quantity < cartItem.Quantity)
            {
                return NotFound("Product not found.");
            }

            var existingCartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId && ci.CartId == cartItem.CartId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += cartItem.Quantity;
                if (product.Quantity < existingCartItem.Quantity)
                {
                    return BadRequest("Product quantity exceeds available stock.");
                }
                _context.Entry(existingCartItem).State = EntityState.Modified;
            }
            else
            {
                cartItem.Product = product;
                _context.Entry(cartItem.Product).State = EntityState.Unchanged;
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            return Ok(cartItem);
        }

        // PUT: api/Cart/EditCartItem/{id}
        [HttpPut("EditCartItem/{id}")]
        public async Task<IActionResult> EditCartItem(int id, [FromBody] CartItem cartItem)
        {
            if (id != cartItem.Id || cartItem == null)
            {
                return BadRequest();
            }

            var product = await _context.Products.FindAsync(cartItem.ProductId);
            if (product == null || product.Quantity < cartItem.Quantity)
            {
                return BadRequest("Product not available or quantity exceeds available stock.");
            }

            _context.Entry(cartItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Cart/RemoveFromCart/{id}
        [HttpDelete("RemoveFromCart/{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Cart/GetCartItems/{cartId}
        [HttpGet("GetCartItems/{cartId}")]
        public async Task<IActionResult> GetCartItems(int cartId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .Include(ci => ci.Product)
                .ToListAsync();

            return Ok(cartItems);
        }
        [HttpGet("SearchVisibleProducts")]
        public async Task<IActionResult> SearchVisibleProducts(string searchQuery)
        {
            var products = await _context.Products
                .Where(p => p.Visible)
                .ToListAsync();

            var searchedProducts = products
                .Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(searchedProducts);
        }

    }
}
