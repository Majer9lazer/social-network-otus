using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace social_network_otus.Data.Models
{
    public class ApplicationUserFriend
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FriendId { get; set; }

        public DateTime DateAdded { get; set; }

        public virtual ApplicationUser Friend { get; set; }
    }
}