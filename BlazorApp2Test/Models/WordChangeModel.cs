using System.ComponentModel.DataAnnotations;

namespace BlazorApp2Test.Models
{
    public class WordChangeModel
    {
        [Required(ErrorMessage = "Empty Content")]
        public string? Content { get; set; }

        [Required(ErrorMessage = "Empty Word")]
        public string? WordWas { get; set; }

        public string? WordWill { get; set; }

        public string? Result() => Content.Replace(WordWas, WordWill);
    }
}
