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

        public VehicleMovingParametersChangedEventArgs(int deltaPosX, float deltaSpeed, int carIndex) // posX should be only -1, 0 or +1, representing change in posX, speed 
        {
            this.deltaPosX = deltaPosX;
            this.deltaSpeed = deltaSpeed;
            this.carIndex = carIndex;
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
        public List<Vehicle> vehicles =  new List<Vehicle>();
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
                int iterations = 0;
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
                            else if (lastKeyPressed == ConsoleKey.UpArrow)
                            {
                                deltaSpeed++;
                            }
                            else if (lastKeyPressed == ConsoleKey.DownArrow)
                            {
                                deltaSpeed--;
                            }
                            keyProcessed = true; // Mark key as processed
                        }
                        VehicleMovingParametersChanged?.Invoke(this, new VehicleMovingParametersChangedEventArgs(deltaPosX, deltaSpeed, carIndex));
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
                driver.Drive(carIndex,ref deltaPosX,ref deltaSpeed, track, vehicles);       
            }
            VehicleMovingParametersChanged?.Invoke(this, new VehicleMovingParametersChangedEventArgs(deltaPosX, deltaSpeed, carIndex));
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
                    vehicles[i].positionY -= (track.trackPieces.Count+1);
                    vehicles[i].lapCounter += 1;
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

                for (int i = 0; i < vehicles.Count ; i++)
                { 
                VehicleMoved(i);                
                }
                updateCarPositionY();

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

