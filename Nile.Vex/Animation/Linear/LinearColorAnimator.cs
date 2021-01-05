using Nile.Vex.Animation.Setters;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nile.Vex.Animation.Linear
{
    public class LinearColorAnimator : LinearAnimator<Color4>
    {
        private Color4 _current;

        private Color4 _rate;

        private Color4 _to;

        private Color4 _start;

        public override Color4 Rate
        {
            get
            {
                return _rate;
            }
            set
            {
                _rate = value;
                _redAnimator.Rate = value.R;
                _greenAnimator.Rate = value.G;
                _blueAnimator.Rate = value.B;
                _alphaAnimator.Rate = value.A;
            }
        }

        public override Color4 Start
        {
            get
            {
                return _start;
            }
            set
            {
                _start = value;
                _redAnimator.Start = value.R;
                _greenAnimator.Start = value.G;
                _blueAnimator.Start = value.B;
                _alphaAnimator.Start = value.A;
            }
        }

        public override Color4 To
        {
            get
            {
                return _to;
            }
            set
            {
                _to = value;
                _redAnimator.To = value.R;
                _greenAnimator.To = value.G;
                _blueAnimator.To = value.B;
                _alphaAnimator.To = value.A;
            }
        }

        private LinearFloatAnimator _redAnimator;

        private LinearFloatAnimator _greenAnimator;

        private LinearFloatAnimator _blueAnimator;

        private LinearFloatAnimator _alphaAnimator;

        public LinearColorAnimator(Setter<Color4> setter, bool loop, Color4 start, Color4 rate, Color4 to) : base(setter, loop)
        {
            _redAnimator = new LinearFloatAnimator(null, loop, start.R, to.R, rate.R);
            _greenAnimator = new LinearFloatAnimator(null, loop, start.G, to.G, rate.G);
            _blueAnimator = new LinearFloatAnimator(null, loop, start.B, to.B, rate.B);
            _alphaAnimator = new LinearFloatAnimator(null, loop, start.A, to.A, rate.A);
            _start = start;
            _rate = rate;
            _to = to;
            _current = start;
        }

        public override Color4 GetCurrent()
        {
            return _current;
        }

        public override void SetCurrent(Color4 value)
        {
            _current = value;
            _redAnimator.SetCurrent(value.R);
            _greenAnimator.SetCurrent(value.G);
            _blueAnimator.SetCurrent(value.B);
            _alphaAnimator.SetCurrent(value.A);
        }

        protected override void UpdateInternal(double time)
        {
            _redAnimator.Update(time);
            _greenAnimator.Update(time);
            _blueAnimator.Update(time);
            _alphaAnimator.Update(time);

            _current.R = _redAnimator.GetCurrent();
            _current.G = _greenAnimator.GetCurrent();
            _current.B = _blueAnimator.GetCurrent();
            _current.A = _alphaAnimator.GetCurrent();
        }
    }
}
