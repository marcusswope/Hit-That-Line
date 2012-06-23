using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HitThatLine.Domain.Discussion;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace HitThatLine.Infrastructure
{
    public class ThreadCountByTag
    {
        public string Tag { get; set; }
        public long Count { get; set; }
    }

    public class Threads_TagCount : AbstractIndexCreationTask<DiscussionThread, ThreadCountByTag>
    {
        public Threads_TagCount()
        {
            Map = threads => from thread in threads
                             from tag in thread.Tags
                             select new
                             {
                                 Tag = tag,
                                 Count = (long) 1
                             };

            Reduce = results => from result in results
                                group result by result.Tag into g
                                select new
                                {
                                    Tag = g.Key,
                                    Count = g.Sum(x => x.Count)
                                };
            Indexes.Add(x => x.Tag, FieldIndexing.Analyzed);
        }
    }
}