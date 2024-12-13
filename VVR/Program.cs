using VVR.Vehicles.VehicleComponents;
using VVR.Visuals;
using VVR.Vehicles;

namespace VVR
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Car no longer goes vroom:(((");
            Thread.Sleep(1500);
            ImageGenerating imageGenerating = new ImageGenerating();
            //imageGenerating.GenerateFixedTrack();
            //imageGenerating.Play();
            Engine eng = new Engine(4, 2.7f, Configuration.Flat, EngineType.Turbocharged);
            eng.PrintEngineStats();

            Vehicle veh = new Vehicle(4 , 2.7f, 10);
            veh.PrintVehicleInfo();
        }
    }
}
