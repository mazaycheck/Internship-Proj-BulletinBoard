using System.ComponentModel.DataAnnotations;

namespace Baraholka.Web.Models
{
    public class TownCreateModel
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public int CoordX { get; set; }
        public int CoordY { get; set; }
    }
}