using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Data.Dtos
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