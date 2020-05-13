using System.ComponentModel.DataAnnotations;

namespace Baraholka.Web.Data.Dtos
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