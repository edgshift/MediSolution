using Microsoft.AspNetCore.Mvc;
using Medi.Infrastructure.Interfaces;
using Medi.Domain.Entities;

namespace Medi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SesionesController : ControllerBase
    {
        private readonly ISesionRepository _repo;

        public SesionesController(ISesionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sesion>>> GetAll()
            => Ok(await _repo.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Sesion>> GetById(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Sesion>> Create(Sesion sesion)
        {
            var created = await _repo.AddAsync(sesion);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Sesion sesion)
        {
            if (id != sesion.Id) return BadRequest("El ID no coincide.");

            var result = await _repo.UpdateAsync(sesion);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var result = await _repo.SoftDeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}