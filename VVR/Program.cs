using System.Runtime.InteropServices;
using VVR.Vehicles.VehicleComponents;
using VVR.Locations;
using VVR.Visuals;
using VVR.Vehicles;
using VVR.VVR_logic;

namespace VVR
{
    internal class Program
    {
        static void Main(string[] args)  
        {
            Console.CursorVisible = false; // disable winking from the cursor running around the console

            List<Vehicle> vehicles = new List<Vehicle>();
            Vehicle car = new Vehicle(ConsoleColor.DarkRed, ConsoleColor.Red, true);
            vehicles.Add(car);
            Vehicle car2= new Vehicle(ConsoleColor.DarkMagenta, ConsoleColor.Yellow, false);
            vehicles.Add(car2);
            Vehicle car3 = new Vehicle(ConsoleColor.DarkBlue, ConsoleColor.Yellow, false);
            vehicles.Add(car3);

            GameLogic gameLogic = new GameLogic(vehicles); //starting game logic 
            ImageRendering imageRendering = new ImageRendering(gameLogic); //starting visuals
            gameLogic.CheckForKeyColorScheme(); //setting track color scheme

          //  VehicleVisual vehvis1 = new VehicleVisual(ConsoleColor.DarkRed, ConsoleColor.Red, true);
          //VehicleVisual vehvis2 = new VehicleVisual(ConsoleColor.DarkMagenta, ConsoleColor.Yellow , false);
          //  imageRendering.vehicles.Add(vehvis1);
          //  imageRendering.vehicles.Add(vehvis2);

            Thread imageRenderingThread = new Thread(imageRendering.Play); //play, new thread to allow simultaneous logic and play
            imageRenderingThread.Start();

            gameLogic.StartGameLogic();

            imageRenderingThread.Join();

            Engine eng = new Engine(4, 2.7f, Configuration.Flat, EngineType.Turbocharged);
            eng.PrintEngineStats();

            Vehicle veh = new Vehicle(4 , 2.7f, 10);
            veh.PrintVehicleInfo();

            Console.CursorVisible = true;
        }
    }
}
