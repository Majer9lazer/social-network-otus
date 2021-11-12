using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace social_network_otus.Data.Models
{
    public class ChatMessage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime SendDate { get; set; }

        public DateTime ReceiveDate { get; set; }
    }
}
