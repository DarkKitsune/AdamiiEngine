using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace Adamii_Engine
{
    static class GLDebug
    {
        static void OnDebugOutput(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
        {
            byte[] chars = new byte[length];
            System.Runtime.InteropServices.Marshal.Copy(message, chars, 0, length);
            var msg = System.Text.Encoding.ASCII.GetString(chars);
            Console.WriteLine("[GL] " + msg);
        }

        static DebugProc OnDebugProc;
        public static void Init()
        {
#if DEBUG
            GL.Enable(EnableCap.DebugOutput);
            OnDebugProc = new DebugProc(OnDebugOutput);
            GL.DebugMessageCallback(OnDebugProc, IntPtr.Zero);
#endif
        }

        public static void CheckErrors()
        {
#if DEBUG
            var error = GL.GetError();

            if (error != ErrorCode.NoError)
                throw new Exception("OpenGL error occurred: " + error);
#endif
        }
        public static void IgnoreError()
        {
#if DEBUG
            GL.GetError();
#endif
        }
    }
}
