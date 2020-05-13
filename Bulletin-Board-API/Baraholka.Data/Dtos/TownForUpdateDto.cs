using System.ComponentModel.DataAnnotations;

namespace Baraholka.Data.Dtos
{
    public class TownForUpdateDto
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