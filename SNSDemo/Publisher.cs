using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading;

namespace SNSDemo
{
    class Publisher
    {

        private AmazonSimpleNotificationServiceClient _client;
        private Boolean _running;
        private ConcurrentQueue<String> messageQueue; 
        public string TopicArn => "TOPIC_ARN";

        public Publisher()

        {
            _running = true; 
            _client = new AmazonSimpleNotificationServiceClient("KEY", "SECRET", Amazon.RegionEndpoint.USEast1);


        }


        public Thread WorkOn(ConcurrentQueue<String> queue)
        {
            this.messageQueue = queue; 
            return new Thread(new ThreadStart(Run));
        }

        private void Run() {
            String message;

            while (_running)
            {
                if (!messageQueue.IsEmpty)
                {
                    messageQueue.TryDequeue(out message);
                    PublishMessage(message);
                    Console.WriteLine("Publishing: {0}", message);
                }
                else
                {
                    Thread.Sleep(200);
                }
            }

            Console.WriteLine("Done!");
        }

        public void Stop()
        {
            _running = false;
        }



 

         void PublishMessage(string message)
 
        {
            Stopwatch sw = Stopwatch.StartNew(); 

            var request = new PublishRequest
            {
                Message = message,
                TopicArn = TopicArn,
                MessageAttributes = new Dictionary<string, MessageAttributeValue>()
                {
                    {
                        "SentAt",
                        new MessageAttributeValue
                        {
                            DataType = "String",
                            StringValue = DateTime.Now.Ticks.ToString()
                        }
                    }
                }
            };



            try
            {
                _client.PublishAsync(request).Wait();
            } catch (Exception e)
            {
                Console.WriteLine("Error! {0}", e);
            } finally
            {
                sw.Stop();
                Console.WriteLine("Publishing took {0}", sw.ElapsedMilliseconds);
            }

            
        }

    }
}

