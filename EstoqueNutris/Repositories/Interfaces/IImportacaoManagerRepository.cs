using EstoqueNutris.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstoqueNutris.Repositories.Interfaces
{
    public interface IImportacaoManagerRepository
    {
        Task<IEnumerable<ImportacaoManager>> GetAllAsync();
        Task<ImportacaoManager?> GetByIdAsync(int id);
        Task<IEnumerable<ImportacaoManager>> GetByUsuarioEscolaIdAsync(int usuarioEscolaId);
        Task<ImportacaoManager> CreateAsync(ImportacaoManager importacaoManager);
        Task<ImportacaoManager?> UpdateAsync(ImportacaoManager importacaoManager);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> UsuarioEscolaExistsAsync(int usuarioEscolaId);
        Task<IEnumerable<ImportacaoManager>> GetAtivosAsync();
        Task<bool> UpdateUltimaExecucaoAsync(int id);
    }
} 