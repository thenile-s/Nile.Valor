using System;
using System.Collections.Generic;
using System.Text;

namespace Nile.Vex.Animation.Keyframed
{
    public class Keyframe<T>
    {
        public Keyframe(T value, double duration)
        {
            Value = value;
            Duration = duration;
        }

        public double Duration { get; protected set; }

        public T Value { get; protected set; }
    }
}
