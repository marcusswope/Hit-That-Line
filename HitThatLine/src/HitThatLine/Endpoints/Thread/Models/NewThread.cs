using HitThatLine.Infrastructure;
using HitThatLine.Infrastructure.Conventions.Attributes;

namespace HitThatLine.Endpoints.Thread.Models
{
    public class NewThreadRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; } 
    }

    public class NewThreadViewModel : NewThreadRequest
    {
        
    }

    public class NewThreadCommand : NewThreadViewModel, IValidatedCommand
    {
        public object TransferToOnFailed
        {
            get { return new NewThreadRequest(); }
        }
    }
}