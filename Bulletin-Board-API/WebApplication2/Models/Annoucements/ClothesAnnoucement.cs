using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models.Annoucements
{
    public class ClothesAnnoucement : Annoucement
    {
        public bool Sex { get; set; }
        public int Size { get; set; }
        public string Color { get; set; }
    }
}
