using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVR.Vehicles;
using VVR.Vehicles.VehicleComponents;

namespace VVR.Visuals
{
    internal class CarConfigurator
    {
        public void StartConfiguration(Vehicle v)
        {
            Console.WriteLine("\nPlease configure your vehicle\n" +
                "We will start by creating an engine!\n" +
                "How many cylinders?(go crazy build that 52 cylinder monster)");

            string? inputcylinders = Console.ReadLine();
            int cylinders;
            if (!int.TryParse(inputcylinders, out cylinders))
            {
                Console.WriteLine("Invalid number format. Will use default engine");
                cylinders = 4;
            }
            Console.WriteLine("Now please select the displacement you wish to achieve");
            string? inputdisplacement = Console.ReadLine();
            float displacement;
            if (!float.TryParse(inputdisplacement, out displacement))
            {
                Console.WriteLine("Invalid number format. Will use default engine");
                displacement = 2.0f;
            }

            Engine eng = new Engine(cylinders, displacement);
            float engineSize = eng.size;
            Frame frame = new Frame(engineSize);
            v.engine = eng;
            v.frame = frame;
            v.tyresFront = new Tyres();
            v.tyresBack = new Tyres();
            v.mass = v.engine.engineWeight + v.frame.frameWeight;
            v.acceleration = v.CalculateAcceleration();

            Console.WriteLine("------------Your Engine-----------");
            v.PrintVehicleInfo();
            Console.WriteLine("Press anything to continue");
            Console.ReadKey();
        }
    }
}
