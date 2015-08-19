using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace UnitTests
{
    public sealed class Tests
    {
        public void F1()
        {
            // <08/14/2015 11.24.01.663388>
            var substring = "08/14/2015 11.24.01.663388";
                DateTime.ParseExact(substring,
                "MM/dd/yyyy hh.mm.ss.ffffff",//" hh.mm.ss",
                CultureInfo.InvariantCulture)
                .Should().Be(new DateTime(2015, 8, 14, 11, 24, 01));

        }
    }
}
