using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incendia
{
    class Tile
    {
        public float Flammability { get; private set; }
        public int Material { get; private set; }
        public FireState State { get; private set; }
        public string _texture;
        public bool _solid;
        public bool Solid { get { return _solid || State == FireState.Burnt; } }
        public string Texture
        {
            get
            {
                if (State == FireState.Burnt)
                    return _texture + "b";
                else
                    return _texture;
            }
        }

        public Tile(float flammability, int material, FireState state, string texture, bool solid)
        {
            Flammability = flammability;
            Material = material;
            State = state;
            _texture = texture;
            _solid = solid;
        }

        public void UpdateBurning(float flammability, int material, FireState state)
        {
            Flammability = flammability;
            Material = material;
            State = state;
        }
    }
}
