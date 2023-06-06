using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.Service.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace P013EStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IService<Category> _service;

        public CategoriesController(IService<Category> service)
        {
            _service = service;
        }
        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<IEnumerable<Category>> GetAsync()
        {
            return await _service.GetAllAsync();
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public async Task<Category> Get(int id)
        {
            return await _service.FindAsync(id);
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<Category> PostAsync([FromBody] Category value)
        {
            await _service.AddAsync(value);
            await _service.SaveAsync();
            return value;
        }

        // PUT api/<CategoriesController>/5
        [HttpPut]
        public async Task<ActionResult> PutAsync(int id, [FromBody] Category value)
        {
            _service.Update(value);
            int sonuc = await _service.SaveAsync();
            if (sonuc > 0)
            {
                return Ok(value);
            }
            return StatusCode(StatusCodes.Status304NotModified);
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var kayit = await _service.FindAsync(id);
            if (kayit == null)
            {
                return NotFound();
            }
            else
            {
                _service.Delete(kayit);
                await _service.SaveAsync();
                return Ok(kayit);
            }
        }
    }
}
