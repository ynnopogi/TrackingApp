using Microsoft.AspNetCore.Identity;
using System;

namespace Tracking.App.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime DateRegistered { get; set; }
    }
}