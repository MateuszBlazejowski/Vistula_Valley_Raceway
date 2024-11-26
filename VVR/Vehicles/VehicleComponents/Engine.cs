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
            get => size;
            set => size = value;
        }

        //max speed
        float horsePower
        {
            get => horsePower;
            set => horsePower = value;
        }

        //acceleration
        float torque
        {
            get => torque;
            set => torque = value;
        }

        float reliability
        {
            get => reliability;
            set => reliability = value;
        }

        float fuelconsumption
        {
            get => fuelconsumption;
            set => fuelconsumption = value;
        }
        public float engineWeight
        {
            get => engineWeight;
            set => engineWeight = value;
        }



        float CalculateSize(int cylamm, float disp, Configuration conf, EngineType t)
        {
            //example 4 cyl 2l vs 6cyl 2l
            //4cyl 2l size = (2/4 +4) * 100 = 450
            //6cyl 2l size = (2/6 +6)*100 = 633.3
            //16cyl 8l size = (8/16 +16)*100 = 1650
            float size = 0;
            if (config == Configuration.V)
            {
                size = (disp / cylamm + cylamm)* GlobalConsts.VENGINESIZEBIAS * GlobalConsts.ENGINESIZINGCONST;

            }
            else//conf is flat or inline
            {
                size = (disp / cylamm +cylamm)* GlobalConsts.ENGINESIZINGCONST;
            }
            if (type == EngineType.Supercharged || type == EngineType.Turbocharged)
            {
                size += GlobalConsts.FORCEDINDUCTIONSIZE;
            }
            return size;
        }

        float CalculateWeight(int cylamm, float disp, Configuration conf, EngineType t)
        {
            //example 4 cyl 2l vs 6cyl 2l
            //4cyl 2l weight = (4/2)* 100 = 200
            //6cyl 2l weight = 6/2 *100 = 300
            float weight = 0;
            if (config == Configuration.V)
            {
                weight = (cylinderAmmount / displacement) * GlobalConsts.VENGINEMASSBIAS * GlobalConsts.ENGINEWEIGHTCONST;

            }
            else//conf is flat or inline
            {
                weight = (cylinderAmmount / displacement) * GlobalConsts.ENGINEWEIGHTCONST;
            }
            if (type == EngineType.Supercharged)
            {
                weight += GlobalConsts.FORCEDINDUCTIONMASSSC;
            }
            else if (type == EngineType.Turbocharged)
            {
                weight += GlobalConsts.FORCEDINDUCTIONMASSTC;
            }
            return size;
        }

        float CalculateHorsePower(int cylamm, float disp, Configuration conf, EngineType t)
        {
            float hp = 0;
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

        float CalculateTorque(int cylamm, float disp, Configuration conf, EngineType t)
        {
            float torq = 0;
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

        float CalculateReliability(int cylamm, float disp, Configuration conf, EngineType t)
        {
            float reliability = 100.0f;

            if (conf == Configuration.Flat)
            {
                reliability -= GlobalConsts.FLATRELIABILITYDEBUF;
            }
            if (t == EngineType.Supercharged || t == EngineType.Turbocharged)
            {
                reliability -= GlobalConsts.FORCEDINDUCTIONDEBUF;
            }
            reliability -= cylinderAmmount;
            return reliability;
        }

        float CalculateFuelConsumption(int cylamm, float disp, Configuration conf, EngineType t)
        {
            float fuelconsumption = 0;

            fuelconsumption += Math.Min(40.0f, displacement * GlobalConsts.FUELCONSUMPTIONCONST);

            if (t == EngineType.Supercharged || t == EngineType.Turbocharged)
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
            size = CalculateSize(cylamm, disp, conf, t);
            engineWeight = CalculateWeight(cylamm, disp, conf, t);
            horsePower = CalculateHorsePower(cylamm, disp, conf, t);
            torque = CalculateTorque(cylamm, disp, conf, t);
            reliability = CalculateReliability(cylamm, disp, conf, t);
            fuelconsumption = CalculateFuelConsumption(cylamm, disp, conf, t);
        }
    }
}
