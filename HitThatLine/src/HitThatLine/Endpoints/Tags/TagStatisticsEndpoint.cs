using System.Linq;
using HitThatLine.Endpoints.Tags.Models;
using HitThatLine.Infrastructure.Raven;
using Raven.Client;
using FubuCore;

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
                            .Where("Tag: *{0}*".ToFormat(command.TagQuery))
                            .OrderBy("-Count").Take(6)
                            .ToList();

            return new TagCountResponse { Tags = tags };
        }
    }
}