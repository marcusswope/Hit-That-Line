using AutoMapper;
using FubuMVC.Core.Continuations;
using HitThatLine.Domain.Discussion;
using HitThatLine.Endpoints.Home.Models;
using HitThatLine.Endpoints.Thread.Models;
using HitThatLine.Infrastructure;
using Raven.Client;
using System.Linq;

namespace HitThatLine.Endpoints.Thread
{
    public class NewThreadEndpoint
    {
        private readonly IMappingEngine _mapper;
        private readonly IDocumentSession _session;

        public NewThreadEndpoint(IMappingEngine mapper, IDocumentSession session)
        {
            _mapper = mapper;
            _session = session;
        }

        public NewThreadViewModel NewThread(NewThreadRequest request)
        {
            return _mapper.Map<NewThreadRequest, NewThreadViewModel>(request);
        }

        public FubuContinuation NewThread(NewThreadCommand command)
        {
            var thread = new DiscussionThread(command.Title, command.Body, command.Tags.Split(','));
            _session.Store(thread);
            var request = _mapper.Map<DiscussionThread, ViewThreadRequest>(thread);

            return FubuContinuation.RedirectTo<HomeRequest>();
        }

        public TagCountResponse ThreadsByTag(TagCountRequest request)
        {
            var tags = _session.Query<ThreadCountByTag, Threads_TagCount>().ToList();
            return new TagCountResponse { Tags = tags };
        }
    }
}