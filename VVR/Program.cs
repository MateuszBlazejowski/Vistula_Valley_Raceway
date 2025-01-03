﻿using System.Runtime.InteropServices;
using VVR.Vehicles.VehicleComponents;
using VVR.Locations;
using VVR.Visuals;
using VVR.Vehicles;
using VVR.VVR_logic;
using VVR.Technical;

namespace VVR
{
    internal class Program
    {
        static void Main(string[] args)  
        {
            // disabling winking from the cursor running around the console:
            Console.CursorVisible = false; 

            // speed in m/s, (to implement), one track piece is 3m and one car is 4.5m   
            // WARNING :
            // only one car can have true passed, as the game is singleplayer 
            List<Vehicle> vehicles = new List<Vehicle>();
            Vehicle car1 = new Vehicle("you",ConsoleColor.DarkRed, ConsoleColor.Red, true, 20, 1 ,87);
            vehicles.Add(car1);
            Vehicle car2 = new Vehicle("AI_1", ConsoleColor.DarkMagenta, ConsoleColor.Yellow, false, 30, 2, 88);
            vehicles.Add(car2);
            Vehicle car3 = new Vehicle("AI_2", ConsoleColor.DarkMagenta, ConsoleColor.Yellow, false, 25, 3, 82);
            vehicles.Add(car3);
            Vehicle car4 = new Vehicle("AI_3", ConsoleColor.DarkBlue, ConsoleColor.Yellow, false,40, 4, 85);
            vehicles.Add(car4);
            Vehicle car5 = new Vehicle("AI_4", ConsoleColor.DarkBlue, ConsoleColor.Yellow, false, 15, 5, 75);
            vehicles.Add(car5);

            GameLogic gameLogic = new GameLogic(vehicles); //starting game logic 
            ImageRendering imageRendering = new ImageRendering(gameLogic); //starting visuals
            gameLogic.CheckForKeyColorScheme(); //setting track color scheme

          //  VehicleVisual vehvis1 = new VehicleVisual(ConsoleColor.DarkRed, ConsoleColor.Red, true);
          //  VehicleVisual vehvis2 = new VehicleVisual(ConsoleColor.DarkMagenta, ConsoleColor.Yellow , false);
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
