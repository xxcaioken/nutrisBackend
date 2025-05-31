using EstoqueNutris.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstoqueNutris.Services.Interfaces
{
    public interface ILinkService
    {
        Task<IEnumerable<LinkDto>> GetAllAsync();
        Task<LinkDto?> GetByIdAsync(int id);
        Task<IEnumerable<LinkDto>> GetByUsuarioEscolaIdAsync(int usuarioEscolaId);
        Task<LinkDto> CreateAsync(CreateLinkDto linkDto);
        Task<LinkDto?> UpdateAsync(int id, UpdateLinkDto linkDto);
        Task<bool> DeleteAsync(int id);
    }
} 