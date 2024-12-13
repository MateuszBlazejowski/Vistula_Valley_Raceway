using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VVR.Visuals;
using VVR.Vehicles;

namespace VVR.VVR_logic
{
    public class PlayerMovedEventArgs : EventArgs
    {
        public ConsoleKey KeyPressed { get; }

        public PlayerMovedEventArgs(ConsoleKey keyPressed)
        {
            KeyPressed = keyPressed;
        }
    }
    public class TrackColorSchemeSetEventArgs : EventArgs
    {
        public ConsoleKey KeyPressed { get; }

        public TrackColorSchemeSetEventArgs(ConsoleKey keyPressed) 
        {
            KeyPressed = keyPressed;
        }
    }
    public class VehicleMovingParametersChangedEventArgs : EventArgs
    {
        public int deltaPosX;
        public float deltaSpeed;
        public int carIndex;

        public VehicleMovingParametersChangedEventArgs(int deltaPosX, float deltaSpeed, int carIndex) // posX should be only -1, 0 or +1, representing change in posX, speed 
        {
            this.deltaPosX = deltaPosX;
            this.deltaSpeed = deltaSpeed;
            this.carIndex = carIndex;
        }
    }
    internal class GameLogic
    {
        public event EventHandler<PlayerMovedEventArgs> PlayerMoved;
        public event EventHandler<TrackColorSchemeSetEventArgs> ColorSchemeSet;
        public event EventHandler<VehicleMovingParametersChangedEventArgs> VehicleMovingParametersChanged;
        // Method to check for key input

        ScreenMessages messages = new ScreenMessages();
        public List<Vehicle> vehicles =  new List<Vehicle>();
        public List<VehicleVisual> vehiclesVisual = new List<VehicleVisual>();

        public GameLogic(List<Vehicle> _vehicles)
        {
            vehicles = _vehicles;
            foreach (var vehicle in vehicles)
            {
                VehicleVisual car = new VehicleVisual(vehicle); // NEEDS UPDATE 
                vehiclesVisual.Add(car);
            }       
        }
        public void CheckForKeyPlayerMovement()
        {            
                // Read a key without showing it in the console
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

               // Raise the event with the key that was pressed
                PlayerMoved?.Invoke(this, new PlayerMovedEventArgs(keyInfo.Key));

                // Handle ESC key to exit the loop
                if (keyInfo.Key == ConsoleKey.Escape)
                { 
                   // break; // Exit the loop
                }
        }

        public void CheckForKeyColorScheme()
        {
            while (true)
            {
                messages.PrintChooseTrackColorSchemeMessage();
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key != ConsoleKey.D1 && keyInfo.Key != ConsoleKey.D2 && keyInfo.Key != ConsoleKey.D3 && keyInfo.Key != ConsoleKey.D4)
                {
                    messages.PrintInvalidInputMessage();
                    continue;
                }
                else 
                {
                    ColorSchemeSet?.Invoke(this, new TrackColorSchemeSetEventArgs(keyInfo.Key));
                    break;
                }
            }
        }
        public void VehicleMoved()
        {
            int carIndex = 0; // for now only the player car
            float deltaSpeed = 0;
            int deltaPosX = 0;
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
            if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                deltaPosX--; 
            }
            else if (keyInfo.Key == ConsoleKey.RightArrow)
            {
                deltaPosX++; 
            }
            carIndex = vehicles.FindIndex(car => car.isHuman == true);
            VehicleMovingParametersChanged?.Invoke(this, new VehicleMovingParametersChangedEventArgs(deltaPosX, deltaSpeed, carIndex));
        }
        public void StartGameLogic()
        {
            while (true)
            {
                VehicleMoved();
            }
        }
    }
}


/*
 
                // Check if a key is available without blocking
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true); // `true` prevents key from being printed

                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine("Escape key pressed. Exiting...");
                        break; // Exit the loop
                    }
                    else if (keyInfo.Key == ConsoleKey.LeftArrow)
                    {
                        carPosX--;
                    }
                    else if (keyInfo.Key == ConsoleKey.RightArrow)
                    {
                        carPosX++;
                    }
                }
 */