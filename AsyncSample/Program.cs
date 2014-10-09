using System;
using System.Threading.Tasks;

namespace AsyncSample
{
    class Program
    {
        static void Main(string[] args)
        {
            TotallyAsync();
            Console.Write("Press any key ...");
            Console.ReadKey();
            Console.Clear();
            SynchronizedAsync();
            Console.Write("Press any key ...");
            Console.ReadKey();
        }

        // We run two loops the result will be
        // printed when each loop finishes independently
        private static void TotallyAsync()
        {
            // Each loop will run in its own thread
            var s = Run("Loop1");
            var s2 = Run("Loop2");

            // ... then print out the function result
            Console.WriteLine("End of " + s.Result);
            Console.WriteLine("End of " + s2.Result);

        }

        // We run two loops and wait both to finish before 
        // printing the result of the functions
        private static void SynchronizedAsync()
        {
            // Each loop will run in its own thread
            var s = Run("Loop1");
            var s2 = Run("Loop2");

            // Wait for both loops to finish 
            Task.WaitAll(s, s2);

            // ... then print out the function result
            Console.WriteLine("End of " + s.Result);
            Console.WriteLine("End of " + s2.Result);
        }

        private static async Task<string> Run(string pepe)
        {
            // Run the loop in background
            return await Task.Run(() =>
            {
                for (var i = 0; i < 1500; i++)
                {
                    Console.WriteLine("{0} - {1}", pepe, i);
                }
                return pepe;
            });
        }
    }
}
