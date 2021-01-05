using Nile.Vex.Animation.Setters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nile.Vex.Animation
{
    public abstract class Animator<T>
    {
        public Animator(Setter<T> setter, bool loop)
        {
            Setter = setter;
            Active = true;
            TimeScale = 1;
            Loop = loop;
        }

        public abstract bool Finished { get; }

        public void Update(double seconds)
        {
            if(!Finished)
            {
                UpdateInternal(seconds * TimeScale);
                Setter?.SetValue(GetCurrent());
            }
            else if(Loop)
            {
                Reset();
                if(!Finished)
                {
                    UpdateInternal(seconds * TimeScale);
                    Setter?.SetValue(GetCurrent());
                }
            }
        }

        protected abstract void UpdateInternal(double seconds);

        public abstract void Reset();

        public abstract T GetCurrent();

        public Setter<T> Setter { get; set; }

        public bool Active { get; set; }

        public float TimeScale { get; set; }

        public bool Loop { get; set; }
    }
}
