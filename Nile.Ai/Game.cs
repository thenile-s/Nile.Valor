using Nile.Vex.Animation.Linear;
using Nile.Vex.Animation.Setters;
using Nile.Vex.Graphics;
using Nile.Vex.Graphics.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nile.Ai
{
    class Game : GameWindow
    {
        private QuadShader spriteShader;

        private QuadRenderer quadRenderer;

        private Texture texture;

        private Sprite vex;

        private Font nilep;

        private Text helloText;

        private LinearColorAnimator colorAnimator;

        private LinearVector2Animator posAnimator;

        public Game(
            GameWindowSettings gameWindowSettings,
            NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            Title = "Nile -> Ai";
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            var shaderDir = "resources/shader/";

            spriteShader = new QuadShader(shaderDir + "sprite.vert", shaderDir + "sprite.geo", shaderDir + "sprite.frag");

            quadRenderer = new QuadRenderer(4, spriteShader);

            //texture = new Texture(@"C:\Users\bobub\Downloads\garo.jpg");

            texture = new Texture("resources/image/vex.png");

            vex = new Sprite(texture: texture);
            //vex.Color = Color4.White;

            vex.Pos = new Vector2(0f, 0f);
            //vex.Size = new Vector2(2f, 2f);
            //vex.Origin = new Vector2(-1, 1f);
            vex.CenterOrigin();
            //vex.Origin = new Vector2(1f, -1f);
            vex.QuickScale(.5f);

            //vex.Rotation = 45;

            nilep = new MonoFont("resources/font/nilep.png", "resources/font/nilep.json");

            helloText = new Text(nilep);
            helloText.Color = Color4.MediumPurple;
            helloText.String = "HEQ";
            helloText.Position = new Vector2(.5f);
            //helloText.QuickScale(1.5f);
            helloText.CenterOrigin();
            //helloText.Origin.Y = 0;
            //helloText.Origin = new Vector2(.6f, 0.1125f);
            //helloText.Rotation = -90;

            colorAnimator = new LinearColorAnimator(new TextColorSetter(helloText), true, Color4.White, new Color4(.5f, .5f, .5f, .5f), Color4.Black);
            posAnimator = new LinearVector2Animator(new TextPositionSetter(helloText), true, new Vector2(-1, -1), new Vector2(1f, 1), new Vector2(.2f));
            colorAnimator.Active = false;
            //posAnimator.Active = false;
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            spriteShader.Dispose();
            quadRenderer.Dispose();
            texture.Dispose();
            nilep.Dispose();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (colorAnimator.Active)
                colorAnimator.Update(args.Time);
            if (posAnimator.Active)
                posAnimator.Update(args.Time);
            ;
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            quadRenderer.Reset();

            //vex.Render(quadRenderer);

            helloText.Render(quadRenderer);

            quadRenderer.Render();

            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }
    }
}
