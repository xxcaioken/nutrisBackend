using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EstoqueNutris.Models.DTOs;
using EstoqueNutris.Services.Interfaces;

namespace EstoqueNutris.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuarioEscolaController : ControllerBase
    {
        private readonly IUsuarioEscolaService _usuarioEscolaService;

        public UsuarioEscolaController(IUsuarioEscolaService usuarioEscolaService)
        {
            _usuarioEscolaService = usuarioEscolaService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UsuarioEscolaDTO>>> GetAll()
        {
            var usuarioEscolas = await _usuarioEscolaService.GetAllAsync();
            return Ok(usuarioEscolas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioEscolaDTO>> GetById(int id)
        {
            var usuarioEscola = await _usuarioEscolaService.GetByIdAsync(id);
            if (usuarioEscola == null)
                return NotFound();

            return Ok(usuarioEscola);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<UsuarioEscolaDTO>>> GetByUsuarioId(string usuarioId)
        {
            var usuarioEscolas = await _usuarioEscolaService.GetByUsuarioIdAsync(usuarioId);
            return Ok(usuarioEscolas);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UsuarioEscolaDTO>> Create(UsuarioEscolaDTO usuarioEscola)
        {
            var created = await _usuarioEscolaService.CreateAsync(usuarioEscola);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UsuarioEscolaDTO>> Update(int id, UsuarioEscolaDTO usuarioEscola)
        {
            var updated = await _usuarioEscolaService.UpdateAsync(id, usuarioEscola);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _usuarioEscolaService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
} 