
namespace BlazorApp2Test.Models
{
	public class UserFile
	{
		public int id { get; set; }
		public int userId { get; set; }
		public string? fileName { get; set; }
		public string? filePath { get; set; }
		public DateTime? createTime { get; set; }
	}
}