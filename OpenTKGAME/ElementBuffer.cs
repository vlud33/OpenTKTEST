using OpenTK.Graphics.OpenGL4;

namespace GameCore
{
    internal class ElementBuffer
    {
        private int _elementBufferObject;

        public ElementBuffer()
        {
            _elementBufferObject = GL.GenBuffer();
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        }

        public void UnBind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void SetIndexes(uint[] indexes, BufferUsageHint usage)
        {
            GL.BufferData(BufferTarget.ElementArrayBuffer, indexes.Length * sizeof(uint), indexes, usage);
        }
    }
}
