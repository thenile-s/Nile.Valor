using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nile.Vex.Graphics.Text
{
    /*
     * GJ!! Ty Jesus!!!
     */

    public class Text : QuadRenderable
    {
        private StringBuilder _stringBuilder;

        public StringBuilder StringBuilder { get { return _stringBuilder; } set {  _stringBuilder = value ?? throw new ArgumentNullException(); } }

        public string String { get { return _stringBuilder.ToString(); } set { _stringBuilder = new StringBuilder(value); } }

        private Font _font;

        public Font Font { get { return _font; } set { _font = value ?? throw new ArgumentNullException(); } }

        public Vector2 Origin;

        public Vector2 Position;

        public Vector2 Scale;

        public float Rotation;

        public Color4 Color;

        public Text(Font font, StringBuilder builder)
        {
            Font = font;
            StringBuilder = builder;
            Scale = Vector2.One;
            Color = Color4.White;
        }

        public Text(Font font) : this(font, new StringBuilder())
        {
        }

        public override void Render(QuadRenderer renderer)
        {
            if (_stringBuilder.Length == 0)
                return;

            var originPos = Position + Origin;

            Vector2 originBuffer;

            var posBuffer = Position;

            var destBuffer = new Box2();

            var lineStart = posBuffer.X;

            //pos = origin pos
            //origin = desired pos - pos

            for (int i = 0; i < _stringBuilder.Length; i++)
            {
                var character = _stringBuilder[i];

                if(character == '\n')
                {
                    posBuffer.X = lineStart;
                    posBuffer.Y -= _font.Spacing.Y;
                    continue;
                }
                if(character == ' ')
                {
                    posBuffer.X += _font.SpaceWidth;
                    continue;
                }

                var glyph = _font.GetGlyph(character);

                originBuffer = originPos - posBuffer;

                destBuffer.Min = Position;
                destBuffer.Max = destBuffer.Min + glyph.Size;

                renderer.SubmitQuad(destBuffer, glyph.Source, Color, originBuffer, Scale, Rotation, _font.Texture);

                posBuffer.X += glyph.Size.X + _font.Spacing.X;
            }
        }

        public void QuickScale(float scale)
        {
            Scale = new Vector2(scale);
        }

        public void CenterOrigin()
        {
            Origin = Measure() / 2;
        }

        public Vector2 Measure()
        {
            Vector2 buffer = new Vector2(0, _font.LineHeight);
            float maxWidth = 0;
            bool lastCharWasGlyph = false;

            for (int i = 0; i < _stringBuilder.Length; i++)
            {
                if (lastCharWasGlyph)
                {
                    buffer.X += _font.Spacing.X;
                }

                lastCharWasGlyph = false;

                var character = _stringBuilder[i];

                if(character == '\n')
                {
                    buffer.Y += _font.Spacing.Y;
                    maxWidth = Math.Max(buffer.X, maxWidth);
                    buffer.X = 0;
                }
                else if(character == ' ')
                {
                    buffer.X += _font.SpaceWidth;
                }
                else
                {
                    lastCharWasGlyph = true;
                    buffer.X += _font.GetGlyph(character).Size.X;
                }
            }

            maxWidth = Math.Max(buffer.X, maxWidth);

            return buffer;
        }

        //TODO vector 2 animnation, smooth loop animations, keyframe animations, more setters as needed, anim helpers
        //linear anims work :D

        //TODO after those, audio and post processing frag shader

        //TODO after thpse, gui?

        //TODO after thpose, game?
    }
}
