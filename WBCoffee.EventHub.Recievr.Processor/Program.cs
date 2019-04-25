using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Threading.Tasks;

namespace WBCoffee.EventHub.Recievr.Processor
{
    class Program
    {
        const string eventHubPath = "wiredbraincoffeews";
        const string consumerGroupName = "wired_brain_coffee_consol_processor";
        const string eventHubConnectionString = "Endpoint=sb://wiredbraincoffeews.servicebus.windows.net/;SharedAccessKeyName=SendAndListenPolicy;SharedAccessKey=yINayuPeeMU/MwjycyexB786euQpkUWB2NpYIWEj9B0=;EntityPath=wiredbraincoffeews";
        const string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=wiredbraincoffeestorage2;AccountKey=3Vx1lfoZEAt9YDIFQGmUjYqvwcysKL8bs1LQu+Ta2um7FSLDx7fizJ3p9IDLzchp7DQq4NSI8soXhWeRvSQspQ==;EndpointSuffix=core.windows.net";
        const string leaseConatinerName = "processcheckpoint";
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }
        private static async Task MainAsync()
        {
            Console.WriteLine($"Register the {nameof(WiredBrainCoffeeEventProcessor)}");
            var eventProcessorHost = new EventProcessorHost(eventHubPath, consumerGroupName, eventHubConnectionString, storageConnectionString, leaseConatinerName);
            await eventProcessorHost.RegisterEventProcessorAsync<WiredBrainCoffeeEventProcessor>();

            Console.WriteLine("Waiting for incomming event");
            Console.WriteLine("Press any to shutdown");
            Console.ReadLine();

            await eventProcessorHost.UnregisterEventProcessorAsync();

        }
    }
}
