using System;

namespace LogParser.ViewModels
{
    public abstract class Filter
    {
        public abstract bool Test(string line);
        public abstract string Display(string line);

        public static Filter S1(string str)
        {
            return new ContainStringFilter(str);
        }

        public static Filter S1(string str, Func<string, string> convert)
        {
            return new ContainStringFilter2(str, convert);
        }

        public class ContainStringFilter : Filter
        {
            private readonly string _str;

            public ContainStringFilter(string str)
            {
                _str = str;
            }

            public override bool Test(string line)
            {
                return line.Contains(_str);
            }

            public override string Display(string line)
            {
                return _str;
            }
        }

        public class ContainStringFilter2 : Filter
        {
            private readonly string _str;
            private readonly Func<string, string> _convert;

            public ContainStringFilter2(string str, Func<string, string> convert)
            {
                _str = str;
                _convert = convert;
            }

            public override bool Test(string line)
            {
                return line.Contains(_str);
            }

            public override string Display(string line)
            {
                return _convert(line);
            }
        }
    }
}