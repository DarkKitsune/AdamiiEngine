using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using System.Drawing;

namespace Adamii_Engine
{
    class GameObjectLayer
    {
        public Size Size { get; private set; }
        public List<GameObject> Objects { get; private set; } = new List<GameObject>();
        public ShaderProgram ShaderProgram;

        public GameObjectLayer(int width, int height)
        {
            Size = new Size(width, height);
            ShaderProgram = ShaderProgram.DefaultProgram;
        }

        public void Add(GameObject o)
        {
            Objects.Add(o);
        }
        public void Remove(GameObject o)
        {
            Objects.Remove(o);
        }

        public void Update()
        {
            for (var i = 0; i < Objects.Count; i++)
            {
                var obj = Objects[i];
                if (obj.Deleted)
                {
                    Objects.RemoveAt(i);
                    i--;
                    continue;
                }

                obj.Update();
            }
        }

        float[] batchPositions = new float[GraphicsSettings.MaxBatchInstances * 3];
        float[] batchScales = new float[GraphicsSettings.MaxBatchInstances * 3];
        float[] batchUVPositions = new float[GraphicsSettings.MaxBatchInstances * 2];
        float[] batchUVSizes = new float[GraphicsSettings.MaxBatchInstances * 2];
        public void Draw()
        {
            ShaderProgram.DefaultProgram.Bind();
            var proj = Matrix4.CreateOrthographicOffCenter(0, Size.Width, Size.Height, 0, -1f, 1f);
            ShaderProgram.DefaultProgram.SetUniform("projectionMatrix", ref proj);
            GLDebug.CheckErrors();

            if (batchPositions.Length < GraphicsSettings.MaxBatchInstances * 3)
                Array.Resize(ref batchPositions, GraphicsSettings.MaxBatchInstances * 3);
            if (batchScales.Length < GraphicsSettings.MaxBatchInstances * 3)
                Array.Resize(ref batchScales, GraphicsSettings.MaxBatchInstances * 3);
            if (batchUVPositions.Length < GraphicsSettings.MaxBatchInstances * 2)
                Array.Resize(ref batchUVPositions, GraphicsSettings.MaxBatchInstances * 2);
            if (batchUVSizes.Length < GraphicsSettings.MaxBatchInstances * 2)
                Array.Resize(ref batchUVSizes, GraphicsSettings.MaxBatchInstances * 2);

            Texture2D batchTex = null;
            Primitive batchPrim = null;
            var batchCount = 0;
            var batchStart = 0;
            var batchNext = 0;
            for (var i = 0; i < Objects.Count; i++)
            {
                var obj = Objects[i];
                if (obj is PrimitiveObject)
                {
                    var po = (PrimitiveObject)obj;
                    if (po.Primitive != batchPrim || po.Sprite.Texture != batchTex || batchCount >= GraphicsSettings.MaxBatchInstances)
                    {
                        if (batchPrim != null && batchCount > 0)
                            _Draw(ShaderProgram, batchPrim, batchTex, batchStart, batchCount);
                        batchPrim = po.Primitive;
                        batchTex = po.Sprite.Texture;
                        batchStart = i;
                        batchCount = 0;
                        batchNext = 0;
                    }

                    batchPositions[batchNext * 3] = obj.Position.X;
                    batchPositions[batchNext * 3 + 1] = obj.Position.Y;
                    batchPositions[batchNext * 3 + 2] = obj.Position.Z;

                    batchScales[batchNext * 3] = po.Scale.X * po.ScaleMultiplier.X;
                    batchScales[batchNext * 3 + 1] = po.Scale.Y * po.ScaleMultiplier.X;
                    batchScales[batchNext * 3 + 2] = po.Scale.Z * po.ScaleMultiplier.X;
                    
                    var uvPos = po.Sprite.Positions[(int)po.Frame];
                    var w = (float)po.Sprite.Texture.Size.Width;
                    var h = (float)po.Sprite.Texture.Size.Height;
                    batchUVPositions[batchNext * 2] = uvPos.X / w;
                    batchUVPositions[batchNext * 2 + 1] = uvPos.Y / h;
                    batchUVSizes[batchNext * 2] = po.Sprite.Size.X / w;
                    batchUVSizes[batchNext * 2 + 1] = po.Sprite.Size.Y / h;
                    
                    batchNext++;
                    batchCount++;
                }
            }
            if (batchPrim != null && batchCount > 0)
                _Draw(ShaderProgram, batchPrim, batchTex, batchStart, batchCount);

            ShaderProgram.DefaultProgram.Unbind();
        }
        void _Draw(ShaderProgram shader, Primitive prim, Texture2D tex, int start, int count)
        {
            shader.SetUniformVec3Array("instancePosition", batchPositions, count * 3);
            shader.SetUniformVec3Array("instanceScale", batchScales, count * 3);
            shader.SetUniformVec2Array("instanceUVXY", batchUVPositions, count * 2);
            shader.SetUniformVec2Array("instanceUVWH", batchUVSizes, count * 2);
            shader.SetUniform("textureSampler", tex, 0);
            prim.DrawInstanced(count);
        }
    }
}
