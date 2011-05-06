using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Incendia
{
    /// <summary>
    /// Holds and animates a set of frames.
    /// </summary>
    public class Animation
    {
        List<Frame> _frames;
        double _frameTime;
        DateTime _birthTime;

        public Animation(double frameTime, List<Frame> frames)
        {
            _frames = new List<Frame>();
            _birthTime = DateTime.UtcNow;
            _frameTime = frameTime;
            _frames = frames;
        }

        public Frame GetFrame()
        {
            return _frames[(int)((DateTime.UtcNow - _birthTime).TotalSeconds / _frameTime) % _frames.Count];
        }
    }
}
