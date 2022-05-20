using OpenTK.Graphics.OpenGL4;

namespace GameCore
{
    internal static class VertexAttribPointer
    {
        public static void Set(int index, int size, VertexAttribPointerType type, bool normalized, int stride, int offset)
        {
            GL.VertexAttribPointer(index, size, type, normalized, stride, offset);
            GL.EnableVertexAttribArray(index);
        }
    }
}
