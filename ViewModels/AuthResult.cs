using System;
using System.Collections.Generic;

namespace TodoList.Api.ViewModels
{
    public class AuthResult
    {
        public string Token { get; set; }
        public string FullName {get; set;}
        public Guid Id {get; set;}
        // public string RefreshToken { get; set; }
        // public bool Success { get; set; }
        // public List<string> Errors { get; set; }
    }
}