using Medi.Application.DTOs;
using Medi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Medi.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly IPacienteService _service;

        public PacientesController(IPacienteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PacienteDto>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PacienteDto>> GetById(int id)
        {
            var paciente = await _service.GetByIdAsync(id);
            return paciente is null ? NotFound() : Ok(paciente);
        }

        [HttpPost]
        public async Task<ActionResult<PacienteDto>> Create([FromBody] PacienteDto pacienteDto)
        {
            var created = await _service.CreateAsync(pacienteDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PacienteDto pacienteDto)
        {
            var updated = await _service.UpdateAsync(id, pacienteDto);
            if (!updated) return BadRequest("No se pudo actualizar el paciente.");

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