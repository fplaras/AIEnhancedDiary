
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using AIJournal.Services;
using AIJournal.Data;
using AIJournal.Models;

namespace AIJournal
{
    class Program
    {
        private static DatabaseService? _databaseService;
        private static OpenAIService? _openAIService;
        private static JournalService? _journalService;

        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting AI Enhanced Journal...");
                
                // Build configuration
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                Console.WriteLine("Configuration loaded successfully.");

                // Setup dependency injection
                var services = new ServiceCollection();
                ConfigureServices(services, configuration);
                var serviceProvider = services.BuildServiceProvider();

                Console.WriteLine("Services configured successfully.");

                // Get services
                _databaseService = serviceProvider.GetRequiredService<DatabaseService>();
                _openAIService = serviceProvider.GetRequiredService<OpenAIService>();
                _journalService = serviceProvider.GetRequiredService<JournalService>();

                Console.WriteLine("Services retrieved successfully.");

                // Ensure database is created with better error handling
                try
                {
                    Console.WriteLine("Attempting to initialize database...");
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<JournalContext>();
                        
                        // Test connection first
                        Console.WriteLine("Testing database connection...");
                        await context.Database.CanConnectAsync();
                        Console.WriteLine("Database connection successful.");
                        
                        // Create database and tables if they don't exist
                        Console.WriteLine("Ensuring database and tables are created...");
                        await context.Database.EnsureCreatedAsync();
                        Console.WriteLine("Database schema verified/created successfully.");
                    }
                }
                catch (Exception dbEx)
                {
                    Console.WriteLine($"Database initialization error: {dbEx.Message}");
                    if (dbEx.InnerException != null)
                    {
                        Console.WriteLine($"Inner exception: {dbEx.InnerException.Message}");
                    }
                    throw;
                }

                Console.WriteLine("Database initialized successfully.");

                Console.WriteLine("=== AI Enhanced Journal ===");
                Console.WriteLine("Welcome to your personal AI-powered journal!");
                Console.WriteLine();

