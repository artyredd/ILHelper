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


        [Fact]
        public void Test_Properties()
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>("Id")
                .NotNull();

            var instance = new TestClass();

            Assert.True(ruleset.Evaluate(instance));
        }

        [Fact]
        public void Test_Properties_Name()
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .NotNull();

            var instance = new TestClass();

            Assert.True(ruleset.Evaluate(instance));
        }

        [Fact]
        public void Test_Properties_NotNull()
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<string>(x => nameof(x.Name))
                .NotNull();

            var instance = new TestClass();

            Assert.True(ruleset.Evaluate(instance));

            instance.Name = null;

            Assert.False(ruleset.Evaluate(instance));
        }

        [Fact]
        public void Test_Properties_Null()
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<string>(x => nameof(x.Name))
                .Null();

            var instance = new TestClass();

            Assert.False(ruleset.Evaluate(instance));

            instance.Name = null;

            Assert.True(ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(1, 2, false)]
        [InlineData(2, 2, true)]
        [InlineData(3, 2, false)]
        [InlineData(99, 2, false)]
        public void Test_Equals(int input, int other, bool expected)
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .EqualTo(other);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(1, true, 1, 2, 3, 4, 5)]
        [InlineData(2, true, 1, 2, 3, 4, 5)]
        [InlineData(0, false, 1, 2, 3, 4, 5)]
        [InlineData(5, true, 1, 2, 3, 4, 5)]
        [InlineData(99, false, 1, 2, 3, 4, 5)]
        public void Test_EqualsAny(int input, bool expected, params int[] others)
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .EqualToAny(others);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(-1, false, 1, 2, 3, 4, 5)]
        [InlineData(0, false, 1, 2, 3, 4, 5)]
        [InlineData(2, false, 1, 2, 3, 4, 5)]
        [InlineData(5, false, 1, 2, 3, 4, 5)]
        [InlineData(99, false, 1, 2, 3, 4, 5)]
        [InlineData(1, true, 1, 1, 1, 1, 1)]
        [InlineData(2, true, 2, 2)]
        public void Test_EqualsAll(int input, bool expected, params int[] others)
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .EqualToAll(others);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(1, 2, false)]
        [InlineData(2, 2, false)]
        [InlineData(3, 2, true)]
        [InlineData(99, 2, true)]
        public void Test_GreaterThan(int input, int other, bool expected)
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .GreaterThan(other);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(1, false, 1, 2, 3, 4, 5)]
        [InlineData(2, true, 1, 2, 3, 4, 5)]
        [InlineData(0, false, 1, 2, 3, 4, 5)]
        [InlineData(5, true, 1, 2, 3, 4, 5)]
        [InlineData(99, true, 1, 2, 3, 4, 5)]
        public void Test_GreaterThanAny(int input, bool expected, params int[] others)
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .GreaterThanAny(others);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(-1, false, 1, 2, 3, 4, 5)]
        [InlineData(0, false, 1, 2, 3, 4, 5)]
        [InlineData(2, false, 1, 2, 3, 4, 5)]
        [InlineData(5, false, 1, 2, 3, 4, 5)]
        [InlineData(99, true, 1, 2, 3, 4, 5)]
        public void Test_GreaterThanAll(int input, bool expected, params int[] others)
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .GreaterThanAll(others);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(1, 2, false)]
        [InlineData(2, 2, true)]
        [InlineData(0, 2, false)]
        [InlineData(99, 2, true)]
        public void Test_GreaterThanEqual(int input, int other, bool expected)
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .GreaterThanOrEqualTo(other);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(1, true, 1, 2, 3, 4, 5)]
        [InlineData(2, true, 1, 2, 3, 4, 5)]
        [InlineData(0, false, 1, 2, 3, 4, 5)]
        [InlineData(5, true, 1, 2, 3, 4, 5)]
        [InlineData(6, true, 1, 2, 3, 4, 5)]
        [InlineData(99, true, 1, 2, 3, 4, 5)]
        public void Test_GreaterThanEqualAny(int input, bool expected, params int[] others)
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .GreaterThanOrEqualToAny(others);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(-1, false, 1, 2, 3, 4, 5)]
        [InlineData(0, false, 1, 2, 3, 4, 5)]
        [InlineData(2, false, 1, 2, 3, 4, 5)]
        [InlineData(5, true, 1, 2, 3, 4, 5)]
        [InlineData(99, true, 1, 2, 3, 4, 5)]
        public void Test_GreaterThanEqualAll(int input, bool expected, params int[] others)
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .GreaterThanOrEqualToAll(others);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(1, 2, true)]
        [InlineData(2, 2, false)]
        [InlineData(0, 2, true)]
        [InlineData(99, 2, false)]
        public void Test_LessThan(int input, int other, bool expected)
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .LessThan(other);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(1, true, 1, 2, 3, 4, 5)]
        [InlineData(2, true, 1, 2, 3, 4, 5)]
        [InlineData(0, true, 1, 2, 3, 4, 5)]
        [InlineData(5, false, 1, 2, 3, 4, 5)]
        [InlineData(99, false, 1, 2, 3, 4, 5)]
        public void Test_LessThanAny(int input, bool expected, params int[] others)
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .LessThanAny(others);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(-1, true, 1, 2, 3, 4, 5)]
        [InlineData(0, true, 1, 2, 3, 4, 5)]
        [InlineData(2, false, 1, 2, 3, 4, 5)]
        [InlineData(5, false, 1, 2, 3, 4, 5)]
        [InlineData(99, false, 1, 2, 3, 4, 5)]
        public void Test_LessThanAll(int input, bool expected, params int[] others)
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .LessThanAll(others);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(1, 2, true)]
        [InlineData(2, 2, true)]
        [InlineData(0, 2, true)]
        [InlineData(99, 2, false)]
        public void Test_LessThanEqual(int input, int other, bool expected)
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .LessThanOrEqualTo(other);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(1, true, 1, 2, 3, 4, 5)]
        [InlineData(2, true, 1, 2, 3, 4, 5)]
        [InlineData(0, true, 1, 2, 3, 4, 5)]
        [InlineData(5, true, 1, 2, 3, 4, 5)]
        [InlineData(6, false, 1, 2, 3, 4, 5)]
        [InlineData(99, false, 1, 2, 3, 4, 5)]
        public void Test_LessThanEqualAny(int input, bool expected, params int[] others)
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .LessThanOrEqualToAny(others);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(-1, true, 1, 2, 3, 4, 5)]
        [InlineData(0, true, 1, 2, 3, 4, 5)]
        [InlineData(2, false, 1, 2, 3, 4, 5)]
        [InlineData(5, false, 1, 2, 3, 4, 5)]
        [InlineData(99, false, 1, 2, 3, 4, 5)]
        public void Test_LessThanEqualAll(int input, bool expected, params int[] others)
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .LessThanOrEqualToAll(others);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(1, 2, true)]
        [InlineData(2, 2, false)]
        [InlineData(0, 2, true)]
        [InlineData(99, 2, true)]
        [InlineData(0, 0, false)]
        public void Test_NotEqual(int input, int other, bool expected)
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .NotEqualTo(other);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(1, true, 1, 2, 3, 4, 5)]
        [InlineData(2, true, 1, 2, 3, 4, 5)]
        [InlineData(0, true, 1, 2, 3, 4, 5)]
        [InlineData(5, true, 1, 2, 3, 4, 5)]
        [InlineData(6, true, 1, 2, 3, 4, 5)]
        [InlineData(99, true, 1, 2, 3, 4, 5)]
        [InlineData(1, false, 1, 1, 1, 1, 1)]
        public void Test_NotEqualAny(int input, bool expected, params int[] others)
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .NotEqualToAny(others);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(-1, true, 1, 2, 3, 4, 5)]
        [InlineData(0, true, 1, 2, 3, 4, 5)]
        [InlineData(2, false, 1, 2, 3, 4, 5)]
        [InlineData(5, false, 1, 2, 3, 4, 5)]
        [InlineData(99, true, 1, 2, 3, 4, 5)]
        public void Test_NotEqualAll(int input, bool expected, params int[] others)
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .NotEqualToAll(others);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Fact]
        public void Test_Properties_LessThanorEqual()
        {
            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .LessThanOrEqualTo(13);

            var instance = new TestClass();

            Assert.True(ruleset.Evaluate(instance));

            instance.Id = 13;

            Assert.True(ruleset.Evaluate(instance));

            instance.Id = 99;

            Assert.False(ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(1, 2, true)]
        [InlineData(2, 2, false)]
        [InlineData(0, 2, true)]
        [InlineData(99, 2, true)]
        [InlineData(0, 0, false)]
        public void Test_NotEqualDifferentType(byte input, int other, bool expected)
        {
            var other_casted = new IntKnockoff(other);

            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .NotEqualTo(other_casted);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(1, true, 1, 2, 3, 4, 5)]
        [InlineData(2, true, 1, 2, 3, 4, 5)]
        [InlineData(0, true, 1, 2, 3, 4, 5)]
        [InlineData(5, true, 1, 2, 3, 4, 5)]
        [InlineData(6, true, 1, 2, 3, 4, 5)]
        [InlineData(99, true, 1, 2, 3, 4, 5)]
        [InlineData(1, false, 1, 1, 1, 1, 1)]
        public void Test_NotEqualAnyDifferentType(byte input, bool expected, params int[] others)
        {
            var others_casted = new IntKnockoff[others.Length];

            for (int i = 0; i < others.Length; i++)
            {
                others_casted[i] = new(others[i]);
            }

            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .NotEqualToAny(others_casted);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(72, true, 1, 2, 3, 4, 5)]
        [InlineData(0, true, 1, 2, 3, 4, 5)]
        [InlineData(2, false, 1, 2, 3, 4, 5)]
        [InlineData(5, false, 1, 2, 3, 4, 5)]
        [InlineData(99, true, 1, 2, 3, 4, 5)]
        public void Test_NotEqualAllDifferentType(byte input, bool expected, params int[] others)
        {
            var others_casted = new IntKnockoff[others.Length];

            for (int i = 0; i < others.Length; i++)
            {
                others_casted[i] = new(others[i]);
            }

            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .NotEqualToAll(others_casted);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(1, 2, false)]
        [InlineData(2, 2, true)]
        [InlineData(3, 2, false)]
        [InlineData(99, 2, false)]
        public void Test_EqualDifferentType(byte input, int other, bool expected)
        {
            var other_casted = new IntKnockoff(other);

            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .EqualTo(other_casted);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(1, true, 1, 2, 3, 4, 5)]
        [InlineData(2, true, 1, 2, 3, 4, 5)]
        [InlineData(0, false, 1, 2, 3, 4, 5)]
        [InlineData(5, true, 1, 2, 3, 4, 5)]
        [InlineData(99, false, 1, 2, 3, 4, 5)]
        public void Test_EqualAnyDifferentType(byte input, bool expected, params int[] others)
        {
            var others_casted = new IntKnockoff[others.Length];

            for (int i = 0; i < others.Length; i++)
            {
                others_casted[i] = new(others[i]);
            }

            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .EqualToAny(others_casted);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        [Theory]
        [InlineData(0, false, 1, 2, 3, 4, 5)]
        [InlineData(2, false, 1, 2, 3, 4, 5)]
        [InlineData(5, false, 1, 2, 3, 4, 5)]
        [InlineData(99, false, 1, 2, 3, 4, 5)]
        [InlineData(1, true, 1, 1, 1, 1, 1)]
        [InlineData(2, true, 2, 2)]
        public void Test_EqualAllDifferentType(byte input, bool expected, params int[] others)
        {
            var others_casted = new IntKnockoff[others.Length];

            for (int i = 0; i < others.Length; i++)
            {
                others_casted[i] = new(others[i]);
            }

            var ruleset = new RuleCollection<TestClass>();

            ruleset.RequireProperty<int>(x => nameof(x.Id))
                .EqualToAll(others_casted);

            var instance = new TestClass();

            instance.Id = input;

            Assert.Equal(expected, ruleset.Evaluate(instance));
        }

        private class TestClass
        {
            public int Id { get; set; } = 1;
            public string Name { get; set; } = nameof(TestClass);
            public int[] Numbers { get; set; } = { 1, 2, 3, 4, 5 };
        }

        public class IntKnockoff : IComparable<int>, IEquatable<int>
        {
            public int Value { get; set; } = 0;

            public IntKnockoff(int value)
            {
                Value = value;
            }

            public int CompareTo(int other)
            {
                return Value.CompareTo(other);
            }

            public bool Equals(int other)
            {
                return Value.Equals(other);
            }
        }
    }
}
