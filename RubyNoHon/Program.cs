using System;

namespace RubyNoHon
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Gear gear = new Gear(52, 23, 32, 23);
            double inch = gear.GearInch();
            Console.WriteLine(inch);
        }
    }

    class Gear
    {
        private readonly int _cog;
        private readonly int _chainring;
        private Wheel _wheel;

        private double Ratio()
        {
            return (double)_cog / (double)_chainring;
        }

        public double GearInch()
        {
            return this.Ratio() * _wheel.GetDiameter();
        }

        public Gear(int cog, int chainring, int rim, int tire)
        {
            _cog = cog;
            _chainring = chainring;
            _wheel = new Wheel(rim, tire);
        }
    }

    class Wheel
    {
        private readonly int _rim;
        private readonly int _tire;

        public double GetDiameter()
        {
            return (double) _rim + 2 * _tire;
        }

        public double GetCircumference()
        {
            return (double) _rim * Math.PI;
        }

        public Wheel(int rim, int tire)
        {
            _rim = rim;
            _tire = tire;
        }
    }
}
