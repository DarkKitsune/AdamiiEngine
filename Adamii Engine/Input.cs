using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Input;

namespace Adamii_Engine
{
    static class Input
    {
        static KeyboardState LastState;
        static KeyboardState KeyState;

        public static void Update()
        {
            LastState = KeyState;
            KeyState = Keyboard.GetState();
        }

        public static bool GetKeyPressed(Key key)
        {
            return KeyState.IsKeyDown(key) && !LastState.IsKeyDown(key);
        }
        public static bool GetKeyHeld(Key key)
        {
            return !KeyState.IsKeyDown(key);
        }

        public static bool GetKeyReleased(Key key)
        {
            return !KeyState.IsKeyDown(key) && LastState.IsKeyDown(key);
        }
    }
}