using System.ComponentModel.DataAnnotations;

namespace RealTimeChatApp.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The UserId field is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "The Text field is required.")]
        public string Text { get; set; }

        [Required(ErrorMessage = "The Timestamp field is required.")]
        public DateTime Timestamp { get; set; }
        public ApplicationUser User { get; set; }
    }
}
