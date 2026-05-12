namespace OpenTKProject
{
    public class SceneInitializer
    {
        private readonly ModelFactory _modelFactory;

        public SceneInitializer(ModelFactory modelFactory)
        {
            _modelFactory = modelFactory;
        }

        public Model CreateModel(string modelPath, ModelTextures texturePath, ModelFormat modelFormat)
        {
            return _modelFactory.CreateModel(modelPath, texturePath, modelFormat);
        }

        public List<SceneObject> CreateScene()
        {
            return new List<SceneObject>();
        }
    }
}