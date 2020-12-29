using System;
using System.Diagnostics;
using System.Net;

namespace AsyncAwaitTest
{
    class Program
    {

        private static void Main(string[] args)
        {

            var test1 = new AsyncAwaitTest1();
            test1.Test();

            Console.WriteLine("Test 2 Begion!");
            var test2 = new AsyncAwaitTest2();
            test2.Test();

            Console.Read();

        }

    }
}
