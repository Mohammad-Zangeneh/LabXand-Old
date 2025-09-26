using System;
using System.Globalization;
using System.Diagnostics;
using LabXand.Extensions;
using System.Linq.Expressions;

namespace LabXand.Core
{
    public static class Error
    {

        [DebuggerStepThrough]
        public static Exception Application(string message, params object[] args)
        {
            return new ApplicationException(message.FormatInvariant(args));
        }

        [DebuggerStepThrough]
        public static Exception Application(Exception innerException, string message, params object[] args)
        {
            return new ApplicationException(message.FormatInvariant(args), innerException);
        }

        [DebuggerStepThrough]
        public static Exception ArgumentNullOrEmpty(Expression<Func<string>> arg)
        {
            var argName = ExpressionHelper.GetPropertyName(arg);
            return new ArgumentException("String parameter '{0}' cannot be null or all whitespace.".FormatInvariant(arg));
        }

        [DebuggerStepThrough]
        public static Exception ArgumentNull(string argName)
        {
            return new ArgumentNullException(argName);
        }

        [DebuggerStepThrough]
        public static Exception ArgumentNull<T>(Expression<Func<T>> arg)
        {
            var message = "Argument of type '{0}' cannot be null".FormatInvariant(typeof(T));
            var argName = ExpressionHelper.GetPropertyName(arg);
            return new ArgumentNullException(argName, message);
        }

        [DebuggerStepThrough]
        public static Exception ArgumentOutOfRange(string argName)
        {
            return new ArgumentOutOfRangeException(argName);
        }

        [DebuggerStepThrough]
        public static Exception ArgumentOutOfRange(string argName, string message, params object[] args)
        {
            return new ArgumentOutOfRangeException(argName, message.FormatInvariant(args));
        }

        [DebuggerStepThrough]
        public static Exception Argument(string argName, string message, params object[] args)
        {
            return new ArgumentException(message.FormatInvariant(args), argName);
        }

        [DebuggerStepThrough]
        public static Exception Argument<T>(Func<T> arg, string message, params object[] args)
        {
            var argName = arg.Method.Name;
            return new ArgumentException(message.FormatInvariant(args), argName);
        }

        [DebuggerStepThrough]
        public static Exception InvalidOperation(string message, params object[] args)
        {
            return Error.InvalidOperation(message, null, args);
        }

        [DebuggerStepThrough]
        public static Exception InvalidOperation(string message, Exception innerException, params object[] args)
        {
            return new InvalidOperationException(message.FormatCurrent(args), innerException);
        }

        [DebuggerStepThrough]
        public static Exception InvalidOperation<T>(string message, Func<T> member)
        {
            return InvalidOperation<T>(message, null, member);
        }

        [DebuggerStepThrough]
        public static Exception InvalidOperation<T>(string message, Exception innerException, Func<T> member)
        {
            Guard.ArgumentNotNull(message, "message");
            Guard.ArgumentNotNull(member, "member");

            return new InvalidOperationException(message.FormatCurrent(member.Method.Name), innerException);
        }

        [DebuggerStepThrough]
        public static Exception InvalidCast(Type fromType, Type toType)
        {
            return InvalidCast(fromType, toType, null);
        }

        [DebuggerStepThrough]
        public static Exception InvalidCast(Type fromType, Type toType, Exception innerException)
        {
            return new InvalidCastException("Cannot convert from type '{0}' to '{1}'.".FormatCurrent(fromType.FullName, toType.FullName), innerException);
        }

        [DebuggerStepThrough]
        public static Exception NotSupported()
        {
            return new NotSupportedException();
        }

        [DebuggerStepThrough]
        public static Exception NotImplemented()
        {
            return new NotImplementedException();
        }

        [DebuggerStepThrough]
        public static Exception ObjectDisposed(string objectName)
        {
            return new ObjectDisposedException(objectName);
        }

        [DebuggerStepThrough]
        public static Exception ObjectDisposed(string objectName, string message, params object[] args)
        {
            return new ObjectDisposedException(objectName, String.Format(CultureInfo.CurrentCulture, message, args));
        }

        [DebuggerStepThrough]
        public static Exception NoElements()
        {
            return new InvalidOperationException("Sequence contains no elements.");
        }

        [DebuggerStepThrough]
        public static Exception MoreThanOneElement()
        {
            return new InvalidOperationException("Sequence contains more than one element.");
        }

    }
}
