using OpenTK.Graphics.OpenGL4;
using GameCore.Graphics;
using System.Runtime.InteropServices;

namespace GameCore.Graphics
{
    internal sealed class Mesh
    {
        private VertexArray _VAO;
        
        private List<Vertex> _vertices;
        private List<uint> _indices;
        private List<TextureInfo> _texturesInfo;

        public Mesh(List<Vertex> vertices, List<uint> indeces, List<TextureInfo> texturesInfo, BufferUsageHint hint)
        {
            _vertices = vertices;
            _indices = indeces;
            _texturesInfo = texturesInfo;

            SetUpMesh(hint);
        }

        public void SetUpMesh(BufferUsageHint hint)
        {
            _VAO = new VertexArray();
            _VAO.Bind();

            VertexArrayBuffer VBO = new VertexArrayBuffer();
            VBO.Bind();
            VBO.AddVerticies(_vertices.ToArray(), hint);

            ElementBuffer EBO = new ElementBuffer();
            EBO.Bind();
            EBO.SetIndexes(_indices.ToArray(), hint);

            int sizeOfVertex = Marshal.SizeOf<Vertex>();

            VertexAttribPointer.Set(0, 3, VertexAttribPointerType.Float, true, sizeOfVertex, 0);

            VertexAttribPointer.Set(1, 3, VertexAttribPointerType.Float, true, sizeOfVertex, (int)Marshal.OffsetOf<Vertex>("Normal"));

            VertexAttribPointer.Set(2, 2, VertexAttribPointerType.Float, true, sizeOfVertex, (int)Marshal.OffsetOf<Vertex>("TextureCoords"));
        }

        public void Draw(ShaderProgram shaderProgram)
        {
            SetUpTextures(shaderProgram);

            _VAO.Bind();
            GL.DrawElements(BeginMode.Triangles, _indices.Count, DrawElementsType.UnsignedInt, 0);
            _VAO.UnBind();
        }

        private void SetUpTextures(ShaderProgram shaderProgram)
        {
            int diffuseNumber = 0;
            int specularNumber = 0;

            for (int i = 0; i < _texturesInfo.Count; i++)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + i);
                
                string name = "";
                string type = _texturesInfo[i].Type;

                if (type == "texture_diffuse")
                {
                    name = type + diffuseNumber.ToString();
                    diffuseNumber += 1;
                }
                else if (type == "texture_specular")
                {
                    name = type + specularNumber.ToString();
                    specularNumber += 1;
                }

                shaderProgram.SetUniform1(name, i);

                GL.BindTexture(TextureTarget.Texture2D, _texturesInfo[i].Id);
            }

            GL.ActiveTexture(TextureUnit.Texture0);
        }
    }
}
