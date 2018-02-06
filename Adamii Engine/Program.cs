using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adamii_Engine
{
    class Program
    {
        public static Game Window;

        static void Main(string[] args)
        {
            using (Window = new Game())
            {
                Window.Run(60.0, 60.0);
            }
        }
    }
}
