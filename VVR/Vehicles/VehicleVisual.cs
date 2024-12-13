using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VVR.Technical;

namespace VVR.Vehicles
{
    internal class VehicleVisual
    {
        public bool isHuman;
        public ConsoleColor bodyColor;
        public ConsoleColor roofTopColor;
        public int positionX;
        public float positionY;
       // public int? positionOnFrameX; on frame needed because being on frame depends only on posY
        public int? positionOnFrameY;
        public float speed;

        static int botIndex = 0;
        public VehicleVisual(Vehicle car)//ConsoleColor _bodyColor, ConsoleColor _roofTopColor, bool _isHuman
        {
            
            bodyColor = car.bodyColor; 
            roofTopColor = car.roofTopColor; 
            isHuman = car.isHuman; 
            if (isHuman)
            {
                positionX = 25;
                positionOnFrameY = GlobalConsts.TRACKFRAMELENGTH / 2;   // later will depend on logic, specifically on starting position and if it is 
            }
            else
            {
                if (botIndex == 0)
                {
                    positionX = 30;
                    positionOnFrameY = 10;
                    botIndex++;
                }
                else {
                    positionX = 15;
                    positionOnFrameY = GlobalConsts.TRACKFRAMELENGTH;
                }
            }
        }
    }
}
