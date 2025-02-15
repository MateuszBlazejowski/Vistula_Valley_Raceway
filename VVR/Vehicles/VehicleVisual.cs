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
        public double positionY;
        public int? positionOnFrameY; // on frame needed because being on frame depends only on posY
        public double speed;
        public VehicleVisual(Vehicle car)//ConsoleColor _bodyColor, ConsoleColor _roofTopColor, bool _isHuman
        {
            bodyColor = car.bodyColor;
            roofTopColor = car.roofTopColor;
            isHuman = car.isHuman;
            positionX = car.positionX;
            positionY = car.positionY;
            positionOnFrameY = null;
            speed = car.speed;
        }

    }
}
