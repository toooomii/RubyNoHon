using System;
namespace RubyNoHon
{
    public class Wheel2
    {
        private readonly int Rim;
        private readonly int Tire;

        public double GetDiameter()
        {
            return (double) Rim + 2 * Tire;
        }

        public double GetCircumference()
        {
            return (double) Rim * Math.PI;
        }

        public Wheel2(int rim, int tire)
        {
            Rim = rim;
            Tire = tire;
        }
    }
}
