using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Incendia
{
    interface IParticleManipulator
    {
        void ManipulateParticle(Particle particle);
    }
}
