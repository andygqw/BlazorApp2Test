
namespace BlazorApp2Test.Models
{
	public class Reply
	{
		public int id { get; set; }
		public int userId { get; set; }
		public int memoId { get; set; }
		public int? replyId { get; set; }
		public string? content { get; set; }
		public DateTime? create_time { get; set; }

		public Reply()
		{
			replyId = null;
			content = null;
		}
	}
}