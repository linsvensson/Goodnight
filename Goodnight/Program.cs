using System;
using System.Diagnostics;
using System.Threading;

namespace Goodnight
{
    class Program
    {
        private static int timeUntilShutdown;
        private static Timer timer;
        private static bool bDone;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetWindowSize(60, 20);

            PrintText("Hello,\n");
            PrintText("in how many hours do you want me to shut everything down?\n");

            while (timeUntilShutdown == 0)
                AskTime();

            while (!bDone)
            {
                while (Console.KeyAvailable) Console.ReadKey(true);
                ConsoleKeyInfo key = Console.ReadKey(true);
            }
        }

        private static void PrintText(string text)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(50);
            }
        }

        private static void TimerCallback(object o)
        {
            bDone = true;
            timer.Dispose();
            GC.Collect();
            Shutdown();
        }

        private static bool AskTime()
        {
            string time = Console.ReadLine();
            double shutdownTime = 0;

            try
            {
                shutdownTime = double.Parse(time);
                PrintText("Timer started " + DateTime.Now + "\n");
                PrintText("PC will shutdown in " + shutdownTime + " hour(s), sleep tight...\n");
                timeUntilShutdown = (int)TimeSpan.FromHours(shutdownTime).TotalMilliseconds;

                timer = new Timer(TimerCallback, null, timeUntilShutdown, timeUntilShutdown);
                return true;
            }

            catch { }

            PrintText("That is not a valid time, please try again.\n");
            return false;
        }

        private static void Shutdown()
        {
            PrintText("Shutting down...\n");

            var psi = new ProcessStartInfo("shutdown", "/s /t 0");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);
        }
    }
}
