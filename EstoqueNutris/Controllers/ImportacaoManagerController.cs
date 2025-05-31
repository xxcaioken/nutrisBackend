using EstoqueNutris.DTOs;
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
    public class ImportacaoManagerController : ControllerBase
    {
        private readonly IImportacaoManagerService _importacaoManagerService;
        private readonly ILogger<ImportacaoManagerController> _logger;

        public ImportacaoManagerController(IImportacaoManagerService importacaoManagerService, ILogger<ImportacaoManagerController> logger)
        {
            _importacaoManagerService = importacaoManagerService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ImportacaoManagerDto>>> GetAll()
        {
            _logger.LogInformation("Recebida requisição para buscar todos os ImportacaoManagers.");
            var importacaoManagers = await _importacaoManagerService.GetAllAsync();
            return Ok(importacaoManagers);
        }

        [HttpGet("ativos")]
        public async Task<ActionResult<IEnumerable<ImportacaoManagerDto>>> GetAtivos()
        {
            _logger.LogInformation("Recebida requisição para buscar ImportacaoManagers ativos.");
            var importacaoManagers = await _importacaoManagerService.GetAtivosAsync();
            return Ok(importacaoManagers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ImportacaoManagerDto>> GetById(int id)
        {
            _logger.LogInformation("Recebida requisição para buscar ImportacaoManager por Id: {ImportacaoManagerId}", id);
            var importacaoManager = await _importacaoManagerService.GetByIdAsync(id);
            if (importacaoManager == null)
            {
                _logger.LogWarning("ImportacaoManager com Id {ImportacaoManagerId} não encontrado.", id);
                return NotFound(new { message = "ImportacaoManager não encontrado" });
            }
            return Ok(importacaoManager);
        }

        [HttpGet("usuarioescola/{usuarioEscolaId}")]
        public async Task<ActionResult<IEnumerable<ImportacaoManagerDto>>> GetByUsuarioEscolaId(int usuarioEscolaId)
        {
            _logger.LogInformation("Recebida requisição para buscar ImportacaoManagers por UsuarioEscolaId: {UsuarioEscolaId}", usuarioEscolaId);
            var importacaoManagers = await _importacaoManagerService.GetByUsuarioEscolaIdAsync(usuarioEscolaId);
            return Ok(importacaoManagers);
        }

        [HttpPost]
        public async Task<ActionResult<ImportacaoManagerDto>> Create([FromBody] CreateImportacaoManagerDto createImportacaoManagerDto)
        {
            _logger.LogInformation("Recebida requisição para criar novo ImportacaoManager.");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var importacaoManagerDto = await _importacaoManagerService.CreateAsync(createImportacaoManagerDto);
                _logger.LogInformation("ImportacaoManager com Id {ImportacaoManagerId} criado com sucesso.", importacaoManagerDto.Id);
                return CreatedAtAction(nameof(GetById), new { id = importacaoManagerDto.Id }, importacaoManagerDto);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Falha ao criar ImportacaoManager: {ErrorMessage}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar ImportacaoManager.");
                return StatusCode(500, new { message = "Ocorreu um erro interno ao tentar criar o ImportacaoManager." });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ImportacaoManagerDto>> Update(int id, [FromBody] UpdateImportacaoManagerDto updateImportacaoManagerDto)
        {
            _logger.LogInformation("Recebida requisição para atualizar ImportacaoManager com Id: {ImportacaoManagerId}", id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var importacaoManagerDto = await _importacaoManagerService.UpdateAsync(id, updateImportacaoManagerDto);
                if (importacaoManagerDto == null)
                {
                    _logger.LogWarning("ImportacaoManager com Id {ImportacaoManagerId} não encontrado para atualização.", id);
                    return NotFound(new { message = "ImportacaoManager não encontrado" });
                }
                _logger.LogInformation("ImportacaoManager com Id {ImportacaoManagerId} atualizado com sucesso.", importacaoManagerDto.Id);
                return Ok(importacaoManagerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar o ImportacaoManager com Id {ImportacaoManagerId}.", id);
                return StatusCode(500, new { message = "Ocorreu um erro interno ao tentar atualizar o ImportacaoManager." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Recebida requisição para deletar ImportacaoManager com Id: {ImportacaoManagerId}", id);
            try
            {
                var success = await _importacaoManagerService.DeleteAsync(id);
                if (!success)
                {
                    _logger.LogWarning("ImportacaoManager com Id {ImportacaoManagerId} não encontrado para exclusão.", id);
                    return NotFound(new { message = "ImportacaoManager não encontrado" });
                }
                _logger.LogInformation("ImportacaoManager com Id {ImportacaoManagerId} deletado com sucesso.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar o ImportacaoManager com Id {ImportacaoManagerId}.", id);
                return StatusCode(500, new { message = "Ocorreu um erro interno ao tentar deletar o ImportacaoManager." });
            }
        }

        [HttpPost("{id}/executar")]
        public async Task<IActionResult> ExecutarImportacao(int id)
        {
            _logger.LogInformation("Recebida requisição para executar importação do ImportacaoManager com Id: {ImportacaoManagerId}", id);
            try
            {
                var success = await _importacaoManagerService.ExecutarImportacaoAsync(id);
                if (!success)
                {
                    _logger.LogWarning("ImportacaoManager com Id {ImportacaoManagerId} não encontrado ou inativo para execução.", id);
                    return NotFound(new { message = "ImportacaoManager não encontrado ou inativo" });
                }
                _logger.LogInformation("Importação executada com sucesso para ImportacaoManager Id {ImportacaoManagerId}.", id);
                return Ok(new { message = "Importação executada com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao executar importação do ImportacaoManager com Id {ImportacaoManagerId}.", id);
                return StatusCode(500, new { message = "Ocorreu um erro interno ao tentar executar a importação." });
            }
        }
    }
} 