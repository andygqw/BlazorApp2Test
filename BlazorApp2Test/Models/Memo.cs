
namespace BlazorApp2Test.Models
{
    public class Memo
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public string? image { get; set; }
        public DateTime? createTime { get; set; }
    }
}