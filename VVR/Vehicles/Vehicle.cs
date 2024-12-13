using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVR.Technical;
using VVR.Vehicles.VehicleComponents;

namespace VVR.Vehicles
{
    internal class Vehicle
    {
        float mass;
        Engine engine;
        Frame frame;
        Tyres tyresFront;
        Tyres tyresBack;


        public bool isHuman;
        public int positionX;
        public float positionY;
        public int lapCount;
        public float speed;
        public ConsoleColor bodyColor;
        public ConsoleColor roofTopColor;

        public Vehicle(int cylamm, float disp, float _maxEngineSize, float _tireWearFront = 0.0f, TyreType _tireTypeFront = TyreType.Soft, float _tireWearBack = 0.0f, TyreType _tireTypeBack = TyreType.Soft, float _fuelTankSize = GlobalConsts.DEFAULTFUELTANK, float _currentFuel = GlobalConsts.DEFAULTFUELTANK, Configuration conf = Configuration.Inline, EngineType t = EngineType.NaturallyAspirated)
        {
            engine = new Engine(cylamm, disp, conf, t);
            frame = new Frame(_maxEngineSize, _fuelTankSize, _currentFuel);
            tyresFront = new Tyres(_tireWearFront, _tireTypeFront);
            tyresBack = new Tyres(_tireWearBack, _tireTypeBack);
            mass = engine.engineWeight + frame.frameWeight;
            
        }
        public Vehicle(Engine _eng, Frame _frame, Tyres _tyresfront, Tyres _tyresBack)
        {
            engine = _eng;
            frame = _frame;
            tyresFront = _tyresfront;
            tyresBack = _tyresBack;
            mass = engine.engineWeight + frame.frameWeight;
        }

        public Vehicle(ConsoleColor _bodyColor, ConsoleColor _roofTopColor, bool _isHuman)//constructor to test visuals 
        { 
            isHuman = _isHuman;
            bodyColor = _bodyColor;
            roofTopColor = _roofTopColor;
        }
    }
}

/*
            SAMPLE CAR
            
            Console.WriteLine("Sample Car:");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write('\'');
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write('▄');//Console.Write('█');
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine('\'');
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write('\'');
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("▀");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine('\'');
 */

        public void PrintVehicleInfo()
        {
            engine.PrintEngineStats();
            frame.PrintFrame();
            tyresFront.PrintTyres();
            tyresBack.PrintTyres();
            Console.WriteLine($"The vehicle mass is {mass}");
        }
    }
}
