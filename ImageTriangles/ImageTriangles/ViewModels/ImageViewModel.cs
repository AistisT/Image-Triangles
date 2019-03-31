using System.ComponentModel.DataAnnotations;
using Enums.Constants;
using Models.ValidationAttributes;

namespace ImageTriangles.ViewModels
{
    public class ImageViewModel
    {
        [Range(1, Constants.MaxFileSize, ErrorMessage = "File cannot be empty and cannot exceed 50mb.")]
        public long ImageSize { get; set; }
        [Required(ErrorMessage = "File not selected.")]
        public string ImageName { get; set; }
        [Required(ErrorMessage = "Failed to process the Image, it might be corrupted.")]
        public string ImageStream { get; set; }
        [StringRange(AllowableValues = new[] { "image/jpeg", "image/jpg", "image/bpm", "image/png" }, ErrorMessage = "Only jpeg, jpg, bpm and png formats are accepted.")]
        public string ImageType { get; set; }
    }
}