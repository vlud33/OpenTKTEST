using OpenTK.Mathematics;

namespace GameCore
{
    internal struct Vertex
    {
        public readonly Vector3 Position;
        public readonly Vector3 Normal;
        public readonly Vector2 TextureCoords;
        
        public Vertex(Vector3 position, Vector3 normal, Vector2 textureCoords)
        {
            Position = position;
            Normal = normal;
            TextureCoords = textureCoords;
        }
    }
}