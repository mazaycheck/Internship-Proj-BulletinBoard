using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
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