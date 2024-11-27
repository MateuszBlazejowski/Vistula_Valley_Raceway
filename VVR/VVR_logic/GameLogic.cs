using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    internal class GameLogic
    {
        // Define the event using the EventHandler with a custom EventArgs
        public event EventHandler<PlayerMovedEventArgs> PlayerMoved;

        // Method to check for key input
        public void CheckForKey()
        {
            while (true)
            {
                // Read a key without showing it in the console
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                // Raise the event with the key that was pressed
                PlayerMoved?.Invoke(this, new PlayerMovedEventArgs(keyInfo.Key));

                // Handle ESC key to exit the loop
                if (keyInfo.Key == ConsoleKey.Escape)
                { 
                    break; // Exit the loop
                }
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