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
    }
}
