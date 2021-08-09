using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILHelper.Linter;
using ILHelper.Extensions;
using Xunit;

namespace ILHelper.Tests
{
    public class RuleTests
    {
        [Fact]
        public void Test_Disallow()
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.Disallow(x => x.Id == 1);

            var test = new TestClass();

            Assert.False(ruleset.Evaluate(test));

            test.Id = 13;

            Assert.True(ruleset.Evaluate(test));
        }

        [Fact]
        public void Test_Require()
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.Require(x => x.Id == 1);

            var test = new TestClass();

            Assert.True(ruleset.Evaluate(test));

            test.Id = 13;

            Assert.False(ruleset.Evaluate(test));
        }

        [Fact]
        public void Test_NestOR()
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.Require(x => x.Id == 1).Or(x => x.Id == 2);

            var test = new TestClass();

            Assert.True(ruleset.Evaluate(test));

            test.Id = 2;

            Assert.True(ruleset.Evaluate(test));

            test.Id = 13;

            Assert.False(ruleset.Evaluate(test));
        }

        [Fact]
        public void Test_NestAND()
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.Require(x => x.Id == 1).And(x => x.Name == "Hello");

            var test = new TestClass();

            Assert.False(ruleset.Evaluate(test));

            test.Name = "Hello";

            Assert.True(ruleset.Evaluate(test));

            test.Id = 13;

            Assert.False(ruleset.Evaluate(test));

            test.Id = 1;

            Assert.True(ruleset.Evaluate(test));

            test.Name = "World";

            Assert.False(ruleset.Evaluate(test));
        }

        [Fact]
        public void Test_NestXOR()
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.Require(x => x.Id == 1).ButNot(x => x.Name == "Hello");

            var test = new TestClass();

            Assert.True(ruleset.Evaluate(test));

            test.Name = "Hello";

            Assert.False(ruleset.Evaluate(test));
        }


        [Fact]
        public void Test_NestLogicalGates()
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.ForceProcessAllRules = false;

            ruleset.Require(x => x.Id == 1)
                .And(x => x.Numbers.Contains(2))
                .ButNot(x => x.Numbers.First() == 2);

            ruleset.Disallow(x => x.Id == 2)
                .Or(x => x.Id == 3)
                .ButNot(x => x.Id == 1);

            var test = new TestClass();

            Assert.True(ruleset.Evaluate(test));

            test.Numbers[0] = 2;

            Assert.False(ruleset.Evaluate(test));

            test.Numbers[0] = 1;

            Assert.True(ruleset.Evaluate(test));

            test.Id = 3;

            Assert.False(ruleset.Evaluate(test));
        }

        [Fact]
        public void Test_Message()
        {
            var ruleset = new RuleCollection<TestClass>();

            string expected = $"{nameof(Test_Message)} requires the Id must be 1";

            ruleset.Require(x => x.Id == 1)
                .WithMessage(expected);

            var test = new TestClass();

            string actual;

            Assert.True(ruleset.Evaluate(test, out actual));

            Assert.Equal(string.Empty, actual);

            test.Id = 13;

            Assert.False(ruleset.Evaluate(test, out actual));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_Callbacks()
        {
            var ruleset = new RuleCollection<TestClass>();

            string expected = "beforepassafterbeforefailafter";

            string actual = "";

            ruleset.Require(x => x.Id == 1)
                .BeforeEvaluation(() => actual += "before")
                .OnPass(() => actual += "pass")
                .OnFail(() => actual += "fail")
                .AfterEvaluation(() => actual += "after");

            var test = new TestClass();

            Assert.True(ruleset.Evaluate(test));

            test.Id = 13;

            Assert.False(ruleset.Evaluate(test));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_IfCondition()
        {
            var ruleset = new RuleCollection<TestClass>();

            bool skipped = false;

            ruleset.Require(x => x.Name == "Jon")
                .If(x => x.Id == 1)
                .OnSkip(() => skipped = true);

            var test = new TestClass()
            {
                Id = 1,
                Name = "Not Jon"
            };

            Assert.False(ruleset.Evaluate(test));

            test.Name = "Jon";

            Assert.True(ruleset.Evaluate(test));

            test.Id = 14;

            // becuase the rule should only be ran IF the id is 1 when we change it from one this test object should not FAIL any rules
            // and should pass since "skipped" requirements should always return true
            Assert.True(ruleset.Evaluate(test));

            // verify that the callback was properly ran
            Assert.True(skipped);
        }


        private class TestClass
        {
            public int Id { get; set; } = 1;
            public string Name { get; set; } = nameof(TestClass);
            public int[] Numbers { get; set; } = { 1, 2, 3, 4, 5 };
        }
    }
}
