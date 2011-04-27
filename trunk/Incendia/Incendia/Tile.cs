using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incendia
{
    public class Tile
    {
        public float Flammability { get; private set; }
        public int Material { get; private set; }
        public FireState State { get; set; }
        public string _texture;
        public bool _solid;
        public bool Solid { get { return _solid && State != FireState.Burnt; } }
        public bool Outside {get; set;}
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

        public Tile(float flammability, int material, FireState state, string texture, bool solid, bool outside)
        {
            Flammability = flammability;
            Material = material;
            State = state;
            _texture = texture;
            _solid = solid;
        }

        public void UpdateBurning(int material, FireState state)
        {
            Material = material;
            State = state;
        }

        /// <summary>
        /// Fights fire and returns whether there was fire to fight
        /// </summary>
        /// <returns></returns>
        public bool HitByWater()
        {
            if (State == FireState.Burning && Global.rand.Next(0,1001) >= 1000)
            {
                State = FireState.Unburned;
                return true;
            }
            Flammability /= 1.001f;
            return false;

        }
    }
}
