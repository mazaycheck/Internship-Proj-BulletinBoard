﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Baraholka.Domain.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Text { get; set; }

        [Required]
        public int SenderId { get; set; }

        public User Sender { get; set; }

        [Required]
        public int RecieverId { get; set; }

        public User Reciever { get; set; }
        public DateTime? DateTimeSent { get; set; }
        public DateTime? DateTimeRead { get; set; }
        public bool IsRead { get; set; }
    }
}