using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace ILHelper.Linter
{
    public class RuleCollection<T>
    {
        public ICollection<RuleBase<T>> Rules { get; private set; } = new List<RuleBase<T>>();

        public bool ForceProcessAllRules { get; set; } = false;

        public bool Evaluate(T Value)
        {
            return Evaluate(Value, out _);
        }

        public bool Evaluate(T Value, out string Message)
        {
            Message = string.Empty;

            // becuase rules shouldn't introduce a state change we could parellelize the rule eval
            foreach (var rule in Rules)
            {
                if (rule.Enabled)
                {
                    if (rule.Evaluate(Value, out Message) is false)
                    {
                        if (ForceProcessAllRules is false)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public MemberRule<U, T> RequireProperty<U>(Func<T, string> MemberName)
        {
            return CreateAndAdd<U>(MemberName(default!), MemberTypes.Property);
        }

        public MemberRule<U, T> RequireProperty<U>(string MemberName)
        {
            return CreateAndAdd<U>(MemberName, MemberTypes.Property);
        }

        public MemberRule<U, T> RequireField<U>(Func<T, string> MemberName)
        {
            return CreateAndAdd<U>(MemberName(default!), MemberTypes.Field);
        }

        public MemberRule<U, T> RequireField<U>(string MemberName)
        {
            return CreateAndAdd<U>(MemberName, MemberTypes.Field);
        }

        public Rule<T> Disallow(Func<T, bool> Expression)
        {
            var newRule = CreateAndAdd(Expression);

            newRule.InvertResult = true;

            return newRule;
        }

        public Rule<T> Require(Func<T, bool> Expression)
        {
            return CreateAndAdd(Expression);
        }

        public void Clear()
        {
            Rules.Clear();
        }

        private Rule<T> CreateAndAdd(Func<T, bool> Expression)
        {
            Rule<T> newRule = new(Expression);

            Rules.Add(newRule);

            return newRule;
        }

        private MemberRule<U, T> CreateAndAdd<U>(string MemberName, MemberTypes MemberType)
        {
            MemberRule<U, T> newRule = new MemberRule<U, T>()
            {
                MemberName = MemberName,
                MemberType = MemberType
            };

            Rules.Add(newRule);

            return newRule;
        }
    }
}
