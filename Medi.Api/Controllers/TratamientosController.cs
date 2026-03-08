using Medi.Application.DTOs;
using Medi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Medi.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TratamientosController : ControllerBase
    {
        private readonly ITratamientoService _service;

        public TratamientosController(ITratamientoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TratamientoDto>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TratamientoDto>> GetById(int id)
        {
            var tratamiento = await _service.GetByIdAsync(id);
            return tratamiento is null ? NotFound() : Ok(tratamiento);
        }

        [HttpPost]
        public async Task<ActionResult<TratamientoDto>> Create([FromBody] TratamientoDto tratamientoDto)
        {
            var created = await _service.CreateAsync(tratamientoDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TratamientoDto tratamientoDto)
        {
            var updated = await _service.UpdateAsync(id, tratamientoDto);
            if (!updated) return BadRequest("No se pudo actualizar el tratamiento.");

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}