using Microsoft.EntityFrameworkCore;
using EstoqueNutris.Data;
using EstoqueNutris.Models;
using EstoqueNutris.Repositories.Interfaces;

namespace EstoqueNutris.Repositories
{
    public class EscolaRepository : IEscolaRepository
    {
        private readonly ApplicationDbContext _context;

        public EscolaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Escola>> GetAllAsync()
        {
            return await _context.Escolas
                .Include(e => e.UsuarioEscolas)
                .ToListAsync();
        }

        public async Task<Escola?> GetByIdAsync(string id)
        {
            return await _context.Escolas
                .Include(e => e.UsuarioEscolas)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Escola> CreateAsync(Escola escola)
        {
            escola.DataCriacao = DateTime.UtcNow;
            _context.Escolas.Add(escola);
            await _context.SaveChangesAsync();
            return escola;
        }

        public async Task<Escola?> UpdateAsync(Escola escola)
        {
            var existingEscola = await _context.Escolas.FindAsync(escola.Id);
            if (existingEscola == null)
                return null;

            existingEscola.Nome = escola.Nome;
            existingEscola.Endereco = escola.Endereco;
            existingEscola.Telefone = escola.Telefone;
            existingEscola.Email = escola.Email;

            await _context.SaveChangesAsync();
            return existingEscola;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var escola = await _context.Escolas
                .Include(e => e.UsuarioEscolas)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (escola == null)
                return false;

            _context.UsuarioEscolas.RemoveRange(escola.UsuarioEscolas);
            _context.Escolas.Remove(escola);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.Escolas.AnyAsync(e => e.Id == id);
        }
    }
} 