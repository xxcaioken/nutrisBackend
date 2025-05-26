using EstoqueNutris.Models;

namespace EstoqueNutris.Repositories.Interfaces
{
    public interface IUsuarioEscolaRepository
    {
        Task<IEnumerable<UsuarioEscola>> GetAllAsync();
        Task<UsuarioEscola?> GetByIdAsync(int id);
        Task<IEnumerable<UsuarioEscola>> GetByUsuarioIdAsync(string usuarioId);
        Task<UsuarioEscola> CreateAsync(UsuarioEscola usuarioEscola);
        Task<UsuarioEscola?> UpdateAsync(UsuarioEscola usuarioEscola);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByUsuarioAndEscolaAsync(string usuarioId, string escolaId);
    }
} 