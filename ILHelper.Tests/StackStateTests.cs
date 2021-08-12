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

        [Fact]
        public void Test_RuleDictionaryIntegration()
        {
            var stackState = new StackState();

            stackState.Stack.Push(new StackElement()
            {
                UnderlyingType = typeof(int),
                VerificationType = StackVerificationType.Int32
            });


            OpCode currentInstruction = OpCodes.Add;

            var ruleDict = new RuleDictionary<OpCode, StackState>();

            ruleDict.Case(OpCodes.Add)
                .RequireProperty<int>(stack => nameof(stack.Count))
                .GreaterThanOrEqualTo(2)
                .WithMessage("Add expects at least two objects on the stack of which to add together.");

            ruleDict.Case(OpCodes.Add)
                .Require(stack => stack.Top(2).Where(item => item.VerificationType == StackVerificationType.Int32).Count() == 2)
                .WithMessage("The two objects on the stack need to be compatible to add.");

            string message = string.Empty;

            Assert.False(ruleDict.Evaluate(OpCodes.Add, stackState, out message));

            Assert.Equal("Add expects at least two objects on the stack of which to add together.", message);

            stackState.Stack.Push(new StackElement()
            {
                UnderlyingType = typeof(int),
                VerificationType = StackVerificationType.Float64
            });

            Assert.False(ruleDict.Evaluate(OpCodes.Add, stackState, out message));

            Assert.Equal("The two objects on the stack need to be compatible to add.", message);

            stackState.Stack.Pop();

            stackState.Stack.Push(new StackElement()
            {
                UnderlyingType = typeof(int),
                VerificationType = StackVerificationType.Int32
            });

            Assert.True(ruleDict.Evaluate(OpCodes.Add, stackState, out message));

            Assert.Equal("", message);
        }
    }
}
