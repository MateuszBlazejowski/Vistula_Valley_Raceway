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

namespace VVR.Visuals
{
    internal class ImageRendering
    {
        //will store all cars in a container independetnly to logic, that will allow to generate everything separately
        //events will modify cars properties like speed, position(turning left and right on track) etc, which will influence visuals 

        public Track track = new Track();
        private List<Vehicle> vehicles = new List<Vehicle>();

        bool playStop = false;
        int carPosX = 25;
        int carPosY = GlobalConsts.TRACKFRAMELENGTH / 2;   // later all of this will be stored in the array of vehicles 
        char[,] frame = new char[GlobalConsts.MAXTRACKWIDTH, GlobalConsts.TRACKFRAMELENGTH]; //frame chars 
        ConsoleColor[,] fgColors = new ConsoleColor[GlobalConsts.MAXTRACKWIDTH, GlobalConsts.TRACKFRAMELENGTH]; //frame foreground color
        ConsoleColor[,] bgColors = new ConsoleColor[GlobalConsts.MAXTRACKWIDTH, GlobalConsts.TRACKFRAMELENGTH]; //frame background color 
        // COLORING will be later stored in the  Tuple<ConsoleColor, ConsoleColor>[,] colors 
        public ImageRendering(GameLogic gameLogic)
        {
            track.GenerateFixedTrack();
            gameLogic.PlayerMoved += ChangePlayerPosition; 
        }

        private void ChangePlayerPosition(object sender, PlayerMovedEventArgs e) 
        {
            if (e.KeyPressed == ConsoleKey.LeftArrow)
            {
                carPosX--;
            }
            else if (e.KeyPressed == ConsoleKey.RightArrow)
            {
                carPosX++;
            }
            else if (e.KeyPressed == ConsoleKey.Escape)
            {
                playStop = true;
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

                if ((startingRow + i) % 2 == 0)
                {
                    fgColors[track.trackPieces[((startingRow + i) % track.trackPieces.Count)].leftBorder, i] = GlobalConsts.TrackColors[0];
                    fgColors[track.trackPieces[((startingRow + i) % track.trackPieces.Count)].rightBorder, i] = GlobalConsts.TrackColors[0];
                     //for now we only use fgColors to paint the track white and red

                }
                else
                {
                    fgColors[track.trackPieces[((startingRow + i) % track.trackPieces.Count)].leftBorder, i] = GlobalConsts.TrackColors[1];
                    fgColors[track.trackPieces[((startingRow + i) % track.trackPieces.Count)].rightBorder, i] = GlobalConsts.TrackColors[1];
                }

                //trackPieces[((startingRow+i)%trackPieces.Count)
            }
        }
        private void RenderCar(char[,] frame, int posX, int posY, ConsoleColor foreground, ConsoleColor background)// no parameters for now, but in future add position and car to find colors properties 
        {
            frame[posX - 1, posY] = '\'';
            frame[posX + 1, posY] = '\'';
            frame[posX - 1, posY - 1] = '\'';
            frame[posX + 1, posY - 1] = '\'';

            fgColors[posX - 1, posY] = ConsoleColor.DarkGray;
            fgColors[posX + 1, posY] = ConsoleColor.DarkGray;
            fgColors[posX - 1, posY - 1] = ConsoleColor.DarkGray;
            fgColors[posX + 1, posY - 1] = ConsoleColor.DarkGray;


            frame[posX, posY - 1] = '▄'; //using ▄ and set bacground to another color in order to create an impression of the front of the car 
            fgColors[posX, posY - 1] = foreground;
            bgColors[posX, posY - 1] = background;

            frame[posX, posY] = '▀';
            fgColors[posX, posY] = background;

        }
        private void PrintFrame(char[,] frame /*, ConsoleColor[,] fgColors, ConsoleColor[,] bgColors*/)
        {            
            for (int j = 0; j < GlobalConsts.TRACKFRAMELENGTH; j++) // Loop through rows (frame height)
            {
                Console.SetCursorPosition(0, j); // Move cursor to the start of each row
                for (int i = 0; i < GlobalConsts.MAXTRACKWIDTH; i++) // Loop through columns (frame width)
                {
                    Console.ForegroundColor = this.fgColors[i, j];
                    Console.BackgroundColor = this.bgColors[i, j];
                    Console.Write(frame[i, j]); // Write characters in place
                }

            }
            Console.ResetColor(); // Reset to default color
            

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
        public void Play()
        {
            Console.Clear();
            Console.WriteLine($"\nGame Starts in ");
            Console.WriteLine("\n\n\n Hint: Use left and right arrow to move and esc to finish");
           
            for (int i = 3; i > 0; i--)
            {
                Console.SetCursorPosition(15, 1);
                Console.Write(i);
                Thread.Sleep(1000);        
            }
            int startingRow = track.trackPieces.Count;
            while (!playStop) //stop condition, some event like pressing the esc button 
            {

                RenderTrackFrame(startingRow);
                RenderCar(frame, carPosX, carPosY, ConsoleColor.Red, ConsoleColor.DarkRed);// will be in a loop that generates all cars                
                PrintFrame(frame);

                startingRow--;
                if(startingRow<0)startingRow = track.trackPieces.Count; // looping the straighht track 

                Thread.Sleep(50);  //later depending on your speed                                 
            }

            Console.Clear();
            Console.WriteLine("\nESC pressed. Exiting...\n\n");
            Thread.Sleep(2000);
            Console.Clear();
           
            //will independently generate all visuals, respond to events which will change the the values of some things, that will influence display           
        }
    }
}
