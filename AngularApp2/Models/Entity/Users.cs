using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace AngularApp2.Models.Entity
{
    public partial class Users
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Role { get; set; }
        public DateOnly? DayOfBirth { get; set; }
        public string? UserImg { get; set; }
    }
}
