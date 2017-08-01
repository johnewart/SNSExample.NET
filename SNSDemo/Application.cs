using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics; 

namespace SNSDemo
{
    class Application
    {
        private static int MESSAGES = 100; 

        public static void Main(String[] args)
        {
            PublisherPool p = new PublisherPool(20);
            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < MESSAGES; i++)
            {
                string message = String.Format("Message {0}", i);
                p.Publish(message);
                Console.WriteLine("Wrote out {0} messages", i);
            }

            Console.WriteLine("Waiting until empty ...");
            
            p.WaitUntilEmpty();
            sw.Stop();
            p.StopAll();

            Process proc = Process.GetCurrentProcess();
            Console.WriteLine("Took {0}ms to send all {1} messages", sw.ElapsedMilliseconds, MESSAGES);
            Console.WriteLine("Time per message: {0}msec", (float)sw.ElapsedMilliseconds / (float)MESSAGES);
            Console.WriteLine("Using {0} MB of memory", proc.PrivateMemorySize64 / (1024 * 1024));
            Console.WriteLine("Press any key...");
            Console.ReadLine(); 
        }
    }
}
