using System.Collections.Generic;
using System.Web;
using HitThatLine.Domain.Accounts;

namespace HitThatLine.Endpoints.Thread.Models
{
    public class ViewThreadRequest
    {
        public string UriId { get; set; }
        public string Title { get; set; }
        public HttpContextBase HttpContext { get; set; }
        public UserAccount UserAccount { get; set; }
    }

    public class ViewThreadViewModel
    {
        public string UriId { get; set; }
        public string Title { get; set; }
        public string DisplayBody { get; set; }
        public string AuthorProfilePictureUrl { get; set; }
        public string AuthorUsername { get; set; }
        public string ReplyBody { get; set; }
        public PostReplyCommand ReplyCommand { get { return new PostReplyCommand { DiscussionUri = UriId }; } }

        public List<ViewThreadReplyViewModel> Replies { get; set; }

        public ViewThreadViewModel()
        {
            Replies = new List<ViewThreadReplyViewModel>();
        }
    }

    public class ViewThreadReplyViewModel
    {
        public string DisplayBody { get; set; }
        public string UserProfilePictureUrl { get; set; }
        public string Username { get; set; }
    }
}