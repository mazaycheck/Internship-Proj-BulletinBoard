using System.ComponentModel.DataAnnotations;

namespace Baraholka.Data.Dtos
{
    public class TownForCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public int CoordX { get; set; }
        public int CoordY { get; set; }
    }
}