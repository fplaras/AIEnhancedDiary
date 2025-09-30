
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AIJournal.Data;
using AIJournal.Models;

namespace AIJournal.Services
{
    public class DatabaseService
    {
        private readonly JournalContext _context;

        public DatabaseService(JournalContext context)
        {
            _context = context;
        }

        public async Task AddEntry(JournalEntry entry)
        {
            _context.JournalEntries.Add(entry);
            await _context.SaveChangesAsync();
        }

        public async Task<int> AddEntryAsync(JournalEntry entry)
        {
            _context.JournalEntries.Add(entry);
            await _context.SaveChangesAsync();
            return entry.Id;
        }

        public async Task<List<JournalEntry>> GetEntries()
        {
            return await _context.JournalEntries
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<JournalEntry>> GetAllEntriesAsync()
        {
            return await _context.JournalEntries
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<JournalEntry?> GetEntryByIdAsync(int id)
        {
            return await _context.JournalEntries
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<JournalEntry>> SearchEntriesAsync(string searchTerm)
        {
            return await _context.JournalEntries
                .Where(e => e.Title.Contains(searchTerm) || 
                           e.Content.Contains(searchTerm) ||
                           (e.Mood != null && e.Mood.Contains(searchTerm)))
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> UpdateEntryAsync(JournalEntry entry)
        {
            try
            {
                _context.JournalEntries.Update(entry);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteEntryAsync(int id)
        {
            try
            {
                var entry = await _context.JournalEntries.FindAsync(id);
                if (entry == null)
                    return false;

                _context.JournalEntries.Remove(entry);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
