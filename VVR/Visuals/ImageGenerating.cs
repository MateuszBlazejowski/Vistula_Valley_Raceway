using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
//using System.Threading;
using System.Threading.Tasks;
using VVR.Vehicles;
using VVR.Technical;
using VVR.Locations;

namespace VVR.Visuals
{
    internal class ImageGenerating
    {
        //will store all cars in a container independetnly to logic, that will allow to generate everything separately
        //events will modify cars properties like speed, position(turning left and right on track) etc, which will influence visuals 

        public Track track = new Track();

        private List<Vehicle> vehicles = new List<Vehicle>();
        public ImageGenerating()
        {
            track.GenerateFixedTrack();
        }
        
        private char[,] RenderTrackFrame(int startingRow)
        {
            char[,] toReturn = new char[GlobalConsts.MAXTRACKWIDTH, GlobalConsts.TRACKFRAMELENGTH];   
            for (int i = 0; i < GlobalConsts.MAXTRACKWIDTH; i++)
            {
                for (int j = 0; j < GlobalConsts.TRACKFRAMELENGTH; j++)
                {
                    toReturn[i, j] = ' ';
                }
            }

            for (int i = 0; i < GlobalConsts.TRACKFRAMELENGTH; i++)
            {
                toReturn[track.trackPieces[((startingRow + i) % track.trackPieces.Count)].leftBorder, i] = track.trackPieces[((startingRow + i) % track.trackPieces.Count)].leftBorderSign;
                toReturn[track.trackPieces[((startingRow + i) % track.trackPieces.Count)].rightBorder, i] = track.trackPieces[((startingRow + i) % track.trackPieces.Count)].rightBorderSign;
                //trackPieces[((startingRow+i)%trackPieces.Count)
            }
            return toReturn; 
        }
        private void RenderCar(char[,] frame, int posX, int posY)// no parameters for now, but in future add position and car to find colors properties 
        {
            frame[posX - 1, posY -1] = '\'';
            frame[posX, posY -1] = '█'; //later use ▄ and set bacground to another color in order to create an impression of the front of the car 
            frame[posX + 1, posY - 1] = '\'';
            frame[posX - 1, posY] = '\'';
            frame[posX, posY] = '▀';
            frame[posX + 1, posY] = '\'';
        }
        private void PrintFrame(char[,] frame)
        {
            Console.CursorVisible = false; // disable winking from the cursor running around the console

            for (int j = 0; j < GlobalConsts.TRACKFRAMELENGTH; j++) // Loop through rows (frame height)
            {
                // if (j % 2 == 0) Console.ForegroundColor = ConsoleColor.Red;
                //else Console.ForegroundColor = ConsoleColor.White; // COLORING will be addded later and stored in the  Tuple<ConsoleColor, ConsoleColor>[,] colors 
                Console.SetCursorPosition(0, j); // Move cursor to the start of each row
                for (int i = 0; i < GlobalConsts.MAXTRACKWIDTH; i++) // Loop through columns (frame width)
                {
                    Console.Write(frame[i, j]); // Write characters in place
                }

            }
            
            Console.CursorVisible = true;
        }
        public void Play()
        {
            int startingRow = track.trackPieces.Count; 
            char[,] frame = new char[GlobalConsts.MAXTRACKWIDTH, GlobalConsts.TRACKFRAMELENGTH];
            while (true)
            {
                frame = RenderTrackFrame(startingRow);

                int carPosX = 25;
                int carPosY = GlobalConsts.TRACKFRAMELENGTH / 2;   // later all of this will be stored in the array of vehicles 

                RenderCar(frame, carPosX, carPosY);// will be in a loop that generates all cars
                // later add objects to the track like cars etc 
                PrintFrame(frame);
                startingRow--;
                if(startingRow<0)startingRow = track.trackPieces.Count;

                Thread.Sleep(250);  //later depending on your speed  
               
                //stop condition, some event like pressing the esc button 
            }
            //will independently generate all visuals, respond to events which will change the the values of some things, that will influence display 
            // 
        }


    }
}
