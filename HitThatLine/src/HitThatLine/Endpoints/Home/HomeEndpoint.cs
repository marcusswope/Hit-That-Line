using AutoMapper;
using HitThatLine.Domain.Discussion;
using HitThatLine.Endpoints.Home.Models;
using Raven.Client;
using System.Linq;

namespace HitThatLine.Endpoints.Home
{
    public class HomeEndpoint
    {
        private readonly IDocumentSession _session;
        private readonly IMappingEngine _mapper;

        public HomeEndpoint(IDocumentSession session, IMappingEngine mapper)
        {
            _session = session;
            _mapper = mapper;
        }

        public HomeViewModel Home(HomeRequest input)
        {
            var threads = _session
                .Query<DiscussionThread>()
                .OrderByDescending(x => x.Score)
                .Take(20).ToList();
            
            var threadSummaries = threads.Select(x =>
                            {
                                var summary = _mapper.Map<DiscussionThread, ThreadSummaryViewModel>(x);
                                summary.TimeZoneOffset = input.TimeZoneOffset;
                                return summary;
                            });

            return new HomeViewModel { Threads = threadSummaries.ToList() };
        }

        public ThreadSummaryViewModel Summary(ThreadSummaryViewModel model)
        {
            return model;
        }
    }
}