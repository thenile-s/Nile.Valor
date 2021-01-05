using Nile.Vex.Graphics.Text;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nile.Vex.Animation.Setters
{
    public class TextPositionSetter : TextSetter<Vector2>
    {
        public TextPositionSetter(Text text) : base(text)
        {
        }

        public override void SetValue(Vector2 value)
        {
            _text.Position = value;
        }
    }
}
