using HitThatLine.Infrastructure.Validation;
using HitThatLine.Infrastructure.Validation.Attributes;

namespace HitThatLine.Endpoints.Thread.Models
{
    public class NewThreadRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        public string TagInput { get; set; }
        public string Tags { get; set; }
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