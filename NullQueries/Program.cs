using System;
using System.Diagnostics;
using System.Reflection;

namespace NullQueries
{

    // https://twitter.com/jaredpar/status/1115019017297596416
    // why x!= null is bad
    // https://twitter.com/tomasaschan/status/1115120322544586752

    class Program
    {
        static readonly object s_nullObject = null;
        static readonly object s_object = new object();
        static void Main()
        {
            var runs = 100_000_000;
            var configuration = typeof(Program).Assembly?.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration ?? "";

            Startup();

            for (var index = 0; index < 2; index++)
            {
                Console.WriteLine($"NullQueries[{index}] runs={runs} Configuration={configuration}");
                Console.WriteLine();

                Test("NullObject is null", runs, () => s_nullObject is null);
                Test("NullObject is object", runs, () => s_nullObject is object);
                Test("Object is null", runs, () => s_object is null);
                Test("Object is object", runs, () => s_object is object);

                Test("!(NullObject is null)", runs, () => !(s_nullObject is null));
                Test("!(NullObject is object)", runs, () => !(s_nullObject is object));
                Test("!(Object is null)", runs, () => !(s_object is null));
                Test("!(Object is object)", runs, () => !(s_object is object));

                Console.WriteLine();
                Test("NullObject == null", runs, () => s_nullObject == null);
                Test("NullObject != null", runs, () => s_nullObject != null);
                Test("Object == null", runs, () => s_object == null);
                Test("Object != null", runs, () => s_object != null);
                Test("!(NullObject == null)", runs, () => !(s_nullObject == null));
                Test("!(NullObject != null)", runs, () => !(s_nullObject != null));
                Test("!(Object == null)", runs, () => !(s_object == null));
                Test("!(Object != null)", runs, () => !(s_object != null));

                Console.WriteLine();
            }
        }

        static void Test(string description, long runs, Func<bool> func)
        {
            var stopwatch = Stopwatch.StartNew();
            for (var run = 0; run < runs; run++)
                _ = func?.Invoke();
            stopwatch.Stop();
            Console.WriteLine($"{description,-30} time={stopwatch.ElapsedMilliseconds} ms");
        }

        static void Startup()
        {
            _ = s_nullObject is null;
            _ = s_nullObject is object;
            _ = s_object is null;
            _ = s_object is object;

            _ = s_nullObject == null;
            _ = s_nullObject != null;
            _ = s_object == null;
            _ = s_object != null;
        }
    }
}
