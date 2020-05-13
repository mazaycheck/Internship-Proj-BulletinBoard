using System.ComponentModel.DataAnnotations;

namespace Baraholka.Domain.Models
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