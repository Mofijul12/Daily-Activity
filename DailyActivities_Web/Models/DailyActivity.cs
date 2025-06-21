using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DailyActivities_Web.Models
{
    public class DailyActivity
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        [Display(Name = "Get Up Time")]
        public TimeSpan? GetUpTime { get; set; }

        [Display(Name = "Sleep Time")]
        public TimeSpan? SleepTime { get; set; }

        public decimal Expense { get; set; }

        [Display(Name = "Did any kind of Bad Work?")]
        public bool BadWork { get; set; }

        public int Salat { get; set; }

        public bool Quran { get; set; }

        public bool Ramadan { get; set; }

        // Link to Identity user
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }
    }
}
