using System.Runtime.InteropServices;
using VVR.Vehicles.VehicleComponents;
using VVR.Locations;
using VVR.Visuals;
using VVR.Vehicles;
using VVR.VVR_logic;
using VVR.Technical;
using VVR.TrackConverter;
using System.Reflection.Metadata.Ecma335;

namespace VVR
{
    internal class Program
    {
        static void Main(string[] args)  
        {
            GameSetup gameSetup = new GameSetup();

            if (args.Length != 0)
            {
                bool determiner = gameSetup.ArgsHandling(args); // program has to finish from various reasons if we get true
                if (determiner) return;
            }

            List<Vehicle> vehicles = gameSetup.SetGameVariables(); // setting everything like number of cars in the race etc. 

            GameLogic gameLogic = new GameLogic(vehicles); //starting game logic 
            ImageRendering imageRendering = new ImageRendering(gameLogic, gameSetup); //starting visuals

            gameSetup.CheckForKeyColorScheme();

            Thread imageRenderingThread = new Thread(imageRendering.Play); //play, new thread to allow simultaneous logic and play
            imageRenderingThread.Start();
            gameLogic.StartGameLogic();
            imageRenderingThread.Join();
        }
    }
}
