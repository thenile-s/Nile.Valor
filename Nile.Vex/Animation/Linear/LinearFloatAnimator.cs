using Nile.Vex.Animation.Setters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Nile.Vex.Animation.Linear
{
    public class LinearFloatAnimator : LinearAnimator<float>
    {
        //public const float ERROR_MARGIN = .05f;

        private float _current;

        //public override bool Finished => _current - ERROR_MARGIN <= To && _current + ERROR_MARGIN >= To;

        public bool Increasing => _current < To;

        public LinearFloatAnimator(Setter<float> setter, bool loop, float start, float to, float rate) : base(setter, loop)
        {
            Start = start;
            Rate = rate;
            To = to;
            Reset();
        }

        public override float GetCurrent() => _current;

        public override void SetCurrent(float value) => _current = value;

        protected override void UpdateInternal(double seconds)
        {
            var amount = (float)Math.Abs(seconds * Rate);
            if(Increasing)
            {
                SetCurrent(Math.Min(To, _current + amount));
            }
            else
            {
                SetCurrent(Math.Max(To, _current - amount));
            }
        }
    }
}
