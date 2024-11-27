using System.Runtime.InteropServices;

using VVR.Vehicles.VehicleComponents;
using VVR.Locations;
using VVR.Visuals;
using VVR.VVR_logic;

namespace VVR
{
    internal class Program
    {
        static void Main(string[] args)  
        {
            Console.CursorVisible = false; // disable winking from the cursor running around the console

            Console.WriteLine("\nCar goes vroom:)))");
            Thread.Sleep(1000);

            GameLogic gameLogic = new GameLogic();
            ImageRendering imageGenerating = new ImageRendering(gameLogic);
            Thread imageGeneratingThread = new Thread(imageGenerating.Play);
            imageGeneratingThread.Start();

            gameLogic.CheckForKey();

            imageGeneratingThread.Join();

            Engine eng = new Engine(4, 2.7f, Configuration.Flat, EngineType.Turbocharged);
            eng.PrintEngineStats();

            Console.CursorVisible = true;
        }
    }
}
