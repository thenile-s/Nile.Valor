using System;
using System.Collections.Generic;
using System.Text;

namespace Nile.Vex.Animation.Setters
{
    public class Wrapper<T> where T : struct
    {
        public Wrapper(T value)
        {
            Value = value;
        }

        public T Value;
    }
}
