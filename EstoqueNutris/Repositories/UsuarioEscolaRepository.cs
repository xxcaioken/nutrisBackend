using Microsoft.EntityFrameworkCore;
using EstoqueNutris.Data;
using EstoqueNutris.Models;
using EstoqueNutris.Repositories.Interfaces;

namespace EstoqueNutris.Repositories
{
    public class UsuarioEscolaRepository : IUsuarioEscolaRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioEscolaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsuarioEscola>> GetAllAsync()
        {
            return await _context.UsuarioEscolas
                .Include(ue => ue.Usuario)
                .Include(ue => ue.Escola)
                .ToListAsync();
        }

        public async Task<UsuarioEscola?> GetByIdAsync(int id)
        {
            return await _context.UsuarioEscolas
                .Include(ue => ue.Usuario)
                .Include(ue => ue.Escola)
                .FirstOrDefaultAsync(ue => ue.Id == id);
        }

        public async Task<IEnumerable<UsuarioEscola>> GetByUsuarioIdAsync(string usuarioId)
        {
            return await _context.UsuarioEscolas
                .Include(ue => ue.Escola)
                .Where(ue => ue.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<UsuarioEscola> CreateAsync(UsuarioEscola usuarioEscola)
        {
            usuarioEscola.DataCriacao = DateTime.UtcNow;
            _context.UsuarioEscolas.Add(usuarioEscola);
            await _context.SaveChangesAsync();
            return usuarioEscola;
        }

        public async Task<UsuarioEscola?> UpdateAsync(UsuarioEscola usuarioEscola)
        {
            var existingRelacao = await _context.UsuarioEscolas.FindAsync(usuarioEscola.Id);
            if (existingRelacao == null)
                return null;

            existingRelacao.EscolaId = usuarioEscola.EscolaId;
            existingRelacao.LinkPlanilha = usuarioEscola.LinkPlanilha;
            existingRelacao.DataAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingRelacao;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usuarioEscola = await _context.UsuarioEscolas.FindAsync(id);
            if (usuarioEscola == null)
                return false;

            _context.UsuarioEscolas.Remove(usuarioEscola);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.UsuarioEscolas.AnyAsync(ue => ue.Id == id);
        }

        public async Task<bool> ExistsByUsuarioAndEscolaAsync(string usuarioId, string escolaId)
        {
            return await _context.UsuarioEscolas
                .AnyAsync(ue => ue.UsuarioId == usuarioId && ue.EscolaId == escolaId);
        }
    }
} 