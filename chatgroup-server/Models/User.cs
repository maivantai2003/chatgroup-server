﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatgroup_server.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Bio { get; set; } = "None";
        public string Sex { get; set; } = "Other";
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime Birthday { get; set; } = new DateTime(1990, 1, 1);
        public string? Avatar { get; set; }
        public string? CoverPhoto { get; set; }
        public int Status { get; set; } = 1;
        //Friend
        public virtual ICollection<Friends>? Friends { get; set; }
        //Group
        public virtual ICollection<GroupDetail>? groupDetails { get; set; }
        // User Message
        public virtual ICollection<UserMessages>? Senders { get; set; }
        public virtual ICollection<UserMessages>? Receivers {  get; set; }
        //Group Message
        public virtual ICollection<GroupMessages>? SentGroupMessages {  get; set; }
        //Message Reaction
        public virtual ICollection<UserMessageReaction>? userMessageReactions { get; set; }
        public virtual ICollection<GroupMessageReaction>? groupMessageReactions { get; set; }
        //Message Status
        public virtual ICollection<UserMessageStatus>? userMessageStatuses { get; set; }
        public virtual ICollection<GroupMessageStatus>? groupMessageStatuses { get; set; }
    }
}
