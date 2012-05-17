using HitThatLine.Core.Discussion;

namespace HitThatLine.Web.Endpoints.Discussion.Models
{
    public class DiscussionThreadViewModel
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public DiscussionThreadViewModel(DiscussionThread thread)
        {
            Title = thread.Title;
            Body = thread.DisplayBody;
        }
    }
}