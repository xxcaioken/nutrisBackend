using EstoqueNutris.Models;
using EstoqueNutris.Models.DTOs;
using EstoqueNutris.Repositories.Interfaces;
using EstoqueNutris.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstoqueNutris.Services
{
    public class LinkService : ILinkService
    {
        private readonly ILinkRepository _linkRepository;
        private readonly ILogger<LinkService> _logger; // Adicionado para logging

        public LinkService(ILinkRepository linkRepository, ILogger<LinkService> logger)
        {
            _linkRepository = linkRepository;
            _logger = logger;
        }

        public async Task<LinkDto> CreateAsync(CreateLinkDto linkDto)
        {
            _logger.LogInformation("Tentando criar um novo link para UsuarioEscolaId: {UsuarioEscolaId}", linkDto.UsuarioEscolaId);

            if (!await _linkRepository.UsuarioEscolaExistsAsync(linkDto.UsuarioEscolaId))
            {
                _logger.LogWarning("UsuarioEscolaId {UsuarioEscolaId} não encontrado ao tentar criar link.", linkDto.UsuarioEscolaId);
                throw new InvalidOperationException("A relação Usuário-Escola especificada não existe.");
            }

            var link = new Link
            {
                Nome = linkDto.Nome,
                Url = linkDto.Url,
                Descricao = linkDto.Descricao,
                UsuarioEscolaId = linkDto.UsuarioEscolaId,
                DataCriacao = DateTime.UtcNow
            };

            var createdLink = await _linkRepository.CreateAsync(link);
            _logger.LogInformation("Link com Id {LinkId} criado com sucesso.", createdLink.Id);
            return MapToDto(createdLink);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Tentando deletar link com Id: {LinkId}", id);
            if (!await _linkRepository.ExistsAsync(id))
            {
                _logger.LogWarning("Link com Id {LinkId} não encontrado para exclusão.", id);
                return false;
            }
            var result = await _linkRepository.DeleteAsync(id);
            if (result)
            {
                _logger.LogInformation("Link com Id {LinkId} deletado com sucesso.", id);
            }
            else
            {
                _logger.LogError("Falha ao deletar link com Id {LinkId}.", id);
            }
            return result;
        }

        public async Task<IEnumerable<LinkDto>> GetAllAsync()
        {
            _logger.LogInformation("Buscando todos os links.");
            var links = await _linkRepository.GetAllAsync();
            return links.Select(MapToDto);
        }

        public async Task<LinkDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Buscando link com Id: {LinkId}", id);
            var link = await _linkRepository.GetByIdAsync(id);
            if (link == null)
            {
                _logger.LogWarning("Link com Id {LinkId} não encontrado.", id);
                return null;
            }
            return MapToDto(link);
        }

        public async Task<IEnumerable<LinkDto>> GetByUsuarioEscolaIdAsync(int usuarioEscolaId)
        {
            _logger.LogInformation("Buscando links para UsuarioEscolaId: {UsuarioEscolaId}", usuarioEscolaId);
            var links = await _linkRepository.GetByUsuarioEscolaIdAsync(usuarioEscolaId);
            return links.Select(MapToDto);
        }

        public async Task<LinkDto?> UpdateAsync(int id, UpdateLinkDto linkDto)
        {
            _logger.LogInformation("Tentando atualizar link com Id: {LinkId}", id);
            var existingLink = await _linkRepository.GetByIdAsync(id);
            if (existingLink == null)
            {
                _logger.LogWarning("Link com Id {LinkId} não encontrado para atualização.", id);
                return null;
            }

            existingLink.Nome = linkDto.Nome ?? existingLink.Nome;
            existingLink.Url = linkDto.Url ?? existingLink.Url;
            existingLink.Descricao = linkDto.Descricao ?? existingLink.Descricao;
            existingLink.DataAtualizacao = DateTime.UtcNow;

            var updatedLink = await _linkRepository.UpdateAsync(existingLink);
            if (updatedLink != null)
            {
                _logger.LogInformation("Link com Id {LinkId} atualizado com sucesso.", updatedLink.Id);
                return MapToDto(updatedLink);
            }
            _logger.LogError("Falha ao atualizar link com Id {LinkId}.", id);
            return null;
        }

        private static LinkDto MapToDto(Link link)
        {
            return new LinkDto
            {
                Id = link.Id,
                Nome = link.Nome,
                Url = link.Url,
                Descricao = link.Descricao,
                UsuarioEscolaId = link.UsuarioEscolaId,
                DataCriacao = link.DataCriacao,
                DataAtualizacao = link.DataAtualizacao
            };
        }
    }
} 