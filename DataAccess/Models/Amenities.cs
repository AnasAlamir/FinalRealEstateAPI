using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Amenities
    {
        public int Id { get; set; }
        public bool HasGarage { get; set; } 
        public bool Two_Stories { get; set; }
        public bool Laundry_Room { get; set; }
        public bool HasPool { get; set; } 
        public bool HasGarden { get; set; } 
        public bool HasElevator { get; set; }
        public bool HasBalcony { get; set; }
        public bool HasParking { get; set; } 
        public bool HasCentralHeating { get; set; } 
        public bool IsFurnished { get; set; } 
    }
}
