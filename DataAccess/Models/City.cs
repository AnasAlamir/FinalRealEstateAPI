using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }

        // Navigation properties (if needed)
        public IEnumerable<Property> Properties { get; set; }
    }

}
