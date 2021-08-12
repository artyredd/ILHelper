using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using ILHelper.Linter.Extensions;
using System.Diagnostics;

#nullable enable
namespace ILHelper.Linter
{
    public class MemberRule<T, TParent> : RuleBase<TParent>
    {
        private readonly Type MemberTypeInfo = typeof(T);
        private readonly Type ParentTypeInfo = typeof(TParent);
        private FieldInfo? BackingField;
        private PropertyInfo? BackingProperty;

        /// <summary>
        /// Is <see langword="true"/> when <see cref="MemberName"/> actually exists within <see cref="ParentTypeInfo"/> and a backing field/property is populated
        /// </summary>
        private bool Validated;

        /// <summary>
        /// The name of the property or field
        /// </summary>
        public string MemberName { get; init; } = string.Empty;

        /// <summary>
        /// The type of member this should reference
        /// <para>
        /// Allowable Values
        /// <list type="bullet">
        /// <item>
        /// <see cref="MemberTypes.Field"/>
        /// </item>
        /// <item>
        /// <see cref="MemberTypes.Property"/>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        public MemberTypes MemberType { get; init; }

        /// <summary>
        /// Defines the access modifiers for the member, by default the member can be Public/Private and Instanced/Static
        /// </summary>
        public BindingFlags MemberFlags { get; set; } = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        public bool IsField { get; private set; }

        public bool IsProperty { get; private set; }

        public Rule<T>? BackingRule { get; private set; }

        public override bool Evaluate(TParent Value, out string Message)
        {
            Message = string.Empty;

            if (Validated is false)
            {
                // will throw if validation fails
                ValidateBackingFields();
            }

            T? memberValue = GetValue(Value);

            return BackingRule?.Evaluate(memberValue!, out Message) ?? false;
        }

        public override bool Evaluate(TParent Value)
        {
            return Evaluate(Value, out _);
        }

        /// <returns>
        /// <see langword="true"/> when the member was sucessfully found within the parent type. Otherwise throws an error.
        /// </returns>
        /// <exception cref="Exception"> Generic </exception>
        public virtual bool Validate()
        {
            ValidateBackingFields();

            return Validated;
        }

        public virtual MemberRule<T, TParent> NotNull()
        {
            AddBackingExpression((x) => x is not null, SubruleType.LogicalAND);

            return this;
        }

        public virtual MemberRule<T, TParent> Null()
        {
            AddBackingExpression((x) => x is null, SubruleType.LogicalAND);

            return this;

        }

        public virtual MemberRule<T, TParent> EqualTo(T Value)
        {
            AddBackingExpression((x) => Value!.Equals(x), SubruleType.LogicalAND);

            return this;
        }

        public virtual MemberRule<T, TParent> EqualToAll(params T[] Values)
        {
            AddMultiExpression(Values, (x, y) => x!.Equals(y), SubruleType.LogicalAND);

            return this;
        }

        public virtual MemberRule<T, TParent> EqualToAny(params T[] Values)
        {
            AddMultiExpression(Values, (x, y) => x!.Equals(y), SubruleType.LogicalOR);

            return this;
        }

        public virtual MemberRule<T, TParent> NotEqualTo(T Value)
        {
            AddBackingExpression((x) => Value!.Equals(x) is false, SubruleType.LogicalAND);

            return this;
        }

        public virtual MemberRule<T, TParent> NotEqualToAll(params T[] Values)
        {
            AddMultiExpression(Values, (x, y) => x!.Equals(y) is false, SubruleType.LogicalAND); ;

            return this;
        }

        public virtual MemberRule<T, TParent> NotEqualToAny(params T[] Values)
        {
            AddMultiExpression(Values, (x, y) => x!.Equals(y) is false, SubruleType.LogicalOR);

            return this;
        }

