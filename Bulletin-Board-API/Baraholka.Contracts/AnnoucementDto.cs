using System;
using System.Collections.Generic;

namespace Baraholka.Contracts
{
    public class AnnoucementDto
    {
        public int AnnoucementId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public virtual UserDto User { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int Price { get; set; }
        public List<PhotoDto> Photos { get; set; }
        public bool IsActive { get; set; }
        public int BrandCategoryId { get; set; }
        public BrandCategoryDto BrandCategory { get; set; }
    }
}