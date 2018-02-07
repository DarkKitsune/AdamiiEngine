using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Adamii_Engine
{
    class ShaderProgram : IBindableGLObject
    {
        public static ShaderProgram DefaultProgram { get; private set; }
        Shader[] Shaders;
        public bool IsValid
        {
            get
            {
                if (!GL.IsProgram(ID))
                    return false;
                foreach (var shader in Shaders)
                    if (!shader.IsValid)
                        return false;
                return true;
            }
        }

        public static void Init()
        {
            Shader.Init();
            DefaultProgram = new ShaderProgram(Shader.DefaultVertex, Shader.DefaultFragment);
            while (DefaultProgram.GetUniformLocation("instancePosition") == -1 && GraphicsSettings.MaxBatchInstances > 0)
            {
                GLDebug.IgnoreError();
                GraphicsSettings.MaxBatchInstances -= 16;
                Console.WriteLine("Failed to compile default program, trying batch size of " + GraphicsSettings.MaxBatchInstances);
                DefaultProgram.Dispose();
                Shader.CompileDefaults();
                DefaultProgram = new ShaderProgram(Shader.DefaultVertex, Shader.DefaultFragment);
            }
            if (GraphicsSettings.MaxBatchInstances <= 0)
                Console.WriteLine("Yo this GPU is fucked.");
        }

        public int ID { get; set; }

        public ShaderProgram(params Shader[] shaders)
        {
            Shaders = shaders;
            ID = GL.CreateProgram();

            foreach (var shader in shaders)
                GL.AttachShader(ID, shader.ID);
            GLDebug.CheckErrors();

            GL.LinkProgram(ID);
            GLDebug.CheckErrors();

            foreach (var shader in shaders)
                GL.DetachShader(ID, shader.ID);
            GLDebug.CheckErrors();
        }

        public void Dispose()
        {
            GL.DeleteProgram(ID);
        }

        public void Bind()
        {
            GL.UseProgram(ID);
        }

        public void Unbind()
        {
            GL.UseProgram(0);
        }

        string LastUniformName = "";
        int LastUniformLoc = -1;
        public int GetUniformLocation(string name)
        {
            if (name == LastUniformName)
                return LastUniformLoc;
            LastUniformName = name;
            return LastUniformLoc = GL.GetUniformLocation(ID, name);
        }
        public void SetUniform(string name, float v)
        {
            GL.Uniform1(GetUniformLocation(name), v);
            GLDebug.CheckErrors();
        }
        public void SetUniform(string name, Vector2 v)
        {
            GL.Uniform2(GetUniformLocation(name), v);
            GLDebug.CheckErrors();
        }
        public void SetUniform(string name, Vector3 v)
        {
            GL.Uniform3(GetUniformLocation(name), v);
            GLDebug.CheckErrors();
        }
        public void SetUniform(string name, Vector4 v)
        {
            GL.Uniform4(GetUniformLocation(name), v);
            GLDebug.CheckErrors();
        }
        public void SetUniform(string name, ref Matrix4 v)
        {
            GL.UniformMatrix4(GetUniformLocation(name), false, ref v);
            GLDebug.CheckErrors();
        }
        public void SetUniformVec3Array(string name, float[] v)
        {
            GL.Uniform3(GetUniformLocation(name), v.Length, v);
            GLDebug.CheckErrors();
        }
        public void SetUniformVec3Array(string name, float[] v, int count)
        {
            GL.Uniform3(GetUniformLocation(name), count, v);
            GLDebug.CheckErrors();
        }
        public void SetUniformVec2Array(string name, float[] v)
        {
            GL.Uniform2(GetUniformLocation(name), v.Length, v);
            GLDebug.CheckErrors();
        }
        public void SetUniformVec2Array(string name, float[] v, int count)
        {
            GL.Uniform2(GetUniformLocation(name), count, v);
            GLDebug.CheckErrors();
        }
        public void SetUniform(string name, Texture2D v, int textureUnit)
        {
            GL.Uniform1(GetUniformLocation(name), textureUnit);
            GL.ActiveTexture(TextureUnit.Texture0 + textureUnit);
            if (v != null)
                v.Bind();
            else
                GL.BindTexture(TextureTarget.Texture1D, 0);
            //GL.BindSampler()
        }
    }
}
