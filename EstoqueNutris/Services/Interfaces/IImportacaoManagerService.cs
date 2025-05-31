using EstoqueNutris.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstoqueNutris.Services.Interfaces
{
    public interface IImportacaoManagerService
    {
        Task<IEnumerable<ImportacaoManagerDto>> GetAllAsync();
        Task<ImportacaoManagerDto?> GetByIdAsync(int id);
        Task<IEnumerable<ImportacaoManagerDto>> GetByUsuarioEscolaIdAsync(int usuarioEscolaId);
        Task<ImportacaoManagerDto> CreateAsync(CreateImportacaoManagerDto importacaoManagerDto);
        Task<ImportacaoManagerDto?> UpdateAsync(int id, UpdateImportacaoManagerDto importacaoManagerDto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ImportacaoManagerDto>> GetAtivosAsync();
        Task<bool> ExecutarImportacaoAsync(int id);
    }
} 