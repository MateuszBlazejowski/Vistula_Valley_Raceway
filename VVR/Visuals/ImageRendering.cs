using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVR.Vehicles;
using VVR.Technical;
using VVR.Locations;
using VVR.VVR_logic;
using System.Drawing;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace VVR.Visuals
{
    internal class ImageRendering
    {
        //will store all cars in a container independetnly to logic, that will allow to generate everything separately
        //events will modify cars properties like speed, position(turning left and right on track) etc, which will influence visuals 

        public Stopwatch stopwatch = Stopwatch.StartNew();

        public Track track = new Track();
        public List<VehicleVisual> vehicles = new List<VehicleVisual>();
        ScreenMessages messages = new ScreenMessages(); //initializing class containig all messages 

        private bool playStop = false;
        private char[,] frame = new char[GlobalConsts.MAXTRACKWIDTH, GlobalConsts.TRACKFRAMELENGTH]; //frame chars 
        private char[,] previousframe = new char[GlobalConsts.MAXTRACKWIDTH, GlobalConsts.TRACKFRAMELENGTH]; // previous frame chars, to compare what has changed
                                                                                                             
        private ConsoleColor[,] fgColors = new ConsoleColor[GlobalConsts.MAXTRACKWIDTH, GlobalConsts.TRACKFRAMELENGTH]; //frame foreground color
        private ConsoleColor[,] bgColors = new ConsoleColor[GlobalConsts.MAXTRACKWIDTH, GlobalConsts.TRACKFRAMELENGTH]; //frame background color 
        private ConsoleColor[,] previousfgColors = new ConsoleColor[GlobalConsts.MAXTRACKWIDTH, GlobalConsts.TRACKFRAMELENGTH]; //previous frame foreground color, to compare what has changed
        private ConsoleColor[,] previousbgColors = new ConsoleColor[GlobalConsts.MAXTRACKWIDTH, GlobalConsts.TRACKFRAMELENGTH]; //previous frame background color , to compare what has changed

        private ConsoleColor[] trackColorScheme;

        private int humanPosition;
        private int humanIndex;

        private ManualResetEvent logicReady;
        private ManualResetEvent renderReady;
        public ImageRendering(GameLogic gameLogic)
        {
            track = gameLogic.track;
            vehicles = gameLogic.vehiclesVisual;
            humanIndex = vehicles.FindIndex(v => v.isHuman == true);
            //gameLogic.PlayerMoved += ChangePlayerPosition;

            vehicles[humanIndex].positionOnFrameY = GlobalConsts.TRACKFRAMELENGTH/2;

            this.logicReady = gameLogic.logicReady;
            this.renderReady = gameLogic.renderReady;

            gameLogic.VehicleMovingParametersChanged += ChangeVehiclePosition;
            gameLogic.ColorSchemeSet += SetColorScheme;
            gameLogic.TechnicalMessage += TechnicalMessageHandler;
        }

        private void ChangeVehiclePosition(object? sender, VehicleMovingParametersChangedEventArgs e)
        {
            vehicles[e.carIndex].positionX += e.deltaPosX; // changing posX by the delta
            vehicles[e.carIndex].speed += e.deltaSpeed;
        }

        private void TechnicalMessageHandler(object? sender, TechnicalMessageEventArgs e)
        {
            if (e.message == GlobalConsts.GAME_FINISHED_MESSAGE)
                playStop = true;
        }
        private void SetColorScheme(object? sender, TrackColorSchemeSetEventArgs e)
        {
            if (e.KeyPressed == ConsoleKey.D1)
            {
                trackColorScheme = GlobalConsts.DefaultTrackColors;
            }
            else if (e.KeyPressed == ConsoleKey.D2)
            {
                trackColorScheme = GlobalConsts.RainbowTrackColors;
            }
            else if (e.KeyPressed == ConsoleKey.D3)
            {
                trackColorScheme = GlobalConsts.BeachTrackColors;
            }
            else if (e.KeyPressed == ConsoleKey.D4)
            {
                trackColorScheme = GlobalConsts.HelloKittyTrackColors; 
            }
        }
        private void RenderTrackFrame(int startingRow)
        {
            for (int i = 0; i < GlobalConsts.MAXTRACKWIDTH; i++)
            {
                for (int j = 0; j < GlobalConsts.TRACKFRAMELENGTH; j++)
                {
                    frame[i, j] = ' ';
                }
            }
            for (int i = 0; i < GlobalConsts.TRACKFRAMELENGTH; i++)
            {
                frame[track.trackPieces[((startingRow + i) % track.trackPieces.Count)].leftBorder, i] = track.trackPieces[((startingRow + i) % track.trackPieces.Count)].leftBorderSign;
                frame[track.trackPieces[((startingRow + i) % track.trackPieces.Count)].rightBorder, i] = track.trackPieces[((startingRow + i) % track.trackPieces.Count)].rightBorderSign;
                // logic behind looping the track

                if(startingRow + i == track.trackPieces.Count)
                {
                    for (int x = (track.trackPieces[((startingRow + i) % track.trackPieces.Count)].leftBorder + 1); x < track.trackPieces[((startingRow + i) % track.trackPieces.Count)].rightBorder; x++)
                    {
                        if (x%2 == 0)
                        frame[x, i] = '▀';
                        else 
                        frame[x, i] = '▄';
                        fgColors[x, i] = ConsoleColor.Gray;
                    }
                } //starting line rendering

                int colorIndicator = (startingRow + i) % trackColorScheme.Count();
                fgColors[track.trackPieces[((startingRow + i) % track.trackPieces.Count)].leftBorder, i] = trackColorScheme[colorIndicator];
                fgColors[track.trackPieces[((startingRow + i) % track.trackPieces.Count)].rightBorder, i] = trackColorScheme[colorIndicator];                 
            }
        }
        private void RenderCar(char[,] frame, VehicleVisual car)// no parameters for now, but in future add position and car to find colors properties 
        {
            int posX = (int)car.positionX; //not an issue because it is checked earlier
            int posY = (int)car.positionOnFrameY; //not an issue because it is checked earlier
            
            if(posY >= 0 && posY < GlobalConsts.TRACKFRAMELENGTH)
            {
                if (posX - 1 >= 0 && posX - 1 < GlobalConsts.MAXTRACKWIDTH)
                { 
                    frame[posX - 1, posY] = '\'';
                    fgColors[posX - 1, posY] = ConsoleColor.DarkGray;
                }
                if (posX + 1 >= 0 && posX + 1 < GlobalConsts.MAXTRACKWIDTH)
                {  
                    frame[posX + 1, posY] = '\'';
                    fgColors[posX + 1, posY] = ConsoleColor.DarkGray;
                }
                if (posX >= 0 && posX < GlobalConsts.MAXTRACKWIDTH)
                {
                    frame[posX, posY] = '▀';
                    fgColors[posX, posY] = car.bodyColor;
                }
            }
            if (posY - 1 >= 0 && posY -1 < GlobalConsts.TRACKFRAMELENGTH)
            {
                if (posX - 1 >= 0 && posX - 1 < GlobalConsts.MAXTRACKWIDTH)
                {
                    frame[posX - 1, posY - 1] = '\'';
                    fgColors[posX - 1, posY - 1] = ConsoleColor.DarkGray;
                }
                if (posX + 1 >= 0 && posX + 1 < GlobalConsts.MAXTRACKWIDTH)
                {
                    frame[posX + 1, posY - 1] = '\'';
                    fgColors[posX + 1, posY - 1] = ConsoleColor.DarkGray;
                }
                if (posX >= 0 && posX < GlobalConsts.MAXTRACKWIDTH)
                {
                    frame[posX, posY - 1] = '▄'; //using ▄ and set bacground to another color in order to create an impression of the front of the car 
                    fgColors[posX, posY - 1] = car.roofTopColor;
                    bgColors[posX, posY - 1] = car.bodyColor;
                }                                                
            }
            // this is all rendering one car. Checks are neccesary to print only this part of a car that fits in the frame in case car is on the edge of the frame
        }

        private void PrintFrame(char[,] frame)
        {            
            for (int j = 0; j < GlobalConsts.TRACKFRAMELENGTH; j++) // Loop through rows (frame height)
            {
                Console.SetCursorPosition(0, j); // Move cursor to the start of each row
                for (int i = 0; i < GlobalConsts.MAXTRACKWIDTH; i++) // Loop through columns (frame width)
                {
                    if (previousframe[i, j] != frame[i, j] || previousfgColors[i, j] != fgColors[i, j] || previousbgColors[i, j] != bgColors[i, j]) // !!! GAME CHANGING !!!
                    // this check is crucial for the game display smoothness, because, as it turns out, PRINTING TO CONSOLE IS EXTREMELY SLOW IN C# 
                    {
                        Console.SetCursorPosition(i, j);
                        Console.ForegroundColor = this.fgColors[i, j];
                        Console.BackgroundColor = this.bgColors[i, j];
                        Console.Write(frame[i, j]); // Write characters in place
                    }

                }   
            }
            Console.ResetColor(); // Reset to default color
            Console.WriteLine();
            Console.WriteLine($"Your speeed: {vehicles[0].speed}      ");

            for (int i = 0; i < GlobalConsts.MAXTRACKWIDTH; i++)
            {
                for (int j = 0; j < GlobalConsts.TRACKFRAMELENGTH; j++)
                {
                    previousframe[i, j] = frame[i, j];
                    previousfgColors[i, j] = fgColors[i, j];
                    previousbgColors[i, j] = bgColors[i, j];
                }
            }

            // Clear the color arrays for the entire frame (resetting the track background)
            for (int i = 0; i < GlobalConsts.MAXTRACKWIDTH; i++)
            {
                for (int j = 0; j < GlobalConsts.TRACKFRAMELENGTH; j++)
                {
                    fgColors[i, j] = ConsoleColor.White; // Or whatever default you want
                    bgColors[i, j] = ConsoleColor.Black; // Default background color
                }
            }
        }

        // very funny function: 
        // calculating positionY of cars is funky and complicated: 
        //
        // reasons: 
        // impression of different speeds is achieved by changing frame rate, where with every rate we move main car one position forward
        // 
        // it creates an issue, how to move other cars relatively to the main car, without losing their position on the track, not only frame
        //
        // solution:
        // with every new frame, human car is moved by exaclty one
        // other cars move by theirSpeed / mainCarSpeed   (can be derived from v = s/t , where s = 1, v1 = HumanSpeed, v2 = otherCarsSpeed)
        // this solution is not ideal, but it is sufficient if we assume that the car is never staying in one place
        // 
        // car length 4.5m
        // | length 3m 
        //
        // WARNINGS:
        // HUMAN SPEED CANT BE ZERO 
        // SPEEDS SCHOULD NOT DIFFER MUCH, IT RUINS THE VISUAL DISPLAY 
        

        private void updateCarPositionY()
        {
            int humanIndex = vehicles.FindIndex(v => v.isHuman == true);
            vehicles[humanIndex].positionY+=1;
                    if (vehicles[humanIndex].speed == 0) throw new Exception("wrong code, HUMANSPEED schould never be zero");
            for (int i = 0;i<vehicles.Count;i++) 
            {
                if (vehicles[i].isHuman == false)
                {
                    vehicles[i].positionY += vehicles[i].speed / vehicles[humanIndex].speed;
                }
                if (vehicles[i].positionY > track.trackPieces.Count)
                {
                    vehicles[i].positionY -= (track.trackPieces.Count+1);
                }
            }
            humanPosition = (int)Math.Round(vehicles[humanIndex].positionY);
        }
        private void updateCarPositionOnFrameY()
        {
            

            for (int i = 0; i < vehicles.Count; i++)
            {
                if (vehicles[i].isHuman == true)
                    continue;
                int AiPosY = (int)Math.Round(vehicles[i].positionY); //we need to round it to be able to diplay it
                int relativePositionToHumanNOTPassingThrough0Line = AiPosY - humanPosition;
                int relativePositionToHumanPassingThrough0Line;

                // obtaining distance between them passing through finish line, when a car is behind a human then the value is negative
                if (AiPosY < humanPosition)
                {
                    relativePositionToHumanPassingThrough0Line = AiPosY - humanPosition  + track.trackPieces.Count;
                }
                else
                {
                    relativePositionToHumanPassingThrough0Line = (humanPosition - AiPosY + track.trackPieces.Count) *(-1);
                }
                // now only choosing which distance is shorter and setting its value as a position relative to the human 
                if (Math.Abs(relativePositionToHumanPassingThrough0Line) <= Math.Abs(relativePositionToHumanNOTPassingThrough0Line))
                    vehicles[i].positionOnFrameY = vehicles[humanIndex].positionOnFrameY - relativePositionToHumanPassingThrough0Line;
                else
                    vehicles[i].positionOnFrameY = vehicles[humanIndex].positionOnFrameY - relativePositionToHumanNOTPassingThrough0Line;
            }
        }

        private void WaitUntillTheFrameDurationPasses()
        {
            int humanIndex = vehicles.FindIndex(v => v.isHuman == true);
            // determine how long a frame should take (we refresh every 3m, so refreshing frame denpeds on the speed of main vehicle)
            TimeSpan _frameDuraton = TimeSpan.FromSeconds((double)GlobalConsts.ONE_CHAR_LENGTH_IRL / vehicles[humanIndex].speed);
            TimeSpan elapsedTimeWatch = stopwatch.Elapsed;
            // Determine the remaining time to sleep to maintain a consistent frame duration
            TimeSpan remainingTime = _frameDuraton - elapsedTimeWatch;
            if (remainingTime > TimeSpan.Zero) // Sleep only if there's remaining time; avoid negative sleep duration
            {
                Thread.Sleep(remainingTime);
            }
        }
        public void Play()
        {
            messages.PrintGameStartMessage(trackColorScheme);
            int startingRow =  track.trackPieces.Count - GlobalConsts.TRACKFRAMELENGTH/2 + GlobalConsts.DEFAULT_DISTANCE_BETWEEN_CARS_AT_THE_BEGGINING;
            for(int i = 0; i < GlobalConsts.MAXTRACKWIDTH; i++) // filling previous frame with ' ', necessary for the first execution of PrintFrame  
            {
                for (int j = 0; j < GlobalConsts.TRACKFRAMELENGTH; j++)
                {
                    previousframe[i, j] = ' ';
                    previousfgColors[i, j] = ConsoleColor.Black;
                    previousbgColors[i,j] = ConsoleColor.Black;
                }
            }
            while (!playStop) //stop condition, some event like pressing the esc button 
            {
                // Signal that the rendering thread is ready to start
                logicReady.Set(); // Notify game logic thread
                renderReady.WaitOne(); // Wait for game logic thread to finish
                renderReady.Reset();   // Reset for the next iteration

                stopwatch = Stopwatch.StartNew();

                updateCarPositionY();
                updateCarPositionOnFrameY();
                RenderTrackFrame(startingRow);

                foreach (var item in vehicles)
                {
                    if (item.positionOnFrameY != null)
                        RenderCar(frame, item);
                }
                PrintFrame(frame);
                startingRow--;
                if (startingRow < 0) startingRow = track.trackPieces.Count; // looping the straight track 

                WaitUntillTheFrameDurationPasses();

                Console.SetCursorPosition(0, 22);
                Console.Write($"{vehicles[1].positionY}     ");

            }
            //messages.PrintGameEndMessage(); 
        }
    }
}

/*
    to enchance: 
    frame rate: for now speed of frame updates is fixed by GlobalConsts.frameDuration, 
    in future it should be also dependent on some time that differs with the car speed
*/     

