using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Nile.Vex.Util;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Nile.Vex.Graphics
{
    /// <summary>
    /// Represents a valid shader that can be used by a <see cref="QuadRenderer"/>/
    /// </summary>
    public class QuadShader : Disposable
    {
        /// <summary>
        /// Handle to the opengl shader program.
        /// </summary>
        private readonly int _shaderProgramHandle;

        /// <summary>
        /// Location of the position vertex attribute
        /// </summary>
        public readonly int PosLocation;

        /// <summary>
        /// Location of the texture vertex attribute
        /// </summary>
        public readonly int TexLocation;

        /// <summary>
        /// Location of the color vertex attribute
        /// </summary>
        public readonly int ColorLocation;

        /// <summary>
        /// Location of the origin vertex attribute
        /// </summary>
        public readonly int OriginLocation;

        /// <summary>
        /// Location of the scale vertex attribute
        /// </summary>
        public readonly int ScaleLocation;

        /// <summary>
        /// Location of the rotation vertex attribute
        /// </summary>
        public readonly int RotLocation;

        /// <summary>
        /// Location of the transform uniform
        /// </summary>
        public readonly int TransformLocation;

        /// <summary>
        /// constructs a shader and program, checking if it contains the necessary attributes
        /// </summary>
        /// <param name="vertexPath">Path to a file with the vertex shader source</param>
        /// <param name="fragmentPath">Path to a file with the fragment shader source</param>
        public QuadShader(string vertexPath, string geometryPath, string fragmentPath)
        {
            var vertexSource = File.ReadAllText(vertexPath);

            var geometrySource = File.ReadAllText(geometryPath);

            var fragmentSource = File.ReadAllText(fragmentPath);

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexSource);
            GL.CompileShader(vertexShader);

            string shaderLog = GL.GetShaderInfoLog(vertexShader);

            if (shaderLog != string.Empty)
            {
                GL.DeleteShader(vertexShader);
                throw new Exception(shaderLog);
            }

            int geometryShader = GL.CreateShader(ShaderType.GeometryShader);
            GL.ShaderSource(geometryShader, geometrySource);
            GL.CompileShader(geometryShader);

            shaderLog = GL.GetShaderInfoLog(geometryShader);

            if (shaderLog != string.Empty)
            {
                GL.DeleteShader(vertexShader);
                GL.DeleteShader(geometryShader);
                throw new Exception(shaderLog);
            }

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentSource);
            GL.CompileShader(fragmentShader);

            shaderLog = GL.GetShaderInfoLog(fragmentShader);

            if (shaderLog != string.Empty)
            {
                GL.DeleteShader(vertexShader);
                GL.DeleteShader(geometryShader);
                GL.DeleteShader(fragmentShader);
                throw new Exception(shaderLog);
            }

            _shaderProgramHandle = GL.CreateProgram();

            GL.AttachShader(_shaderProgramHandle, vertexShader);
            GL.AttachShader(_shaderProgramHandle, geometryShader);
            GL.AttachShader(_shaderProgramHandle, fragmentShader);

            GL.LinkProgram(_shaderProgramHandle);

            GL.DetachShader(_shaderProgramHandle, vertexShader);
            GL.DetachShader(_shaderProgramHandle, geometryShader);
            GL.DetachShader(_shaderProgramHandle, fragmentShader);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(geometryShader);
            GL.DeleteShader(fragmentShader);

            // Check for linking errors
            GL.GetProgram(_shaderProgramHandle, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
                GL.GetProgramInfoLog(_shaderProgramHandle, out string log);
                throw new Exception($"Error occurred whilst linking Program({_shaderProgramHandle}). {log}");
            }

            PosLocation = GetAttribLocation("ipos");

            TexLocation = GetAttribLocation("itex");

            ColorLocation = GetAttribLocation("icolor");

            OriginLocation = GetAttribLocation("iorigin");

            ScaleLocation = GetAttribLocation("iscale");

            RotLocation = GetAttribLocation("irot");

            TransformLocation = GL.GetUniformLocation(_shaderProgramHandle, "transform");

            if (TransformLocation == -1)
                throw new Exception($"\"transform\" uniform does not exist in shader {_shaderProgramHandle}!");
        }

        /// <summary>
        /// Makes the program of this shader the current program
        /// </summary>
        public void Use()
        {
            ThrowIfDisposed();
            GL.UseProgram(_shaderProgramHandle);
        }

        /// <summary>
        /// Sets the transform uniform of the shader
        /// </summary>
        /// <param name="matrix"></param>
        public void SetTransform(ref Matrix4 matrix)
        {
            ThrowIfDisposed();
            Use();
            GL.UniformMatrix4(TransformLocation, false, ref matrix);
        }

        /// <summary>
        /// Sets the transform uniform of the shader
        /// </summary>
        /// <param name="matrix"></param>
        public void SetTransform(Matrix4 matrix)
        {
            ThrowIfDisposed();
            Use();
            GL.UniformMatrix4(TransformLocation, false, ref matrix);
        }

        /// <summary>
        /// Deletes the shader program
        /// </summary>
        protected override void DisposeInternal()
        {
            GL.DeleteProgram(_shaderProgramHandle);
        }

        /// <summary>
        /// Used to get an attribute location and check if it exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private int GetAttribLocation(string name)
        {
            int location = GL.GetAttribLocation(_shaderProgramHandle, name);
            if (location == -1)
                throw new Exception($"Attribute {name} does not exist in shader {_shaderProgramHandle}!");
            return location;
        }
    }
}
