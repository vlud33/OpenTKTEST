namespace GameCore
{
    internal struct TextureInfo
    {
        public readonly uint Id;
        public readonly string Type;

        public TextureInfo(uint id, string type)
        {
            Id = id;
            Type = type;
        }
    }
}