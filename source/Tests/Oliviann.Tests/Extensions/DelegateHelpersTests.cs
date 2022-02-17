namespace Oliviann.Tests.Extensions
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using Oliviann.Diagnostics;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class DelegateHelpersTests
    {
        #region WaitTimeout Tests

        [Fact]
        public void WaitTimeoutTest_NullAction()
        {
            Assert.Throws<ArgumentNullException>(() => DelegateHelpers.WaitTimeout(null, 0, 0));
        }

        [Fact]
        public void WaitTimeoutTest_ZeroTimeout()
        {
            bool actionExecuted = false;
            Action act = () =>
                {
                    actionExecuted = true;
                    Thread.SpinWait(20);
                };

            DelegateHelpers.WaitTimeout(act, 0);
            Assert.False(actionExecuted, "Action was executed even though it shouldn't have been.");
        }

        [Fact]
        public void WaitTimeoutTest_Execute()
        {
            bool actionExecuted = false;
            Action act = () =>
                {
                    actionExecuted = true;
                    Thread.SpinWait(20);
                };

            DelegateHelpers.WaitTimeout(act, 1000);
            Assert.True(actionExecuted, "Action was executed even though it shouldn't have been.");
        }

        #endregion WaitTimeout Tests

        #region ElapsedActionTrace Tests

        [Fact]
        public void ElapsedActionTraceTest_Nulls()
        {
            Action function = null;

            DelegateHelpers.ElapsedActionTrace(function, null, null, null);
        }

        [Fact]
        public void ElapsedActionTraceTest_ExecuteNullLocation()
        {
            const string Loc = null;
            const string Meth = "30-Execute";
            const string Act = "Test";
            string event1String = string.Format("Executing action '{0}' on {1} : {2}", Act, Loc, Meth);
            string event2String = string.Concat(Loc, " -> ", Meth, " -> ", Act, ": ");

            bool event1Executed = false;
            bool event2Executed = false;

            var listener = new DelegateTraceListener(
                s =>
                {
                    if (s.StartsWith(event1String))
                    {
                        event1Executed = true;
                        return;
                    }

                    if (s.StartsWith(event2String))
                    {
                        event2Executed = true;
                    }
                },
                    "ElapsedActionTraceTest_ExecuteNullLocation");

            Trace.Listeners.Add(listener);

            bool funcExecuted = false;
            Action function = () => { funcExecuted = true; };

            DelegateHelpers.ElapsedActionTrace(function, Loc, Meth, Act);
            Assert.True(funcExecuted, "Function was not executed.");

#if DEBUG
            Assert.True(event1Executed, "Trace event 1 was not executed.");
            Assert.True(event2Executed, "Trace event 2 was not executed.");
#endif

            Trace.Listeners.Remove(listener);
        }

        [Fact]
        public void ElapsedActionTraceTest_ExecuteNullMethod()
        {
            const string Loc = "DelegateHelpersTests";
            const string Meth = null;
            const string Act = "Test";
            string event1String = string.Format("Executing action '{0}' on {1} : {2}", Act, Loc, Meth);
            string event2String = string.Concat(Loc, " -> ", Meth, " -> ", Act, ": ");

            bool event1Executed = false;
            bool event2Executed = false;

            var listener = new DelegateTraceListener(
                s =>
                {
                    if (s.StartsWith(event1String))
                    {
                        event1Executed = true;
                        return;
                    }

                    if (s.StartsWith(event2String))
                    {
                        event2Executed = true;
                    }
                },
                    "ElapsedActionTraceTest_ExecuteNullMethod");

            Trace.Listeners.Add(listener);

            bool funcExecuted = false;
            Action function = () => { funcExecuted = true; };

            DelegateHelpers.ElapsedActionTrace(function, Loc, Meth, Act);
            Assert.True(funcExecuted, "Function was not executed.");

#if DEBUG
            Assert.True(event1Executed, "Trace event 1 was not executed.");
            Assert.True(event2Executed, "Trace event 2 was not executed.");
#endif

            Trace.Listeners.Remove(listener);
        }

        [Fact]
        public void ElapsedActionTraceTest_ExecuteNullAction()
        {
            const string Loc = "DelegateHelpersTests";
            const string Meth = "31-Execute";
            const string Act = null;
            string event1String = string.Format("Executing action '{0}' on {1} : {2}", Act, Loc, Meth);
            string event2String = string.Concat(Loc, " -> ", Meth, " -> ", Act, ": ");

            bool event1Executed = false;
            bool event2Executed = false;

            var listener = new DelegateTraceListener(
                s =>
                {
                    if (s.StartsWith(event1String))
                    {
                        event1Executed = true;
                        return;
                    }

                    if (s.StartsWith(event2String))
                    {
                        event2Executed = true;
                    }
                },
                    "ElapsedActionTraceTest_ExecuteNullAction");

            Trace.Listeners.Add(listener);

            bool funcExecuted = false;
            Action function = () => { funcExecuted = true; };

            DelegateHelpers.ElapsedActionTrace(function, Loc, Meth, Act);
            Assert.True(funcExecuted, "Function was not executed.");

#if DEBUG
            Assert.True(event1Executed, "Trace event 1 was not executed.");
            Assert.True(event2Executed, "Trace event 2 was not executed.");
#endif

            Trace.Listeners.Remove(listener);
        }

        [Fact]
        public void ElapsedActionTraceTest_Execute()
        {
            const string Loc = "DelegateHelpersTests";
            const string Meth = "32-Execute";
            const string Act = "Test";
            string event1String = string.Format("Executing action '{0}' on {1} : {2}", Act, Loc, Meth);
            string event2String = string.Concat(Loc, " -> ", Meth, " -> ", Act, ": ");

            bool event1Executed = false;
            bool event2Executed = false;

            var listener = new DelegateTraceListener(
                s =>
                {
                    if (s.StartsWith(event1String))
                    {
                        event1Executed = true;
                        return;
                    }

                    if (s.StartsWith(event2String))
                    {
                        event2Executed = true;
                    }
                },
                    "ElapsedActionTraceTest_Execute");

            Trace.Listeners.Add(listener);

            bool funcExecuted = false;
            Action function = () => { funcExecuted = true; };

            DelegateHelpers.ElapsedActionTrace(function, Loc, Meth, Act);
            Assert.True(funcExecuted, "Function was not executed.");

#if DEBUG
            Assert.True(event1Executed, "Trace event 1 was not executed.");
            Assert.True(event2Executed, "Trace event 2 was not executed.");
#endif

            Trace.Listeners.Remove(listener);
        }

        #endregion ElapsedActionTrace Tests

        #region ElapsedFuncTraceT Tests

        [Fact]
        public void ElapsedFuncTraceTTest_Nulls()
        {
            Func<string> function = null;

            var result = DelegateHelpers.ElapsedFuncTrace(function, null, null, null);
            Assert.Null(result);
        }

        [Fact]
        public void ElapsedFuncTraceTTest_ExecuteNullLocation()
        {
            const string Loc = null;
            const string Meth = "20-Execute";
            const string Act = "Test";
            string event1String = string.Format("Executing action '{0}' on {1} : {2}", Act, Loc, Meth);
            string event2String = string.Concat(Loc, " -> ", Meth, " -> ", Act, ": ");

            bool event1Executed = false;
            bool event2Executed = false;

            var listener = new DelegateTraceListener(
                s =>
                {
                    if (s.StartsWith(event1String))
                    {
                        event1Executed = true;
                        return;
                    }

                    if (s.StartsWith(event2String))
                    {
                        event2Executed = true;
                    }
                },
                    "ElapsedFuncTraceTTest_ExecuteNullLocation");

            Trace.Listeners.Add(listener);

            bool funcExecuted = false;
            Func<string> function = () =>
            {
                funcExecuted = true;
                return "Executed";
            };

            string result = DelegateHelpers.ElapsedFuncTrace(function, Loc, Meth, Act);
            Assert.True(funcExecuted, "Function was not executed.");
            Assert.True(result == "Executed", "Function did not return the correct value.");

#if DEBUG
            Assert.True(event1Executed, "Trace event 1 was not executed.");
            Assert.True(event2Executed, "Trace event 2 was not executed.");
#endif

            Trace.Listeners.Remove(listener);
        }

        [Fact]
        public void ElapsedFuncTraceTTest_ExecuteNullMethod()
        {
            const string Loc = "DelegateHelpersTests";
            const string Meth = null;
            const string Act = "Test";
            string event1String = string.Format("Executing action '{0}' on {1} : {2}", Act, Loc, Meth);
            string event2String = string.Concat(Loc, " -> ", Meth, " -> ", Act, ": ");

            bool event1Executed = false;
            bool event2Executed = false;

            var listener = new DelegateTraceListener(
                s =>
                {
                    if (s.StartsWith(event1String))
                    {
                        event1Executed = true;
                        return;
                    }

                    if (s.StartsWith(event2String))
                    {
                        event2Executed = true;
                    }
                },
                    "ElapsedFuncTraceTTest_ExecuteNullMethod");

            Trace.Listeners.Add(listener);

            bool funcExecuted = false;
            Func<string> function = () =>
            {
                funcExecuted = true;
                return "Executed";
            };

            string result = DelegateHelpers.ElapsedFuncTrace(function, Loc, Meth, Act);
            Assert.True(funcExecuted, "Function was not executed.");
            Assert.Equal("Executed", result);

#if DEBUG
            Assert.True(event1Executed, "Trace event 1 was not executed.");
            Assert.True(event2Executed, "Trace event 2 was not executed.");
#endif

            Trace.Listeners.Remove(listener);
        }

        [Fact]
        public void ElapsedFuncTraceTTest_ExecuteNullAction()
        {
            const string Loc = "DelegateHelpersTests";
            const string Meth = "21-Execute";
            const string Act = null;
            string event1String = string.Format("Executing action '{0}' on {1} : {2}", Act, Loc, Meth);
            string event2String = string.Concat(Loc, " -> ", Meth, " -> ", Act, ": ");

            bool event1Executed = false;
            bool event2Executed = false;

            var listener = new DelegateTraceListener(
                s =>
                {
                    if (s.StartsWith(event1String))
                    {
                        event1Executed = true;
                        return;
                    }

                    if (s.StartsWith(event2String))
                    {
                        event2Executed = true;
                    }
                },
                    "ElapsedFuncTraceTTest_ExecuteAction");

            Trace.Listeners.Add(listener);

            bool funcExecuted = false;
            Func<string> function = () =>
            {
                funcExecuted = true;
                return "Executed";
            };

            string result = DelegateHelpers.ElapsedFuncTrace(function, Loc, Meth, Act);
            Assert.True(funcExecuted, "Function was not executed.");
            Assert.Equal("Executed", result);

#if DEBUG
            Assert.True(event1Executed, "Trace event 1 was not executed.");
            Assert.True(event2Executed, "Trace event 2 was not executed.");
#endif

            Trace.Listeners.Remove(listener);
        }

        [Fact]
        public void ElapsedFuncTraceTTest_Execute()
        {
            const string Loc = "DelegateHelpersTests";
            const string Meth = "22-Execute";
            const string Act = "Test";
            string event1String = string.Format("Executing action '{0}' on {1} : {2}", Act, Loc, Meth);
            string event2String = string.Concat(Loc, " -> ", Meth, " -> ", Act, ": ");

            bool event1Executed = false;
            bool event2Executed = false;

            var listener = new DelegateTraceListener(
                s =>
                    {
                        if (s.StartsWith(event1String))
                        {
                            event1Executed = true;
                            return;
                        }

                        if (s.StartsWith(event2String))
                        {
                            event2Executed = true;
                        }
                    },
                    "ElapsedFuncTraceTTest_Execute");

            Trace.Listeners.Add(listener);

            bool funcExecuted = false;
            Func<string> function = () =>
                {
                    funcExecuted = true;
                    return "Executed";
                };

            string result = DelegateHelpers.ElapsedFuncTrace(function, Loc, Meth, Act);
            Assert.True(funcExecuted, "Function was not executed.");
            Assert.Equal("Executed", result);

#if DEBUG
            Assert.True(event1Executed, "Trace event 1 was not executed.");
            Assert.True(event2Executed, "Trace event 2 was not executed.");
#endif

            Trace.Listeners.Remove(listener);
        }

        #endregion ElapsedFuncTraceT Tests

        #region ElapsedFuncTraceCountT Tests

        [Fact]
        public void ElapsedFuncTraceCountTTest_Nulls()
        {
            Func<string> function = null;

            var result = DelegateHelpers.ElapsedFuncTraceCount(function, null, null, null, 0);
            Assert.Null(result);
        }

        [Fact]
        public void ElapsedFuncTraceCountTTest_ExecuteNullLocation()
        {
            const string Loc = null;
            const string Meth = "20-Execute";
            const string Act = "Test";
            const int Cnt = 27;
            string event1String = string.Format("Executing action '{0}' on {1} : {2}", Act, Loc, Meth);
            string event2String = string.Concat(Loc, " -> ", Meth, " -> ", Act, ": ", Cnt, " items in");

            bool event1Executed = false;
            bool event2Executed = false;

            var listener = new DelegateTraceListener(
                s =>
                    {
                        if (s.StartsWith(event1String))
                        {
                            event1Executed = true;
                            return;
                        }

                        if (s.StartsWith(event2String))
                        {
                            event2Executed = true;
                        }
                    },
                "ElapsedFuncTraceCountTTest_ExecuteNullLocation");

            Trace.Listeners.Add(listener);

            bool funcExecuted = false;
            Func<string> function = () =>
                {
                    funcExecuted = true;
                    return "Executed";
                };

            string result = DelegateHelpers.ElapsedFuncTraceCount(function, Loc, Meth, Act, Cnt);
            Assert.True(funcExecuted, "Function was not executed.");
            Assert.Equal("Executed", result);

#if DEBUG
            Assert.True(event1Executed, "Trace event 1 was not executed.");
            Assert.True(event2Executed, "Trace event 2 was not executed.");
#endif

            Trace.Listeners.Remove(listener);
        }

        [Fact]
        public void ElapsedFuncTraceCountTTest_ExecuteNullMethod()
        {
            const string Loc = "DelegateHelpersTests";
            const string Meth = null;
            const string Act = "Test";
            const int Cnt = 27;
            string event1String = string.Format("Executing action '{0}' on {1} : {2}", Act, Loc, Meth);
            string event2String = string.Concat(Loc, " -> ", Meth, " -> ", Act, ": ", Cnt, " items in");

            bool event1Executed = false;
            bool event2Executed = false;

            var listener = new DelegateTraceListener(
                s =>
                    {
                        if (s.StartsWith(event1String))
                        {
                            event1Executed = true;
                            return;
                        }

                        if (s.StartsWith(event2String))
                        {
                            event2Executed = true;
                        }
                    },
                "ElapsedFuncTraceTTest_ExecuteNullMethod");

            Trace.Listeners.Add(listener);

            bool funcExecuted = false;
            Func<string> function = () =>
                {
                    funcExecuted = true;
                    return "Executed";
                };

            string result = DelegateHelpers.ElapsedFuncTraceCount(function, Loc, Meth, Act, Cnt);
            Assert.True(funcExecuted, "Function was not executed.");
            Assert.Equal("Executed", result);

#if DEBUG
            Assert.True(event1Executed, "Trace event 1 was not executed.");
            Assert.True(event2Executed, "Trace event 2 was not executed.");
#endif

            Trace.Listeners.Remove(listener);
        }

        [Fact]
        public void ElapsedFuncTraceCountTTest_ExecuteNullAction()
        {
            const string Loc = "DelegateHelpersTests";
            const string Meth = "21-Execute";
            const string Act = null;
            const int Cnt = 27;
            string event1String = string.Format("Executing action '{0}' on {1} : {2}", Act, Loc, Meth);
            string event2String = string.Concat(Loc, " -> ", Meth, " -> ", Act, ": ", Cnt, " items in");

            bool event1Executed = false;
            bool event2Executed = false;

            var listener = new DelegateTraceListener(
                s =>
                    {
                        if (s.StartsWith(event1String))
                        {
                            event1Executed = true;
                            return;
                        }

                        if (s.StartsWith(event2String))
                        {
                            event2Executed = true;
                        }
                    },
                "ElapsedFuncTraceCountTTest_ExecuteNullAction");

            Trace.Listeners.Add(listener);

            bool funcExecuted = false;
            Func<string> function = () =>
                {
                    funcExecuted = true;
                    return "Executed";
                };

            string result = DelegateHelpers.ElapsedFuncTraceCount(function, Loc, Meth, Act, Cnt);
            Assert.True(funcExecuted, "Function was not executed.");
            Assert.Equal("Executed", result);

#if DEBUG
            Assert.True(event1Executed, "Trace event 1 was not executed.");
            Assert.True(event2Executed, "Trace event 2 was not executed.");
#endif

            Trace.Listeners.Remove(listener);
        }

        [Fact]
        public void ElapsedFuncTraceCountTTest_Execute()
        {
            const string Loc = "DelegateHelpersTests";
            const string Meth = "22-Execute";
            const string Act = "Test";
            const int Cnt = 27;
            string event1String = string.Format("Executing action '{0}' on {1} : {2}", Act, Loc, Meth);
            string event2String = string.Concat(Loc, " -> ", Meth, " -> ", Act, ": ", Cnt, " items in");

            bool event1Executed = false;
            bool event2Executed = false;

            var listener = new DelegateTraceListener(
                s =>
                    {
                        if (s.StartsWith(event1String))
                        {
                            event1Executed = true;
                            return;
                        }

                        if (s.StartsWith(event2String))
                        {
                            event2Executed = true;
                        }
                    },
                "ElapsedFuncTraceCountTTest_Execute");

            Trace.Listeners.Add(listener);

            bool funcExecuted = false;
            Func<string> function = () =>
                {
                    funcExecuted = true;
                    return "Executed";
                };

            string result = DelegateHelpers.ElapsedFuncTraceCount(function, Loc, Meth, Act, Cnt);
            Assert.True(funcExecuted, "Function was not executed.");
            Assert.Equal("Executed", result);

#if DEBUG
            Assert.True(event1Executed, "Trace event 1 was not executed.");
            Assert.True(event2Executed, "Trace event 2 was not executed.");
#endif

            Trace.Listeners.Remove(listener);
        }

        [Fact]
        public void ElapsedFuncTraceCountIEnumTest_Nulls()
        {
            Func<IEnumerable<string>> function = null;

            var result = DelegateHelpers.ElapsedFuncTraceCount(function, null, null, null, 0);
            Assert.Null(result);
        }

        [Fact]
        public void ElapsedFuncTraceCountIEnumTest_ExecuteNullLocation()
        {
            const string Loc = null;
            const string Meth = "23-Execute";
            const string Act = "Test";
            const int Cnt = 20;
            string event1String = string.Format("Executing action '{0}' on {1} : {2}", Act, Loc, Meth);
            string event2String = string.Concat(Loc, " -> ", Meth, " -> ", Act, ": ", Cnt, " items in");

            bool event1Executed = false;
            bool event2Executed = false;

            var listener = new DelegateTraceListener(
                s =>
                    {
                        if (s.StartsWith(event1String))
                        {
                            event1Executed = true;
                            return;
                        }

                        if (s.StartsWith(event2String))
                        {
                            event2Executed = true;
                        }
                    },
                "ElapsedFuncTraceCountIEnumTest_ExecuteNullLocation");

            Trace.Listeners.Add(listener);

            bool funcExecuted = false;
            Func<IEnumerable<string>> function = () =>
                {
                    funcExecuted = true;
                    return Enumerable.Repeat("Test Fill", Cnt);
                };

            IEnumerable<string> result = DelegateHelpers.ElapsedFuncTraceCount(function, Loc, Meth, Act);
            Assert.True(funcExecuted, "Function was not executed.");
            Assert.Equal(Cnt, result.Count());

#if DEBUG
            Assert.True(event1Executed, "Trace event 1 was not executed.");
            Assert.True(event2Executed, "Trace event 2 was not executed.");
#endif

            Trace.Listeners.Remove(listener);
        }

        [Fact]
        public void ElapsedFuncTraceCountIEnumTest_ExecuteNullMethod()
        {
            const string Loc = "DelegateHelpersTests";
            const string Meth = null;
            const string Act = "Test";
            const int Cnt = 20;
            string event1String = string.Format("Executing action '{0}' on {1} : {2}", Act, Loc, Meth);
            string event2String = string.Concat(Loc, " -> ", Meth, " -> ", Act, ": ", Cnt, " items in");

            bool event1Executed = false;
            bool event2Executed = false;

            var listener = new DelegateTraceListener(
                s =>
                    {
                        if (s.StartsWith(event1String))
                        {
                            event1Executed = true;
                            return;
                        }

                        if (s.StartsWith(event2String))
                        {
                            event2Executed = true;
                        }
                    },
                "ElapsedFuncTraceCountIEnumTest_ExecuteNullMethod");

            Trace.Listeners.Add(listener);

            bool funcExecuted = false;
            Func<IEnumerable<string>> function = () =>
                {
                    funcExecuted = true;
                    return Enumerable.Repeat("Test Fill", Cnt);
                };

            IEnumerable<string> result = DelegateHelpers.ElapsedFuncTraceCount(function, Loc, Meth, Act);
            Assert.True(funcExecuted, "Function was not executed.");
            Assert.Equal(Cnt, result.Count());

#if DEBUG
            Assert.True(event1Executed, "Trace event 1 was not executed.");
            Assert.True(event2Executed, "Trace event 2 was not executed.");
#endif

            Trace.Listeners.Remove(listener);
        }

        [Fact]
        public void ElapsedFuncTraceCountIEnumTest_ExecuteNullAction()
        {
            const string Loc = "DelegateHelpersTests";
            const string Meth = "24-Execute";
            const string Act = null;
            const int Cnt = 20;
            string event1String = string.Format("Executing action '{0}' on {1} : {2}", Act, Loc, Meth);
            string event2String = string.Concat(Loc, " -> ", Meth, " -> ", Act, ": ", Cnt, " items in");

            bool event1Executed = false;
            bool event2Executed = false;

            var listener = new DelegateTraceListener(
                s =>
                    {
                        if (s.StartsWith(event1String))
                        {
                            event1Executed = true;
                            return;
                        }

                        if (s.StartsWith(event2String))
                        {
                            event2Executed = true;
                        }
                    },
                "ElapsedFuncTraceCountIEnumTest_ExecuteNullAction");

            Trace.Listeners.Add(listener);

            bool funcExecuted = false;
            Func<IEnumerable<string>> function = () =>
                {
                    funcExecuted = true;
                    return Enumerable.Repeat("Test Fill", Cnt);
                };

            IEnumerable<string> result = DelegateHelpers.ElapsedFuncTraceCount(function, Loc, Meth, Act);
            Assert.True(funcExecuted, "Function was not executed.");
            Assert.Equal(Cnt, result.Count());

#if DEBUG
            Assert.True(event1Executed, "Trace event 1 was not executed.");
            Assert.True(event2Executed, "Trace event 2 was not executed.");
#endif

            Trace.Listeners.Remove(listener);
        }

        [Fact]
        public void ElapsedFuncTraceCountIEnumTest_Execute()
        {
            const string Loc = "DelegateHelpersTests";
            const string Meth = "25-Execute";
            const string Act = "Test";
            const int Cnt = 20;
            string event1String = string.Format("Executing action '{0}' on {1} : {2}", Act, Loc, Meth);
            string event2String = string.Concat(Loc, " -> ", Meth, " -> ", Act, ": ", Cnt, " items in");

            bool event1Executed = false;
            bool event2Executed = false;

            var listener = new DelegateTraceListener(
                s =>
                {
                    if (s.StartsWith(event1String))
                    {
                        event1Executed = true;
                        return;
                    }

                    if (s.StartsWith(event2String))
                    {
                        event2Executed = true;
                    }
                },
                "ElapsedFuncTraceCountTTest_Execute");

            Trace.Listeners.Add(listener);

            bool funcExecuted = false;
            Func<IEnumerable<string>> function = () =>
                {
                    funcExecuted = true;
                    return Enumerable.Repeat("Test Fill", Cnt);
                };

            IEnumerable<string> result = DelegateHelpers.ElapsedFuncTraceCount(function, Loc, Meth, Act);
            Assert.True(funcExecuted, "Function was not executed.");
            Assert.Equal(Cnt, result.Count());

#if DEBUG
            Assert.True(event1Executed, "Trace event 1 was not executed.");
            Assert.True(event2Executed, "Trace event 2 was not executed.");
#endif

            Trace.Listeners.Remove(listener);
        }

        #endregion ElapsedFuncTraceCountT Tests

        #region ElapsedFunc Tests

        [Fact]
        public void ElapsedFuncTest_Nulls()
        {
            Func<string, string> function = null;
            string output;

            Assert.Throws<NullReferenceException>(() => DelegateHelpers.ElapsedFunc(function, null, out output));
        }

        [Fact]
        public void ElapsedFuncTest_ExecuteNullInput()
        {
            const string Input = null;
            bool funcExecuted = false;
            Func<string, string> function = s =>
                {
                    funcExecuted = true;
                    return s + "-Executed";
                };

            string output;
            var result = DelegateHelpers.ElapsedFunc(function, Input, out output);
            Assert.True(funcExecuted, "Function was not executed.");
            Assert.Equal(Input + "-Executed", output);
        }

        [Fact]
        public void ElapsedFuncTest_Execute()
        {
            const string Input = "Input";
            bool funcExecuted = false;
            Func<string, string> function = s =>
                {
                    funcExecuted = true;
                    return s + "-Executed";
                };

            string output;
            var result = DelegateHelpers.ElapsedFunc(function, Input, out output);
            Assert.True(funcExecuted, "Function was not executed.");
            Assert.Equal(Input + "-Executed", output);
        }

        #endregion ElapsedFunc Tests
    }
}