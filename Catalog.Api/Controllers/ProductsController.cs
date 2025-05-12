namespace Catalog.Api.Controllers;

public class ProductsController(ICatalogContext context) : BaseApiController(context)
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _context.Products.Find(_ => true).ToListAsync();
        return Ok(products);
    }

    [HttpGet("{id:length(24)}", Name = "GetProduct")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product>> GetProduct(string id)
    {
        var product = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpGet("[action]/{category}", Name = "GetProductByCategory")]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
    {
        if (string.IsNullOrEmpty(category))
        {
            return BadRequest("Category cannot be null or empty");
        }

        var products = await _context.Products.Find(p => p.Category == category).ToListAsync();
        return Ok(products);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        if (product is null)
        {
            return BadRequest("Product cannot be null");
        }

        await _context.Products.InsertOneAsync(product);
        return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
    }

    [HttpPut(Name = "UpdateProduct")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Product>> UpdateProduct([FromBody] Product product)
    {
        if (product is null)
        {
            return BadRequest("Product cannot be null");
        }

        var result = await _context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
        if (result.IsAcknowledged && result.ModifiedCount > 0)
        {
            return Ok(product);
        }

        return NotFound();
    }

    [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        var result = await _context.Products.DeleteOneAsync(p => p.Id == id);
        if (result.IsAcknowledged && result.DeletedCount > 0)
        {
            return NoContent();
        }

        return NotFound();
    }
}
