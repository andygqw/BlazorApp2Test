namespace BlazorApp2Test.Models;

public class Log
{
    public int id { get; set; }
    public int? user_id { get; set; }
    public String? operation { get; set; }
    public String? description { get; set; }
    public DateTime? create_time { get; set; }
}