using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.Service.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace P013EStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IEnumerable<Product>> GetAsync()
        {
            return await _service.GetProductsByIncludeAsync();
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public async Task<Product> GetAsync(int id)
        {
            return await _service.GetProductByIncludeAsync(id);
        }

        // POST api/<ProductsController>
        [HttpPost]
        public async Task Post([FromBody] Product value)
        {
            await _service.AddAsync(value);
            await _service.SaveAsync();
        }

        // PUT api/<ProductsController>/5
        [HttpPut]
        public async Task<ActionResult> PutAsync(int id, [FromBody] Product value)
        {
            _service.Update(value);
            var sonuc = await _service.SaveAsync();
            if (sonuc > 0)
            {
                return Ok();
            }
            return Problem();
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var data = await _service.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            _service.Delete(data);
            var sonuc = await _service.SaveAsync();
            if (sonuc > 0)
            {
                return Ok(data);
            }
            return Problem();
        }
    }
}
