using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVR.Technical;

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
        public float frameWeight;
        float maxEnigneSize;
        //float handling;
        //FrameType type; to be implemented later

        public float CalculateFrameWeight(float _maxEngineSize)
        {
            float weight = 0.0f;

            weight += _maxEngineSize * GlobalConsts.FRAMEWEIGHTCONST;

            return weight;
        }
        public Frame(float _maxEngineSize, float _fuelTankSize = GlobalConsts.DEFAULTFUELTANK, float _currentFuel = GlobalConsts.DEFAULTFUELTANK)
        {
            maxEnigneSize = _maxEngineSize;
            frameWeight = CalculateFrameWeight(maxEnigneSize);
        }

        public void PrintFrame()
        {
            Console.WriteLine($"Fueltank size is {fuelTankSize},\n" +
                $"current fuel: {currentFuel},\n" +
                $"the frame weighs {frameWeight},\n" +
                $"the biggest engine it can fit is {maxEnigneSize}\n");
        }

    }
}
