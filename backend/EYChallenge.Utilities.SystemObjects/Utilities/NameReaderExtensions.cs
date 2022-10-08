using EYChallenge.Utilities.SystemObjects.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EYChallenge.Utilities.SystemObjects.Utilities
{
    public static class NameReaderExtensions
    {
        private static readonly string expressionCannotBeNullMessage = "The expression cannot be null.";
        private static readonly string invalidExpressionMessage = "Invalid expression.";

        public static string GetMemberName<T>(this T instance, Expression<Func<T, object>> expression)
        {
            return GetMemberName(expression.Body);
        }

        public static string GetMemberName<T>(this T instance, Expression<Action<T>> expression)
        {
            return GetMemberName(expression.Body);
        }

        public static List<string> GetMemberNames<T>(this T instance, params Expression<Func<T, object>>[] expressions)
        {
            List<string> memberNames = new List<string>();
            foreach (var cExpression in expressions)
            {
                memberNames.Add(GetMemberName(cExpression.Body));
            }

            return memberNames;
        }

        private static string GetMemberName(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(expressionCannotBeNullMessage);
            }
            // Reference type property or field

            if (expression is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }
            // Reference type method

            if (expression is MethodCallExpression methodCallExpression)
            {
                return methodCallExpression.Method.Name;
            }
            // Property, field of method returning value type

            if (expression is UnaryExpression unaryExpression)
            {
                return GetMemberName(unaryExpression);
            }

            throw new ArgumentException(invalidExpressionMessage);
        }

        private static string GetMemberName(UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression methodExpression)
            {
                return methodExpression.Method.Name;
            }

            return ((MemberExpression)unaryExpression.Operand).Member.Name;
        }
        public static object GetValue<T>(this T instance, Expression<Func<T, object>> expression)
        {
            string propertyName = instance.GetMemberName(expression);
            return instance.GetPropertyValue(propertyName);
        }
    }
}
