using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace social_network_otus.Data.Models
{
    public class Chat
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationUser AnotherUser { get; set; }

        public virtual ChatMessage Message { get; set; }
    }
}