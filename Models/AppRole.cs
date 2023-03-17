using System;
using Microsoft.AspNetCore.Identity;

namespace TodoList.Api.Models
{
    public class AppRole : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}