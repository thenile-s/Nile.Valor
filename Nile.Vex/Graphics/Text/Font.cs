using Newtonsoft.Json;
using Nile.Vex.Util;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Nile.Vex.Graphics.Text
{
    public abstract class Font : Disposable
    {
        public abstract Texture Texture { get; }

        public abstract Vector2 Spacing { get; }

        public abstract Glyph GetGlyph(char character);

        public virtual float SpaceWidth => Spacing.X * 2;

        public abstract float LineHeight { get; }
    }
}
