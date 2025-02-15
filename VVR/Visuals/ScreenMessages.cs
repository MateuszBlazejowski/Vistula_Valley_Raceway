using System;
using VVR.Vehicles;
using VVR.Technical;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVR.Visuals
{
    internal class ScreenMessages
    { 
        public void PrintGameStartMessage(ConsoleColor[] trackColorScheme)
        {
            Console.Clear();
            Console.WriteLine("\n    Game Starts in ");
            Console.WriteLine("\n\n\n    Hint: Use arrows to steer");
            for (int i = 3; i > 0; i--)
            {
                Console.SetCursorPosition(19, 1);
                Console.Write(i);
                Thread.Sleep(1000);
            }
        }
        public void PrintGameEndMessage(List<Vehicle> finishedVehicles)
        {
            Console.Clear ();
            for (int i = 0; i < finishedVehicles.Count  ; i++)
            {
                Console.WriteLine($"Position {i+1}: {finishedVehicles[i].id}");
                if (finishedVehicles[i].isHuman == true) break;
            }
        }
        public void PrintChooseTrackColorSchemeMessage()
        {
            Console.Clear();
            Console.WriteLine("\n Choose the color scheme: \n");
            Console.WriteLine(" Default (Red and White): press 1");
            Console.WriteLine(" Rainbow edition: press 2");
            Console.WriteLine(" Beach edition (Blue and Yellow): press 3");
            Console.WriteLine(" HelloKitty editon (Pink and White): press 4");
        }
        public void PrintInvalidInputMessage()
        {
            Console.Clear();
            Console.WriteLine("Invalid input");
            Thread.Sleep(2000);
            Console.Clear();
        }
    }
}
