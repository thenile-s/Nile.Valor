using Nile.Vex.Graphics.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nile.Vex.Animation.Setters
{
    public abstract class TextSetter<T> : Setter<T>
    {
        protected Text _text;

        public TextSetter(Text text)
        {
            _text = text;
        }
    }
}
