using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models.Annoucements
{
    public class VehicleAnnoucement : Annoucement
    {
        public int VehicleAnnoucementId { get; set; }                
        public string EngineType { get; set; }
        public int Mileage { get; set; }
        public string Color { get; set; }
        public int Year { get; set; }
    }
}
