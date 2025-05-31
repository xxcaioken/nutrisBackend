using EstoqueNutris.DTOs;
using EstoqueNutris.Models.DTOs;
using EstoqueNutris.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstoqueNutris.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LinkController : ControllerBase
    {
        private readonly ILinkService _linkService;
        private readonly ILogger<LinkController> _logger;

        public LinkController(ILinkService linkService, ILogger<LinkController> logger)
        {
            _linkService = linkService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<LinkDto>>> GetAll()
        {
            _logger.LogInformation("Recebida requisição para buscar todos os links.");
            var links = await _linkService.GetAllAsync();
            return Ok(links);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LinkDto>> GetById(int id)
        {
            _logger.LogInformation("Recebida requisição para buscar link por Id: {LinkId}", id);
            var link = await _linkService.GetByIdAsync(id);
            if (link == null)
            {
                _logger.LogWarning("Link com Id {LinkId} não encontrado.", id);
                return NotFound(new { message = "Link não encontrado" });
            }
            return Ok(link);
        }

        [HttpGet("usuarioescola/{usuarioEscolaId}")]
        public async Task<ActionResult<IEnumerable<LinkDto>>> GetByUsuarioEscolaId(int usuarioEscolaId)
        {
            _logger.LogInformation("Recebida requisição para buscar links por UsuarioEscolaId: {UsuarioEscolaId}", usuarioEscolaId);
            var links = await _linkService.GetByUsuarioEscolaIdAsync(usuarioEscolaId);
            return Ok(links);
        }

        [HttpPost]
        public async Task<ActionResult<LinkDto>> Create([FromBody] CreateLinkDto createLinkDto)
        {
            _logger.LogInformation("Recebida requisição para criar novo link.");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var linkDto = await _linkService.CreateAsync(createLinkDto);
                _logger.LogInformation("Link com Id {LinkId} criado com sucesso.", linkDto.Id);
                return CreatedAtAction(nameof(GetById), new { id = linkDto.Id }, linkDto);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Falha ao criar link: {ErrorMessage}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar link.");
                return StatusCode(500, new { message = "Ocorreu um erro interno ao tentar criar o link." });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LinkDto>> Update(int id, [FromBody] UpdateLinkDto updateLinkDto)
        {
            _logger.LogInformation("Recebida requisição para atualizar link com Id: {LinkId}", id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var linkDto = await _linkService.UpdateAsync(id, updateLinkDto);
                if (linkDto == null)
                {
                    _logger.LogWarning("Link com Id {LinkId} não encontrado para atualização.", id);
                    return NotFound(new { message = "Link não encontrado" });
                }
                _logger.LogInformation("Link com Id {LinkId} atualizado com sucesso.", linkDto.Id);
                return Ok(linkDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar o link com Id {LinkId}.", id);
                return StatusCode(500, new { message = "Ocorreu um erro interno ao tentar atualizar o link." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Recebida requisição para deletar link com Id: {LinkId}", id);
            try
            {
                var success = await _linkService.DeleteAsync(id);
                if (!success)
                {
                    _logger.LogWarning("Link com Id {LinkId} não encontrado para exclusão.", id);
                    return NotFound(new { message = "Link não encontrado" });
                }
                _logger.LogInformation("Link com Id {LinkId} deletado com sucesso.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar o link com Id {LinkId}.", id);
                return StatusCode(500, new { message = "Ocorreu um erro interno ao tentar deletar o link." });
            }
        }
    }
} 