using Nile.Vex.Animation.Setters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nile.Vex.Animation.Keyframed
{
    public class KeyframeAnimator<T> : Animator<T>
    {
        private KeyframeAnimation<T> _currentAnimation;

        private int _currentFrameIndex;

        private double _frameTime;

        public KeyframeAnimation<T> _defaultAnimation;

        public KeyframeAnimator(Setter<T> setter, bool loop, KeyframeAnimation<T> defaultAnimation) : base(setter, loop)
        {
            _defaultAnimation = defaultAnimation;
            _currentAnimation = defaultAnimation;
        }

        public override bool Finished => 
            _currentAnimation == null ||
            (_currentFrameIndex == _currentAnimation.Count &&
            _currentAnimation[_currentFrameIndex].Duration <= _frameTime);

        public override T GetCurrent()
        {
            return _currentAnimation[_currentFrameIndex].Value;
        }

        public override void Reset()
        {
            _currentAnimation = _defaultAnimation;
            _frameTime = 0;
            _currentFrameIndex = 0;
        }

        protected override void UpdateInternal(double seconds)
        {
            _frameTime += seconds;

            if(_frameTime >= _currentAnimation[_currentFrameIndex].Duration)
            {
                _frameTime -= _currentAnimation[_currentFrameIndex].Duration;
                if (_currentFrameIndex < _currentAnimation.Count - 1)
                {
                    _currentFrameIndex++;
                }
            }
        }

        public void Play(KeyframeAnimation<T> animation, bool setDefault)
        {
            _currentAnimation = animation;
            _frameTime = 0;
            _currentFrameIndex = 0;
            if(setDefault)
            {
                _defaultAnimation = animation;
            }
        }
    }
}
