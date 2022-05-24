using Assimp;
using Assimp.Configs;
using GameCore;
using GameCore.Graphics;
using OpenTK.Mathematics;

namespace GameAddition.LoadModel
{
    internal sealed class ModelLoader
    {
        private Scene _scene;
        private List<GameCore.Graphics.Mesh> _meshes;

        public ModelLoader(string path)
        {
            Load(path);
        }

        private void Load(string path)
        {
            AssimpContext loader = new AssimpContext();
            loader.SetConfig(new TangentSmoothingAngleConfig(66f));
            _scene = loader.ImportFile(path);

            if (_scene == null || _scene.SceneFlags == SceneFlags.Incomplete || _scene.RootNode == null)
            {
                throw new Exception("SCENE IS NOT LOADED CORRECTLY");
            }

            ProcessNode(_scene.RootNode, _scene);
        }

        private void ProcessNode(Node node, Scene scene)
        {

        }

        private void ProcessMesh(Assimp.Mesh mesh)
        {
            List<int> indices;
            List<Vertex> vertices;

            indices = GetIndicesFromModel(mesh);
            vertices = GetVertexFromModel(mesh);

        }

        private List<int> GetIndicesFromModel(Assimp.Mesh mesh)
        {
            List<int> indices = new List<int>();

            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                Face face = mesh.Faces[i];

                for (int j = 0; j < face.IndexCount; j++)
                {
                    indices.Add(face.Indices[j]);
                }
            }

            return indices;
        }

        private List<Vertex> GetVertexFromModel(Assimp.Mesh mesh)
        {
            List<Vertex> vertices = new List<Vertex>();

            for (int i = 0; i < mesh.VertexCount; i++)
            {
                Vector3 position = new Vector3(); 
                Vector3 normal = new Vector3();
                Vector2 textureCoords = new Vector2();

                // setting vertices
                position.X = mesh.Vertices[i].X;
                position.Y = mesh.Vertices[i].Y;
                position.Z = mesh.Vertices[i].Z;
                
                // setting normal
                normal.X = mesh.Normals[i].X;
                normal.Y = mesh.Normals[i].Y;
                normal.Z = mesh.Normals[i].Z;

                if (mesh.HasTextureCoords(0))
                {
                    for (int j = 0; j < mesh.TextureCoordinateChannelCount; j++)
                    {
                        textureCoords.X = mesh.TextureCoordinateChannels[0][j].X;
                        textureCoords.Y = mesh.TextureCoordinateChannels[0][j].Y;
                    }
                }
                else
                {
                    textureCoords = new Vector2(0);
                }

                vertices.Add(new Vertex(position, normal, textureCoords));
            }

            return vertices;
        }

        private void GetTexture(Assimp.Mesh mesh, Scene scene)
        {
            if (mesh.MaterialIndex < 0) return;

            Material material = scene.Materials[mesh.MaterialIndex];
        }

        private void LoadMaterial(Material material, TextureType textureType, string type)
        {
            for (int i = 0; i < material.GetMaterialTextureCount(textureType); i++)
            {
                TextureSlot textureSlot;
                TextureInfo textureInfo;

                if (material.GetMaterialTexture(textureType, 0, out textureSlot))
                {
                    string pathToTexture = textureSlot.FilePath;
                    TextureConfigure textureConfigure = new TextureConfigure(pathToTexture, OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
                }
            }
        }
    }
}
