
using System;
using System.ComponentModel.DataAnnotations;

namespace AIJournal.Models
{
    public class JournalEntry
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
        
        [MaxLength(50)]
        public string? Mood { get; set; }
        
        [MaxLength(500)]
        public string? Tags { get; set; }
        
        public string? AIInsights { get; set; }
        
        // For backward compatibility if you had Date property before
        public DateTime Date => CreatedAt;
    }
}
