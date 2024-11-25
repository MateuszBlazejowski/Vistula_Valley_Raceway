using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVR.Vehicles.VehicleComponents
{
    enum FrameType
    {
        Car,
        Bike,
        OpenWheeler,
    }
    internal class Frame
    {
        float fuelTankSize;
        float currentFuel;
        float aerodynamics;
        float balance;
        float handling;
        float frameWeight;
    }
}
