using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Baraholka.Domain.Models
{
    public class Annoucement
    {
        public int AnnoucementId { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Maximum title lenght is 100 characters")]
        public string Title { get; set; }

        [MaxLength(2000, ErrorMessage = "Advert description can contain only 2000 characters")]
        public string Description { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CreateDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExpirationDate { get; set; }

        [Required]
        [Range(1, 9999999)]
        public int Price { get; set; }

        public virtual List<Photo> Photos { get; set; }
        public bool IsActive { get; set; }

        [Required]
        public int BrandCategoryId { get; set; }

        public BrandCategory BrandCategory { get; set; }

        public Annoucement()
        {
        }
    }
}