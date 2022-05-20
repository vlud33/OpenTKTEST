using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace GameCore
{
    internal class VertexArrayBuffer
    {
        private int _arrayBufferObject;

        public VertexArrayBuffer()
        {
            _arrayBufferObject = GL.GenBuffer();
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _arrayBufferObject);
        }

        public void AddVerticies<T>(T[] vertices, BufferUsageHint usage) where T : struct
        {
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Marshal.SizeOf<T>(), vertices, usage);
        }

        public void UnBind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }
}