        public virtual MemberRule<T, TParent> EqualTo<TCompare>(TCompare Value) where TCompare : IEquatable<T>
        {
            AddBackingExpression((x) => Value.Equals(x), SubruleType.LogicalAND);

            return this;
        }

        public virtual MemberRule<T, TParent> EqualToAll<TCompare>(params TCompare[] Values) where TCompare : IEquatable<T>
        {
            AddMultiExpression(Values, (x, y) => y.Equals(x), SubruleType.LogicalAND);

            return this;
        }

        public virtual MemberRule<T, TParent> EqualToAny<TCompare>(params TCompare[] Values) where TCompare : IEquatable<T>
        {
            AddMultiExpression(Values, (x, y) => y.Equals(x), SubruleType.LogicalOR);

            return this;
        }

        public virtual MemberRule<T, TParent> NotEqualTo<TCompare>(TCompare Value) where TCompare : IEquatable<T>
        {
            AddBackingExpression((x) => Value.Equals(x) is false, SubruleType.LogicalAND);

            return this;
        }

        public virtual MemberRule<T, TParent> NotEqualToAll<TCompare>(params TCompare[] Values) where TCompare : IEquatable<T>
        {
            AddMultiExpression(Values, (x, y) => y.Equals(x) is false, SubruleType.LogicalAND);

            return this;
        }

        public virtual MemberRule<T, TParent> NotEqualToAny<TCompare>(params TCompare[] Values) where TCompare : IEquatable<T>
        {
            AddMultiExpression(Values, (x, y) => y.Equals(x) is false, SubruleType.LogicalOR);

            return this;
        }

        public virtual MemberRule<T, TParent> GreaterThan<TCompare>(TCompare Value) where TCompare : IComparable<T>
        {
            AddBackingExpression((x) => Value.CompareTo(x) < 0, SubruleType.LogicalAND);

            return this;
        }

        public virtual MemberRule<T, TParent> GreaterThanAll<TCompare>(params TCompare[] Values) where TCompare : IComparable<T>
        {
            AddMultiExpression(Values, (x, y) => y.CompareTo(x) < 0, SubruleType.LogicalAND);

            return this;
        }

        public virtual MemberRule<T, TParent> GreaterThanAny<TCompare>(params TCompare[] Values) where TCompare : IComparable<T>
        {
            AddMultiExpression(Values, (x, y) => y.CompareTo(x) < 0, SubruleType.LogicalOR);

            return this;
        }

        public virtual MemberRule<T, TParent> LessThan<TCompare>(TCompare Value) where TCompare : IComparable<T>
        {
            AddBackingExpression((x) => Value.CompareTo(x) > 0, SubruleType.LogicalAND);

            return this;
        }

        public virtual MemberRule<T, TParent> LessThanAll<TCompare>(params TCompare[] Values) where TCompare : IComparable<T>
        {
            AddMultiExpression(Values, (x, y) => y.CompareTo(x) > 0, SubruleType.LogicalAND);

            return this;
        }

        public virtual MemberRule<T, TParent> LessThanAny<TCompare>(params TCompare[] Values) where TCompare : IComparable<T>
        {
            AddMultiExpression(Values, (x, y) => y.CompareTo(x) > 0, SubruleType.LogicalOR);

            return this;
        }

        public virtual MemberRule<T, TParent> GreaterThanOrEqualTo<TCompare>(TCompare Value) where TCompare : IComparable<T>
        {
            AddBackingExpression((x) => Value.CompareTo(x) <= 0, SubruleType.LogicalAND);

            return this;
        }

        public virtual MemberRule<T, TParent> GreaterThanOrEqualToAll<TCompare>(params TCompare[] Values) where TCompare : IComparable<T>
        {
            AddMultiExpression(Values, (x, y) => y.CompareTo(x) <= 0, SubruleType.LogicalAND);

            return this;
        }

