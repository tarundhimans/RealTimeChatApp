using System.ComponentModel.DataAnnotations;

namespace RealTimeChatApp.Models
{
    public class FileMessage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The UserId field is required.")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "The Content field is required.")]
        public byte[] Content { get; set; }
        [Required(ErrorMessage = "The Timestamp field is required.")]
        public DateTime Timestamp { get; set; }
        public ApplicationUser User { get; set; }
    }
}
