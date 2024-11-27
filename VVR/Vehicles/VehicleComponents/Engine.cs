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
        Flat,//+++bonus to handling -reliability
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
        int cylinderAmmount;//^ = +hp +fuelcons +size +weight -reliability 
        float displacement;//^ = +hp +torque +fuelcons +size +weight
        //same displacement but with more cylinders takes up less space but is heavier
        Configuration config;
        EngineType type;

        //calculated based on the above parameters

        //can be changed to be vertical and horizontal size
        float size
        {
            get;
            set;
        }

        //max speed
        float horsePower
        {
            get;
            set;
        }

        //acceleration
        float torque
        {
            get;
            set;
        }

        float reliability
        {
            get;
            set;
        }

        float fuelconsumption
        {
            get;
            set;
        }
        public float engineWeight
        {
            get;
            set;
        }



        float CalculateSize()
        {
            //example 4 cyl 2l vs 6cyl 2l
            //4cyl 2l size = (2/4 +4) * 100 = 450
            //6cyl 2l size = (2/6 +6)*100 = 633.3
            //16cyl 8l size = (8/16 +16)*100 = 1650
            float size = 0.0f;
            if (config == Configuration.V)
            {
                size = (displacement / cylinderAmmount + cylinderAmmount) * GlobalConsts.VENGINESIZEBIAS * GlobalConsts.ENGINESIZINGCONST;

            }
            else//conf is flat or inline
            {
                size = (displacement / cylinderAmmount + cylinderAmmount) * GlobalConsts.ENGINESIZINGCONST;
            }
            if (type == EngineType.Supercharged || type == EngineType.Turbocharged)
            {
                size += GlobalConsts.FORCEDINDUCTIONSIZE;
            }
            return size;
        }

        float CalculateWeight()
        {
            //example 4 cyl 2l vs 6cyl 2l
            //4cyl 2l weight = (4/2)* 100 = 200
            //6cyl 2l weight = 6/2 *100 = 300
            float weight = 0.0f;
            if (config == Configuration.V)
            {
                weight = (cylinderAmmount / displacement + displacement) * GlobalConsts.VENGINEMASSBIAS * GlobalConsts.ENGINEWEIGHTCONST;

            }
            else//conf is flat or inline
            {
                weight = (cylinderAmmount / displacement + displacement) * GlobalConsts.ENGINEWEIGHTCONST;
            }
            if (type == EngineType.Supercharged)
            {
                weight += GlobalConsts.FORCEDINDUCTIONMASSSC;
            }
            else if (type == EngineType.Turbocharged)
            {
                weight += GlobalConsts.FORCEDINDUCTIONMASSTC;
            }
            return weight;
        }

        float CalculateHorsePower()
        {
            float hp = 0.0f;
            hp = (cylinderAmmount * displacement) * GlobalConsts.HORSEPOWERMULTIPLIER;
            if (type == EngineType.Turbocharged)
            {
                hp += GlobalConsts.TURBOBONUS;
            }
            else if (type == EngineType.Supercharged)
            {
                hp += GlobalConsts.SUPERCHARGERBONUS;
            }
            return hp;
        }

        float CalculateTorque()
        {
            float torq = 0.0f;
            torq = displacement * GlobalConsts.TORQMULTIPLIER;
            if (type == EngineType.Turbocharged)
            {
                torq += GlobalConsts.TURBOBONUSTORQ;
            }
            else if (type == EngineType.Supercharged)
            {
                torq += GlobalConsts.SUPERCHARGERBONUSTORQ;
            }
            return torq;
        }

        float CalculateReliability()
        {
            float reliability = 100.0f;

            if (config == Configuration.Flat)
            {
                reliability -= GlobalConsts.FLATRELIABILITYDEBUF;
            }
            if (type == EngineType.Supercharged || type == EngineType.Turbocharged)
            {
                reliability -= GlobalConsts.FORCEDINDUCTIONDEBUF;
            }
            reliability -= cylinderAmmount;
            return reliability;
        }

        float CalculateFuelConsumption()
        {
            float fuelconsumption = 0;

            fuelconsumption += Math.Min(40.0f, displacement * GlobalConsts.FUELCONSUMPTIONCONST);

            if (type == EngineType.Supercharged || type == EngineType.Turbocharged)
            {
                fuelconsumption += GlobalConsts.FORCEDINDUCTIONFUELCONSUMPTION;
            }

            return fuelconsumption;
        }

        public Engine(int cylamm, float disp, Configuration conf = Configuration.Inline, EngineType t = EngineType.NaturallyAspirated)
        {
            cylinderAmmount = cylamm;
            displacement = disp;
            config = conf;
            type = t;
            size = CalculateSize();
            engineWeight = CalculateWeight();
            horsePower = CalculateHorsePower();
            torque = CalculateTorque();
            reliability = CalculateReliability();
            fuelconsumption = CalculateFuelConsumption();
        }

        public void PrintEngineStats()
        {
            Console.WriteLine($"Engine is: {Enum.GetName(typeof(Configuration), config)},\n" +
                $"inudction is: {type.ToString()},\n" +
                $"the cylinder ammount is: {cylinderAmmount},\n" +
                $"the displacement is {displacement},\n" +
                $"the size is {size} and weight {engineWeight}\n" +
                $"the horsepower {horsePower} and torque {torque}\n" +
                $"the reliability {reliability} and fuel consumption {fuelconsumption}\n");
        }
    }
}