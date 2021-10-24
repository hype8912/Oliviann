namespace Oliviann
{
    #region Usings

    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Threading;
    using Oliviann.Linq;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used helper methods for making
    /// working with <see cref="Delegate"/>s easier.
    /// </summary>
    public static class DelegateHelpers
    {
        #region Fields

        /// <summary>
        /// The debug format string template.
        /// </summary>
        private const string DebugTemplate = @"Executing action '{0}' on {1} : {2}";

        #endregion Fields

        #region Action

        /// <summary>
        /// Executes the specified action for the specified number of
        /// milliseconds and suspends the current thread for a specified time.
        /// </summary>
        /// <param name="executeDelegate">The delegate action to be executed on
        /// each iteration.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to
        /// continue the operation before the method exits.</param>
        /// <param name="millisecondsIteration">The number of milliseconds for
        /// which the thread is blocked. Specify zero (0) to indicate that this
        /// thread should be suspended to allow other waiting threads to
        /// execute. The default value is 250.
        /// </param>
        public static void WaitTimeout(Action executeDelegate, uint millisecondsTimeout, int millisecondsIteration = 250)
        {
            ADP.CheckArgumentNull(executeDelegate, nameof(executeDelegate));

            int elapsedMilliseconds = 0;
            while (elapsedMilliseconds < millisecondsTimeout)
            {
                executeDelegate();
                Thread.Sleep(millisecondsIteration);
                elapsedMilliseconds += millisecondsIteration;
            }
        }

        #endregion Action

        #region Func

        /// <summary>Executes the specified action and returns back the time
        /// it took for the specified action to execute.</summary>
        /// <param name="actionDelegate">The delegate action to be executed.
        /// </param>
        /// <returns>
        /// The time it took for the specified delegate to execute.
        /// </returns>
        public static TimeSpan ElapsedAction(Action actionDelegate)
        {
            Stopwatch sw = Stopwatch.StartNew();
            actionDelegate?.Invoke();

            sw.Stop();
            return sw.Elapsed;
        }

        /// <summary>
        /// Executes the specified action and returns back the result of that
        /// action. After the delegate completes the location, method, action,
        /// and milliseconds elapsed to execute the delegate are traced.
        /// </summary>
        /// <param name="actionDelegate">The delegate action to be executed.
        /// </param>
        /// <param name="location">The location this method is being called
        /// from. Typically the class name of the method.</param>
        /// <param name="method">The method name that is calling this method.
        /// </param>
        /// <param name="action">The action being perform by the function.
        /// Typically, READ, CREATE, UPDATE, or DELETE.</param>
        public static void ElapsedActionTrace(Action actionDelegate, string location, string method, string action)
        {
            Debug.WriteLine(DebugTemplate.FormatWith(action, location, method));
            TimeSpan time = ElapsedAction(actionDelegate);
            Debug.WriteLine(FormatTraceTemplate(location, method, action, time.TotalMilliseconds));
        }

        /// <summary>Executes the specified function and returns back the time
        /// it took for the specified function to execute.</summary>
        /// <typeparam name="TOut">The type of the output.</typeparam>
        /// <param name="actionDelegate">The delegate function to be executed.
        /// </param>
        /// <param name="result">The result from executing the specified
        /// delegate.</param>
        /// <returns>
        /// The time it took for the specified delegate to execute.
        /// </returns>
        public static TimeSpan ElapsedFunc<TOut>(Func<TOut> actionDelegate, out TOut result)
        {
            Stopwatch sw = Stopwatch.StartNew();
            result = actionDelegate == null ? default(TOut) : actionDelegate();

            sw.Stop();
            return sw.Elapsed;
        }

        /// <summary>
        /// Executes the specified function and returns back the result of that
        /// function. After the delegate completes the location, method, action,
        /// and milliseconds elapsed to execute the delegate are traced.
        /// </summary>
        /// <typeparam name="TOut">The type of the output.</typeparam>
        /// <param name="actionDelegate">The delegate function to be executed.
        /// </param>
        /// <param name="location">The location this method is being called
        /// from. Typically the class name of the method.</param>
        /// <param name="method">The method name that is calling this method.
        /// </param>
        /// <param name="action">The action being perform by the function.
        /// Typically, READ, CREATE, UPDATE, or DELETE.</param>
        /// <returns>The result from executing the specified delegate.</returns>
        public static TOut ElapsedFuncTrace<TOut>(Func<TOut> actionDelegate, string location, string method, string action)
        {
            Debug.WriteLine(DebugTemplate.FormatWith(action, location, method));
            TimeSpan time = ElapsedFunc(actionDelegate, out TOut result);

            Debug.WriteLine(FormatTraceTemplate(location, method, action, time.TotalMilliseconds));
            return result;
        }

        /// <summary>
        /// Executes the specified function and returns back the result of that
        /// function. After the delegate completes the location, method, action,
        /// input count, and milliseconds elapsed to execute the delegate are
        /// traced.
        /// </summary>
        /// <typeparam name="TOut">The type of the output.</typeparam>
        /// <param name="actionDelegate">The delegate function to be executed.
        /// </param>
        /// <param name="location">The location this method is being called
        /// from. Typically the class name of the method.</param>
        /// <param name="method">The method name that is calling this method.
        /// </param>
        /// <param name="action">The action being perform by the function.
        /// Typically, READ, CREATE, UPDATE, or DELETE.</param>
        /// <param name="count">The number of items to be traced in the message.</param>
        /// <returns>The result from executing the specified delegate.</returns>
        public static TOut ElapsedFuncTraceCount<TOut>(
                                                       Func<TOut> actionDelegate,
                                                       string location,
                                                       string method,
                                                       string action,
                                                       int count)
        {
            Debug.WriteLine(DebugTemplate.FormatWith(action, location, method));
            TimeSpan time = ElapsedFunc(actionDelegate, out TOut result);

            Debug.WriteLine(FormatTraceCountTemplate(location, method, action, count, time.TotalMilliseconds));
            return result;
        }

        /// <summary>
        /// Executes the specified function and returns back the result of that
        /// function. After the delegate completes the location, method, action,
        /// output count, and milliseconds elapsed to execute the delegate are traced.
        /// </summary>
        /// <typeparam name="TOut">The type of the output. Must implement
        /// <see cref="IEnumerable"/>.</typeparam>
        /// <param name="actionDelegate">The delegate function to be executed.
        /// </param>
        /// <param name="location">The location this method is being called
        /// from. Typically the class name of the method.</param>
        /// <param name="method">The method name that is calling this method.
        /// </param>
        /// <param name="action">The action being perform by the function.
        /// Typically, READ, CREATE, UPDATE, or DELETE.</param>
        /// <returns>The result from executing the specified delegate.</returns>
        public static TOut ElapsedFuncTraceCount<TOut>(Func<TOut> actionDelegate, string location, string method, string action)
                      where TOut : IEnumerable
        {
            Debug.WriteLine(DebugTemplate.FormatWith(action, location, method));
            TimeSpan time = ElapsedFunc(actionDelegate, out TOut result);

            Debug.WriteLine(FormatTraceCountTemplate(location, method, action, result.Count(), time.TotalMilliseconds));
            return result;
        }

        /// <summary>Executes the specified function and returns back the time
        /// it took for the specified function to execute.</summary>
        /// <typeparam name="TIn">The type of input.</typeparam>
        /// <typeparam name="TOut">The type of the output.</typeparam>
        /// <param name="actionDelegate">The delegate function to be executed.
        /// </param>
        /// <param name="input">The input parameter to the specified function.
        /// </param>
        /// <param name="result">The result from executing the specified
        /// delegate.</param>
        /// <returns>
        /// The time it took for the specified delegate to execute.
        /// </returns>
        public static TimeSpan ElapsedFunc<TIn, TOut>(Func<TIn, TOut> actionDelegate, TIn input, out TOut result)
        {
            return ElapsedFunc(() => actionDelegate(input), out result);
        }

        #endregion Func

        #region String Formatters

        /// <summary>
        /// Formats the trace template string.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="method">The method.</param>
        /// <param name="action">The action.</param>
        /// <param name="milliseconds">The milliseconds.</param>
        /// <returns>A formatted trace template string.</returns>
        /// <remarks>This method was created because string.join is faster than
        /// string.format.</remarks>
        private static string FormatTraceTemplate(string location, string method, string action, double milliseconds)
        {
            return string.Concat(location, " -> ", method, " -> ", action, ": ", milliseconds, " ms");
        }

        /// <summary>
        /// Formats the trace count template string.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="method">The method.</param>
        /// <param name="action">The action.</param>
        /// <param name="count">The number of objects.</param>
        /// <param name="milliseconds">The milliseconds.</param>
        /// <returns>A formatted trace count template string.</returns>
        /// <remarks>This method was created because string.join is faster than
        /// string.format.</remarks>
        private static string FormatTraceCountTemplate(string location, string method, string action, int count, double milliseconds)
        {
            return string.Concat(location, " -> ", method, " -> ", action, ": ", count, " items in ", milliseconds, " ms");
        }

        #endregion String Formatters
    }
}