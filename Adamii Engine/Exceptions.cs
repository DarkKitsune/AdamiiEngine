using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adamii_Engine
{
    class ShaderCompileException : Exception
    {
        public ShaderCompileException(Shader shader, string message) : base(message.Trim() + " in " + shader.Name)
        {
            
        }
    }

    class HighSeverityGLDebugException : Exception
    {
        public HighSeverityGLDebugException(string message) : base(message)
        {

        }
    }
}
