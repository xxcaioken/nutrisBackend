using EstoqueNutris.Models.DTOs;

namespace EstoqueNutris.Services.Interfaces
{
    public interface IUsuarioEscolaService
    {
        Task<IEnumerable<UsuarioEscolaDTO>> GetAllAsync();
        Task<UsuarioEscolaDTO?> GetByIdAsync(int id);
        Task<IEnumerable<UsuarioEscolaDTO>> GetByUsuarioIdAsync(string usuarioId);
        Task<UsuarioEscolaDTO> CreateAsync(UsuarioEscolaDTO usuarioEscola);
        Task<UsuarioEscolaDTO?> UpdateAsync(int id, UsuarioEscolaDTO usuarioEscola);
        Task<bool> DeleteAsync(int id);
    }
} 