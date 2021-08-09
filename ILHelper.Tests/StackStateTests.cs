using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ILHelper.Linter;
using System.Reflection.Emit;

namespace ILHelper.Tests
{
    public class StackStateTests
    {
        [Fact]
        public void Test_()
        {
            var stackState = new StackState();

            stackState.Stack.Push(new StackElement()
            {
                UnderlyingType = typeof(int),
                VerificationType = StackVerificationType.Int32
            });
            stackState.Stack.Push(new StackElement()
            {
                UnderlyingType = typeof(int),
                VerificationType = StackVerificationType.Int32
            });

            OpCode currentInstruction = OpCodes.Add;

            var rules = new RuleCollection<StackState>();

            rules.Require(x => x.HasCount(2))
                .And(x => x.Top(2).Where(x => x.VerificationType == StackVerificationType.Int32).Count() == 2)
                .If(() => currentInstruction == OpCodes.Add)
                .OnPass(x => x.Pop(2))
                .OnPass(x => x.Stack.Push(new StackElement() { VerificationType = StackVerificationType.Int32, UnderlyingType = typeof(int) }));


            Assert.True(rules.Evaluate(stackState));

            Assert.Equal(1, stackState.Count);

            Assert.Equal(StackVerificationType.Int32, stackState.Top().VerificationType);
        }
    }
}
