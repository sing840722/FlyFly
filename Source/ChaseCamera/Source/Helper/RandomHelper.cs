using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaseCameraSample
{
    public static class RandomHelper
    {
        private static Random rand = new Random();

        // Return a float between -1.0f and 1.0f
        public static float RandFloat()
        {
            return (float)(rand.NextDouble() - rand.NextDouble());
        }

        public static int Rand()
        {
            return rand.Next();
        }
    }
}