        public virtual MemberRule<T, TParent> GreaterThanOrEqualToAny<TCompare>(params TCompare[] Values) where TCompare : IComparable<T>
        {
            AddMultiExpression(Values, (x, y) => y.CompareTo(x) <= 0, SubruleType.LogicalOR);

            return this;
        }

        public virtual MemberRule<T, TParent> LessThanOrEqualTo<TCompare>(TCompare Value) where TCompare : IComparable<T>
        {
            AddBackingExpression((x) => Value.CompareTo(x) >= 0, SubruleType.LogicalAND);

            return this;
        }

        public virtual MemberRule<T, TParent> LessThanOrEqualToAll<TCompare>(params TCompare[] Values) where TCompare : IComparable<T>
        {
            AddMultiExpression(Values, (x, y) => y.CompareTo(x) >= 0, SubruleType.LogicalAND);

            return this;
        }

        public virtual MemberRule<T, TParent> LessThanOrEqualToAny<TCompare>(params TCompare[] Values) where TCompare : IComparable<T>
        {
            AddMultiExpression(Values, (x, y) => y.CompareTo(x) >= 0, SubruleType.LogicalOR);

            return this;
        }

        public virtual MemberRule<T, TParent> OnSkip(Action<T> Callback)
        {
            return WithCallback(Callback, CallbackType.OnSkip);
        }

        public virtual MemberRule<T, TParent> OnSkip(Action Callback)
        {
            return WithCallback(Callback, CallbackType.OnSkip);
        }

        public virtual MemberRule<T, TParent> OnPass(Action<T> Callback)
        {
            return WithCallback(Callback, CallbackType.OnPass);
        }

        public virtual MemberRule<T, TParent> OnPass(Action Callback)
        {
            return WithCallback(Callback, CallbackType.OnPass);
        }

        public virtual MemberRule<T, TParent> OnFail(Action<T> Callback)
        {
            return WithCallback(Callback, CallbackType.OnFail);
        }

        public virtual MemberRule<T, TParent> OnFail(Action Callback)
        {
            return WithCallback(Callback, CallbackType.OnFail);
        }

        public virtual MemberRule<T, TParent> BeforeEvaluation(Action<T> Callback)
        {
            return WithCallback(Callback, CallbackType.BeforeEvaluation);
        }

        public virtual MemberRule<T, TParent> BeforeEvaluation(Action Callback)
        {
            return WithCallback(Callback, CallbackType.BeforeEvaluation);
        }

        public virtual MemberRule<T, TParent> AfterEvaluation(Action<T> Callback)
        {
            return WithCallback(Callback, CallbackType.AfterEvaluation);
        }

        public virtual MemberRule<T, TParent> AfterEvaluation(Action Callback)
        {
            return WithCallback(Callback, CallbackType.AfterEvaluation);
        }

        public virtual MemberRule<T, TParent> OnThrow(Action<T> Callback)
        {
            return WithCallback(Callback, CallbackType.OnThrow);
        }

        public virtual MemberRule<T, TParent> OnThrow(Action Callback)
        {
            return WithCallback(Callback, CallbackType.OnThrow);
        }

        public virtual MemberRule<T, TParent> WithCallback(Action<T> Callback, CallbackType CallbackKind)
        {
            // becuase this is just a wrapper for a property or field on the TParent, redirect all backbacks to the backing rule
            BackingRule?.WithCallback(Callback, CallbackKind);

            return this;
        }

        public virtual MemberRule<T, TParent> WithCallback(Action Callback, CallbackType CallbackKind)
        {
            // becuase this is just a wrapper for a property or field on the TParent, redirect all backbacks to the backing rule
            BackingRule?.WithCallback(Callback, CallbackKind);

            return this;
        }

        public virtual MemberRule<T, TParent> WithMessage(string Message)
        {
            BackingRule?.WithMessage(Message);

            return this;
        }

