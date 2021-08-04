using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILHelper;
using ILHelper.Linter;
using Xunit;

namespace ILHelper.Tests
{
    public class StackTests
    {
        [Fact]
        public void PushPopBasic()
        {
            ILHelper.Linter.Stack<int> stack = new();

            stack.Push(14);

            Assert.Equal(14, stack.Pop());
        }
        [Fact]
        public void LIFO()
        {
            ILHelper.Linter.Stack<int> stack = new();

            int[] nums = { 1, 2, 3, 4, 5 };

            foreach (var item in nums)
            {
                stack.Push(item);
            }

            foreach (var item in nums.Reverse())
            {
                Assert.Equal(item, stack.Pop());
            }
        }

        [Fact]
        public void CopyToWorks()
        {
            ILHelper.Linter.Stack<int> stack = new(6);

            int[] nums = { 1, 2, 3, 4, 5 };

            foreach (var item in nums)
            {
                stack.Push(item);
            }

            int[] copied = new int[6];

            stack.CopyTo(copied, 0);

            for (int i = 0; i < nums.Length; i++)
            {
                Assert.Equal(nums[i], copied[i]);
            }
        }

        [Fact]
        public void TryPopWorks()
        {
            ILHelper.Linter.Stack<int> stack = new();

            int[] nums = { 1, 2, 3, 4, 5 };

            foreach (var item in nums)
            {
                stack.Push(item);
            }

            foreach (var item in nums.Reverse())
            {
                Assert.Equal(item, stack.Pop());
            }

            Assert.Equal(0, stack.Pop());

            Assert.False(stack.TryPop(out _));

            stack.Push(12);

            Assert.True(stack.TryPop(out _));

            Assert.False(stack.TryPop(out _));
            Assert.False(stack.TryPop(out _));
            Assert.False(stack.TryPop(out _));
        }

        [Fact]
        public void CloneWorks()
        {
            ILHelper.Linter.Stack<int> stack = new();

            int[] nums = { 1, 2, 3, 4, 5 };

            // push only one element so we can verify that values are copied as both will start with empty arrays
            stack.Push(14);

            // clone the array
            var other = stack.Clone();

            // add new elements to the original, these should not reflect in the clone
            foreach (var item in nums)
            {
                stack.Push(item);
            }

            // ensure the backing array is not a reference
            // the clone should not reflect new elements in the original
            Assert.True(other.TryPop(out _));
            Assert.False(other.TryPop(out _));
        }

        [Fact]
        public void Clear()
        {
            ILHelper.Linter.Stack<int> stack = new();

            int[] nums = { 1, 2, 3, 4, 5 };

            foreach (var item in nums)
            {
                stack.Push(item);
            }

            stack.Clear();

            Assert.False(stack.TryPop(out _));

            stack.Push(12);

            Assert.Equal(12, stack.Pop());
        }

        [Fact]
        public void CountWorks()
        {
            ILHelper.Linter.Stack<int> stack = new();

            int[] nums = { 1, 2, 3, 4, 5 };

            foreach (var item in nums)
            {
                stack.Push(item);
            }

            Assert.Equal(5, stack.Count);
        }

        [Fact]
        public void CapacityWorks()
        {
            ILHelper.Linter.Stack<int> stack = new(101);

            int[] nums = { 1, 2, 3, 4, 5 };

            foreach (var item in nums)
            {
                stack.Push(item);
            }

            int[] copied = new int[101];

            stack.CopyTo(copied, 0);

            Assert.Equal(5, stack.Count);
            Assert.Equal(101, copied.Length);

            for (int i = 0; i < 999; i++)
            {
                stack.Push(i);
            }

            // at this point the destination array should NOT be long enough
            Assert.Throws<ArgumentException>(() =>
            {
                stack.CopyTo(copied, 0);
            });
        }

        [Fact]
        public void TryPeekWorks()
        {
            ILHelper.Linter.Stack<int> stack = new();

            int[] nums = { 1, 2, 3, 4, 5 };

            foreach (var item in nums)
            {
                stack.Push(item);
            }

            bool pass = stack.TryPeek(out int Value, 0);

            Assert.True(pass);
            Assert.Equal(5, Value);

            pass = stack.TryPeek(out Value, 1);

            Assert.True(pass);
            Assert.Equal(4, Value);

            pass = stack.TryPeek(out Value, 2);

            Assert.True(pass);
            Assert.Equal(3, Value);

            pass = stack.TryPeek(out Value, 3);

            Assert.True(pass);
            Assert.Equal(2, Value);

            pass = stack.TryPeek(out Value, 4);

            Assert.True(pass);
            Assert.Equal(1, Value);

            pass = stack.TryPeek(out Value, 5);

            Assert.False(pass);
            Assert.Equal(0, Value);

            pass = stack.TryPeek(out Value, 9999);

            Assert.False(pass);
            Assert.Equal(0, Value);

            pass = stack.TryPeek(out Value, -9999);

            Assert.False(pass);
            Assert.Equal(0, Value);

            pass = stack.TryPeek(out Value, int.MaxValue);

            Assert.False(pass);
            Assert.Equal(0, Value);

            pass = stack.TryPeek(out Value, int.MinValue);

            Assert.False(pass);
            Assert.Equal(0, Value);
        }

        [Fact]
        public void MultiPeekWorks()
        {
            int[] peeked;
            bool pass;

            ILHelper.Linter.Stack<int> stack = new();

            int[] nums = { 1, 2, 3, 4, 5 };

            pass = stack.TryPeek(out peeked, 0, 0);

            Assert.False(pass);
            Assert.Empty(peeked);

            foreach (var item in nums)
            {
                stack.Push(item);
            }

            pass = stack.TryPeek(out peeked, 0, 0);

            Assert.True(pass);
            Assert.Single(peeked);
            Assert.Equal(5, peeked[0]);

            pass = stack.TryPeek(out peeked, 0, 1);

            Assert.True(pass);
            Assert.Equal(2, peeked.Length);
            Assert.Equal(5, peeked[0]);
            Assert.Equal(4, peeked[1]);

            pass = stack.TryPeek(out peeked, 0, stack.Count - 1);

            Assert.True(pass);
            Assert.Equal(5, peeked.Length);
            Assert.Equal(5, peeked[0]);
            Assert.Equal(4, peeked[1]);
            Assert.Equal(3, peeked[2]);
            Assert.Equal(2, peeked[3]);
            Assert.Equal(1, peeked[4]);
        }
    }
}
