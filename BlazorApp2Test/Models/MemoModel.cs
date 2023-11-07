using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp2Test.Models
{
    public class MemoModel
    {
        public MemoModel() { }

        public MemoModel(MemoModel model) 
        {
            Id = model.Id;
            Name = model.Name;
            Description = model.Description;
            Image = model.Image;
            Time = model.Time;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "A memo title is required")]
        [DisplayName("Title")]
        public string? Name { get; set; }

        [DisplayName("Content")]
        public string? Description { get; set; }

        public string? Image { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [DisplayName("Uploaded Time")]
        public DateTime Time { get; set; }
    }
}
