using System;
using System.Collections.Generic;
using System.Text;

namespace Nile.Vex.Animation.Keyframed
{
    public class KeyframeAnimation<T>
    {
        public KeyframeAnimation()
        {
            _keyframes = new List<Keyframe<T>>();
        }

        public KeyframeAnimation(int capacity)
        {
            _keyframes = new List<Keyframe<T>>(capacity);
        }

        public KeyframeAnimation(Keyframe<T>[] keyframes)
        {
            _keyframes = new List<Keyframe<T>>(keyframes);
        }

        private List<Keyframe<T>> _keyframes;

        public bool Locked { get; private set; }

        public int Count => _keyframes.Count;

        public Keyframe<T> this[int index]
        {
            get
            {
                return _keyframes[index];
            }
        }

        public KeyframeAnimation<T> AddFrame(Keyframe<T> keyframe)
        {
            if (Locked)
                throw new InvalidOperationException();
            _keyframes.Add(keyframe ?? throw new ArgumentNullException());
            return this;
        }

        public KeyframeAnimation<T> Lock()
        {
            if(!Locked)
            {
                Locked = true;
                _keyframes.TrimExcess();
            }
            return this;
        }
    }
}
