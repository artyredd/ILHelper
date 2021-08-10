using System;
using System.Collections.Generic;

namespace ILHelper.Linter
{
    public abstract class RuleBase<T>
    {
        public RuleBase()
        {

        }

        /// <summary>
        /// Whether the result of the <see cref="EvaluationFunction"/> should be inverted, this may be necessary for rules that define a positive result is not desired for example .DoesNot() or .DoesntContain() etc..
        /// </summary>
        public bool InvertResult { get; set; } = false;

        /// <summary>
        /// Whether or not this should be evaluated during runtime
        /// </summary>
        public bool Enabled { get; set; } = true;

        public abstract bool Evaluate(T Value, out string Message);

        public abstract bool Evaluate(T Value);
    }
}