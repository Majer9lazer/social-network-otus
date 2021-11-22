using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace social_network_otus.Data.Models
{
    public class FirebaseUserToken
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Token { get; set; }

        [StringLength(45)]
        public string IpAddress { get; set; }

        public string AdditionalData { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
