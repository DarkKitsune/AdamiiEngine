using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
//using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;

namespace Adamii_Engine
{
    class Game : GameWindow
    {
        public Game() : base(
                1280, 720,
                GraphicsMode.Default,
                "OpenTK Test",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                3,
                1,
                GraphicsContextFlags.ForwardCompatible
            )
        {
            Title += ": OpenGL Version: " + GL.GetString(StringName.Version);
        }

        protected override void OnLoad(EventArgs e)
        {
            System.IO.File.Delete("errorlog.txt");

            try
            {
                base.OnLoad(e);

                GraphicsSettings.Init();
                GLDebug.Init();
                ShaderProgram.Init();
                Primitive.Init();

                testLayer = new GameObjectLayer(ClientSize.Width, ClientSize.Height);


                testTexture = Texture2D.FromFile("testball.png");
                var testSprite = new Sprite(testTexture, new Vector2(32, 32),
                    new Vector2(0, 0),
                    new Vector2(16, 0),
                    new Vector2(32, 0),
                    new Vector2(48, 0),
                    new Vector2(64, 0),
                    new Vector2(80, 0),
                    new Vector2(96, 0),
                    new Vector2(112, 0)
                );

                var rand = new Random();
                for (int i = 0; i < 20000; i++)
                {
                    var newObj = new SpriteObject(testSprite, new Vector3(rand.Next(0, ClientSize.Width), rand.Next(0, ClientSize.Height), 0));
                    newObj.Layer = testLayer;
                    newObj.Frame = rand.Next(0, 7);
                    newObj.AnimationFPS = 15f;
                }

                CursorVisible = true;
                FPSSampleStart = DateTime.Now;
            }
            catch (Exception te)
            {
                System.IO.File.AppendAllText("errorlog.txt", te.ToString());
                Exit();
            }
        }

        protected override void OnResize(EventArgs e)
        {

            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            try
            {
                Input.Update();

                testLayer.Update();

                if (Input.GetKeyPressed(OpenTK.Input.Key.Escape))
                    Exit();
            }
            catch (Exception te)
            {
                System.IO.File.AppendAllText("errorlog.txt", te.ToString());
                Exit();
            }
        }

        Texture2D testTexture;
        Primitive testPrimitive;
        GameObjectLayer testLayer;
        int FPS;
        double FPSSum;
        int AvgFPS;
        DateTime FPSSampleStart;
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            try
            {
                FPS = (int)Math.Round(1.0 / e.Time);
                FPSSum += FPS * e.Time;
                var fpsSecondsSampled = (DateTime.Now.Ticks - FPSSampleStart.Ticks) / (double)TimeSpan.TicksPerSecond;
                AvgFPS = (int)Math.Round(FPSSum / fpsSecondsSampled);
                if (fpsSecondsSampled > 3.0)
                {
                    FPSSum = 0.0;
                    FPSSampleStart = DateTime.Now;
                }

                Title = $"Batch size: {GraphicsSettings.MaxBatchInstances} Objects: {testLayer.Objects.Count} FPS: {FPS} Avg.: {AvgFPS}";

                GL.ClearColor(new Color4(0.1f, 0.1f, 0.3f, 1.0f));
                GL.ClearDepth(1.0);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                testLayer.Draw();

                SwapBuffers();
            }
            catch (Exception te)
            {
                System.IO.File.AppendAllText("errorlog.txt", te.ToString());
                Exit();
            }
}
    }
}
