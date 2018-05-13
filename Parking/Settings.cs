using System.Collections.Generic;

namespace Parking
{
    public class Settings
    {
        public int Timeout { get; private set; } = 3_000; // 3 seconds
        public int ParkingSpace { get; private set; } = 100;
        public decimal Fine { get; private set; } = 0.03M; // 3% per transaction
        public int TransactionSaveTime { get; set; } = 60_000; //1 minute
        public Dictionary<string, decimal> Prices { get; private set; } = new Dictionary<string, decimal>
        {
            { "Truck", 1.5M},
            { "Passenger", 1.0M},
            { "Bus", 1.3M},
            { "Motorcycle", 0.4M }
        };
    }
}
