using Nile.Vex.Graphics.Text;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nile.Vex.Animation.Setters
{
    public class TextColorSetter : TextSetter<Color4>
    {
        public TextColorSetter(Text text) : base(text)
        {
        }

        public override void SetValue(Color4 value)
        {
            _text.Color = value;
        }
    }
}
