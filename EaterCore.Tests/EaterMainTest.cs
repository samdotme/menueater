using System;
using Xunit;
using EaterCore;

namespace EaterCore.Tests
{
    public class EaterMainTest
    {
        [Fact]
        public void ShouldEvaluateTrueAsTrue()
        {
            var main = new EaterMain();
            Assert.True(main.IsTrue());
        }
    }
}
