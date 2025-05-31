using EstoqueNutris.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstoqueNutris.Repositories.Interfaces
{
    public interface ILinkRepository
    {
        Task<IEnumerable<Link>> GetAllAsync();
        Task<Link?> GetByIdAsync(int id);
        Task<IEnumerable<Link>> GetByUsuarioEscolaIdAsync(int usuarioEscolaId);
        Task<Link> CreateAsync(Link link);
        Task<Link?> UpdateAsync(Link link);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> UsuarioEscolaExistsAsync(int usuarioEscolaId); // Novo método
    }
} 