using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nile.Vex.Graphics
{
    public class Sprite : QuadRenderable
    {
        public override void Render(QuadRenderer renderer)
        {
            renderer.SubmitQuad(Dest, Source, Color, Origin, Scale, Rotation, _texture);
        }

        /// <summary>
        /// Stores the texture used by the sprite.
        /// </summary>
        private Texture _texture;

        public Texture Texture
        {
            get
            {
                return _texture;
            }
            set
            {
                _texture = value ?? throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// Centers the origin based on the size.
        /// </summary>
        public void CenterOrigin()
        {
            Origin = Dest.HalfSize;
        }

        /// <summary>
        /// The Bottom left corner of the destination rectangle.
        /// </summary>
        public Vector2 Pos 
        {
            get
            {
                return Dest.Min;
            }
            set
            {
                Dest.Translate(value - Dest.Min);
            }
        }

        /// <summary>
        /// The size of the destination rectangle.
        /// </summary>
        public Vector2 Size
        {
            get
            {
                return Dest.Size;
            }
            set
            {
                Dest = new Box2(Dest.Min, Dest.Min + value);
            }
        }

        private Box2 Dest;

        private Box2 Source;

        public Vector2 Origin;

        public Vector2 Scale;

        /// <summary>
        /// Easily sets the scale with a float to both the x and y values
        /// </summary>
        /// <param name="scale">the scale</param>
        public void QuickScale(float scale)
        {
            Scale.X = scale;
            Scale.Y = scale;
        }

        /// <summary>
        /// The color.
        /// </summary>
        public Color4 Color;

        /// <summary>
        /// Specifies the angle of rotation for the quad
        /// </summary>
        public float Rotation;

        public Sprite(
            Box2? dest = null,
            Box2? source = null,
            Vector2? origin = null,
            Vector2? scale = null,
            float rotation = 0,
            Color4? color = null,
            Texture texture = null)
        {
            _texture = texture ?? throw new ArgumentNullException(nameof(texture));
            Dest = dest ?? new Box2(-1, -1, 1, 1);
            Source = source ?? new Box2(0, 0, 1, 1);
            Origin = origin ?? Vector2.Zero;
            Scale = scale ?? Vector2.One;
            Rotation = rotation;
            Color = color ?? Color4.White;
        }
    }
}
