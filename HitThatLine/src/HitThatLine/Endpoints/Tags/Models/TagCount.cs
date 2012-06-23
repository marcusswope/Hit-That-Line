using System.Collections.Generic;
using FubuMVC.Core;
using HitThatLine.Infrastructure.Raven;

namespace HitThatLine.Endpoints.Tags.Models
{
    public class TagCountCommand
    {
        public string TagQuery { get; set; }
    }

    public class TagCountResponse : JsonMessage
    {
        public List<ThreadCountByTag> Tags { get; set; }
    }
}