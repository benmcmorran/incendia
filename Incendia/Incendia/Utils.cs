using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Incendia
{
    static class Utils
    {
        public static double NextDouble(this Random random, double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }

        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }

        public static float NextFloat(this Random random, float min, float max)
        {
            return (float)random.NextDouble(min, max);
        }

        public static Curve ConstantCurve(float value)
        {
            Curve curve = new Curve();
            curve.Keys.Add(new CurveKey(0, value));
            return curve;
        }

        public static Vector2 Vector2FromSpeedAndDirection(float speed, float direction)
        {
            return new Vector2(speed * (float)Math.Cos(direction), speed * (float)Math.Sin(direction));
        }
    }
}
