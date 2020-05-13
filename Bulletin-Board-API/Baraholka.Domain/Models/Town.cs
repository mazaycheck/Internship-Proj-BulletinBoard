using System.ComponentModel.DataAnnotations;

namespace Baraholka.Domain.Models
{
    public class Town
    {
        public int TownId { get; set; }

        [Required]
        [MaxLength(30)]
        public string Title { get; set; }

        public int CoordX { get; set; }
        public int CoordY { get; set; }
    }
}