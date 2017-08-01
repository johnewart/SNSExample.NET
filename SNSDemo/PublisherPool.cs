using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System.Threading;

namespace SNSDemo
{
    class PublisherPool
    {

        private ConcurrentQueue<String> messageQueue;
        private List<Publisher> workerPool;
        private List<Thread> threadPool;

        public PublisherPool(int number)
        {
            this.messageQueue = new ConcurrentQueue<string>();
            this.workerPool = new List<Publisher>(number);
            this.threadPool = new List<Thread>(number);
            for(int i = 0; i < number; i++)
            {
                Publisher p = new Publisher();
                workerPool.Add(p);

                Thread t = p.WorkOn(messageQueue);
                t.Start();
                threadPool.Add(t);
            }
        }

        public void Publish(String message)
        {
            this.messageQueue.Enqueue(message);
        }

        public void WaitUntilEmpty()
        {
            while(!messageQueue.IsEmpty)
            {
                // NOOP
                Thread.Sleep(200);
            }

        }

        public void StopAll()
        {
            Console.Write("Sending stop signal");
            foreach (Publisher p in workerPool)
            {
                Console.Write(".");
                p.Stop();
            }
            Console.WriteLine();

            Console.Write("Joining threads");
            foreach (Thread t in threadPool)
            {
                Console.Write(".");
                t.Join();
            }
            Console.WriteLine();
           
        }
    }
}
