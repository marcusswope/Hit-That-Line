using AutoMapper;
using HitThatLine.Web.Endpoints.Account.Models;

namespace HitThatLine.Web.Endpoints.Account
{
    public class SummaryEndpoint
    {
        private readonly IMappingEngine _mapper;
        public SummaryEndpoint(IMappingEngine mapper)
        {
            _mapper = mapper;
        }

        public SummaryViewModel Summary(SummaryRequest request)
        {
            return _mapper.Map<SummaryRequest, SummaryViewModel>(request);
        }
    }
}