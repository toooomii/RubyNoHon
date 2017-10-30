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
}
