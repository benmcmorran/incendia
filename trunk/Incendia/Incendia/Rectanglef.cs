using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incendia
{
    struct Rectanglef
    {
        //In tiles
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public Rectanglef(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public void Multiply(float value)
        {
            X *= value;
            Y *= value;
            Width *= value;
            Height *= value;
        }
    }
}
