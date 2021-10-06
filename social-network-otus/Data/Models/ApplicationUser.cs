﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace social_network_otus.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime BirthDate { get; set; }

        [StringLength(100)]
        public string Gender { get; set; }

        [StringLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string RangeOfInterests { get; set; }

        [StringLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string CityName { get; set; }

        [StringLength(250)]
        [Column(TypeName = "nvarchar(250)")]
        public string UserLastName { get; set; }
    }
}
