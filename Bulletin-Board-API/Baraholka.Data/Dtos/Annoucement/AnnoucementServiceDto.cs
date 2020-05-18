using Baraholka.Domain.Models;
using System;
using System.Collections.Generic;

namespace Baraholka.Data.Dtos.Annoucement
{
    public class AnnoucementServiceDto
    {
        public int AnnoucementId { get; set; }      
        public string Title { get; set; }        
        public string Description { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }       
        public DateTime? CreateDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int Price { get; set; }
        public List<Photo> Photos { get; set; }
        public bool IsActive { get; set; }      
        public int BrandCategoryId { get; set; }
        public BrandCategory BrandCategory { get; set; }
    }
}
