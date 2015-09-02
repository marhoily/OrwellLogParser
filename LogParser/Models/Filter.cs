using System;
using System.Linq.Expressions;
using LogParser.ViewModels;

namespace LogParser.Models
{
    public abstract class Filter
    {
        public abstract bool Test(LogLine line);

        public string Name { get; protected set; }

        public static Filter S1(Expression<Func<LogLine, bool>> predicate)
        {
            return new DelefateFilter(predicate.ToString(), predicate.Compile());
        }

        private class DelefateFilter : Filter
        {
            private readonly Func<LogLine, bool> _predicate;

            public DelefateFilter(string name, Func<LogLine, bool> predicate)
            {
                Name = name;
                _predicate = predicate;
            }

            public override bool Test(LogLine line)
            {
                return _predicate(line);
            }
        }
    }
}