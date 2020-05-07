using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models.Annoucements
{
    public class ElectronicsAnnoucement : Annoucement
    {
        public double ScreenSize { get; set; }
        public string Processor { get; set; }
        public int Ram { get; set; }        
    }
}
