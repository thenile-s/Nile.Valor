using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nile.Vex.Graphics.Text
{
    public abstract class Glyph
    {
        public abstract Box2 Source { get; }

        public abstract Vector2 Size { get; }
    }
}
