using System;
using System.Collections.Generic;
using ModelLayer.Model;

namespace ModelLayer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        // ✅ Reset Password Properties
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }

        // ✅ Navigation Property for Greetings
        public virtual ICollection<GreetingModel> Greetings { get; set; } = new List<GreetingModel>();
    }
}
