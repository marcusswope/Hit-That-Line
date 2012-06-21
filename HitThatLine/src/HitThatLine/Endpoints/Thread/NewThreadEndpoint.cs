using AutoMapper;
using HitThatLine.Endpoints.Thread.Models;

namespace HitThatLine.Endpoints.Thread
{
    public class NewThreadEndpoint
    {
        private readonly IMappingEngine _mapper;
        public NewThreadEndpoint(IMappingEngine mapper)
        {
            _mapper = mapper;
        }

        public NewThreadViewModel NewThread(NewThreadRequest request)
        {
            return _mapper.Map<NewThreadRequest, NewThreadViewModel>(request);
        }

        public void NewThread(NewThreadCommand command)
        {
            
        }
    }
}