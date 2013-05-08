using System.Collections.Generic;
using AutoMapper;
using FubuMVC.Core;
using HitThatLine.Domain.Accounts;
using HitThatLine.Domain.Discussion;
using HitThatLine.Endpoints.Thread.Models;
using HitThatLine.Endpoints.Thread.Tasks;
using HitThatLine.Infrastructure.TaskExecution;
using Raven.Client;
using Raven.Client.Linq;
using System.Linq;

namespace HitThatLine.Endpoints.Thread
{
    public class ViewThreadEndpoint
    {
        private readonly IDocumentSession _session;
        private readonly IMappingEngine _mapper;
        private readonly ITaskQueue _taskQueue;

        public ViewThreadEndpoint(IDocumentSession session, IMappingEngine mapper, ITaskQueue taskQueue)
        {
            _session = session;
            _mapper = mapper;
            _taskQueue = taskQueue;
        }

        [UrlPattern("threads/{UriId}/{Title}")]
        public ViewThreadViewModel Thread(ViewThreadRequest request)
        {
            _taskQueue.ExecuteLater(new RecordThreadViewTask(request));

            var thread = _session.Load<DiscussionThread>(DiscussionThread.Key(request.UriId));
            var replies = _session.Query<Post>().Where(x => x.ThreadId == thread.Id).ToList();

            var userKeys = new List<string> { UserAccount.Key(thread.AuthorUsername) };
            userKeys.AddRange(replies.Select(x => UserAccount.Key(x.Username)).Distinct().ToList());
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