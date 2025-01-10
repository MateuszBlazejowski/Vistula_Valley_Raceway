using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVR.Locations;
using VVR.Technical;
using VVR.Vehicles; 

namespace VVR.VVR_logic
{
    internal class DrivingSystem 
    {
        private Random Random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
 
        public void Drive(int carIndex, ref int deltaPosX, ref float deltaSpeed, Track track, List<Vehicle> vehicles, int crashedIntoWall, int crashedIntoCar)
        {
            if (vehicles[carIndex].positionY < 0) return;
            if (crashedIntoWall != 0)
            {
                if (crashedIntoCar == -1)
                    deltaPosX = 1;
                else if (crashedIntoWall == 1)
                    deltaPosX = -1;
            }
            else if (crashedIntoCar != 0)
            {
                if (crashedIntoCar == -1)
                {
                    deltaPosX = -1;
                    deltaSpeed = -1;
                }
                else if (crashedIntoCar == 1)
                {
                    deltaPosX = 1;
                    deltaSpeed = -1;
                }
                else if (crashedIntoCar == 2)
                {
                    deltaSpeed = -1;

                    int random = Random.Next(0, 1);
                    if (random == 0)
                    {
                        deltaPosX = -1;
                    }
                    else
                    {
                       deltaPosX = 1;
                    }
                }
            }

            else
            {

                deltaSpeed = 1; // change to acceleration of current car
            }

            // last check 
            if (deltaSpeed > 0 && (deltaSpeed + vehicles[carIndex].speed) > GlobalConsts.MAX_SPEED)
            {
                deltaSpeed = 0;
            }
            else if (deltaSpeed < 0 && (deltaSpeed + vehicles[carIndex].speed) < GlobalConsts.MAX_SPEED)
            {
                deltaSpeed = 0;
            }

        }
    }
}
