using AutoMapper;
using FubuMVC.Core.Continuations;
using HitThatLine.Domain.Accounts;
using HitThatLine.Domain.Discussion;
using HitThatLine.Endpoints.Home.Models;
using HitThatLine.Endpoints.Thread.Models;
using HitThatLine.Infrastructure.Raven;
using HitThatLine.Infrastructure.Security;
using Raven.Client;
using System.Linq;
using System.Collections.Generic;

namespace HitThatLine.Endpoints.Thread
{
    [OnlyAllowRoles(UserAccount.BasicUserRole)]
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
            var thread = new DiscussionThread(command.Title, command.Body, command.AllTagsEntered, command.Author);
            _session.Store(thread);
            return FubuContinuation.RedirectTo(new HomeRequest());
        }
    }
}