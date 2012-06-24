using AutoMapper;
using HitThatLine.Domain.Discussion;
using HitThatLine.Endpoints.Home.Models;
using Raven.Client;
using System.Linq;
using Raven.Client.Linq;

namespace HitThatLine.Endpoints.Home
{
    public class HomeEndpoint
    {
        public const int DefaultPageCount = 20;
        private readonly IDocumentSession _session;
        private readonly IMappingEngine _mapper;

        public HomeEndpoint(IDocumentSession session, IMappingEngine mapper)
        {
            _session = session;
            _mapper = mapper;
        }

        public HomeViewModel Home(HomeRequest input)
        {
            RavenQueryStatistics stats;
            var threads = _session.Query<DiscussionThread>()
                            .Statistics(out stats)
                            .OrderByDescending(x => x.Score)
                            .Take(DefaultPageCount).ToList();

            var totalPages = stats.TotalResults / DefaultPageCount;

            var threadSummaries = threads.Select(x =>
                            {
                                var summary = _mapper.Map<DiscussionThread, ThreadSummaryViewModel>(x);
                                summary.TimeZoneOffset = input.TimeZoneOffset;
                                return summary;
                            });

            return new HomeViewModel { TotalPages = totalPages, Threads = threadSummaries.ToList() };
        }

        public ThreadSummaryViewModel Summary(ThreadSummaryViewModel model)
        {
            return model;
        }
    }
}