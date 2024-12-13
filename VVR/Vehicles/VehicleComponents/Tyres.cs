using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVR.Vehicles.VehicleComponents
{
    enum TyreType
    {
        Soft,//wears the fastest but the most grip
        Hard,//wears the slowest but less grip
        Wet,//only for the wet least grip in dry
    }
    internal class Tyres
    {
        float tireWear
        //0 is no wear = new tyres
        //100 is fully worn tyres, no grip and should pop 
        {
            get ;
            set ;
        }
        TyreType tireType
        {
            get;
            set ;
        }
        public Tyres(float _tireWear = 0.0f, TyreType _tireType = TyreType.Soft)
        {
            tireWear = _tireWear;
            tireType = _tireType;
        }

        public void PrintTyres()
        {
            Console.WriteLine($"The tyre compound is {tireType}, and their current wear is {tireWear}");
        }
    }
}
