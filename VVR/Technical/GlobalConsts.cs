using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVR.Technical
{
    public static class GlobalConsts
    {
        public const float VENGINEMASSBIAS = 1.2f;
        public const float VENGINESIZEBIAS = 0.8f;
        public const float ENGINESIZINGCONST = 0.1f;
        public const float ENGINEWEIGHTCONST = 35.0f;
        public const float FORCEDINDUCTIONMASSTC = 10.0f;
        public const float FORCEDINDUCTIONMASSSC = 20.0f;
        public const float FORCEDINDUCTIONSIZE = 0.01f;
        public const float HORSEPOWERMULTIPLIER = 15.0f;
        public const float TORQMULTIPLIER = 125.0f;
        public const float TURBOBONUS = 100.0f;
        public const float TURBOBONUSTORQ = 70.0f;
        public const float SUPERCHARGERBONUS= 70.0f;
        public const float SUPERCHARGERBONUSTORQ= 100.0f;
        public const float FLATRELIABILITYDEBUF= 10.0f;
        public const float FORCEDINDUCTIONDEBUF= 10.0f;
        public const float FUELCONSUMPTIONCONST= 4.0f;
        public const float FORCEDINDUCTIONFUELCONSUMPTION= 1.0f;
        public const float FORCEDINDUCTIONMASS = 20.0f;
        public const int MAXTRACKWIDTH = 100;
        public const int TRACKFRAMELENGTH = 15;
        public const float DEFAULTFUELTANK = 60.0f;
        public const float FRAMEWEIGHTCONST = 0.25f;
    }
}
