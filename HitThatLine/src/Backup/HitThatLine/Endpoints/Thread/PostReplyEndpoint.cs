using FubuMVC.Core;
using FubuMVC.Core.Continuations;
using HitThatLine.Domain.Discussion;
using HitThatLine.Endpoints.Thread.Models;
using Raven.Client;

namespace HitThatLine.Endpoints.Thread
{
    public class PostReplyEndpoint
    {
        private readonly IDocumentSession _session;
        public PostReplyEndpoint(IDocumentSession session)
        {
            _session = session;
        }

        [UrlPattern("reply/{DiscussionUri}")]
        public FubuContinuation Reply(PostReplyCommand command)
        {
            var discussionKey = DiscussionThread.Key(command.DiscussionUri);
            var thread = _session.Load<DiscussionThread>(discussionKey);
            thread.AddPost(command.UserAccount.Username);
            var post = new Post(command.ReplyBody, discussionKey, command.UserAccount);
            _session.Store(post);
            return FubuContinuation.RedirectTo(new ViewThreadRequest { UriId = command.DiscussionUri, Title = thread.Title });
        }
    }
}