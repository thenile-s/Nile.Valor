using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Nile.Vex.Graphics.Text
{
    public class MonoFont : Font
    {
        private Vector2 _spacing;

        private Texture _texture;

        private Dictionary<char, MonoGlyph> _glyphs;

        public MonoFont(string texturePath, string jsonPath)
        {
            _texture = new Texture(texturePath);
            var json = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(jsonPath));
            var chars = json.GetValue("characters").Value<string>();
            var size = json.GetValue("size");
            var width = size.Value<int>("width");
            var height = size.Value<int>("height");
            var scale = json.GetValue("glScale").Value<float>();

            var spacing = json.GetValue("spacing");

            _spacing = new Vector2(spacing.Value<int>("x") * scale, spacing.Value<int>("y") * scale);

            CharacterSize = new Vector2(width * scale, height * scale);

            _glyphs = new Dictionary<char, MonoGlyph>(chars.Length);

            int y = 0;

            int x = 0;

            for (int i = 0; i < chars.Length; i++)
            {
                if (x == _texture.Width)
                {
                    x = 0;
                    y += height;
                }

                _glyphs.Add(chars[i], new MonoGlyph(this, _texture.GetRect(x, y, width, height)));

                x += width;
            }

            //foreach (var item in offsets)
            //{
            //    offsets.em
            //}
        }

        public override Texture Texture => _texture;

        public Vector2 CharacterSize { get; private set; }

        public override Vector2 Spacing => _spacing;

        public override float LineHeight => CharacterSize.Y;

        public override Glyph GetGlyph(char character)
        {
            return _glyphs[character];
        }

        protected override void DisposeInternal()
        {
            _texture.Dispose();
        }
    }
}
