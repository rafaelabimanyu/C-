using System;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        // Menjalankan 3 tugas secara paralel
        Task task1 = Task.Run(() => Process1());
        Task task2 = Task.Run(() => Process2());
        Task task3 = Task.Run(() => Process3());

        Task.WaitAll(task1, task2, task3); // Tunggu semua tugas selesai
        Console.WriteLine("Semua proses selesai.");
    }

    static void Process1()
    {
        Console.WriteLine("Process 1 dimulai...");
        // Logika proses 1
        System.Threading.Thread.Sleep(2000); // Simulasi waktu proses
        Console.WriteLine("Process 1 selesai.");
    }

    static void Process2()
    {
        Console.WriteLine("Process 2 dimulai...");
        // Logika proses 2
        System.Threading.Thread.Sleep(3000);
        Console.WriteLine("Process 2 selesai.");
    }

    static void Process3()
    {
        Console.WriteLine("Process 3 dimulai...");
        // Logika proses 3
        System.Threading.Thread.Sleep(1000);
        Console.WriteLine("Process 3 selesai.");
    }
}
