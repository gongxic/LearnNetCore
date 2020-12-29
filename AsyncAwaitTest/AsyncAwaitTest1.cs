using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitTest
{
    public class AsyncAwaitTest1
    {
        private static readonly Stopwatch Watch = new Stopwatch();

        public void Test()
        {
            Watch.Start();

            const string url1 = "http://www.cnblogs.com/";
            const string url2 = "http://www.cnblogs.com/liqingwen/";

            //两次调用 CountCharacters 方法（下载某网站内容，并统计字符的个数）
            var result1 = CountCharacters(1, url1);
            var result2 = CountCharacters(2, url2);

            //三次调用 ExtraOperation 方法（主要是通过拼接字符串达到耗时操作）
            for (var i = 0; i < 5; i++)
            {
                System.Threading.Thread.Sleep(100);
                ExtraOperation(i + 1);
            }

            //控制台输出
            Console.WriteLine($"{url1} 的字符个数：{result1}");
            Console.WriteLine($"{url2} 的字符个数：{result2}");

            Console.WriteLine($"Test 方法完成：{Watch.ElapsedMilliseconds} ms");


        }

        private static int CountCharacters(int id, string address)
        {
            Console.WriteLine($"Thread id is {Thread.CurrentThread.ManagedThreadId}");
            using (var wc = new WebClient())
            {
                Console.WriteLine($" 开 始调用 id = {id}：{Watch.ElapsedMilliseconds} ms");
                System.Threading.Thread.Sleep(1000);
                var result = wc.DownloadString(address);
                Console.WriteLine($" 调 用完成 id = {id}：{Watch.ElapsedMilliseconds} ms");
                return result.Length;
            }
        }


        private static void ExtraOperation(int id)
        {
            Console.WriteLine($"Thread id is {Thread.CurrentThread.ManagedThreadId}");
            var s = "";

            for (var i = 0; i < 6000; i++)
            {
                s += i;
            }

            Console.WriteLine($"id = {id} 的 ExtraOperation 方法完成：{Watch.ElapsedMilliseconds} ms");
        }
    }
}
