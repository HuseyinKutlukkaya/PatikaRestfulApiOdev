using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace PatikaRestfulApiOdev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly List<Product> _products = new List<Product>
    {
        new Product { Id = 1, Name = "Product 1", Price = 10, Description = "product 1" },
        new Product { Id = 2, Name = "Product 2", Price = 10, Description = "product 2" },
        new Product { Id = 3, Name = "Product 3", Price = 10, Description = "product 3" }
    };

        [HttpGet("list")]
        public ActionResult<List<Product>> Get()
        {
            return _products;
        }


        [HttpGet("{id}")]
        public ActionResult<Product> GetById([FromBody] int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public ActionResult<Product> Post([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _products.Add(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public ActionResult<Product> Put(int id, [FromBody] Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            return Ok(existingProduct);
        }


        [HttpDelete("{id}")]
        public ActionResult<Product> Delete(int id)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            _products.Remove(existingProduct);
            return Ok(existingProduct);
        }

        [HttpPatch("{id}")]
        public ActionResult<Product> Patch(int id, [FromBody] JsonPatchDocument<Product> patch)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            List<JsonPatchError> errors = new List<JsonPatchError>();
            patch.ApplyTo(existingProduct, e => errors.Add(e));

            if (errors.Count > 0)
            {
                return BadRequest(errors);
            }

            return Ok(existingProduct);
        }
    }
}
