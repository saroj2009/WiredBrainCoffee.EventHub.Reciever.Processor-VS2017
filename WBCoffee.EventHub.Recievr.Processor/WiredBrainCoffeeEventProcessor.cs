using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WiredBrainCoffee.EventHub.Model;

namespace WBCoffee.EventHub.Recievr.Processor
{
    public class WiredBrainCoffeeEventProcessor : IEventProcessor
    {
        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine($"Shuting down processor for partition{context.PartitionId}. Reason:{reason}");
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"Initialized processor for partition{context.PartitionId}");
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine($"Error for partition{context.PartitionId}:{error.Message}");
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> EventDatas)
        {
            if (EventDatas != null)
            {
                foreach (var eventdata in EventDatas)
                {
                    var dataASJson = Encoding.UTF8.GetString(eventdata.Body.Array);
                    var CoffeeMachineData = JsonConvert.DeserializeObject<CoffeMachineData>(dataASJson);
                    Console.WriteLine($"{CoffeeMachineData} | PartitionID: {context.PartitionId} | Offset:{eventdata.SystemProperties.Offset}");
                }
            }
            //This stors the current offset in the Azure bolb storage
           return context.CheckpointAsync();
          
        }
    }
}
