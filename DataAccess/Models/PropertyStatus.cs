using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class PropertyStatus
    {
        public int Id { get; set; }
        public string Status { get; set; }

        // Navigation properties
        public IEnumerable<Property> Properties { get; set; }
    }
}
