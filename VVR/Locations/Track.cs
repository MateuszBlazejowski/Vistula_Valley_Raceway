using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVR.Locations
{
    internal class Track
    {
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
    }
}
