using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace RubberStamp.Internals
{
    internal static class LambdaExtensions
    {
        /// <summary>
        /// Extracts the name of the property inside the lambdaexpression
        /// - UnaryExpression: Expression{Func{Warrior, object}} unaryObject = w => w.ID; --> ID
        /// - MemberExpression: Expression{Func{Warrior, int}} memberInt = w => w.ID; --> ID
        /// - BinaryExpression: Expression{Func{Warrior, bool}} binaryInt = w => w.ID == 1; --> ID (Takes the left side and casts to MemberExpression)
        /// - BinaryExpression: Expression{Func{Warrior, bool}} binaryInt = w => 1 == w.ID; --> ID (Takes the right side and casts to MemberExpression)
        /// - Compiled Expression: Expression{Func{int}} binaryInt = () => 5; --> 5
        /// - ToString: Expression{Func{Warrior, bool}} binaryInt = w => 1 == 1; --> w => True
        /// </summary>
        /// <param name="propertyExpression">The propertyexpression</param>
        /// <returns>The property name</returns>
        public static string TryExtractPropertyName(this LambdaExpression propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                // try get the member from the operand of the unaryexpression
                var unary = propertyExpression.Body as UnaryExpression;
                if (unary != null)
                {
                    memberExpression = unary.Operand as MemberExpression;
                }

                if (memberExpression == null)
                {
                    var binary = propertyExpression.Body as BinaryExpression;
                    if (binary != null)
                    {
                        memberExpression = binary.Left as MemberExpression;
                        if (memberExpression == null)
                        {
                            memberExpression = binary.Right as MemberExpression;
                        }
                    }
                }

                if (memberExpression == null)
                {
                    Trace.WriteLine($"## Validation - Property is not a MemberAccessExpression: {propertyExpression}");

                    try
                    {
                        return propertyExpression.Compile().DynamicInvoke().ToString();
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e.Message);
                        return propertyExpression.ToString();
                    }
                }
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                Trace.WriteLine($"## Validation - Property {memberExpression.Member} is not a PropertyInfo");
                return memberExpression.Member.ToString();
            }

            if (propertyInfo.GetGetMethod(true).IsStatic)
            {
                Trace.WriteLine($"## PersistenceMap - Property {memberExpression.Member.Name} is static");
                return memberExpression.Member.Name;
            }

            return memberExpression.Member.Name;
        }
    }
}
