namespace Parking.Cars
{
    public abstract class Vehicle
    {
        private static int CurrentId { get; set; }
        public int Id { get; }
        public decimal Balance { get; set; }
        public string CarType => ToString();

        protected Vehicle()
        {
            Id = CurrentId++;
        }
        protected Vehicle(decimal balance) : this()
        {
            Balance = balance;
        }

        public override string ToString()
        {
            return base.ToString().Split('.')[2]; ;
        }

    }
}
