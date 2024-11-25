using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
//using System.Threading;
using System.Threading.Tasks;
using VVR.Vehicles; 

public static class GlobalConstants
{
    public const int MAXTRACKWIDTH = 100;
    public const int TRACKFRAMELENGTH = 15;
}

namespace VVR.Visuals
{
    internal class ImageGenerating
    {
        //will store all cars in a container independetnly to logic, that will allow to generate everything separately
        //events will modify cars properties like speed, position(turning left and right on track) etc, which will influence visuals 
  

        public ImageGenerating()
        {}

        private List<TrackPiece> trackPieces = new List<TrackPiece>();
        private List<Vehicle> vehicles = new List<Vehicle>();
        public void GenerateFixedTrack() //for now track is fixed, but in future it is possible to write a function that will generate random track 
        {
            trackPieces.Clear();
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '\\', '\\'));// when assigning \ the other sign \ is neccesary to ommit the special meaning of char \
            trackPieces.Add(new TrackPiece(1, 51, '\\', '\\'));
            trackPieces.Add(new TrackPiece(2, 52, '|', '|'));
            trackPieces.Add(new TrackPiece(2, 52, '|', '|'));
            trackPieces.Add(new TrackPiece(2, 52, '|', '|'));
            trackPieces.Add(new TrackPiece(2, 52, '|', '|'));
            trackPieces.Add(new TrackPiece(2, 52, '|', '|'));
            trackPieces.Add(new TrackPiece(2, 52, '|', '|'));
            trackPieces.Add(new TrackPiece(1, 51, '/', '/'));
            trackPieces.Add(new TrackPiece(1, 51, '|', '|'));
            trackPieces.Add(new TrackPiece(1, 51, '|', '|'));
            trackPieces.Add(new TrackPiece(1, 51, '|', '|'));
            trackPieces.Add(new TrackPiece(1, 51, '|', '|'));
            trackPieces.Add(new TrackPiece(1, 51, '|', '|'));
            trackPieces.Add(new TrackPiece(1, 51, '/', '/'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|')); // the track is very short but sufficient for testing //FOR FURTHER DEVELOPMENT 
        }

        private char[,] RenderTrackFrame(int startingRow)
        {
            char[,] toReturn = new char[GlobalConstants.MAXTRACKWIDTH, GlobalConstants.TRACKFRAMELENGTH];
            for (int i = 0; i < GlobalConstants.MAXTRACKWIDTH; i++)
            {
                for (int j = 0; j < GlobalConstants.TRACKFRAMELENGTH; j++)
                {
                    toReturn[i, j] = ' ';
                }
            }

            for (int i = 0; i < GlobalConstants.TRACKFRAMELENGTH; i++)
            {
                toReturn[trackPieces[((startingRow + i) % trackPieces.Count)].leftBorder, i] = trackPieces[((startingRow + i) % trackPieces.Count)].leftBorderSign;
                toReturn[trackPieces[((startingRow + i) % trackPieces.Count)].rightBorder, i] = trackPieces[((startingRow + i) % trackPieces.Count)].rightBorderSign;
                //trackPieces[((startingRow+i)%trackPieces.Count)
            }
            return toReturn; 
        }
        private void PrintFrame(char[,] frame)
        {
            Console.CursorVisible = false; // disable winking from the cursor running around the console

            for (int j = 0; j < GlobalConstants.TRACKFRAMELENGTH; j++) // Loop through rows (frame height)
            {
                Console.SetCursorPosition(0, j); // Move cursor to the start of each row
                for (int i = 0; i < GlobalConstants.MAXTRACKWIDTH; i++) // Loop through columns (frame width)
                {
                    Console.Write(frame[i, j]); // Write characters in place
                }
            }
            Console.CursorVisible = true;
        }

        public void Play()
        {
            int startingRow = trackPieces.Count; 
            char[,] frame = new char[GlobalConstants.MAXTRACKWIDTH, GlobalConstants.TRACKFRAMELENGTH];
            while (true)
            {
                frame = RenderTrackFrame(startingRow); 


                // later add objects to the track like cars etc 
                PrintFrame(frame);
                startingRow--;
                if(startingRow<0)startingRow = trackPieces.Count;

                Thread.Sleep(250);  //later depending on your speed  
               
                //stop condition, some event like pressing the esc button 
            }
            //will independently generate all visuals, respond to events which will change the the values of some things, that will influence display 
            // 
        }


    }
}
