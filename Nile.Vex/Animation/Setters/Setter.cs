using System;
using System.Collections.Generic;
using System.Text;

namespace Nile.Vex.Animation.Setters
{
    public abstract class Setter<T>
    {
        public abstract void SetValue(T value);
    }
}
