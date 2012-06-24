using System.Collections.Generic;
using AutoMapper;
using FubuMVC.Core;
using HitThatLine.Domain.Accounts;
using HitThatLine.Domain.Discussion;
using HitThatLine.Endpoints.Thread.Models;
using Raven.Client;
using Raven.Client.Linq;
using System.Linq;

namespace HitThatLine.Endpoints.Thread
{
    public class ViewThreadEndpoint
    {
        private readonly IDocumentSession _session;
        private readonly IMappingEngine _mapper;

        public ViewThreadEndpoint(IDocumentSession session, IMappingEngine mapper)
        {
            _session = session;
            _mapper = mapper;
        }

        [UrlPattern("threads/{UriId}/{Title}")]
        public ViewThreadViewModel Thread(ViewThreadRequest request)
        {
            var thread = _session.Load<DiscussionThread>(DiscussionThread.BuildDocumentKey(request.UriId));
            var replies = _session.Query<Post>().Where(x => x.ThreadId == thread.Id).ToList();

            var userKeys = new List<string> { UserAccount.BuildDocumentKey(thread.AuthorUsername) };
            userKeys.AddRange(replies.Select(x => UserAccount.BuildDocumentKey(x.Username)).Distinct().ToList());
            var users = _session.Load<UserAccount>(userKeys).ToList();

            var viewModel = _mapper.Map<DiscussionThread, ViewThreadViewModel>(thread);
            foreach (var reply in replies)
            {
                var user = users.First(x => x.Username == reply.Username);
                var replyModel = new ViewThreadReplyViewModel
                                     {
                                         DisplayBody = reply.DisplayText,
                                         Username = reply.Username,
                                         UserProfilePictureUrl = user.ProfilePictureUrl
                                     };
                viewModel.Replies.Add(replyModel);
            }

            return viewModel;
        }

        public ViewThreadReplyViewModel ReplyPassThrough(ViewThreadReplyViewModel model)
        {
            return model;
        }
    }
}