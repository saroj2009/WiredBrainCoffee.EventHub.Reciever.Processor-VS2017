using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;

namespace WiredBrainCoffee.EventHub.Rcver.AllPartition
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
            var runTimeInformation = await eventHubClient.GetRuntimeInformationAsync();
            var PartitionRecievers = runTimeInformation.PartitionIds.Select(partitionId => eventHubClient.CreateReceiver("$Default", partitionId, EventPosition.FromEnd())).ToList();

            Console.WriteLine("Waiting for incomming event...");
            int i = 0;
            foreach(var partitionrcvr in PartitionRecievers)
            {
                partitionrcvr.SetReceiveHandler(new WiredBrainCoffeePartitionRecieveHandler(partitionrcvr.PartitionId));
                if(i==0){ Console.WriteLine("Press any key 1st time for next partition"); i++; }
                else
                {
                    Console.WriteLine("Press any key to shoutdown...");
                }
                Console.ReadLine();
                //await eventHubClient.CloseAsync();
            }
            
        }
    }
}
