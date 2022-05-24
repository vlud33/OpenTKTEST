using GameCore.Graphics;

namespace GameCore
{
    internal struct TextureInfo
    {
        public readonly TextureConfigure Texture;
        public readonly string Type;
        public readonly string Path;

        public TextureInfo(TextureConfigure texture, string type, string path)
        {
            Texture = texture;
            Type = type;
            Path = path;
        }
    }
}