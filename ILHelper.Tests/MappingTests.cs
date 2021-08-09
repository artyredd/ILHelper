using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILHelper;
using Xunit;
using ILHelper.Linter;

namespace ILHelper.Tests
{
    public class MappingTests
    {
        [Fact]
        public void Test_()
        {
            var mapper = new GenericMapper<Type, StackVerificationType>();

            mapper.Map(typeof(int), StackVerificationType.Int32);

            Assert.Equal(StackVerificationType.Int32, mapper.GetMapping(typeof(int)));
        }
    }
}
