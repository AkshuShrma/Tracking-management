using System.ComponentModel.DataAnnotations;

namespace Tracker.Models
{
    public class Tracking
    {
        [Key]
        public string TrackingId { get; set; } = Guid.NewGuid().ToString();
        public DateTime TrackingDate { get; set; }
        public int BookId { get; set; } = 0;
        public Book? Book { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
