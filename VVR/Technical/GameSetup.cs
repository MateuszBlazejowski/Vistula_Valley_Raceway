using System.Runtime.InteropServices;
using VVR.Vehicles.VehicleComponents;
using VVR.Locations;
using VVR.Visuals;
using VVR.Vehicles;
using VVR.VVR_logic;
using VVR.Technical;
using VVR.TrackConverter;
using System.Reflection.Metadata.Ecma335;

namespace VVR.Technical
{
    public class TrackColorSchemeSetEventArgs : EventArgs
    {
        public ConsoleKey KeyPressed { get; }
        public TrackColorSchemeSetEventArgs(ConsoleKey keyPressed)
        {
            KeyPressed = keyPressed;
        }
    }
    public class GameSetup
    {


        public event EventHandler<TrackColorSchemeSetEventArgs> ColorSchemeSet;
        ScreenMessages messages = new ScreenMessages();

        CarConfigurator cr = new CarConfigurator();

        private int selectedTrackWidthAtTheStart = 50;// dont have still chosing tracks implemented so its taken from the present track  
        public GameSetup()
        {
            Console.CursorVisible = false; // disabling winking
            ColorSchemeSet += (sender, e) => { };
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

        public bool ArgsHandling(string[] args)
        {
            if (args[0] == "debugEngine")
            //c# doesnt store program name under args[0], so we expect to find the only argument there
            {
                Console.WriteLine("Entering debug mode for engine components");
                Console.WriteLine("----------------Engine 1 info------------------");
                Vehicle vehicle1 = new Vehicle(4, 2.0f, 0.5f);
                //vehicle1.PrintVehicleInfo();
                Console.WriteLine("----------------Engine 2 info------------------");
                Vehicle vehicle2 = new Vehicle(12, 4.0f, 1.3f);
                //vehicle2.PrintVehicleInfo();
                Console.WriteLine("Press 1 if you want to launch the game!(any other input will close)");
                if (!(Console.ReadKey().Key == ConsoleKey.D1))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Adding tracks to the game....");
                try
                {
                    TrackConverter.Program.RunTrackConverter(args);
                    Console.WriteLine("Track added successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to add track: {ex.Message}");
                }

                return true;
            }
        }

        internal List<Vehicle> SetGameVariables()
        {
            Console.Clear();
            Console.WriteLine("Choose difficulty level:");
            Console.WriteLine();

            Console.WriteLine("Absoulte computer drivers abomination: type 0");
            Console.WriteLine("Begginer: Type 1");
            Console.WriteLine("Intermediate: Type 2");
            Console.WriteLine("Advanced: Type 3");
            Console.WriteLine("Expert: Type 4");
            Console.WriteLine("Insane: Type 5");

            string? difficultyInput = Console.ReadLine();
            int difficulty;
            if (!int.TryParse(difficultyInput, out difficulty))
            {
                Console.WriteLine("Invalid number format. Will use default difficulty: Intermediate");
                difficulty = 2;
            }
            if (difficulty < 0 || difficulty > 5)
            {
                Console.WriteLine("Invalid number. Will use default difficulty: Intermediate");
                difficulty = 2;
            }

            string difficultyName = difficulty switch
            {
                0 => "Absolute computer drivers abomination",
                1 => "Beginner",
                2 => "Intermediate",
                3 => "Advanced",
                4 => "Expert",
                5 => "Insane",
                _ => "Unknown"
            };

            GlobalConsts.DifficultyLevelMultiplier = difficulty switch
            {
                0 => 1.5f,
                1 => 1.2f,
                2 => 1.1f,
                3 => 1.06f,
                4 => 1,
                5 => 0.95f,
                _ => 1
            };

            Console.WriteLine($"You have chosen difficulty: {difficultyName}");
            Console.WriteLine();

            Console.WriteLine("Type how many laps you want your race to last(from 0 to 10):  (recomended: 5 - 10)");
            string? lapsInput = Console.ReadLine();
            int laps;
            if (!int.TryParse(lapsInput, out laps))
            {
                Console.WriteLine("Invalid number format. Will set the race to take 5 laps");
                laps = 5;
            }
            if (laps < 0 || laps > 10)
            {
                Console.WriteLine("Invalid number. Will set the race to take 5 laps");
                laps = 5;
            }
            GlobalConsts.RACE_IN_LAPS = laps;
            Console.WriteLine();

            Console.WriteLine("Type how many opponents do you want: (from 0 to 4)");
            string? opponentsNumInput = Console.ReadLine();
            int opponentsNum;
            if (!int.TryParse(opponentsNumInput, out opponentsNum))
            {
                Console.WriteLine("Invalid number format. Will set the number of opponents to 4");
                opponentsNum = 4;
            }
            if (opponentsNum < 0 || opponentsNum > 4)
            {
                Console.WriteLine("Invalid number. Will set the number of opponents to 4");
                opponentsNum = 4;
            }
            Console.WriteLine();


            Console.WriteLine("Now type which position do you want to start from");
            Console.WriteLine("It must be from 1 to number of cars, which means number of opponents plus you");
            string? startingPosInput = Console.ReadLine();
            int startingPos;
            if (!int.TryParse(startingPosInput, out startingPos))
            {
                Console.WriteLine("Invalid position format. Will set it to 1");
                startingPos = 1;
            }
            if (startingPos < 1 || startingPos > (opponentsNum + 1))
            {
                Console.WriteLine("Invalid position. Will set it to 1");
                startingPos = 1;
            }



            Console.WriteLine();
            Console.WriteLine("Press anything to continue");
            Console.ReadKey();
            Console.Clear();


            List<Vehicle> vehicles = new List<Vehicle>();

            // speed in m/s, one track piece is 3m and one car is 4.5m   
            // WARNING :
            // only one car can have true passed, as the game is singleplayer             
            Vehicle car1 = new Vehicle("you", ConsoleColor.DarkRed, ConsoleColor.Red, true, StartingPosXCalculator(startingPos), startingPos, GlobalConsts.STARTING_SPEED);
            cr.StartConfiguration(car1);
            vehicles.Add(car1);
            List<(ConsoleColor primary, ConsoleColor secondary)> aiColorPairs = new List<(ConsoleColor, ConsoleColor)>
            {
                (ConsoleColor.DarkGreen, ConsoleColor.Yellow),
                (ConsoleColor.DarkMagenta, ConsoleColor.Yellow),
                (ConsoleColor.DarkBlue, ConsoleColor.Yellow),
                (ConsoleColor.DarkBlue, ConsoleColor.White)
            };

            Random random = new Random();

            for (int i = 1; i <= opponentsNum; i++)
            {
                var colorPair = aiColorPairs[random.Next(aiColorPairs.Count)];

                int position = (i >= startingPos) ? i + 1 : i;

                Vehicle aiCar = new Vehicle($"AI_{i}", colorPair.primary, colorPair.secondary, false, StartingPosXCalculator(position), position, GlobalConsts.STARTING_SPEED);
                vehicles.Add(aiCar);
            }
            return vehicles;
        }

        private int StartingPosXCalculator(int startingPos)
        {
            if (startingPos % 2 == 1)
            {
                return selectedTrackWidthAtTheStart / 2 - GlobalConsts.DOUBLE_STARTING_LINE_WIDTH / 2;
            }
            else
            {
                return selectedTrackWidthAtTheStart / 2 + GlobalConsts.DOUBLE_STARTING_LINE_WIDTH / 2;
            }
        }
    }

}
