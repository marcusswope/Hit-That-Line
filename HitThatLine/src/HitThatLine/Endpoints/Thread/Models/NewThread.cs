using System.Collections.Generic;
using FubuMVC.Core;
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
        [Required]
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

    public class TagCountCommand
    {
        public string TagQuery { get; set; }
    }

    public class TagCountResponse : JsonMessage
    {
        public List<ThreadCountByTag> Tags { get; set; }
    }
}