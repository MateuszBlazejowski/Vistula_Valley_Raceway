﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVR.Technical;

namespace VVR.Locations
{
    internal class Track // track list is implemented in such a way that the first piece is the top part and the last piece is the lowest part
        //cars are travelling from the bottom to the the top, which means the last item in the list is crossed first after the game starts 
    {
        private readonly string[] _trackComponentFileLocations = Directory.GetFiles(Path.Combine("Locations", "TrackAssets"), "*.track");
        float Condition { get; set; }//overall track condition, the lower the less grip a car has
        bool isWet = false;
        int Width {get; set;}//how many cars can fit ar once side by side
        float length { get; } //track length
        string? name { get; }
        int pitBoxAmmount{ get;}//how many pitboxes are on track, defines max car number

        public Track(float condition, int width, float length, string? name, int pitBoxAmmount)
        {
            Condition = condition;
            Width = width;
            this.length = length;
            this.name = name;
            this.pitBoxAmmount = pitBoxAmmount;
        }

        public List<TrackPiece> trackPieces = new List<TrackPiece>();
        public Track()
        {
        }
        
        

        public void GenerateFixedTrack() //for now track is fixed, but in future it is possible to write a function that will generate random track
        {
            trackPieces.Clear();
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '\\', '\\'));// when assigning \ the other sign \ is neccesary to ommit the special meaning of char \
            trackPieces.Add(new TrackPiece(1, 51, '\\', '\\'));
            trackPieces.Add(new TrackPiece(2, 52, '\\', '\\'));
            trackPieces.Add(new TrackPiece(3, 53, '\\', '\\'));
            trackPieces.Add(new TrackPiece(4, 54, '\\', '\\'));
            trackPieces.Add(new TrackPiece(5, 55, '\\', '\\'));
            trackPieces.Add(new TrackPiece(6, 56, '\\', '\\'));
            trackPieces.Add(new TrackPiece(7, 57, '\\', '\\'));
            trackPieces.Add(new TrackPiece(8, 58, '\\', '\\'));
            trackPieces.Add(new TrackPiece(9, 59, '\\', '\\'));
            trackPieces.Add(new TrackPiece(10, 60, '|', '|'));
            trackPieces.Add(new TrackPiece(10, 60, '|', '|'));
            trackPieces.Add(new TrackPiece(10, 60, '|', '|'));
            trackPieces.Add(new TrackPiece(10, 60, '|', '|'));
            trackPieces.Add(new TrackPiece(10, 60, '|', '|'));
            trackPieces.Add(new TrackPiece(10, 60, '|', '|'));
            trackPieces.Add(new TrackPiece(9, 59, '/', '/'));
            trackPieces.Add(new TrackPiece(8, 58, '/', '/'));
            trackPieces.Add(new TrackPiece(7, 57, '/', '/'));
            trackPieces.Add(new TrackPiece(6, 56, '/', '/'));
            trackPieces.Add(new TrackPiece(5, 55, '/', '/'));
            trackPieces.Add(new TrackPiece(4, 54, '/', '/'));
            trackPieces.Add(new TrackPiece(3, 53, '/', '/'));
            trackPieces.Add(new TrackPiece(2, 52, '/', '/'));
            trackPieces.Add(new TrackPiece(1, 51, '/', '/'));
            trackPieces.Add(new TrackPiece(0, 50, '/', '/'));
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
            trackPieces.Add(new TrackPiece(0, 50, '/', '/'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|'));
            trackPieces.Add(new TrackPiece(0, 50, '|', '|')); // the track is very short but sufficient for testing //FOR FURTHER DEVELOPMENT
        }

        public void GenerateTrack()
        {
            Random random = new Random(2137); // Is that a Maciej Spychała reference???
            for (int i = 0; i < GlobalConsts.TRACK_COMPONENT_AMOUNT; i++)
            {
                TrackParser.ParseCSV(_trackComponentFileLocations[random.Next(_trackComponentFileLocations.Length)], trackPieces);
            }
        }
    }
}
