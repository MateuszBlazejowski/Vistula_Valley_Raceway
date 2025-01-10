using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVR.Technical
{
    public static class GlobalConsts
    {
        // logic constants: 
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
        public const float DEFAULTFUELTANK = 60.0f;
        public const float FRAMEWEIGHTCONST = 1650.0f;
        public const int MAX_SPEED = 150;
        public const int MIN_SPEED = 30;

        public const string GAME_FINISHED_MESSAGE = "END";

        public const int RACE_IN_LAPS = 5;
        public const int DEFAULT_DISTANCE_BETWEEN_CARS_AT_THE_BEGGINING = 3;  
        

        // visual constants:  
        public const int MAXTRACKWIDTH = 100;
        public const int ONE_CHAR_LENGTH_IRL = 3; // value represents scaled char length in real life, ex: 3 - a char is 3m long irl
        // WARNING: TRACKFRAMELENGTH greater than the track length   !!!! IS NOT ALLOWED  !!!! and will crash the game
        // WARNING: TRACKFRAMELENGTH + OTHER DISPLAYED LINES must NOT be bigger than the console Y dimension, otherwise everything crashes
        public const int TRACKFRAMELENGTH = 20; // however the smaller the frame the shorter is printing it to the console
        public static readonly ConsoleColor[] DefaultTrackColors = { ConsoleColor.White, ConsoleColor.Red };
        public static readonly ConsoleColor[] RainbowTrackColors = { ConsoleColor.Red, ConsoleColor.DarkYellow /*kind of orange*/, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.DarkBlue, ConsoleColor.DarkMagenta }; // red orange yellow green blue darkblue purple  
        public static readonly ConsoleColor[] BeachTrackColors = {ConsoleColor.Yellow, ConsoleColor.Blue };
        public static readonly ConsoleColor[] HelloKittyTrackColors = { ConsoleColor.Magenta/*kind of pink*/, ConsoleColor.White };
        public static readonly TimeSpan frameDuration = TimeSpan.FromMilliseconds(37); 
    }
}
 