using System.Collections.Generic;
using System.Linq;
using HitThatLine.Core.Discussion;

namespace HitThatLine.Web.Endpoints.Discussion.Models
{
    public class DiscussionListSummaryViewModel
    {
        public IList<DiscussionThreadListViewModel> Threads { get; set; }

        public DiscussionListSummaryViewModel(IList<DiscussionThread> threads)
        {
            Threads = threads.Select(x => new DiscussionThreadListViewModel(x)).ToList();
        }
    }

    public class DiscussionThreadListViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public DiscussionThreadListViewModel()
        {
            
        }

        public DiscussionThreadListViewModel(DiscussionThread thread)
        {
            Id = thread.Id;
            Title = thread.Title;
        }
    }
}