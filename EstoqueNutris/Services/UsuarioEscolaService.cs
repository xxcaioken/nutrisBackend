using EstoqueNutris.Models;
using EstoqueNutris.Models.DTOs;
using EstoqueNutris.Repositories.Interfaces;
using EstoqueNutris.Services.Interfaces;

namespace EstoqueNutris.Services
{
    public class UsuarioEscolaService : IUsuarioEscolaService
    {
        private readonly IUsuarioEscolaRepository _repository;
        private readonly IEscolaRepository _escolaRepository;

        public UsuarioEscolaService(
            IUsuarioEscolaRepository repository,
            IEscolaRepository escolaRepository)
        {
            _repository = repository;
            _escolaRepository = escolaRepository;
        }

        public async Task<IEnumerable<UsuarioEscolaDTO>> GetAllAsync()
        {
            var usuarioEscolas = await _repository.GetAllAsync();
            return usuarioEscolas.Select(MapToDTO);
        }

        public async Task<UsuarioEscolaDTO?> GetByIdAsync(int id)
        {
            var usuarioEscola = await _repository.GetByIdAsync(id);
            return usuarioEscola != null ? MapToDTO(usuarioEscola) : null;
        }

        public async Task<IEnumerable<UsuarioEscolaDTO>> GetByUsuarioIdAsync(string usuarioId)
        {
            var usuarioEscolas = await _repository.GetByUsuarioIdAsync(usuarioId);
            return usuarioEscolas.Select(MapToDTO);
        }

        public async Task<UsuarioEscolaDTO> CreateAsync(UsuarioEscolaDTO dto)
        {
            if (!await _escolaRepository.ExistsAsync(dto.EscolaId))
                throw new InvalidOperationException("Escola não encontrada.");

            if (await _repository.ExistsByUsuarioAndEscolaAsync(dto.UsuarioId, dto.EscolaId))
                throw new InvalidOperationException("Já existe uma relação entre este usuário e esta escola.");

            var usuarioEscola = new UsuarioEscola
            {
                UsuarioId = dto.UsuarioId,
                EscolaId = dto.EscolaId,
                LinkPlanilha = dto.LinkPlanilha
            };

            var created = await _repository.CreateAsync(usuarioEscola);
            return MapToDTO(created);
        }

        public async Task<UsuarioEscolaDTO?> UpdateAsync(int id, UsuarioEscolaDTO dto)
        {
            if (!await _repository.ExistsAsync(id))
                return null;

            if (!await _escolaRepository.ExistsAsync(dto.EscolaId))
                throw new InvalidOperationException("Escola não encontrada.");

            var usuarioEscola = new UsuarioEscola
            {
                Id = id,
                UsuarioId = dto.UsuarioId,
                EscolaId = dto.EscolaId,
                LinkPlanilha = dto.LinkPlanilha
            };

            var updated = await _repository.UpdateAsync(usuarioEscola);
            return updated != null ? MapToDTO(updated) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static UsuarioEscolaDTO MapToDTO(UsuarioEscola usuarioEscola)
        {
            return new UsuarioEscolaDTO
            {
                Id = usuarioEscola.Id,
                UsuarioId = usuarioEscola.UsuarioId,
                EscolaId = usuarioEscola.EscolaId,
                LinkPlanilha = usuarioEscola.LinkPlanilha,
                DataCriacao = usuarioEscola.DataCriacao,
                DataAtualizacao = usuarioEscola.DataAtualizacao,
                Escola = usuarioEscola.Escola != null ? new EscolaDTO
                {
                    Id = usuarioEscola.Escola.Id,
                    Nome = usuarioEscola.Escola.Nome,
                    Endereco = usuarioEscola.Escola.Endereco,
                    Telefone = usuarioEscola.Escola.Telefone,
                    Email = usuarioEscola.Escola.Email,
                    DataCriacao = usuarioEscola.Escola.DataCriacao
                } : null!
            };
        }
    }
} 