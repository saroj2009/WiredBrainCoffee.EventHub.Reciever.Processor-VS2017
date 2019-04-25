using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;

namespace WiredBrainCoffee.EventHub.Reciever.Direct
{
    class Program
    {
        const string EventHubConnectionString = "Endpoint=sb://wiredbraincoffeews.servicebus.windows.net/;SharedAccessKeyName=SendAndListenPolicy;SharedAccessKey=yINayuPeeMU/MwjycyexB786euQpkUWB2NpYIWEj9B0=;EntityPath=wiredbraincoffeews";
        static async Task Main(string[] args)
        {
         await MainAsync();
        }

        private static async Task MainAsync()
        {
            Console.WriteLine("Connection to event hub....");
            var eventHubClient = EventHubClient.CreateFromConnectionString(EventHubConnectionString);
            var PartitionReciever = eventHubClient.CreateReceiver(PartitionReceiver.DefaultConsumerGroupName, "0", EventPosition.FromStart());
            
            Console.WriteLine("Waiting for incomming event...");
            while (true)
            {
                var EventData = await PartitionReciever.ReceiveAsync(10);
                if(EventData!=null)
                {
                    foreach(var evntdata in EventData)
                    {

                        var DataAsJson = Encoding.UTF8.GetString(evntdata.Body.Array);
                        Console.WriteLine($"{DataAsJson} | Partition ID: {PartitionReciever.PartitionId}");
                    }
                }
            }
        }
    }
}
