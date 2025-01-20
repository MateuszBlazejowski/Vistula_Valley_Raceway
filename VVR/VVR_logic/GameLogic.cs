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
        public TechnicalMessageEventArgs(string _message) 
        {
            message = _message;
        }
    }
    internal class GameLogic
    {

        public event EventHandler<VehicleMovingParametersChangedEventArgs> VehicleMovingParametersChanged;
        public event EventHandler<TechnicalMessageEventArgs> TechnicalMessage;
        // Method to check for key input

        ScreenMessages messages = new ScreenMessages();
        public List<Vehicle> vehicles = new List<Vehicle>();
        public List<VehicleVisual> vehiclesVisual = new List<VehicleVisual>();
        public List<Vehicle> finnishedVehicles = new List<Vehicle>();
        public Track track = new Track();
        public DrivingSystem drivingSystem = new DrivingSystem();

        public ManualResetEvent logicReady = new ManualResetEvent(false);
        public ManualResetEvent renderReady = new ManualResetEvent(false);

        private bool isRunning = true;
        private ConsoleKey? lastKeyPressed = null;
        private bool keyProcessed = true;
        public int humanIndex; 


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
            // Initialize events to avoid null values
            VehicleMovingParametersChanged += (sender, e) => { };
            TechnicalMessage += (sender, e) => { };
        }

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey); // Declare GetAsyncKeyState
        public void StartKeyListener()
        {
            Thread inputThread = new Thread(() =>
            {
                int carIndex = vehicles.FindIndex(v => v.isHuman == true);
                int deltaPosX = 0;
                float deltaSpeed = 0;
                int humanMaxSpeed;
                while (true)
                {
                    // Reset changes for each iteration
                    deltaPosX = 0;
                    deltaSpeed = 0;
                    humanMaxSpeed = (int)((float)GlobalConsts.MAX_SPEED * GlobalConsts.DifficultyLevelMultiplier);
                    // Check if LeftArrow is pressed 
                    if ((GetAsyncKeyState((int)ConsoleKey.LeftArrow) & 0x8000) != 0)
                    {
                        deltaPosX = -1;
                    }
                    // Check if RightArrow is pressed
                    if ((GetAsyncKeyState((int)ConsoleKey.RightArrow) & 0x8000) != 0)
                    {
                        deltaPosX = 1;
                    }
                    // Check if UpArrow is pressed (accelerate)
                    if ((GetAsyncKeyState((int)ConsoleKey.UpArrow) & 0x8000) != 0 && vehicles[carIndex].speed < humanMaxSpeed)
                    {
                        deltaSpeed = Math.Min(vehicles[carIndex].acceleration, humanMaxSpeed - (float)vehicles[carIndex].speed);
                    }
                    // Check if DownArrow is pressed (brake)
                    if ((GetAsyncKeyState((int)ConsoleKey.DownArrow) & 0x8000) != 0 && vehicles[carIndex].speed > GlobalConsts.MIN_SPEED)
                    {
                        deltaSpeed = -2;
                    }

                    // Apply changes to the vehicle
                    VehicleMovingParametersChanged?.Invoke(this, new VehicleMovingParametersChangedEventArgs(deltaPosX, deltaSpeed, carIndex, false));
                    vehicles[carIndex].positionX += deltaPosX;
                    vehicles[carIndex].speed += deltaSpeed;

                    Thread.Sleep(50); // Prevent high CPU usage
                }
            });

            inputThread.IsBackground = true;
            inputThread.Start();
        }
        public void VehicleMoved(int carIndex)
        {
            float deltaSpeed = 0;
            int deltaPosX = 0;
            if (!vehicles[carIndex].isHuman == true)
            {
                int humanIndex = vehicles.FindIndex(v => v.isHuman == true);
                int PosY = (int)Math.Round(vehicles[carIndex].positionY);
                int predictedPosition = PosY + 3 * (int)Math.Round((vehicles[carIndex].speed / vehicles[humanIndex].speed));

                int crashedIntoWallReturn = CrashedIntoWall(carIndex, predictedPosition);
                if (crashedIntoWallReturn != 0)
                {
                   
                }
                int crashedIntoCarReturn = 0;

                int aiPosY;
                int aiPredictedPosition = 0;
                for (int i = 0; i < vehicles.Count; i++) // checking if we crashed with any other car in the next iteration(predicted position)
                {
                    if (i == carIndex) continue;

                    aiPosY = (int)Math.Round(vehicles[i].positionY);
                    aiPredictedPosition = aiPosY + 2 * (int)Math.Round((vehicles[i].speed / vehicles[humanIndex].speed));

                    if (aiPredictedPosition > track.trackPieces.Count)
                    {
                        aiPredictedPosition -= (track.trackPieces.Count + 1);
                    }
                    aiPredictedPosition = (track.trackPieces.Count + track.trackPieces.Count - aiPredictedPosition) % track.trackPieces.Count;

                    crashedIntoCarReturn = CrashedIntoCar(carIndex,i,predictedPosition,aiPredictedPosition);
                    if (crashedIntoCarReturn != 0) break;

                }
                drivingSystem.Drive(carIndex, ref deltaPosX, ref deltaSpeed, track, vehicles, crashedIntoWallReturn, crashedIntoCarReturn);
            }
            VehicleMovingParametersChanged?.Invoke(this, new VehicleMovingParametersChangedEventArgs(deltaPosX, deltaSpeed, carIndex, false));
            vehicles[carIndex].positionX += deltaPosX;
            vehicles[carIndex].speed += deltaSpeed;
        }
        private void updateCarPositionY()
        {
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
            int newX;
            if (track.trackPieces.Count - Yposition >= 0 || track.trackPieces.Count - Yposition < track.trackPieces.Count)
            {
                newX = (track.trackPieces[track.trackPieces.Count - Yposition].rightBorder
                            + track.trackPieces[track.trackPieces.Count - Yposition].leftBorder) / 2;
            }
            else
            {
                newX = 30;
            }
            float deltaSpeed = 0;
            //if player already at minimum speed no need to slow them down
            if (vehicles[carIndex].speed > GlobalConsts.MIN_SPEED)
            {
                deltaSpeed = -(2 * (float)vehicles[carIndex].speed / 4);
                if ((vehicles[carIndex].speed + deltaSpeed) < GlobalConsts.MIN_SPEED)
                {
                    deltaSpeed = (float)((double)GlobalConsts.MIN_SPEED - vehicles[carIndex].speed); 
                }

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
                if (Yposition < 0)
                    { continue; }
                if (CrashedIntoWall(i, Yposition) == -1 || CrashedIntoWall(i, Yposition) == 1)
                {
                    //Console.WriteLine("You crashed!");
                    BanishToMiddle(i, Yposition);
                }

                for (int j = i + 1; j < vehicles.Count; j++)
                {
                    int AIpositionY = (int)Math.Round(vehicles[j].positionY);
                    if (CrashedIntoCar(i, j, Yposition, AIpositionY) != 0)
                    {
                        //Console.WriteLine("Crashed into AI!");
                        BanishToMiddle(i, Yposition);
                       
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
            humanIndex = vehicles.FindIndex(v => v.isHuman == true);
            StartKeyListener();
            while (true)
            {
                logicReady.WaitOne(); // Wait for rendering thread to be ready
                logicReady.Reset();   // Reset for the next iteration

                for (int i = 0; i < vehicles.Count; i++) // every ai makes a move 
                {
                    VehicleMoved(i);
                }
                updateCarPositionY();

                CheckIfAnyoneCrashed();

                if (CheckAndFindWinners()) // if human finished then we end the game 
                {
                    break;
                }
                Console.SetCursorPosition(0, GlobalConsts.TRACKFRAMELENGTH + 3);
                Console.Write($"LAP :{vehicles[humanIndex].lapCounter+1}");
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

