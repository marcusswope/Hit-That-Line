using System.Linq;
using FubuMVC.Core.Continuations;
using HitThatLine.Core.Discussion;
using HitThatLine.Core.Utility;
using HitThatLine.Endpoints.Discussion.Models;
using Raven.Client;
using Raven.Client.Linq;

namespace HitThatLine.Endpoints.Discussion
{
    public class DiscussionEndpoint
    {
        private readonly IDocumentSession _session;
        public DiscussionEndpoint(IDocumentSession session)
        {
            _session = session;
        }

        public DiscussionListSummaryViewModel List(DiscussionHomeInputModel input)
        {
            var threads = _session.Query<DiscussionThread>().OrderByDescending(x => x.Score).ToList();
            return new DiscussionListSummaryViewModel(threads);
        }

        public DiscussionThreadViewModel discussion_view_Id(DiscussionThreadListViewModel input)
        {
            var thread = _session.Load<DiscussionThread>(input.Id);
            return new DiscussionThreadViewModel(thread);
        }

        public CreateThreadViewModel New(NewThreadViewModel input)
        {
            return new CreateThreadViewModel();
        }

        public FubuContinuation Create(CreateThreadCommand input)
        {
            var thread = new DiscussionThread(input.Title, input.Body);
            _session.Store(thread);
            return FubuContinuation.RedirectTo<DiscussionHomeInputModel>();
        }
    }
}