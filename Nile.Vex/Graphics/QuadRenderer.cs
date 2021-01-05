using System;
using System.Collections.Generic;
using System.Text;
using Nile.Vex.Util;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Nile.Vex.Graphics
{
    /*
     * After a lot of trial, error and effort,
     * I reached this solution which I like.
     * 
     * It is very similar to what I did with the Monogame SpriteBatch.
     * 
     * :D - The Nile
     */

    /// <summary>
    /// <para>A QuadRenderer is used to efficiently render quads with position, color, texture and scale attributes. Inspired form the Monogame SpriteBatch, but perhaps with a better shader and performance...</para>
    /// </summary>
    public class QuadRenderer : Disposable
    {
        /// <summary>
        /// <para>Amount of floating point numbers used to represent each quad.</para>
        /// <list type="bullet">
        /// <item>+4 floats for the destination rectangle</item>
        /// <item>+4 floats for the texture source rectangle</item>
        /// <item>+4 floats for the color tint of the quad</item>
        /// <item>+2 floats for the origin of the quad</item>
        /// <item>+2 floats for the scale of the quad</item>
        /// <item>+1 float for the rotation around the z axis</item>
        /// </list>
        /// </summary>
        public const int FLOATS_PER_QUAD = 17;

        /// <summary>
        /// The handle to the buffer containing the vertex data.
        /// </summary>
        private readonly int _vertexBufferHandle;

        /// <summary>
        /// The handle to the vertex array containing the necessary configuration for rendering.
        /// </summary>
        private readonly int _vertexArrayHandle;

        /// <summary>
        /// The array which stores the vertex data.
        /// </summary>
        private readonly float[] _vertices;

        /// <summary>
        /// Used to specify an offset of zero in certain OpenTK functions.
        /// </summary>
        private readonly IntPtr _zeroPtr = (IntPtr)0;

        /// <summary>
        /// The shader used by the renderer.
        /// </summary>
        private QuadShader _shader;

        /// <summary>
        /// The maximum amount of quads that may be submitted for rendering at once, before resetting.
        /// </summary>
        public int MaxQuadCount { get; }

        /// <summary>
        /// The current count of quads submitted for rendering. 
        /// </summary>
        public int QuadCount { get; private set; }

        /// <summary>
        /// <para>The index in the <see cref="_vertices"/> array at which the quads currently being submitted should be appended at.</para>
        /// <para>Care! This uses the <see cref="QuadCount"/> property to calculate this property. Therefore, QuadCount should only be incremented after submitting quads, not only in case submitting the quads throws an exception, but also due to the fact that by incrementing QuadCount before submitting quads, the quads will be appended to the <see cref="_vertices"/> array at the wrong offset; an offset equal to <see cref="FLOATS_PER_QUAD"/> multiplied by the amount of quads submitted plus the intended offset. Therefore, the submitted quads will not be rendered and submissions in the renderer will be wasted.</para>
        /// </summary>
        private int _newQuadIndex => FLOATS_PER_QUAD * QuadCount;

        /// <summary>
        /// The amount of quads that can be submitted before rendering and reseting is required.
        /// </summary>
        public int QuadsLeft => MaxQuadCount - QuadCount;

        /// <summary>
        /// backing store for <see cref="CurrentTexture"/> property
        /// </summary>
        private Texture _currentTexture;

        /// <summary>
        /// The texture which the renderer will use when rendering. Must not be null. Setting this value manually can render previously submitted quads with a texture other than the one specified in <see cref="SubmitQuad(Box2, Box2, Color4, Vector2, Vector2, float, Texture)"/>.
        /// </summary>
        public Texture CurrentTexture 
        {
            get
            {
                return _currentTexture;
            }
            set
            {
                _currentTexture = value ?? throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// Keeps track of whether the uniform transform for all quads has changed and needs to be submitted to the shader program.
        /// </summary>
        private bool _transformChanged;

        /// <summary>
        /// Stores the transform of all quads.
        /// </summary>
        private Matrix4 _transform;

        /// <summary>
        /// Used to change and get the tranform of all quads.
        /// </summary>
        public Matrix4 Transform
        {
            get { return _transform; }
            set { _transform = value; _transformChanged = true; }
        }

        /// <summary>
        /// Constructs a QuadRenderer with the specified max quad count and shader
        /// </summary>
        /// <param name="maxQuadCount">The maximum amount of quads that may be submitted for rendering at once.</param>
        /// <param name="shader">The shader the renderer will use.</param>
        public QuadRenderer(int maxQuadCount, QuadShader shader)
        {
            //Check that the arguments are valid

            _shader = shader ?? throw new ArgumentNullException(nameof(shader));

            MaxQuadCount = 
                maxQuadCount < 1 || maxQuadCount == int.MaxValue
                ? throw new ArgumentOutOfRangeException(nameof(maxQuadCount)) 
                : maxQuadCount;

            //Generate OpenGL buffers

            _vertexBufferHandle = GL.GenBuffer();

            _vertexArrayHandle = GL.GenVertexArray();

            //Create the array to store the vertices client side

            _vertices = new float[MaxQuadCount * FLOATS_PER_QUAD];

            int sf = sizeof(float);

            //Configure the vertex array and set the size of the buffer

            GL.BindVertexArray(_vertexArrayHandle);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferHandle);

            GL.BufferData(BufferTarget.ArrayBuffer, sf * _vertices.Length, _vertices, BufferUsageHint.StreamDraw);

            int stride = FLOATS_PER_QUAD * sf;

            GL.VertexAttribPointer(_shader.PosLocation, 4, VertexAttribPointerType.Float, false, stride, 0);

            GL.VertexAttribPointer(_shader.TexLocation, 4, VertexAttribPointerType.Float, false, stride, 4 * sf );

            GL.VertexAttribPointer(_shader.ColorLocation, 4, VertexAttribPointerType.Float, false, stride,  8 * sf);

            GL.VertexAttribPointer(_shader.OriginLocation, 2, VertexAttribPointerType.Float, false, stride, sf * 12);

            GL.VertexAttribPointer(_shader.ScaleLocation, 2, VertexAttribPointerType.Float, false, stride, sf * 14);

            GL.VertexAttribPointer(_shader.RotLocation, 1, VertexAttribPointerType.Float, false, stride, sf * 16);

            GL.EnableVertexAttribArray(_shader.PosLocation);

            GL.EnableVertexAttribArray(_shader.TexLocation);

            GL.EnableVertexAttribArray(_shader.ColorLocation);

            GL.EnableVertexAttribArray(_shader.OriginLocation);

            GL.EnableVertexAttribArray(_shader.ScaleLocation);

            GL.EnableVertexAttribArray(_shader.RotLocation);

            //Give the transform the identity value initially

            Transform = Matrix4.Identity;
        }

        /// <summary>
        /// Resets the renderer by setting the <see cref="QuadCount"/> to 0. This means that 0 quads will be rendered if <see cref="Render"/> would be called right after. <see cref="QuadsLeft"/> will be equal to <see cref="MaxQuadCount"/>.
        /// </summary>
        public void Reset()
        {
            QuadCount = 0;
        }

        /// <summary>
        /// Renders the submitted quads. This may be called multiple times without calling reset in order to render the previously submitted quads multiple times.
        /// </summary>
        public void Render()
        {
            //Make sure the opengl buffers are valid and we are not disposed
            ThrowIfDisposed();

            //Don't bother wasting time if noithing was submitted.
            if (QuadCount == 0)
                return;

            //Update the uniforms if they changed
            UpdateUniforms();

            //Make sure we use our shader and texture
            UseShaderAndTexture();
            
            //bind the vertex array
            GL.BindVertexArray(_vertexArrayHandle);

            //send the quad data to the vertex buffer
            GL.BufferSubData(BufferTarget.ArrayBuffer, _zeroPtr, sizeof(float) * FLOATS_PER_QUAD * QuadCount, _vertices);

            //render quads
            GL.DrawArrays(PrimitiveType.Points, 0, QuadCount);

            //unbind the vertex array
            GL.BindVertexArray(0);
        }

        /// <summary>
        /// <para>Submits a quad to be rendered. If <see cref="MaxQuadCount"/> == <see cref="QuadCount"/>, <see cref="RenderAndReset"/> is called too.</para>
        /// </summary>
        /// <param name="dest">The destination of the quad in opengl screen coordinates</param>
        /// <param name="source">The texture source of the quad in opengl texture coordinates</param>
        /// <param name="color">A color multiplier for the quad</param>
        /// <param name="origin">The origin of transformations for the quad. See the geometry shader for its significance.</param>
        /// <param name="scale">The scale transform of the quad.</param>
        /// <param name="rotation">The rotation transform of the quad.</param>
        /// <param name="texture">The texture to render the submitted quads with.</param>
        public void SubmitQuad(Box2 dest, Box2 source, Color4 color, Vector2 origin, Vector2 scale, float rotation, Texture texture)
        {
            //we cannot render if we are disposed, so no point in submitting
            ThrowIfDisposed();

            //make sure we are using the correct texture for this quad
            ChangeTexture(texture);

            if (QuadCount == MaxQuadCount)
            {
                //If there is no more room, render and reset
                RenderAndReset();
            }

            //TODO maybe use pointers to set quad data in vertex array, to skip array overhead like monogame?

            //if all is well, submit the quad
            _vertices[_newQuadIndex] = dest.Min.X;
            _vertices[_newQuadIndex + 1] = dest.Min.Y; 
            _vertices[_newQuadIndex + 2] = dest.Size.X; 
            _vertices[_newQuadIndex + 3] = dest.Size.Y;
            _vertices[_newQuadIndex + 4] = source.Min.X;
            _vertices[_newQuadIndex + 5] = source.Min.Y;
            _vertices[_newQuadIndex + 6] = source.Size.X;
            _vertices[_newQuadIndex + 7] = source.Size.Y;
            _vertices[_newQuadIndex + 8] = color.R;
            _vertices[_newQuadIndex + 9] = color.G;
            _vertices[_newQuadIndex + 10] = color.B;
            _vertices[_newQuadIndex + 11] = color.A;
            _vertices[_newQuadIndex + 12] = origin.X;
            _vertices[_newQuadIndex + 13] = origin.Y;
            _vertices[_newQuadIndex + 14] = scale.X;
            _vertices[_newQuadIndex + 15] = scale.Y;
            _vertices[_newQuadIndex + 16] = rotation;

            //don't forget to increment the count
            QuadCount++;
        }

        /// <summary>
        /// Calls <see cref="Render"/> and <see cref="Reset"/> one after another.
        /// </summary>
        public void RenderAndReset()
        {
            Render();
            Reset();
        }

        /// <summary>
        /// Changes the texture that will be used when rendering. If quads are already submitted with a different texture, <see cref="RenderAndReset"/> is called.
        /// </summary>
        /// <param name="texture">The new texture to use. Must not be null.</param>
        public void ChangeTexture(Texture texture)
        {
            if (texture == null)
                throw new ArgumentNullException();
            if(texture != CurrentTexture)
            {
                RenderAndReset();
                //skip null check in CurrentTexture set by writing to the backing store directly.
                _currentTexture = texture;
            }
        }

        /// <summary>
        /// Updates any uniforms to the shader program if they have changed. For example: <see cref="Transform"/>
        /// </summary>
        public void UpdateUniforms()
        {
            if(_transformChanged)
            {
                _shader.SetTransform(Transform);
                _transformChanged = false;
            }
        }

        /// <summary>
        /// Makes sure that the texture and shader program used by this renderer are active and bound.
        /// </summary>
        private void UseShaderAndTexture()
        {
            _shader.Use();
            CurrentTexture.Use();
        }

        /// <summary>
        /// Deletes the vertex buffer and the vertex array.
        /// </summary>
        protected override void DisposeInternal()
        {
            //delete opengl buffers
            GL.DeleteVertexArray(_vertexArrayHandle);
            GL.DeleteBuffer(_vertexBufferHandle);

            //free certain fields?.
            _shader = null;
            CurrentTexture = null;
        }
    }
}
