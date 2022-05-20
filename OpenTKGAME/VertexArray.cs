using OpenTK.Graphics.OpenGL4; 

namespace GameCore
{
    internal class VertexArray
    {
        private int _vertexArrayObject;

        public VertexArray()
        {
            _vertexArrayObject = GL.GenVertexArray();
            Bind();
        }

        public void Bind()
        {
            GL.BindVertexArray(_vertexArrayObject);
        }
    }
}
