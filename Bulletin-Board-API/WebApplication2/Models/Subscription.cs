using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        [Required]
        public virtual User User { get; set; }
        [Required]
        public string Description { get; set; }
    }
}