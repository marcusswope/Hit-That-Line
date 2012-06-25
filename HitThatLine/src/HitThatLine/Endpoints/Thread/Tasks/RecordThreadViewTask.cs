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
            var ipAddress = _request.HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrWhiteSpace(ipAddress)) ipAddress = _request.HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrWhiteSpace(ipAddress)) ipAddress = _request.HttpContext.Request.UserHostAddress;
            var username = _request.UserAccount.IfNotNull(x => x.Username);
            var threadId = DiscussionThread.Key(_request.UriId);

            var exists = Session.Query<ThreadView>()
                                .Any(x => x.DiscussionThreadId == threadId &&
                                          x.IPAddress == ipAddress &&
                                          x.Username == username);
            if (!exists)
            {
                Session.Store(new ThreadView(ipAddress, username, threadId));
                Store.DatabaseCommands.Patch(threadId, new[] { new PatchRequest { Type = PatchCommandType.Inc, Name = "ViewCount" } });
            }
        }
    }
}