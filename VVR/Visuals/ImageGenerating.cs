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
        {

        }

        public List<TrackPiece> trackPieces = new List<TrackPiece>();
        public List<Vehicle> vehicles = new List<Vehicle>();
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

        public char[,] RenderTrackFrame(int startingRow)
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
                toReturn[((startingRow + i) % trackPieces.Count), i] = ' ';
                //trackPieces[((startingRow+i)%trackPieces.Count)
            }
            return toReturn; 
        }   
        public void Play()
        {
            int startingRow = 0; 
            char[,] frame = new char[GlobalConstants.MAXTRACKWIDTH, GlobalConstants.TRACKFRAMELENGTH];
            while (true)
            {
                frame = RenderTrackFrame(startingRow); 

                // later add objects to the track like cars etc 
                startingRow++;
                startingRow = startingRow % trackPieces.Count;

                //stop condition, some event like pressing the esc button 
                Thread.Sleep(500);  //later depending on your speed  
            }
            //will independently generate all visuals, respond to events which will change the the values of some things, that will influence display 
            // 
        }


    }
}
