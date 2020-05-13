namespace Baraholka.Web.Data.Dtos
{
    public class BrandCategoryDto
    {
        public int BrandCategoryId { get; set; }
        public int BrandId { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}