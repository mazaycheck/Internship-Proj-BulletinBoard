namespace Baraholka.Contracts
{
    public class BrandCategoryDto
    {
        public int BrandCategoryId { get; set; }  
        public int BrandId { get; set; }
        public BrandDto Brand { get; set; }
        public int CategoryId { get; set; }
        public CategoryDto Category { get; set; }
    }
}
