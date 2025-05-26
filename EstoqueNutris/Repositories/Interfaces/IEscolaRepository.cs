using EstoqueNutris.Models;

namespace EstoqueNutris.Repositories.Interfaces
{
    public interface IEscolaRepository
    {
        Task<IEnumerable<Escola>> GetAllAsync();
        Task<Escola?> GetByIdAsync(string id);
        Task<Escola> CreateAsync(Escola escola);
        Task<Escola?> UpdateAsync(Escola escola);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
    }
} 