        private void AddMultiExpression<U>(IEnumerable<U> Values, Func<T, U, bool> Expression, SubruleType RuleType)
        {
            if (RuleType.Contains(SubruleType.LogicalAND))
            {
                CreateBackingRule((x) => true);
                BackingRule?.And((t) => MultiExpressionAND<U>(t, Values, Expression));
            }
            else
            if (RuleType.Contains(SubruleType.LogicalOR))
            {
                CreateBackingRule((x) => false);
                BackingRule?.Or((t) => MultiExpressionOR<U>(t, Values, Expression));
            }
            else
            {
                throw Helpers.Exceptions.Generic($"Only LogicalAND and LogicalOR Expressions are supported with multiple comparisons, use Rule<T>.ButNot() instead of MemberBase<>.ComparisonNameAny()");
            }
        }

        private static bool MultiExpressionAND<U>(T Value, IEnumerable<U> Values, Func<T, U, bool> Expression)
        {
            return BaseMultiExpression<U>(Value, Values, Expression, (left, right) => left && right, true);
        }

        private static bool MultiExpressionOR<U>(T Value, IEnumerable<U> Values, Func<T, U, bool> Expression)
        {
            return BaseMultiExpression<U>(Value, Values, Expression, (left, right) => left || right);
        }

        private static bool BaseMultiExpression<U>(T Value, IEnumerable<U> Values, Func<T, U, bool> Expression, Func<bool, bool, bool> Operator, bool startingValue = false)
        {
            foreach (var item in Values)
            {
                startingValue = Operator(startingValue, Expression(Value, item));
            }

            return startingValue;
        }

        private void AddBackingExpression(Func<T, bool> Expression, SubruleType RuleType)
        {
            CreateBackingRule(Expression);

            if (RuleType.Contains(SubruleType.LogicalAND))
            {
                BackingRule?.And(Expression);
            }
            else
            if (RuleType.Contains(SubruleType.LogicalOR))
            {
                BackingRule?.Or(Expression);
            }
            else
            if (RuleType.Contains(SubruleType.LogicalXOR))
            {
                BackingRule?.ButNot(Expression);
            }
        }

        private void CreateBackingRule(Func<T, bool> Expression)
        {
            BackingRule ??= new(Expression); ;
        }

        /// <summary>
        /// Ensures that during runtime the assigned field actually exists within the parent type
        /// </summary>
        private void ValidateBackingFields()
        {
            if (MemberType.Contains(MemberTypes.Field))
            {
                BackingField = ParentTypeInfo.GetField(MemberName, MemberFlags);

                Validated = BackingField is not null && BackingField.FieldType == MemberTypeInfo;

                // if the backing fields is still null, either the field doesn't exist or the wrong member type was selected
                if (Validated)
                {
                    // store boolean flag for speed, we dont want to constantly compare backing fields
                    IsField = true;

                    return;
                }
            }
            else if (MemberType.Contains(MemberTypes.Property))
            {
                BackingProperty = ParentTypeInfo.GetProperty(MemberName, MemberFlags);

                Validated = BackingProperty is not null && BackingProperty.PropertyType == MemberTypeInfo;

                // if the backing fields is still null, either the field doesn't exist or the wrong member type was selected
                if (Validated)
                {
                    // store boolean flag for speed, we dont want to constantly compare backing fields
                    IsProperty = true;

                    return;
                }
            }

            throw Helpers.Exceptions.Generic($"Can not validate that '{MemberTypeInfo.Name} {MemberName}' exists within the containing type of '{ParentTypeInfo.Name}' as a {MemberType}. The only supported {nameof(MemberTypes)} are {nameof(MemberTypes.Field)} and {nameof(MemberTypes.Property)}");
        }

        [DebuggerHidden]
        private T? GetValue(TParent Parent)
        {
            // backing field and backing property can't be null becuase ValidateBackingFields should ALWAYS be ran first
            if (IsField)
            {
                return (T?)BackingField!.GetValue(Parent);
            }

            return (T?)BackingProperty!.GetValue(Parent);
        }
    }
}
