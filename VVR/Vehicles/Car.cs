namespace VVR.Vehicles
{
    internal class Car : Vehicles
    {
        public Car(string brand, string model, float _hp, float _maxfuel, float _torque, float _mass) : base(_hp, _maxfuel, _mass, _torque)
        {
            Brand = brand;
            Model = model;

        }

        string Brand { get; }
        string Model { get; }

        public override string? ToString()
        {
            return $"Object Car (Brand: {Brand}, Model: {Model}, Power: {hp}, Current fuel: {currentFuel})";
        }
    }
}
