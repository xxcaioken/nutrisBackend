using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EstoqueNutris.Models.DTOs;
using EstoqueNutris.Services.Interfaces;

namespace EstoqueNutris.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class EscolaController : ControllerBase
    {
        private readonly IEscolaService _escolaService;

        public EscolaController(IEscolaService escolaService)
        {
            _escolaService = escolaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EscolaDTO>>> GetAll()
        {
            var escolas = await _escolaService.GetAllAsync();
            return Ok(escolas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EscolaDTO>> GetById(string id)
        {
            var escola = await _escolaService.GetByIdAsync(id);
            if (escola == null)
                return NotFound();

            return Ok(escola);
        }

        [HttpPost]
        public async Task<ActionResult<EscolaDTO>> Create(EscolaDTO escola)
        {
            var created = await _escolaService.CreateAsync(escola);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EscolaDTO>> Update(string id, EscolaDTO escola)
        {
            var updated = await _escolaService.UpdateAsync(id, escola);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _escolaService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
} 