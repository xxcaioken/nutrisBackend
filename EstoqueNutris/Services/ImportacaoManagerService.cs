using EstoqueNutris.DTOs;
using EstoqueNutris.Models;
using EstoqueNutris.Repositories.Interfaces;
using EstoqueNutris.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstoqueNutris.Services
{
    public class ImportacaoManagerService : IImportacaoManagerService
    {
        private readonly IImportacaoManagerRepository _importacaoManagerRepository;
        private readonly ILogger<ImportacaoManagerService> _logger;

        public ImportacaoManagerService(IImportacaoManagerRepository importacaoManagerRepository, ILogger<ImportacaoManagerService> logger)
        {
            _importacaoManagerRepository = importacaoManagerRepository;
            _logger = logger;
        }

        public async Task<ImportacaoManagerDto> CreateAsync(CreateImportacaoManagerDto importacaoManagerDto)
        {
            _logger.LogInformation("Tentando criar um novo ImportacaoManager para UsuarioEscolaId: {UsuarioEscolaId}", importacaoManagerDto.UsuarioEscolaId);

            if (!await _importacaoManagerRepository.UsuarioEscolaExistsAsync(importacaoManagerDto.UsuarioEscolaId))
            {
                _logger.LogWarning("UsuarioEscolaId {UsuarioEscolaId} não encontrado ao tentar criar ImportacaoManager.", importacaoManagerDto.UsuarioEscolaId);
                throw new InvalidOperationException("A relação Usuário-Escola especificada não existe.");
            }

            var importacaoManager = new ImportacaoManager
            {
                Nome = importacaoManagerDto.Nome,
                PlanilhaOrigemUrl = importacaoManagerDto.PlanilhaOrigemUrl,
                PlanilhaDestinoUrl = importacaoManagerDto.PlanilhaDestinoUrl,
                CelulasMapping = importacaoManagerDto.CelulasMapping,
                Descricao = importacaoManagerDto.Descricao,
                IsAtivo = importacaoManagerDto.IsAtivo,
                UsuarioEscolaId = importacaoManagerDto.UsuarioEscolaId,
                DataCriacao = DateTime.UtcNow
            };

            var createdImportacaoManager = await _importacaoManagerRepository.CreateAsync(importacaoManager);
            _logger.LogInformation("ImportacaoManager com Id {ImportacaoManagerId} criado com sucesso.", createdImportacaoManager.Id);
            return MapToDto(createdImportacaoManager);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Tentando deletar ImportacaoManager com Id: {ImportacaoManagerId}", id);
            if (!await _importacaoManagerRepository.ExistsAsync(id))
            {
                _logger.LogWarning("ImportacaoManager com Id {ImportacaoManagerId} não encontrado para exclusão.", id);
                return false;
            }
            var result = await _importacaoManagerRepository.DeleteAsync(id);
            if (result)
            {
                _logger.LogInformation("ImportacaoManager com Id {ImportacaoManagerId} deletado com sucesso.", id);
            }
            else
            {
                _logger.LogError("Falha ao deletar ImportacaoManager com Id {ImportacaoManagerId}.", id);
            }
            return result;
        }

        public async Task<bool> ExecutarImportacaoAsync(int id)
        {
            _logger.LogInformation("Executando importação para ImportacaoManager com Id: {ImportacaoManagerId}", id);
            var importacaoManager = await _importacaoManagerRepository.GetByIdAsync(id);
            if (importacaoManager == null || !importacaoManager.IsAtivo)
            {
                _logger.LogWarning("ImportacaoManager com Id {ImportacaoManagerId} não encontrado ou inativo.", id);
                return false;
            }

            var result = await _importacaoManagerRepository.UpdateUltimaExecucaoAsync(id);
            if (result)
            {
                _logger.LogInformation("Importação executada com sucesso para ImportacaoManager Id {ImportacaoManagerId}.", id);
            }
            return result;
        }

        public async Task<IEnumerable<ImportacaoManagerDto>> GetAllAsync()
        {
            _logger.LogInformation("Buscando todos os ImportacaoManagers.");
            var importacaoManagers = await _importacaoManagerRepository.GetAllAsync();
            return importacaoManagers.Select(MapToDto);
        }

        public async Task<IEnumerable<ImportacaoManagerDto>> GetAtivosAsync()
        {
            _logger.LogInformation("Buscando ImportacaoManagers ativos.");
            var importacaoManagers = await _importacaoManagerRepository.GetAtivosAsync();
            return importacaoManagers.Select(MapToDto);
        }

        public async Task<ImportacaoManagerDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Buscando ImportacaoManager com Id: {ImportacaoManagerId}", id);
            var importacaoManager = await _importacaoManagerRepository.GetByIdAsync(id);
            if (importacaoManager == null)
            {
                _logger.LogWarning("ImportacaoManager com Id {ImportacaoManagerId} não encontrado.", id);
                return null;
            }
            return MapToDto(importacaoManager);
        }

        public async Task<IEnumerable<ImportacaoManagerDto>> GetByUsuarioEscolaIdAsync(int usuarioEscolaId)
        {
            _logger.LogInformation("Buscando ImportacaoManagers para UsuarioEscolaId: {UsuarioEscolaId}", usuarioEscolaId);
            var importacaoManagers = await _importacaoManagerRepository.GetByUsuarioEscolaIdAsync(usuarioEscolaId);
            return importacaoManagers.Select(MapToDto);
        }

        public async Task<ImportacaoManagerDto?> UpdateAsync(int id, UpdateImportacaoManagerDto importacaoManagerDto)
        {
            _logger.LogInformation("Tentando atualizar ImportacaoManager com Id: {ImportacaoManagerId}", id);
            var existingImportacaoManager = await _importacaoManagerRepository.GetByIdAsync(id);
            if (existingImportacaoManager == null)
            {
                _logger.LogWarning("ImportacaoManager com Id {ImportacaoManagerId} não encontrado para atualização.", id);
                return null;
            }

            existingImportacaoManager.Nome = importacaoManagerDto.Nome ?? existingImportacaoManager.Nome;
            existingImportacaoManager.PlanilhaOrigemUrl = importacaoManagerDto.PlanilhaOrigemUrl ?? existingImportacaoManager.PlanilhaOrigemUrl;
            existingImportacaoManager.PlanilhaDestinoUrl = importacaoManagerDto.PlanilhaDestinoUrl ?? existingImportacaoManager.PlanilhaDestinoUrl;
            existingImportacaoManager.CelulasMapping = importacaoManagerDto.CelulasMapping ?? existingImportacaoManager.CelulasMapping;
            existingImportacaoManager.Descricao = importacaoManagerDto.Descricao ?? existingImportacaoManager.Descricao;
            existingImportacaoManager.IsAtivo = importacaoManagerDto.IsAtivo ?? existingImportacaoManager.IsAtivo;
            existingImportacaoManager.DataAtualizacao = DateTime.UtcNow;

            var updatedImportacaoManager = await _importacaoManagerRepository.UpdateAsync(existingImportacaoManager);
            if (updatedImportacaoManager != null)
            {
                _logger.LogInformation("ImportacaoManager com Id {ImportacaoManagerId} atualizado com sucesso.", updatedImportacaoManager.Id);
                return MapToDto(updatedImportacaoManager);
            }
            _logger.LogError("Falha ao atualizar ImportacaoManager com Id {ImportacaoManagerId}.", id);
            return null;
        }

        private static ImportacaoManagerDto MapToDto(ImportacaoManager importacaoManager)
        {
            return new ImportacaoManagerDto
            {
                Id = importacaoManager.Id,
                Nome = importacaoManager.Nome,
                PlanilhaOrigemUrl = importacaoManager.PlanilhaOrigemUrl,
                PlanilhaDestinoUrl = importacaoManager.PlanilhaDestinoUrl,
                CelulasMapping = importacaoManager.CelulasMapping,
                Descricao = importacaoManager.Descricao,
                IsAtivo = importacaoManager.IsAtivo,
                UsuarioEscolaId = importacaoManager.UsuarioEscolaId,
                DataCriacao = importacaoManager.DataCriacao,
                DataAtualizacao = importacaoManager.DataAtualizacao,
                UltimaExecucao = importacaoManager.UltimaExecucao
            };
        }
    }
} 