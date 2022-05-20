using OpenTK.Graphics.OpenGL4;
using GameCore.Graphics;
using System.Runtime.InteropServices;

namespace GameCore.Graphics
{
    internal sealed class Mesh
    {
        private List<Vertex> _vertices;
        private List<uint> _indeces;
        private List<TextureInfo> _texturesInfo;

        public Mesh(List<Vertex> vertices, List<uint> indeces, List<TextureInfo> texturesInfo, BufferUsageHint hint)
        {
            _vertices = vertices;
            _indeces = indeces;
            _texturesInfo = texturesInfo;

            SetUpMesh(hint);
        }

        public void SetUpMesh(BufferUsageHint hint)
        {
            VertexArray VAO = new VertexArray();
            VAO.Bind();

            VertexArrayBuffer VBO = new VertexArrayBuffer();
            VBO.Bind();
            VBO.AddVerticies(_vertices.ToArray(), hint);

            ElementBuffer EBO = new ElementBuffer();
            EBO.Bind();
            EBO.SetIndexes(_indeces.ToArray(), hint);

            int sizeOfVertex = Marshal.SizeOf<Vertex>();

            VertexAttribPointer.Set(0, 3, VertexAttribPointerType.Float, true, sizeOfVertex, 0);

            VertexAttribPointer.Set(1, 3, VertexAttribPointerType.Float, true, sizeOfVertex, (int)Marshal.OffsetOf<Vertex>("Normal"));

            VertexAttribPointer.Set(2, 2, VertexAttribPointerType.Float, true, sizeOfVertex, (int)Marshal.OffsetOf<Vertex>("TextureCoords"));
        }

        public void Draw()
        {

        }
    }
}
