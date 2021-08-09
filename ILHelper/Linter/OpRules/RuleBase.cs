using System;

namespace ILHelper.Linter
{
    public abstract class RuleBase<T>
    {
        protected readonly Func<T, bool> EvaluationFunction;

        public RuleBase()
        {

        }

        protected RuleBase(Func<T, bool> evaluationFunction)
        {
            EvaluationFunction = evaluationFunction;
        }


        /// <summary>
        /// Whether the result of the <see cref="EvaluationFunction"/> should be inverted, this may be necessary for rules that define a positive result is not desired for example .DoesNot() or .DoesntContain() etc..
        /// </summary>
        public bool InvertResult { get; set; } = false;

        public abstract bool Evaluate(T Value, out string Message);

        public abstract bool Evaluate(T Value);
    }
}