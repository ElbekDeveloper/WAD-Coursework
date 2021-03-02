using System.ComponentModel.DataAnnotations;

namespace Core.Resources
{
    public class AddArticleResource
    {
        [Required]
        [MaxLength(2000)]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
    }
}
