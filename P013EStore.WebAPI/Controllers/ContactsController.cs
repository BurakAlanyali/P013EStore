using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.Service.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace P013EStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IService<Contact> _service;

        public ContactsController(IService<Contact> service)
        {
            _service = service;
        }
        // GET: api/<ContactsController>
        [HttpGet]
        public IEnumerable<Contact> Get()
        {
            return _service.GetAll();
        }

        // GET api/<ContactsController>/5
        [HttpGet("{id}")]
        public Contact Get(int id)
        {
            return _service.Find(id);
        }

        // POST api/<ContactsController>
        [HttpPost]
        public async Task<Contact> PostAsync([FromBody] Contact value)
        {
            await _service.AddAsync(value);
            await _service.SaveAsync();
            return value;
        }

        // PUT api/<ContactsController>/5
        [HttpPut]
        public async Task<ActionResult> PutAsync(int id, [FromBody] Contact value)
        {
            _service.Update(value);
            int sonuc = await _service.SaveAsync();
            if (sonuc > 0)
            {
                return Ok(value);
            }
            return StatusCode(StatusCodes.Status304NotModified);
        }

        // DELETE api/<ContactsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var kayit = await _service.FindAsync(id);
            if (kayit == null)
            {
                return NoContent();
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
