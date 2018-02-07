using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace Adamii_Engine
{
    class Shader : IGLObject, IContent
    {
        public enum ShaderType
        {
            Vertex = OpenTK.Graphics.OpenGL4.ShaderType.VertexShader,
            Fragment = OpenTK.Graphics.OpenGL4.ShaderType.FragmentShader
        }

        public static Shader DefaultVertex { get; private set; }
        public static Shader DefaultFragment { get; private set; }
        public bool IsValid
        {
            get
            {
                return GL.IsShader(ID);
            }
        }

        public static void Init()
        {
            CompileDefaults();
        }

        public static void CompileDefaults()
        {
            if (DefaultVertex != null && DefaultVertex.IsValid)
                DefaultVertex.Dispose();
            DefaultVertex = new Shader(Shader.ShaderType.Vertex, "DefaultVertex", @"
#version 140

uniform mat4 projectionMatrix;
uniform vec3 instancePosition[<instances>];
uniform vec3 instanceScale[<instances>];
uniform vec2 instanceUVXY[<instances>];
uniform vec2 instanceUVWH[<instances>];

in vec3 vPosition;
in vec2 vUV;
in vec4 vColor;

out vec4 fColor;
out vec2 UV;

void main()
{
    gl_Position = projectionMatrix * vec4(vPosition * instanceScale[gl_InstanceID] + instancePosition[gl_InstanceID], 1.0);
    fColor = vColor;

    UV = instanceUVXY[gl_InstanceID] + instanceUVWH[gl_InstanceID] * vUV;
}");
            if (DefaultFragment != null && DefaultFragment.IsValid)
                DefaultFragment.Dispose();
            DefaultFragment = new Shader(Shader.ShaderType.Fragment, "DefaultFragment", @"
#version 140

uniform sampler2D textureSampler;

in vec4 fColor;
in vec2 UV;

out vec4 fragColor;

void main()
{
    fragColor = fColor * texture( textureSampler, UV );
}");
        }

        public int ID { get; set; }
        public string Name { get; private set; }
        public string Source { get; private set; }

        public Shader(Shader.ShaderType type, string name, string code)
        {
            code = code.Replace("<instances>", GraphicsSettings.MaxBatchInstances.ToString());
            Name = name;
            Source = code;
            ID = GL.CreateShader((OpenTK.Graphics.OpenGL.ShaderType)type);
            GLDebug.CheckErrors();

            GL.ShaderSource(ID, code);
            GL.CompileShader(ID);
#if DEBUG
            var log = GL.GetShaderInfoLog(ID);
            if (log != null && log != "")
                throw new ShaderCompileException(this, log);
#endif
            GLDebug.CheckErrors();
        }

        public void Dispose()
        {
            GL.DeleteShader(ID);
        }
    }
}
