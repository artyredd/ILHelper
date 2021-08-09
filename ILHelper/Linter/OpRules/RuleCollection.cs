using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace ILHelper.Linter
{
    public class RuleCollection<T>
    {
        public ICollection<Rule<T>> Rules { get; private set; } = new List<Rule<T>>();

        private Rule<T>? LastChanged = null;

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

        private Rule<T> CreateAndAdd(Func<T, bool> Expression)
        {
            Rule<T> newRule = new(Expression);

            Rules.Add(newRule);

            LastChanged = newRule;

            return newRule;
        }
    }
}
