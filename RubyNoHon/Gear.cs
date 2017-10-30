using System;
namespace RubyNoHon
{
    public class Gear2
    {
        private readonly int Cog;
        private readonly int Chainring;
        private Wheel Wheel;

        private double Ratio()
        {
            return (double) Cog / (double) Chainring;
        }

        public double GearInch()
        {
            return this.Ratio() * _wheel.GetDiameter();
        }

        public Gear(int cog, int chainring, int rim, int tire)
        {
            Cog = cog;
            Chainring = chainring;
            Wheel = new Wheel(rim, tire);
        }
    }

    public class Gear3_1
    {
        private readonly int Cog;
        private readonly int Chainring;
        private readonly int Rim;
        private readonly int Tire;

        private double Ratio()
        {
            return (double)Cog / (double)Chainring;
        }

        private double GearInches()
        {
            // have to change here!
            // initializing the other class in class is not good.
            Wheel2 wheel = new Wheel2(this.Rim, this.Tire);
            return Ratio() * wheel.GetDiameter();
        }

        public Gear3_1(int cog, int chainring, int rim, int tire)
        {
            this.Cog = cog;
            this.Chainring = chainring;
            this.Rim = rim;
            this.Tire = tire;
        }
    }

    public class Gear3_2
    {
        private readonly int Cog;
        private readonly int Chainring;
        private readonly Wheel2 Wheel;

        private double Ratio()
        {
            return (double)Cog / (double)Chainring;
        }

        private double GearInch()
        {
            return Ratio() * this.Wheel.GetDiameter();
        }

        public Gear3_2(int cog, int chainring, Wheel2 wheel)
        {
            this.Cog = cog;
            this.Chainring = chainring;
            // put instantiating other class out of the class.
            // no rim and tire in this class anymore.
            this.Wheel = wheel;
        }
    }
}
