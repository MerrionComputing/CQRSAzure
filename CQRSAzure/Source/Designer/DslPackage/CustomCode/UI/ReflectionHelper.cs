using System;
using System.Collections.Generic;
using System.Reflection;

namespace CQRSAzure.CQRSdsl.Dsl
{
    /// <summary>
    /// Provides reflection helper methods.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Gets attribute of specified type from reflection.
        /// </summary>
        /// <typeparam name="T">Type of attribute to get.</typeparam>
        /// <param name="memberInfo">Type or member to get attribute from.</param>
        /// <returns>Retreived attribute or null if not found.</returns>
        public static T GetAttribute<T>(MemberInfo memberInfo) where T : Attribute
        {
            if (memberInfo == null) throw new ArgumentNullException("memberInfo");
            return Attribute.GetCustomAttribute(memberInfo, typeof(T), true) as T;
        }

        /// <summary>
        /// Gets attribute of specified type from reflection.
        /// </summary>
        /// <typeparam name="T">Type of attributes to get.</typeparam>
        /// <param name="memberInfo">Type or member to get attribute from.</param>
        /// <returns>Retreived attributes or null if not found.</returns>
        public static ICollection<T> GetAttributes<T>(MemberInfo memberInfo) where T : Attribute
        {
            if (memberInfo == null) throw new ArgumentNullException("memberInfo");
            return Attribute.GetCustomAttributes(memberInfo, typeof(T), true) as T[];
        }

        /// <summary>
        /// Invokes a method on a given object.
        /// </summary>
        /// <param name="obj">Object to invoke a method on.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="args">A list of argument type/argument value pairs.</param>
        /// <returns>Return result of the method.</returns>
        public static object Invoke(object obj, string methodName, params object[] args)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException("methodName");
            if (null == args) throw new ArgumentNullException("args");
            if (args.Length % 2 != 0) throw new ArgumentOutOfRangeException("args");

            Type[] argumentTypes = new Type[args.Length / 2];
            object[] arguments = new object[argumentTypes.Length];
            for (int i = 0; i < argumentTypes.Length; i++)
            {
                argumentTypes[i] = args[i * 2] as Type;
                if (argumentTypes[i] == null) throw new InvalidOperationException();
                arguments[i] = args[i * 2 + 1];
            }

            Type type = obj.GetType();
            return type.GetMethod(methodName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                argumentTypes,
                null).
                Invoke(obj, arguments);
        }
    }
}