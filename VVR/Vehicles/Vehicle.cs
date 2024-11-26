using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVR.Vehicles.VehicleComponents;

namespace VVR.Vehicles
{
    internal abstract class Vehicle
    {
        float mass;
        Engine engine;
        Frame frame;
        Tyres tyresFront;
        Tyres tyresBack;

        public Vehicle(Engine _eng, Frame _frame, Tyres _tyresfront, Tyres _tyresBack)
        {
            engine = _eng;
            frame = _frame;
            tyresFront = _tyresfront;
            tyresBack = _tyresBack;
            mass = engine.engineWeight;//+frame.weight
        }
    }
}
