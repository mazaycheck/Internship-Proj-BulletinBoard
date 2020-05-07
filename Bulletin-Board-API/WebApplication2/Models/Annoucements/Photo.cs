namespace WebApplication2.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public string PhotoUrl { get; set; }
        public virtual Annoucement Annoucement { get; set; }
    }
}