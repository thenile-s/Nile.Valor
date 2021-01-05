using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nile.Vex.Graphics.Text
{
    public class MonoGlyph : Glyph
    {


        public MonoGlyph(MonoFont font, Box2 source)
        {
            _font = font ?? throw new ArgumentNullException();
            _source = source;
        }

        private MonoFont _font;

        private Box2 _source;

        public override Box2 Source => _source;

        public override Vector2 Size => _font.CharacterSize;
    }
}
