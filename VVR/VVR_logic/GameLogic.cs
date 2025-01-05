using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VVR.Visuals;
using VVR.Vehicles;
using VVR.Locations;
using System.Runtime.InteropServices;
using VVR.Technical;

namespace VVR.VVR_logic
{


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
        public bool isCrashed;

        public VehicleMovingParametersChangedEventArgs(int deltaPosX, float deltaSpeed, int carIndex, bool isCrashed) // posX should be only -1, 0 or +1, representing change in posX, speed 
        {
            this.deltaPosX = deltaPosX;
            this.deltaSpeed = deltaSpeed;
            this.carIndex = carIndex;
            this.isCrashed = isCrashed;
        }
    }
    public class TechnicalMessageEventArgs : EventArgs
    {
        public string message;
        public TechnicalMessageEventArgs(string _message) // posX should be only -1, 0 or +1, representing change in posX, speed 
        {
            message = _message;
        }
    }
    internal class GameLogic
    {

        public event EventHandler<TrackColorSchemeSetEventArgs> ColorSchemeSet;
        public event EventHandler<VehicleMovingParametersChangedEventArgs> VehicleMovingParametersChanged;
        public event EventHandler<TechnicalMessageEventArgs> TechnicalMessage;
        // Method to check for key input

        ScreenMessages messages = new ScreenMessages();
        public List<Vehicle> vehicles = new List<Vehicle>();
        public List<VehicleVisual> vehiclesVisual = new List<VehicleVisual>();
        public List<Vehicle> finnishedVehicles = new List<Vehicle>();
        public Track track = new Track();

        public ManualResetEvent logicReady = new ManualResetEvent(false);
        public ManualResetEvent renderReady = new ManualResetEvent(false);

        private bool isRunning = true;
        private ConsoleKey? lastKeyPressed = null;
        private bool keyProcessed = true;

        public GameLogic(List<Vehicle> _vehicles)
        {
            vehicles = _vehicles;
            track.GenerateFixedTrack();
            foreach (var vehicle in vehicles)
            {
                VehicleVisual car = new VehicleVisual(vehicle);
                vehiclesVisual.Add(car);
            }
            finnishedVehicles.Clear();
        }
        public void StartKeyListener()
        {
            Thread inputThread = new Thread(() =>
            {
                int carIndex = vehicles.FindIndex(v => v.isHuman == true);
                int deltaPosX = 0;
                float deltaSpeed = 0;
                //int iterations = 0;
                while (true)
                {

                    if (Console.KeyAvailable)
                    {
                        lastKeyPressed = Console.ReadKey(intercept: true).Key;
                        if (lastKeyPressed.HasValue)
                        {
                            if (lastKeyPressed == ConsoleKey.LeftArrow)
                            {
                                deltaPosX--;
                            }
                            else if (lastKeyPressed == ConsoleKey.RightArrow)
                            {
                                deltaPosX++;
                            }
                            else if (lastKeyPressed == ConsoleKey.UpArrow && vehicles[carIndex].speed < GlobalConsts.MAX_SPEED)
                            {
                                deltaSpeed++;
                            }
                            else if (lastKeyPressed == ConsoleKey.DownArrow && vehicles[carIndex].speed > GlobalConsts.MIN_SPEED)
                            {
                                deltaSpeed--;
                            }
                            keyProcessed = true; // Mark key as processed
                        }
                        VehicleMovingParametersChanged?.Invoke(this, new VehicleMovingParametersChangedEventArgs(deltaPosX, deltaSpeed, carIndex, false));
                        vehicles[carIndex].positionX += deltaPosX;
                        vehicles[carIndex].speed += deltaSpeed;
                    }
                    deltaPosX = 0;
                    deltaSpeed = 0;
                    Thread.Sleep(10); // Prevent high CPU usage
                }
            });
            inputThread.IsBackground = true;
            inputThread.Start();
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
        public void VehicleMoved(int carIndex)
        {
            float deltaSpeed = 0;
            int deltaPosX = 0;
            if (!vehicles[carIndex].isHuman == true)
            {
                AiDriver driver = new AiDriver();
                driver.Drive(carIndex, ref deltaPosX, ref deltaSpeed, track, vehicles);
            }
            VehicleMovingParametersChanged?.Invoke(this, new VehicleMovingParametersChangedEventArgs(deltaPosX, deltaSpeed, carIndex, false));
            vehicles[carIndex].positionX += deltaPosX;
            vehicles[carIndex].speed += deltaSpeed;
        }
        private void updateCarPositionY()
        {
            int humanIndex = vehicles.FindIndex(v => v.isHuman == true);
            vehicles[humanIndex].positionY += (double)1;
            if (vehicles[humanIndex].speed == 0) throw new Exception("wrong code, HUMANSPEED schould never be zero");
            for (int i = 0; i < vehicles.Count; i++)
            {
                if (vehicles[i].isHuman == false)
                {
                    vehicles[i].positionY += vehicles[i].speed / vehicles[humanIndex].speed;
                }
                if (vehicles[i].positionY > track.trackPieces.Count)
                {
                    vehicles[i].positionY -= (track.trackPieces.Count + 1);
                    vehicles[i].lapCounter += 1;
                }
            }
        }

        private void BanishToMiddle(int carIndex, int Yposition)
        {
            int newX = (track.trackPieces[track.trackPieces.Count - Yposition].rightBorder
                        + track.trackPieces[track.trackPieces.Count - Yposition].leftBorder) / 2;
            float deltaSpeed = 0;
            //if player already at minimum speed no need to slow them down
            if (vehicles[carIndex].speed > GlobalConsts.MIN_SPEED)
            {
                deltaSpeed = -(3 * (float)vehicles[carIndex].speed / 4);
            }

            VehicleMovingParametersChanged?.Invoke(this,
                new VehicleMovingParametersChangedEventArgs(newX, deltaSpeed, carIndex, true));

            vehicles[carIndex].positionX = newX;
            vehicles[carIndex].speed += deltaSpeed;
        }

        /// <summary>
        /// checks if a car from vehicle list at index carindex and with y position has crashed into a wall and if so from which side
        /// </summary>
        /// <param name="carIndex"></param>
        /// <param name="Yposition"></param>
        /// <returns> 0->not crashed, -1->crashed from the left, 1->crashed from the right</returns>
        public int CrashedIntoWall(int carIndex, int Yposition)
        {
            //check if it is on track
            //these calculations need the modulo bc we start with negative nums
            //they also need to be reffered from the bottom of the trackpiece list so if you want to access the 2 tile
            //you need to access the 3rd tile from the bottom
            //into left wall
            if (vehicles[carIndex].positionX < (track.trackPieces[(track.trackPieces.Count + track.trackPieces.Count - Yposition) % track.trackPieces.Count].leftBorder + 2) ||
                    vehicles[carIndex].positionX < (track.trackPieces[(track.trackPieces.Count + track.trackPieces.Count - Yposition + 1) % track.trackPieces.Count].leftBorder + 2))
            {
                return -1;
            }
            //into right wall
            if (vehicles[carIndex].positionX > (track.trackPieces[(track.trackPieces.Count + track.trackPieces.Count - Yposition) % track.trackPieces.Count].rightBorder - 2) ||
                    vehicles[carIndex].positionX > (track.trackPieces[(track.trackPieces.Count + track.trackPieces.Count - Yposition + 1) % track.trackPieces.Count].rightBorder - 2))
            {
                return 1;
            }
            //no crash
            return 0;
        }

        /// <summary>
        /// checks if 2 cars collided and from which side if they did
        /// sides are based on car1
        /// so if car 2 is on the right of car 1 and they crash the crash returns from the right
        /// </summary>
        /// <param name="car1Index"></param>
        /// <param name="car2Index"></param>
        /// <param name="car1Yposition"></param>
        /// <param name="car2Yposition"></param>
        /// <returns> 0->not crashed, -1->crashed from the left, 1->crashed from the right, 2->crashed from the front</returns>
        public int CrashedIntoCar(int car1Index, int car2Index, int car1Yposition, int car2Yposition)
        {

            //if theyre not aligned on the y axis they cant crash
            if ((car1Yposition < car2Yposition) || (car1Yposition > (car2Yposition + 1)))
            {
                return 0;
            }
            //check if aligned on x axis
            if ((vehicles[car1Index].positionX + 1) < (vehicles[car2Index].positionX - 1) || (vehicles[car1Index].positionX - 1) > (vehicles[car2Index].positionX + 1))
            {
                return 0;
            }
            //from now on we know that they crashed now to check the side
            //car 1 is to the left of car 2
            if (vehicles[car1Index].positionX < vehicles[car2Index].positionX)
            {
                return 1;
            }
            //car1 is to the right of car2
            else if (vehicles[car1Index].positionX > vehicles[car2Index].positionX)
            {
                return -1;
            }
            //if they didnt crash from the sides it has to be from top or bottom
            else
            {
                return 2;
            }

            //another approach based on checking wheel position may come in handy later

            ////check if right wheels of car2 touch left wheels of car2 
            //if (vehicles[car1Index].positionX + 1 == vehicles[car2Index].positionX - 1)
            //{
            //    return 1;
            //}
            ////check if left wheels of car2 touch right wheels of car2 
            //if (vehicles[car1Index].positionX - 1 == vehicles[car2Index].positionX + 1)
            //{
            //    return -1;
            //}
            ////checks if front of car1 is in the back of car2
            //if (car1Yposition + 1 == car2Yposition)
            //{
            //    return 2;
            //}
            ////no crash
            //return 0;
        }

        /// <summary>
        /// checks if any car has crashed with the wall or another car if so it reduces its speed by 3/4 and moves it to the middle of the track
        /// </summary>
        private void CheckIfAnyoneCrashed()
        {
            for (int i = 0; i < vehicles.Count; i++)
            {
                //calculating where the vehicle is
                int Yposition = (int)Math.Round(vehicles[i].positionY);
                if (CrashedIntoWall(i, Yposition) == -1 || CrashedIntoWall(i, Yposition) == 1)
                {
                    Console.WriteLine("You crashed!");
                    BanishToMiddle(i, Yposition);
                }

                for (int j = i + 1; j < vehicles.Count; j++)
                {
                    int AIpositionY = (int)Math.Round(vehicles[j].positionY);
                    if (CrashedIntoCar(i, j, Yposition, AIpositionY) != 0)
                    {
                        Console.WriteLine("Crashed into AI!");
                        BanishToMiddle(i, Yposition);
                        //Thread.Sleep(300);
                    }
                }
            }
        }

        private bool CheckAndFindWinners()
        {
            List<Vehicle> finishedCarsLocal = new List<Vehicle>();
            for (int i = 0; i < vehicles.Count; i++)
            {
                if (vehicles[i].isRaceFinished != true && vehicles[i].lapCounter >= GlobalConsts.RACE_IN_LAPS)
                {
                    vehicles[i].isRaceFinished = true;
                    finishedCarsLocal.Add(vehicles[i]);
                }
            }
            finishedCarsLocal.Sort((a, b) => a.positionY.CompareTo(b.positionY));
            foreach (Vehicle vehicle in finishedCarsLocal)
            {
                finnishedVehicles.Add(vehicle);
            }
            if (finishedCarsLocal.Any(obj => obj.isHuman == true))
            {
                TechnicalMessage?.Invoke(this, new TechnicalMessageEventArgs(GlobalConsts.GAME_FINISHED_MESSAGE));
                return true;
            }
            return false;

        }
        public void StartGameLogic()
        {

            StartKeyListener();
            while (true)
            {
                logicReady.WaitOne(); // Wait for rendering thread to be ready
                logicReady.Reset();   // Reset for the next iteration

                for (int i = 0; i < vehicles.Count; i++)
                {
                    VehicleMoved(i);
                }
                updateCarPositionY();

                CheckIfAnyoneCrashed();

                if (CheckAndFindWinners()) // if human finished then we end the game 
                {
                    break;
                }

                renderReady.Set();

                //int humanIndex = vehicles.FindIndex(v => v.isHuman == true);
                //Thread.Sleep(100);
                //Console.SetCursorPosition(0, 23);
                //Console.Write($"{vehicles[1].positionY}     ");
                //Console.SetCursorPosition(0, 24);
                //Console.Write($"{vehicles[1].lapCounter}     ");
                //Thread.Sleep(300);
            }
            messages.PrintGameEndMessage(finnishedVehicles);
        }
    }
}

