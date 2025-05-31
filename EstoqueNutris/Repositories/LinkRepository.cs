using EstoqueNutris.Data;
using EstoqueNutris.Models;
using EstoqueNutris.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstoqueNutris.Repositories
{
    public class LinkRepository : ILinkRepository
    {
        private readonly ApplicationDbContext _context;

        public LinkRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Link> CreateAsync(Link link)
        {
            _context.Links.Add(link);
            await _context.SaveChangesAsync();
            return link;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var link = await _context.Links.FindAsync(id);
            if (link == null)
                return false;

            _context.Links.Remove(link);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Links.AnyAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Link>> GetAllAsync()
        {
            return await _context.Links.ToListAsync();
        }

        public async Task<Link?> GetByIdAsync(int id)
        {
            return await _context.Links.FindAsync(id);
        }

        public async Task<IEnumerable<Link>> GetByUsuarioEscolaIdAsync(int usuarioEscolaId)
        {
            return await _context.Links
                                 .Where(l => l.UsuarioEscolaId == usuarioEscolaId)
                                 .ToListAsync();
        }

        public async Task<Link?> UpdateAsync(Link link)
        {
            var existingLink = await _context.Links.FindAsync(link.Id);
            if (existingLink == null)
                return null;

            _context.Entry(existingLink).CurrentValues.SetValues(link);
            existingLink.DataAtualizacao = DateTime.UtcNow; 
            await _context.SaveChangesAsync();
            return existingLink;
        }

        public async Task<bool> UsuarioEscolaExistsAsync(int usuarioEscolaId)
        {
            return await _context.UsuarioEscolas.AnyAsync(ue => ue.Id == usuarioEscolaId);
        }
    }
} 