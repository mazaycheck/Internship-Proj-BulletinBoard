﻿using System;

namespace Baraholka.Web.Models
{
    public class MessageWebModel
    {
        public int MessageId { get; set; }
        public string Text { get; set; }
        public int SenderId { get; set; }
        public int RecieverId { get; set; }
        public string SenderName { get; set; }
        public string RecieverName { get; set; }
        public DateTime? DateTimeSent { get; set; }
        public DateTime? DateTimeRead { get; set; }
        public bool IsRead { get; set; }
    }
}