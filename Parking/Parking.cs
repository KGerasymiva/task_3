using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Parking.Cars;

namespace Parking
{
    public class Parking
    {
        private static readonly Lazy<Parking> _lazy = new Lazy<Parking>(() => new Parking());

        public static Parking Instance => _lazy.Value;

        public Settings Settings { get; }

        public List<Vehicle> Cars { get; }
        public Queue<List<Trasaction>> Trasactions { get; }
        public decimal Balance { get; private set; }

        private InternalTimer InternalTimer { get; }

        public int TransactionCapacity { get; }
        public Logger Logger { get; }

        private Parking()
        {
            Settings = new Settings();
            Cars = new List<Vehicle>()
            {
                new Bus(100),
                new Motorcycle(250),
                new Truck(55),
            };
            InternalTimer = new InternalTimer(Settings.Timeout, UpdateBalance);
            TransactionCapacity = Settings.TransactionSaveTime / Settings.Timeout;
            Trasactions = new Queue<List<Trasaction>>();
            Logger = new Logger(this, Settings.TransactionSaveTime);
        }

        private void UpdateBalance()
        {

            var listTransactions = new List<Trasaction>();
            foreach (var car in Cars)
            {
                var carType = car.ToString();
                var carPrice = Settings.Prices[carType];
                decimal carUnitPayment;

                if (car.Balance >= carPrice)
                {
                    carUnitPayment = carPrice;
                }
                else if ((car.Balance > 0) && (car.Balance < carPrice))
                {
                    carUnitPayment = carPrice * (Settings.Fine + 1) - car.Balance * Settings.Fine;
                }
                else
                {
                    carUnitPayment = carPrice * (Settings.Fine + 1);
                }

                car.Balance -= carUnitPayment;
                Balance += carUnitPayment;


                var transaction = new Trasaction
                {
                    DateTime = DateTime.Now,
                    Id = car.Id,
                    Debit = carUnitPayment
                };

                listTransactions.Add(transaction);
            }

            Trasactions.Enqueue(listTransactions);
            if (Trasactions.Count > TransactionCapacity)
            {
                Trasactions.Dequeue();
            }

        }

        public int FreeSpaces()
        {
            return Settings.ParkingSpace - Cars.Count;
        }

        public int AddCarToParking(Vehicle vehicle)
        {
            Cars.Add(vehicle);
            return vehicle.Id;
        }

        public Vehicle CreateCar(decimal balance, int carType)
        {
            switch (carType)
            {
                case 1:
                    return new Truck(balance);
                case 2:
                    return new Passenger(balance);
                case 3:
                    return new Bus(balance);
                case 4:
                    return new Motorcycle(balance);
            }

            return null;
        }

        public decimal? RemoveCar(int id)
        {
            for (var i = 0; i < Cars.Count; i++)
            {
                if (Cars[i].Id != id) continue;
                if (Cars[i].Balance > 0)
                {
                    var balance = Cars[i].Balance;
                    Cars.Remove(Cars[i]);
                    return balance;
                }

                return Cars[i].Balance;

            }

            return null;
        }

        public decimal? TopUpBalance(int id, decimal toppingup)
        {
            foreach (var car in Cars)
            {
                if (car.Id != id) continue;
                car.Balance += toppingup;
                return car.Balance;
            }

            return null;
        }

        public decimal CalcBananceForMinute()
        {
            decimal balance = 0;

            foreach (var list in Trasactions)
            {
                foreach (var transaction in list)
                {
                    balance += transaction.Debit;
                }

            }

            return balance;
        }

        public string PrintLogForMinute()
        {
            var stringBuilder = new StringBuilder();
            var str = string.Empty;
            foreach (var list in Trasactions)
            {

                foreach (var trasaction in list)

                {
                    str = string.Format("{0}   Id={1}   {2:C}"+Environment.NewLine, trasaction.DateTime, trasaction.Id, trasaction.Debit); 
                    stringBuilder.Append(str);
                }

            }

            return stringBuilder.ToString();
        }



        public string PrintLogForMinute(int id)
        {
            var stringBuilder = new StringBuilder();
            var str = string.Empty;
            foreach (var list in Trasactions)
            {

                foreach (var trasaction in list)

                {
                    if (trasaction.Id == id)
                    {
                        str = string.Format("{0}   Id={1}   {2:C}" + Environment.NewLine, trasaction.DateTime,
                            trasaction.Id, trasaction.Debit);
                        stringBuilder.Append(str);
                    }
                }

            }

            return stringBuilder.ToString();
        }

    }
}


