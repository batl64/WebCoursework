using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppShares.Models
{
    public class ImageModel
    {
        public int Id { get; set; }

        public string? ImageTitle { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
