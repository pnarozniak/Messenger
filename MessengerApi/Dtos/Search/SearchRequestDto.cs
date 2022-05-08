using System.ComponentModel.DataAnnotations;

namespace MessengerApi.Dtos.Search
{
    public class SearchRequestDto
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int? SkipCount { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? TakeCount { get; set; }

        public string SearchPhraze { get; set; } = string.Empty;
    }
}