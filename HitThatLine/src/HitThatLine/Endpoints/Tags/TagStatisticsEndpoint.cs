using System.Linq;
using HitThatLine.Endpoints.Tags.Models;
using HitThatLine.Infrastructure.Raven;
using Raven.Client;

namespace HitThatLine.Endpoints.Tags
{
    public class TagStatisticsEndpoint
    {
        private readonly IDocumentSession _session;
        public TagStatisticsEndpoint(IDocumentSession session)
        {
            _session = session;
        }

        public TagCountResponse ThreadsByTag(TagCountCommand command)
        {
            var tags = _session.Advanced.LuceneQuery<ThreadCountByTag, Threads_TagCount>()
                            .Where(string.Format("Tag: *{0}*", command.TagQuery))
                            .OrderBy("-Count").Take(6)
                            .ToList();

            return new TagCountResponse { Tags = tags };
        }
    }
}