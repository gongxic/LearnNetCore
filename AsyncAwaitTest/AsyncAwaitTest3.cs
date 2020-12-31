using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitTest
{
    public class AsyncAwaitTest3
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
            Console.WriteLine($"{url1} 的字符个数：{result1.Result}");
            Console.WriteLine($"{url2} 的字符个数：{result2.Result}");

            Console.WriteLine($"Test 方法完成：{Watch.ElapsedMilliseconds} ms");

        }

        private Task<int> CountCharacters(int id, string address)
        {
            CountCharactersStateMachine stateMachine = new CountCharactersStateMachine();
            stateMachine.builder= AsyncTaskMethodBuilder<int>.Create();
            stateMachine._this = this;
            stateMachine.id = id;
            stateMachine.address = address;
            stateMachine.state = -1;
            stateMachine.builder.Start(ref stateMachine);
            return stateMachine.builder.Task;

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


        public sealed class CountCharactersStateMachine : IAsyncStateMachine
        {
            public int state;
            public AsyncTaskMethodBuilder<int> builder;
            public int id;
            public string address;
            public AsyncAwaitTest3 _this;
            private WebClient client;
            private string result;
            private string s;
            private TaskAwaiter<string> u;

            void IAsyncStateMachine.MoveNext()
            {
                int num = state;
                int length;
                try
                {
                    if (num != 0)
                    {
                        Console.WriteLine(string.Format("Thread id is {0}", Thread.CurrentThread.ManagedThreadId));

                        client = new WebClient();
                    }
                    try
                    {
                        TaskAwaiter<string> awaiter;
                        if (num != 0)
                        {
                            Console.WriteLine(string.Format(" 开 始调用 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds));
                            Thread.Sleep(1000);
                            awaiter = client.DownloadStringTaskAsync(address).GetAwaiter();
                            if (!awaiter.IsCompleted)
                            {
                                num = (state = 0);

                                u = awaiter;

                                CountCharactersStateMachine stateMachine = this;

                                builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
                                return;
                            }
                        }
                        else
                        {
                            awaiter = u;

                            u = default(TaskAwaiter<string>);
                            num = (state = -1);
                        }

                        s = awaiter.GetResult();

                        result = s;

                        s = null;
                        Console.WriteLine(string.Format(" 调 用完成 id = {0}：{1} ms", id, Watch.ElapsedMilliseconds));
                        length = result.Length;
                    }
                    finally
                    {
                        if (num < 0 && client != null)
                        {
                            ((IDisposable)client).Dispose();
                        }
                    }
                }
                catch (Exception exception)
                {
                    state = -2;
                    builder.SetException(exception);
                    return;
                }
                state = -2;
                builder.SetResult(length);
            }

            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
            {
                throw new NotImplementedException();
            }
        }
    }
}
