using EstoqueNutris.Data;
using EstoqueNutris.Models;
using EstoqueNutris.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstoqueNutris.Repositories
{
    public class ImportacaoManagerRepository : IImportacaoManagerRepository
    {
        private readonly ApplicationDbContext _context;

        public ImportacaoManagerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ImportacaoManager> CreateAsync(ImportacaoManager importacaoManager)
        {
            _context.ImportacaoManagers.Add(importacaoManager);
            await _context.SaveChangesAsync();
            return importacaoManager;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var importacaoManager = await _context.ImportacaoManagers.FindAsync(id);
            if (importacaoManager == null)
                return false;

            _context.ImportacaoManagers.Remove(importacaoManager);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ImportacaoManagers.AnyAsync(im => im.Id == id);
        }

        public async Task<IEnumerable<ImportacaoManager>> GetAllAsync()
        {
            return await _context.ImportacaoManagers.ToListAsync();
        }

        public async Task<IEnumerable<ImportacaoManager>> GetAtivosAsync()
        {
            return await _context.ImportacaoManagers
                                 .Where(im => im.IsAtivo)
                                 .ToListAsync();
        }

        public async Task<ImportacaoManager?> GetByIdAsync(int id)
        {
            return await _context.ImportacaoManagers.FindAsync(id);
        }

        public async Task<IEnumerable<ImportacaoManager>> GetByUsuarioEscolaIdAsync(int usuarioEscolaId)
        {
            return await _context.ImportacaoManagers
                                 .Where(im => im.UsuarioEscolaId == usuarioEscolaId)
                                 .ToListAsync();
        }

        public async Task<ImportacaoManager?> UpdateAsync(ImportacaoManager importacaoManager)
        {
            var existingImportacaoManager = await _context.ImportacaoManagers.FindAsync(importacaoManager.Id);
            if (existingImportacaoManager == null)
                return null;

            _context.Entry(existingImportacaoManager).CurrentValues.SetValues(importacaoManager);
            existingImportacaoManager.DataAtualizacao = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return existingImportacaoManager;
        }

        public async Task<bool> UpdateUltimaExecucaoAsync(int id)
        {
            var importacaoManager = await _context.ImportacaoManagers.FindAsync(id);
            if (importacaoManager == null)
                return false;

            importacaoManager.UltimaExecucao = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UsuarioEscolaExistsAsync(int usuarioEscolaId)
        {
            return await _context.UsuarioEscolas.AnyAsync(ue => ue.Id == usuarioEscolaId);
        }
    }
} 