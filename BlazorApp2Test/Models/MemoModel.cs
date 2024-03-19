using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp2Test.Models
{
    public class MemoModel
    {
        public MemoModel()
        {
            Replies = new List<ReplyModel>();
        }

        public MemoModel(MemoModel model)
        {
            Id = model.Id;
            Name = model.Name;
            Description = model.Description;
            Image = model.Image;
            Time = model.Time;
            Replies = new List<ReplyModel>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "A memo title is required")]
        [DisplayName("Title")]
        public string? Name { get; set; }

        [DisplayName("Content")]
        public string? Description { get; set; }

        public string? Image { get; set; }

        public string? CreatedBy { get; set; }

        public int UserId { get; set; }

        public List<ReplyModel> Replies { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [DisplayName("Uploaded Time")]
        public DateTime Time { get; set; }

        public void UpdateFromRawData(Memo m)
        {
            Id = m.id;
            UserId = m.userId;
            if (m.name != null) Name = m.name;
            if (m.description != null) Description = m.description;
            if (m.image != null) Image = m.image;
            if (m.createTime != null) Time = m.createTime.Value;
        }
    }
}
