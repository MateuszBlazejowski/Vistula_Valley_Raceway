using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVR.Vehicles.VehicleComponents
{
    enum Configuration
    {
        Inline,//+++reliability
        Flat,//+++Balance ---reliability
        V60deg,//each cylinder in this config brings up the size less than in other configs but is heavier
    }
    enum Type
    {
        NaturallyAspirated,//+reliability
        Turbocharged,//++power +weight -reliability takes up sapce
        Supercharged,//+power +torque ++weight -reliability takes up space
    }
    internal class Engine
    {
        int cylinderAmmount;//^ = +hp +torque +fuelcons +size -reliability 
        float displacement;//^ = +hp +torque +fuelcons +size
        Configuration config;
        Type type;
        //calculated based on the above parameters
        float size;//can be changed to be vertical and horizontal size
        float horsePower; //max speed
        float torque; //acceleration
        float reliability;
        float balance;// gives bonus to handling
        float fuelconsumption;
        float engineWeight;
    }
}
