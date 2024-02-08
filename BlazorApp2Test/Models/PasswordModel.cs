using System.Text.Json.Serialization;

namespace BlazorApp2Test.Models
{
    internal class PasswordModel
    {
        [JsonPropertyName("password")]
        public string? Password { get; set; }
        [JsonPropertyName("username")]
        public string? Username { get; set; }
    }
}