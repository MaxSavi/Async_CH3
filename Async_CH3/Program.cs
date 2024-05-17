//using System;
//using System.Threading;
//using System.Threading.Tasks;

//class Program
//{
//    static SemaphoreSlim semaphore;

//    static void Main(string[] args)
//    {
//        int resourceCount = 3;
//        semaphore = new SemaphoreSlim(resourceCount);

//        for (int i = 0; i < 5; i++)
//        {
//            Task.Run(() => AccessResource());
//        }

//        Console.ReadLine();
//    }

//    static void AccessResource()
//    {
//        Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} ожидает доступа к ресурсу.");
//        semaphore.Wait();

//        Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} получил доступ к ресурсу.");
//        Thread.Sleep(2000);

//        Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} освободил ресурс.");
//        semaphore.Release();
//    }
//}

//using System;
//using System.Threading;
//using System.Threading.Tasks;

//class Program
//{
//    static SemaphoreSlim semaphore;

//    static async Task Main(string[] args)
//    {
//        int resourceCount = 3;
//        semaphore = new SemaphoreSlim(resourceCount);

//        Task[] tasks = new Task[5];
//        for (int i = 0; i < tasks.Length; i++)
//        {
//            tasks[i] = AccessResourceAsync();
//        }
//        await Task.WhenAll(tasks);

//        Console.ReadLine();
//    }

//    static async Task AccessResourceAsync()
//    {
//        Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} ожидает доступа к ресурсу.");
//        await semaphore.WaitAsync();

//        Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} получил доступ к ресурсу.");
//        await Task.Delay(2000);

//        Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} освободил ресурс.");
//        semaphore.Release();
//    }
//}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static Dictionary<int, SemaphoreSlim> semaphores;

    static async Task Main(string[] args)
    {
        semaphores = new Dictionary<int, SemaphoreSlim>();
        semaphores.Add(1, new SemaphoreSlim(2));
        semaphores.Add(2, new SemaphoreSlim(2));
        semaphores.Add(3, new SemaphoreSlim(1));

        Task[] tasks = new Task[6];
        for (int i = 0; i < 2; i++)
        {
            tasks[i] = AccessResourceAsync(1);
        }
        for (int i = 2; i < 4; i++)
        {
            tasks[i] = AccessResourceAsync(2);
        }
        for (int i = 4; i < 6; i++)
        {
            tasks[i] = AccessResourceAsync(3);
        }
        await Task.WhenAll(tasks);

        Console.ReadLine();
    }

    static async Task AccessResourceAsync(int priority)
    {
        Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} ожидает доступа к ресурсу с приоритетом {priority}.");
        await semaphores[priority].WaitAsync();

        Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} получил доступ к ресурсу с приоритетом {priority}.");
        await Task.Delay(2000);

        Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} освободил ресурс с приоритетом {priority}.");
        semaphores[priority].Release();
    }
}