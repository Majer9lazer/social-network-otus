using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace social_network_otus.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string RangeOfInterests { get; set; }
        public string CityName { get; set; }
    }
}
