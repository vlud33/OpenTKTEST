using Assimp;
using Assimp.Configs;

namespace GameAddition.LoadModel
{
    internal sealed class ModelLoader
    {
        private Scene _scene;
        
        public ModelLoader(string path)
        {
            AssimpContext importer = new AssimpContext();
            importer.SetConfig(new NormalSmoothingAngleConfig(66f));
            _scene = importer.ImportFile(path);
        }
    }
}
