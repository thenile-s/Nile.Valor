using Nile.Vex.Animation.Setters;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nile.Vex.Animation.Linear
{
    public class LinearVector2Animator : LinearAnimator<Vector2>
    {
        private Vector2 _current;

        private Vector2 _rate;

        private Vector2 _to;

        private Vector2 _start;

        public override Vector2 Rate
        {
            get
            {
                return _rate;
            }
            set
            {
                _rate = value;
                _xAnimator.Rate = value.X;
                _yAnimator.Rate = value.Y;
            }
        }

        public override Vector2 To
        {
            get
            {
                return _to;
            }
            set
            {
                _to = value;
                _xAnimator.To = value.X;
                _yAnimator.To = value.Y;
            }
        }

        public override Vector2 Start
        {
            get
            {
                return _start;
            }
            set
            {
                _start = value;
                _xAnimator.Start = value.X;
                _yAnimator.Start = value.Y;
            }
        }

        private LinearFloatAnimator _xAnimator;

        private LinearFloatAnimator _yAnimator;

        public LinearVector2Animator(Setter<Vector2> setter, bool loop, Vector2 start, Vector2 to, Vector2 rate) : base(setter, loop)
        {
            _xAnimator = new LinearFloatAnimator(null, loop, start.X, to.X, rate.X);
            _yAnimator = new LinearFloatAnimator(null, loop, start.Y, to.Y, rate.Y);
            _start = start;
            _to = to;
            _rate = rate;
        }

        public override Vector2 GetCurrent()
        {
            return _current;
        }

        public override void SetCurrent(Vector2 value)
        {
            _current = value;
            _xAnimator.SetCurrent(value.X);
            _yAnimator.SetCurrent(value.Y);
        }

        protected override void UpdateInternal(double seconds)
        {
            _xAnimator.Update(seconds);
            _yAnimator.Update(seconds);

            _current.X = _xAnimator.GetCurrent();
            _current.Y = _yAnimator.GetCurrent();
        }
    }
}