                await ShowMainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting application: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Add DbContext for Azure SQL Database
            services.AddDbContext<JournalContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Add services
            services.AddScoped<DatabaseService>();
            services.AddScoped<JournalService>();
            services.AddSingleton<OpenAIService>(provider =>
                new OpenAIService(configuration["OpenAI:ApiKey"]!));
        }

        static async Task ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Main Menu ---");
                Console.WriteLine("1. Create new journal entry");
                Console.WriteLine("2. View all entries");
                Console.WriteLine("3. Search entries");
                Console.WriteLine("4. Get AI insights for an entry");
                Console.WriteLine("5. Generate journal prompts");
                Console.WriteLine("6. Exit");
                Console.Write("Choose an option (1-6): ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await CreateNewEntry();
                        break;
                    case "2":
                        await ViewAllEntries();
                        break;
                    case "3":
                        await SearchEntries();
                        break;
                    case "4":
                        await GetAIInsights();
                        break;
                    case "5":
                        await GeneratePrompts();
                        break;
                    case "6":
                        Console.WriteLine("Thank you for using AI Enhanced Journal. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static async Task CreateNewEntry()
        {
            Console.WriteLine("\n--- Create New Journal Entry ---");
            
            Console.Write("Enter title: ");
            var title = Console.ReadLine() ?? "";
            
            Console.Write("Enter your mood (optional): ");
            var mood = Console.ReadLine();
            
            Console.WriteLine("Enter your journal content (press Enter twice to finish):");
            var content = "";
            var line = "";
            var emptyLineCount = 0;
            
            while (emptyLineCount < 2)
            {
                line = Console.ReadLine() ?? "";
                if (string.IsNullOrEmpty(line))
                {
                    emptyLineCount++;
                }
                else
                {
                    emptyLineCount = 0;
                }
                content += line + Environment.NewLine;
            }

            var entry = new JournalEntry
            {
                Title = title,
                Content = content.Trim(),
                CreatedAt = DateTime.Now,
                Mood = mood
            };

            // Get AI insights
            Console.WriteLine("Generating AI insights...");
            var insights = await _openAIService!.GetInsights($"Please analyze this journal entry and provide insights: {content}", "");
            entry.AIInsights = insights;

            var entryId = await _databaseService!.AddEntryAsync(entry);
            
            Console.WriteLine($"\nEntry created successfully with ID: {entryId}");
            Console.WriteLine("\nAI Insights:");
            Console.WriteLine(insights);
        }

        static async Task ViewAllEntries()
        {
            Console.WriteLine("\n--- All Journal Entries ---");
            
            var entries = await _databaseService!.GetAllEntriesAsync();
            
            if (!entries.Any())
            {
                Console.WriteLine("No entries found.");
                return;
            }

            foreach (var entry in entries)
            {
                Console.WriteLine($"\n[{entry.Id}] {entry.Title}");
                Console.WriteLine($"Date: {entry.CreatedAt:yyyy-MM-dd HH:mm}");
                if (!string.IsNullOrEmpty(entry.Mood))
                    Console.WriteLine($"Mood: {entry.Mood}");
                Console.WriteLine($"Content: {entry.Content.Substring(0, Math.Min(100, entry.Content.Length))}...");
                Console.WriteLine(new string('-', 50));
            }
        }

        static async Task SearchEntries()
        {
            Console.Write("\nEnter search term: ");
            var searchTerm = Console.ReadLine() ?? "";
            
            var entries = await _databaseService!.SearchEntriesAsync(searchTerm);
            
            if (!entries.Any())
            {
                Console.WriteLine("No entries found matching your search.");
                return;
            }

            Console.WriteLine($"\nFound {entries.Count} entries:");
            foreach (var entry in entries)
            {
                Console.WriteLine($"\n[{entry.Id}] {entry.Title}");
                Console.WriteLine($"Date: {entry.CreatedAt:yyyy-MM-dd HH:mm}");
                Console.WriteLine($"Content: {entry.Content.Substring(0, Math.Min(100, entry.Content.Length))}...");
            }
        }

        static async Task GetAIInsights()
        {
            Console.Write("\nEnter entry ID to get insights for: ");
            if (int.TryParse(Console.ReadLine(), out int entryId))
            {
                var entry = await _databaseService!.GetEntryByIdAsync(entryId);
                if (entry != null)
                {
                    Console.WriteLine($"\nEntry: {entry.Title}");
                    Console.WriteLine($"Content: {entry.Content}");
                    
                    if (!string.IsNullOrEmpty(entry.AIInsights))
                    {
                        Console.WriteLine("\nExisting AI Insights:");
                        Console.WriteLine(entry.AIInsights);
                    }
                    else
                    {
                        Console.WriteLine("\nGenerating new AI insights...");
                        var insights = await _openAIService!.GetInsights($"Please analyze this journal entry: {entry.Content}", "");
                        
                        entry.AIInsights = insights;
                        entry.UpdatedAt = DateTime.Now;
                        await _databaseService.UpdateEntryAsync(entry);
                        
                        Console.WriteLine("\nAI Insights:");
                        Console.WriteLine(insights);
                    }
                }
                else
                {
                    Console.WriteLine("Entry not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid entry ID.");
            }
        }

        static async Task GeneratePrompts()
        {
            Console.Write("\nEnter your current mood (optional): ");
            var mood = Console.ReadLine();
            
            Console.Write("Enter a topic you'd like to explore (optional): ");
            var topic = Console.ReadLine();
            
            Console.WriteLine("\nGenerating journal prompts...");
            var prompts = await _openAIService!.GeneratePrompts(mood ?? "", topic ?? "");
            
            Console.WriteLine("\nHere are some journal prompts for you:");
            Console.WriteLine(prompts);
        }
    }
}