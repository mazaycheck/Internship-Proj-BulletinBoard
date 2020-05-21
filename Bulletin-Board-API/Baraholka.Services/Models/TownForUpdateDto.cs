using System.ComponentModel.DataAnnotations;

namespace Baraholka.Services.Models
{
    public class TownUpdateModel
    {
        [Required]
        public int TownId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public int CoordX { get; set; }
        public int CoordY { get; set; }
    }
}