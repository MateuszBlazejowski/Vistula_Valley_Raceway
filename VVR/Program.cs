using VVR.Vehicles.VehicleComponents;
using VVR.Locations;
using VVR.Visuals;

namespace VVR
{
    internal class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine("Car no longer goes vroom:(((");
            Console.WriteLine("Sample Car:");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write('\'');
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write('▄');//Console.Write('█');
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine('\'');
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write('\'');
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("▀");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine('\'');


            Thread.Sleep(2000);
            ImageGenerating imageGenerating = new ImageGenerating();
            //imageGenerating.GenerateFixedTrack();
            imageGenerating.Play();
            Engine eng = new Engine(4, 2.7f, Configuration.Flat, EngineType.Turbocharged);
            eng.PrintEngineStats();
        }
    }
}
