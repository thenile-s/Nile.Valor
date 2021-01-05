using Nile.Vex.Animation.Setters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nile.Vex.Animation.Linear
{
    public abstract class LinearAnimator<T> : Animator<T> where T : IEquatable<T>
    {
        protected LinearAnimator(Setter<T> setter, bool loop) : base(setter, loop)
        {
        }

        public abstract void SetCurrent(T value);

        public virtual T Rate { get; set; }

        public virtual T Start { get; set; }

        public virtual T To { get; set; }

        public override bool Finished => GetCurrent().Equals(To);

        public override void Reset()
        {
            SetCurrent(Start);
        }
    }
}
