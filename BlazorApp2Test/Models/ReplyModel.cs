using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp2Test.Models
{
	public class ReplyModel
	{
		public ReplyModel()
		{
		}

		public int Id { get; set; }
		
		public int MemoId { get; set; }

		public int? ReplyId { get; set; }

		public int? UserId { get; set; }

		public string? CreatedBy { get; set; }

		public string? RepliedBy { get; set; }

		[Required(ErrorMessage = "Reply content is required")]
		[DisplayName("Replay")]
        public string? Content { get; set; }

		public void UpdateFromRawData(Reply data)
		{
			if (data.content != null) Content = data.content;
			if (data.replyId != null) ReplyId = data.replyId;
			Id = data.id;
			MemoId = data.memoId;
			UserId = data.userId;
		}
    }
}