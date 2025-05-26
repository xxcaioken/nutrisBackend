using EstoqueNutris.Models.DTOs;

namespace EstoqueNutris.Services.Interfaces
{
    public interface IEscolaService
    {
        Task<IEnumerable<EscolaDTO>> GetAllAsync();
        Task<EscolaDTO?> GetByIdAsync(string id);
        Task<EscolaDTO> CreateAsync(EscolaDTO escola);
        Task<EscolaDTO?> UpdateAsync(string id, EscolaDTO escola);
        Task<bool> DeleteAsync(string id);
    }
} 