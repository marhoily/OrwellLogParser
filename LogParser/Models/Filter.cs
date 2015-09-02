using System;

namespace LogParser.ViewModels
{
    public abstract class Filter
    {
        public abstract bool Test(LogLine line);
        public abstract string Display(string line);

        public static Filter S1(Func<LogLine, bool> predicate)
        {
            return new DelefateFilter(predicate);
        }

        private class DelefateFilter : Filter
        {
            private readonly Func<LogLine, bool> _predicate;

            public DelefateFilter(Func<LogLine, bool> predicate)
            {
                _predicate = predicate;
            }

            public override bool Test(LogLine line)
            {
                return _predicate(line);
            }

            public override string Display(string line)
            {
                return line;
            }
        }
    }
}