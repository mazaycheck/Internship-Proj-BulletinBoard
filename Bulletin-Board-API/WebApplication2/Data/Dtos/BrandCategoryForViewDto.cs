﻿namespace WebApplication2.Data.Dtos
{
    public class BrandCategoryForViewDto
    {
        public int BrandCategoryId { get; set; }
        public int BrandId { get; set; }
        public string BrandTitle { get; set; }
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
    }
}