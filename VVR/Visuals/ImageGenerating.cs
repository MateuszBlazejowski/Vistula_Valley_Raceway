using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVR.Vehicles; 

public static class GlobalConstants
{
    public const int MaxTrackWidth = 100;
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


        public void Play()
        {

            //will independently generate all visuals, respond to events which will change the the values of some things, that will influence display 
            // 
        }


    }
}
