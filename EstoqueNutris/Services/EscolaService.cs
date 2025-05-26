using EstoqueNutris.Models;
using EstoqueNutris.Models.DTOs;
using EstoqueNutris.Repositories.Interfaces;
using EstoqueNutris.Services.Interfaces;

namespace EstoqueNutris.Services
{
    public class EscolaService : IEscolaService
    {
        private readonly IEscolaRepository _repository;

        public EscolaService(IEscolaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EscolaDTO>> GetAllAsync()
        {
            var escolas = await _repository.GetAllAsync();
            return escolas.Select(MapToDTO);
        }

        public async Task<EscolaDTO?> GetByIdAsync(string id)
        {
            var escola = await _repository.GetByIdAsync(id);
            return escola != null ? MapToDTO(escola) : null;
        }

        public async Task<EscolaDTO> CreateAsync(EscolaDTO dto)
        {
            var escola = new Escola
            {
                Nome = dto.Nome,
                Endereco = dto.Endereco,
                Telefone = dto.Telefone,
                Email = dto.Email
            };

            var created = await _repository.CreateAsync(escola);
            return MapToDTO(created);
        }

        public async Task<EscolaDTO?> UpdateAsync(string id, EscolaDTO dto)
        {
            if (!await _repository.ExistsAsync(id))
                return null;

            var escola = new Escola
            {
                Id = id,
                Nome = dto.Nome,
                Endereco = dto.Endereco,
                Telefone = dto.Telefone,
                Email = dto.Email
            };

            var updated = await _repository.UpdateAsync(escola);
            return updated != null ? MapToDTO(updated) : null;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static EscolaDTO MapToDTO(Escola escola)
        {
            return new EscolaDTO
            {
                Id = escola.Id,
                Nome = escola.Nome,
                Endereco = escola.Endereco,
                Telefone = escola.Telefone,
                Email = escola.Email,
                DataCriacao = escola.DataCriacao
            };
        }
    }
} 