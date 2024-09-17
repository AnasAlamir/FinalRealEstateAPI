using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dto.User
{
    public class UserUpdateDto
    {
        public int UserId { get; set; }
        [Required]
        [StringLength(20)]
        public string UserFirstName { get; set; }
        [Required]
        [StringLength(20)]
        public string UserLastName { get; set; }
        public string UserFullName { get { return UserFirstName + " " + UserLastName; } }
        [EmailAddress]
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string UserPassword { get; set; }
        [Required]
        public string UserPhoneNumber { get; set; }
        public string UserAddress { get; set; }
        public string UserProfilePicture { get; set; }
    }
}
