using System.ComponentModel.DataAnnotations;

namespace BlazorApp2Test.Models
{
    public class WordChangeSearchModel
    {
        [Required(ErrorMessage = "Empty Content")]
        public string? Content { get; set; }

        [Required(ErrorMessage = "Empty Word")]
        public string? WordWas { get; set; }

        public string? WordWill { get; set; }

        public bool CaseSensitive = true;
    }
}