using System;
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

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)] 
        public DateTime Time { get; set; }
    }
}
