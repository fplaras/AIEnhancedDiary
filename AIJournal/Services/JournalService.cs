using AIJournal.Models;

namespace AIJournal.Services
{
    public class JournalService
    {
        private readonly DatabaseService _databaseService;
        private readonly OpenAIService _openAIService;

        public JournalService(DatabaseService databaseService, OpenAIService openAIService)
        {
            _databaseService = databaseService;
            _openAIService = openAIService;
        }

        public async Task AddEntry(string title, string content, string? mood = null)
        {
            var aiInsights = await _openAIService.GetInsights(content);
            var entry = new JournalEntry
            {
                Title = title,
                Content = content,
                CreatedAt = DateTime.Now,
                Mood = mood,
                AIInsights = aiInsights
            };
            await _databaseService.AddEntryAsync(entry);
        }

        public async Task<IEnumerable<JournalEntry>> GetEntries()
        {
            return await _databaseService.GetAllEntriesAsync();
        }

        public async Task<string> GetInsight(string question)
        {
            var entries = await _databaseService.GetAllEntriesAsync();
            var context = string.Join("\n", entries.Select(e => e.Content));
            return await _openAIService.GetInsights(question, context);
        }

        public async Task<JournalEntry?> GetEntryById(int id)
        {
            return await _databaseService.GetEntryByIdAsync(id);
        }

        public async Task<IEnumerable<JournalEntry>> SearchEntries(string searchTerm)
        {
            return await _databaseService.SearchEntriesAsync(searchTerm);
        }

        public async Task<bool> UpdateEntry(JournalEntry entry)
        {
            entry.UpdatedAt = DateTime.Now;
            return await _databaseService.UpdateEntryAsync(entry);
        }

        public async Task<bool> DeleteEntry(int id)
        {
            return await _databaseService.DeleteEntryAsync(id);
        }

        public async Task<string> GeneratePrompts(string? mood = null, string? topic = null)
        {
            return await _openAIService.GeneratePrompts(mood ?? "", topic ?? "");
        }
    }
}
