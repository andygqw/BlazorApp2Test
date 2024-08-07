﻿
namespace BlazorApp2Test.Models
{
    public class User
    {
        public int id { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public DateTime registrationTime { get; set; }
        public string? notes;
        public bool can_access_memo { get; set; }
        
        public bool can_access_resource { get; set; }

        public User()
        {
            email = string.Empty;
            username = string.Empty;
            password = string.Empty;
            registrationTime = DateTime.MinValue;
            can_access_memo = false;
        }
    }
}