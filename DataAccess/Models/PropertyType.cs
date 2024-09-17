using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class PropertyType
    {
        public int Id { get; set; }
        public string Type { get; set; }

        // Navigation properties
        public IEnumerable<Property> Properties { get; set; }
    }
}
