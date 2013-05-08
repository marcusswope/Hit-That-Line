using System.Linq;
using HitThatLine.Domain.Discussion;
using HitThatLine.Endpoints.Thread.Models;
using HitThatLine.Infrastructure.TaskExecution;
using FubuCore;
using Raven.Abstractions.Data;

namespace HitThatLine.Endpoints.Thread.Tasks
{
    public class RecordThreadViewTask : BackgroundTask
    {
        private readonly ViewThreadRequest _request;
        public RecordThreadViewTask(ViewThreadRequest request)
        {
            _request = request;
        }

        public override void Execute()
        {
            var username = _request.UserAccount.IfNotNull(x => x.Username);
            var threadId = DiscussionThread.Key(_request.UriId);

            var exists = Session.Query<ThreadView>()
                                .Any(x => x.DiscussionThreadId == threadId &&
                                          x.IPAddress == _request.IPAddress &&
                                          x.Username == username);
            if (!exists)
            {
                Session.Store(new ThreadView(_request.IPAddress, username, threadId));
                Store.DatabaseCommands.Patch(threadId, new[] { new PatchRequest { Type = PatchCommandType.Inc, Name = "ViewCount", Value = 1 } });
            }
        }
    }
}