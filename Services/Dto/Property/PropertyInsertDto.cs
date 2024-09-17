using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dto.Property
{
    public class PropertyInsertDto
    {
        [StringLength(20)]
        public string PropertyName { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PropertyPrice { get; set; }
        public string PropertyAddress { get; set; }
        public int PropertyCityId { get; set; }
        public double AreaInMeters { get; set; }
        public int PropertyTypeId { get; set; } // نوع العقار (شقة، فيلا، مكتب، إلخ)
        [Required]
        public int BedroomsNumber { get; set; }
        [Required]
        public int BathroomsNumber { get; set; }

        public DateTime PropertyDateAdded { get; set; }
        public int YearBuilt { get; set; }
        [Required]
        public string PropertyStatusId { get; set; } // حالة العقار (متاح، مباع، تحت التفاوض)
        public int AmenitiesId { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Description { get; set; }
        public string AdditionalNotes { get; set; }

    }
}
