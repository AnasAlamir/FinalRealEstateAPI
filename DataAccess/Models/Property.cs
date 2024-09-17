using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Property
    {

        public int Id { get; set; }
        [StringLength(20)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public double AreaInMeters { get; set; }
        [Required]
        public int BedroomsNumber { get; set; }
        [Required]
        public int BathroomsNumber { get; set; }

        public DateTime DateAdded { get; set; }
        public int YearBuilt { get; set; }
        [Required]
        public int StatusId { get; set; }
        public int PropertyTypeId { get; set; }
        public int AmenitiesId { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Description { get; set; }
        public string AdditionalNotes { get; set; }
        public IEnumerable<PropertyImage> PropertyImages { get; set; }
        public IEnumerable<Inquiry> Inquiries { get; set; }
        public IEnumerable<Favorite> Favorites { get; set; }
        public User User { get; set; }
        public  Amenities Amenities { get; set; }
        public PropertyStatus PropertyStatus { get; set; }
        public PropertyType PropertyType { get; set; }
        public City City { get; set; }
    }
}