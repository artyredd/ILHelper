using ILHelper.Linter.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public class Rule<T> : RuleBase<T>
    {
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// whether or not this rule should be evaluated at runtime
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Whether or not this object should consume(but still log) errors encountered during runtime.
        /// </summary>
        public bool ConsumeErrors { get; set; } = false;

        protected ICollection<(SubruleType RuleType, Rule<T> Rule)> Subrules { get; set; } = new List<(SubruleType RuleType, Rule<T> Rule)>();
        protected IDictionary<CallbackType, List<Delegate>> Callbacks { get; set; } = new Dictionary<CallbackType, List<Delegate>>();

        public Rule(Func<T, bool> evaluationFunction) : base(evaluationFunction)
        { }

        /// <summary>
        /// Evaluates the rule returns true if this rule is satisfied.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public override bool Evaluate(T Value, out string Message)
        {
            InvokeCallback(Value, CallbackType.BeforeEvaluation);

            Message = string.Empty;

            try
            {
                // check to see if we should actually evaluate this rule or not
                if (EvaluateSubrules(true, Value, out Message, SubruleType.If) is false)
                {
                    InvokeCallback(Value, CallbackType.OnSkip);

                    return true;
                }

                bool result = EvaluationFunction(Value);

                result = InvertResult ? !result : result;

                result = EvaluateSubrules(result, Value, out Message);

                if (result is false)
                {
                    InvokeCallback(Value, CallbackType.OnFail);
                    Message = this.Message;
                }
                else
                {
                    InvokeCallback(Value, CallbackType.OnPass);
                }

                InvokeCallback(Value, CallbackType.AfterEvaluation);

                return result;
            }
            catch (Exception e)
            {
                InvokeCallback(Value, CallbackType.OnThrow);

                if (ConsumeErrors is false)
                {
                    throw;
                }

                Helpers.Exceptions.Consume(e);
            }

            return false;
        }

        public override bool Evaluate(T Value)
        {
            return Evaluate(Value, out _);
        }

        private bool EvaluateSubrules(bool PreviousResult, T Value, out string Message, SubruleType Filter = SubruleType.All)
        {
            Message = string.Empty;

            foreach (var item in Subrules)
            {
                if (Filter.Contains(item.RuleType) is false)
                {
                    continue;
                }

                switch (item.RuleType)
                {
                    case SubruleType.If:
                    case SubruleType.LogicalAND:
                        PreviousResult = PreviousResult && item.Rule.Evaluate(Value, out Message);
                        continue;
                    case SubruleType.LogicalOR:
                        PreviousResult = PreviousResult || item.Rule.Evaluate(Value, out Message);
                        continue;
                    case SubruleType.LogicalXOR:
                        PreviousResult ^= item.Rule.Evaluate(Value, out Message);
                        continue;
                    case SubruleType.none:
                    default:
                        break;
                }
            }

            return PreviousResult;
        }

        /// <summary>
        /// Applies a condition that should be met for this rule to run, if the condition is <see langword="true"/> then the rule is ran and can evalutate to <see langword="false"/>, other wise if the condition is not met, this rule will always evaluate to <see langword="true"/>
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public Rule<T> If(Func<T, bool> Expression)
        {
            CreateAndAdd(Expression, SubruleType.If).InvertResult = this.InvertResult;

            // prevent infinite nesting by returning this object instead of the new rule
            return this;
        }

        public Rule<T> If(Func<bool> Expression)
        {
            CreateAndAdd((T value) => Expression(), SubruleType.If).InvertResult = this.InvertResult;

            // prevent infinite nesting by returning this object instead of the new rule
            return this;
        }

        public Rule<T> And(Func<T, bool> Expression)
        {
            CreateAndAdd(Expression, SubruleType.LogicalAND).InvertResult = this.InvertResult;

            // prevent infinite nesting by returning this object instead of the new rule
            return this;
        }

        public Rule<T> Or(Func<T, bool> Expression)
        {
            CreateAndAdd(Expression, SubruleType.LogicalOR).InvertResult = this.InvertResult;

            // prevent infinite nesting by returning this object instead of the new rule
            return this;
        }

        public Rule<T> ButNot(Func<T, bool> Expression)
        {
            CreateAndAdd(Expression, SubruleType.LogicalXOR).InvertResult = this.InvertResult;

            // prevent infinite nesting by returning this object instead of the new rule
            return this;
        }

        public Rule<T> WithMessage(string Message)
        {
            this.Message = Message;

            return this;
        }

        public Rule<T> OnSkip(Action<T> Callback)
        {
            return WithCallback(Callback, CallbackType.OnSkip);
        }

        public Rule<T> OnSkip(Action Callback)
        {
            return WithCallback(Callback, CallbackType.OnSkip);
        }

        public Rule<T> OnPass(Action<T> Callback)
        {
            return WithCallback(Callback, CallbackType.OnPass);
        }

        public Rule<T> OnPass(Action Callback)
        {
            return WithCallback(Callback, CallbackType.OnPass);
        }

        public Rule<T> OnFail(Action<T> Callback)
        {
            return WithCallback(Callback, CallbackType.OnFail);
        }

        public Rule<T> OnFail(Action Callback)
        {
            return WithCallback(Callback, CallbackType.OnFail);
        }

        public Rule<T> BeforeEvaluation(Action<T> Callback)
        {
            return WithCallback(Callback, CallbackType.BeforeEvaluation);
        }

        public Rule<T> BeforeEvaluation(Action Callback)
        {
            return WithCallback(Callback, CallbackType.BeforeEvaluation);
        }

        public Rule<T> AfterEvaluation(Action<T> Callback)
        {
            return WithCallback(Callback, CallbackType.AfterEvaluation);
        }

        public Rule<T> AfterEvaluation(Action Callback)
        {
            return WithCallback(Callback, CallbackType.AfterEvaluation);
        }

        public Rule<T> OnThrow(Action<T> Callback)
        {
            return WithCallback(Callback, CallbackType.OnThrow);
        }

        public Rule<T> OnThrow(Action Callback)
        {
            return WithCallback(Callback, CallbackType.OnThrow);
        }

        public Rule<T> WithCallback(Action<T> Callback, CallbackType CallbackKind)
        {
            return AddOrUpdateCallback(CallbackKind, Callback);
        }

        public Rule<T> WithCallback(Action Callback, CallbackType CallbackKind)
        {
            return AddOrUpdateCallback(CallbackKind, Callback);
        }

        private void InvokeCallback(T Value, CallbackType CallbackKind)
        {
            if (Callbacks.ContainsKey(CallbackKind))
            {
                List<Delegate> callbacks = Callbacks[CallbackKind];

                foreach (Delegate callback in callbacks)
                {
                    if (callback is Action action)
                    {
                        action();
                    }
                    else if (callback is Action<T> actionWithArg)
                    {
                        actionWithArg(Value);
                    }
                }
            }
        }

        private Rule<T> AddOrUpdateCallback(CallbackType Key, Delegate Callback)
        {
            if (Callbacks.ContainsKey(Key))
            {
                Callbacks[Key].Add(Callback);
            }
            else
            {
                List<Delegate> newList = new()
                {
                    Callback
                };

                Callbacks.Add(Key, newList);
            }

            return this;
        }

        private Rule<T> CreateAndAdd(Func<T, bool> Expression, SubruleType RuleType)
        {
            Rule<T> newRule = new(Expression);

            Subrules.Add((RuleType, newRule));

            return newRule;
        }
    }
}
