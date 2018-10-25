using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Template.Core;
using Template.Model;

namespace Template.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IValueService _service;

        public ValuesController(IValueService service)
        {
            _service = service;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ValueModel>>> Get()
        {
            return Ok(await _service.GetAsync());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ValueModel>> Get(long id)
        {
            return Ok(await _service.GetAsync(id));
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody] ValueModel value)
        {
            await _service.CreateAsync(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task Put(long id, [FromBody] ValueModel value)
        {
            await _service.UpdateAsync(id, value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task Delete(long id)
        {
            await _service.DeleteAsync(id);
        }
    }
}
