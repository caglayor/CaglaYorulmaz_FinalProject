using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CETHotelProject_CY.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }


        [NotMapped]
        public string NameSurname
        {
            get
            {
                return $"{Name} {Surname}";
            }
        }
    }
}
