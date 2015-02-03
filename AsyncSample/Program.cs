using System;
using System.Threading.Tasks;

namespace AsyncSample
{
    class Program
    {
        #region .: Constants :.
        private const int SHORT_LOOP = 50;
        private const int LONG_LOOP = 150;

        private const string LOOP1_NAME = ">> Loop1 >>";
        private const string LOOP2_NAME = "<< Loop2 <<";
        #endregion

        #region .: Main Program :.
        static void Main(string[] args)
        {
            // Test 1 : Conventional not async looping
            Console.Clear();
            NotAsync();
            PressAnyKey();

            // Test 2 : Totally async looping
            Console.Clear();
            TotallyAsync();
            PressAnyKey();

            // Test 3 : Synchronized async looping
            Console.Clear();
            SynchronizedAsync();
            PressAnyKey();
        }
        #endregion 

        #region .: Tests :.
        // We run two loops the result will be 
        // printed one loop after another
        private static void NotAsync()
        {
            Console.WriteLine("Conventional non async loop. Each loop runs on main " +
                "thread blocking anything from happening while it's looping...");
            PressAnyKey();

            // Each loop will run in its own thread
            var s1 = RunLoop(LOOP1_NAME, SHORT_LOOP);
            var s2 = RunLoop(LOOP2_NAME, LONG_LOOP);

            // ... then print out the function result
            Console.WriteLine("End of {0}", s1);
            Console.WriteLine("End of {0}", s2);
            Console.WriteLine();
            Console.WriteLine("Loop2 should start right after finishing Loop1 and both End of Loop should be at bottom. "
                                +"Scroll up to check it, and press any key for next test...");
        }

        // We run two loops the result will be
        // printed when each loop finishes independently
        private static void TotallyAsync()
        {
            Console.WriteLine("Totally async loop. Each loop runs on its own "+
                "thread and result is shown as soon as each thread ends...");
            PressAnyKey();

            // Each loop will run in its own thread
            var s1 = RunLoopAsync(LOOP1_NAME, SHORT_LOOP);
            var s2 = RunLoopAsync(LOOP2_NAME, LONG_LOOP);
            
            // ... then print out the function result
            Console.WriteLine("End of {0}", s1.Result);
            Console.WriteLine("End of {0}", s2.Result);
            Console.WriteLine();
            Console.WriteLine("Loop1 and Loop2 should be mixed, End of Loop1 should be somewhere in the "+
                                    "middle and End of Loop2 at bottom. Scroll up to check it and press "+
                                    "any key for next test...");
        }

        // We run two loops and wait both to finish before 
        // printing the result of the functions
        private static void SynchronizedAsync()
        {
            Console.WriteLine("Synchronized async loop. Each loop runs on its own thread, "+
                "but we wait until the end of both threads before showing the results...");
            PressAnyKey();

            // Each loop will run in its own thread
            var s1 = RunLoopAsync(LOOP1_NAME, SHORT_LOOP);
            var s2 = RunLoopAsync(LOOP2_NAME, LONG_LOOP);

            // Wait for both loops to finish 
            Task.WaitAll(s1, s2);

            // ... then print out the function result
            Console.WriteLine("End of {0}", s1.Result);
            Console.WriteLine("End of {0}", s2.Result);
            Console.WriteLine();
            Console.WriteLine("Loop1 and Loop2 should be mixed, but both End of Loop should "+
                                    "be at the bottom. Scroll up to check it and press any key for next test...");
        }

        private static async Task<string> RunLoopAsync(string loopName, int iterations)
        {
            // Run the loop in background
            return await Task.Run(() => RunLoop(loopName, iterations));
        }

        private static string RunLoop(string loopName, int iterations)
        {
            for (var i = 0; i < iterations; i++)
            {
                Console.WriteLine("{0} - {1}", loopName, i);
            }
            return loopName;
        }
        #endregion

        #region .: Utility :.
        private static void PressAnyKey()
        {
            Console.WriteLine();
            Console.WriteLine("---------------");
            Console.WriteLine(" Press any key ");
            Console.WriteLine("---------------");
            Console.ReadKey();
        }
        #endregion

    }
}
