using HitThatLine.Domain.Accounts;
using HitThatLine.Infrastructure.Validation.Attributes;

namespace HitThatLine.Endpoints.Thread.Models
{
    public class PostReplyCommand
    {
        [Required]
        public string ReplyBody { get; set; }

        public string DiscussionUri { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}