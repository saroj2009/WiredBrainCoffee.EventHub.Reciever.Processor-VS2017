using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;

namespace WiredBrainCoffee.EventHub.Rcver.AllPartition
{
    public class WiredBrainCoffeePartitionRecieveHandler: IPartitionReceiveHandler
    {
        int _number= 10;
        int IPartitionReceiveHandler.MaxBatchSize { get { return this._number; } set { this._number = 10; } }
        public WiredBrainCoffeePartitionRecieveHandler(string partitionID)
        {
            PartitionID = partitionID;
            //IPartitionReceiveHandler.MaxBatchSize = 100;
        }
        public string PartitionID { get; }
       

        public Task ProcessErrorAsync(Exception error)
        {
            Console.WriteLine($"Exception: {error.Message}");
            return Task.CompletedTask;
        }
       
        public Task ProcessEventsAsync(IEnumerable<EventData> events)
        {
            if (events != null)
            {
                foreach (var evntdata in events)
                {

                    var DataAsJson = Encoding.UTF8.GetString(evntdata.Body.Array);
                    Console.WriteLine($"{DataAsJson} | Partition ID: {PartitionID}");
                }
            }
            return Task.CompletedTask;
        }
    }
}
