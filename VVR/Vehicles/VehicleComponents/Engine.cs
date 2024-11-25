using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVR.Technical;

namespace VVR.Vehicles.VehicleComponents
{

    enum Configuration
    {
        Inline,//+++reliability
        Flat,//+++Balance ---reliability
        V,//each cylinder in this config brings up the size less than in other configs but is heavier
    }
    enum EngineType
    {
        NaturallyAspirated,//+reliability
        Turbocharged,//++power +weight -reliability takes up sapce
        Supercharged,//+power +torque ++weight -reliability takes up space
    }
    internal class Engine
    {
        int cylinderAmmount;//^ = +hp +torque +fuelcons +size -reliability 
        float displacement;//^ = +hp +torque +fuelcons +size
        //same displacement but with more cylinders takes up less space but is heavier
        Configuration config;
        EngineType type;

        //calculated based on the above parameters

        //can be changed to be vertical and horizontal size
        float size
        {
            get => size;
            set => size = value;
        }
        float horsePower; //max speed
        float torque; //acceleration
        float reliability;
        float balance;// gives bonus to handling
        float fuelconsumption;
        float engineWeight;



        void CalculateParameters()
        {

        }



        Engine(int cylamm, float disp, Configuration conf = Configuration.Inline, EngineType t = EngineType.NaturallyAspirated)
        {
            cylinderAmmount = cylamm;
            displacement = disp;
            config = conf;
            type = t;
            //example 4 cyl 2l vs 6cyl 2l
            //4cyl 2l size = 2/4 * 100 = 50
            //6cyl 2l size = 2/6 *100 = 33.3
            if (config == Configuration.V)
            {
                size = (displacement / cylinderAmmount) * GlobalConsts.VENGINEMASSBIAS * GlobalConsts.ENGINESIZINGCONST;     

            }
            else//conf is flat or inline
            {
                size = displacement / cylinderAmmount * GlobalConsts.ENGINESIZINGCONST;
            }
            if (type == EngineType.Supercharged || type == EngineType.Turbocharged)
            {
                size += GlobalConsts.FORCEDINDUCTIONMASS;
            }
        }
    }
}
