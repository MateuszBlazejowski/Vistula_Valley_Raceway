using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVR.Vehicles
{
    internal abstract class Vehicles
    {
        protected Vehicles(float hp, float maxFuel, float mass, float torque, float currentFuel = 0)
        {
            this.hp = hp;
            this.maxFuel = maxFuel;
            this.mass = mass;
            this.torque = torque;
            this.currentFuel = currentFuel;
        }

        public float hp { get; }
        public float maxFuel { get; }
        public float mass { get; }
        public float torque { get; }
        public float currentFuel { get; set; }
    }
}
