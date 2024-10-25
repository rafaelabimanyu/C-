using System;

class Program
{
    static void Main(string[] args)
    {
        // Mengubah warna teks menjadi merah
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Ini adalah teks berwarna merah!");

        // Mengubah warna teks menjadi hijau
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Ini adalah teks berwarna hijau!");

        // Mengembalikan warna teks ke default
        Console.ResetColor();
        Console.WriteLine("Ini adalah teks dengan warna default.");
    }
}
