using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class UrlMapping
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string OriginalUrl { get; set; }

        public string ShortCode { get; set; }
    }
}
