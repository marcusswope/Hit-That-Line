using System.Collections.Generic;
using HitThatLine.Domain.Accounts;
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
        public UserAccount Author { get; set; }
        public List<string> AllTagsEntered
        {
            get
            {
                var tags = new List<string>();
                if (!string.IsNullOrWhiteSpace(Tags))
                {
                    tags.AddRange(Tags.Split(','));
                }
                if (!string.IsNullOrWhiteSpace(TagInput))
                {
                    tags.AddRange(TagInput.Split(' '));
                }
                return tags;
            }
        }

        public object TransferToOnFailed
        {
            get { return new NewThreadRequest(); }
        }
    }
}