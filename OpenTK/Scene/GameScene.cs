using OpenTK.Mathematics;
using OpenTKProject;

namespace WinFormsOpenTK
{
    internal class GameScene
    {
        private ScenePreferences _preferences;

        private string[] skyboxTexturePaths = new string[6];

        private SceneObject _skyboxObject;
        private SceneObject _cameraObject;
        private PhysicsWorld _physicsWorld;
        private SceneInitializer _sceneInitializer;
        private Renderer _renderer;

        private List<SceneObject> lightObject = new List<SceneObject>();
        private List<SceneObject> _sceneObjects = new List<SceneObject>();
        private List<SceneObject> _renderList = new List<SceneObject>();

        public string Name => _preferences.Name;
        public bool IsPhysicsEnabled => _preferences.EnablePhysics;
        public int ShadowMapQuality => _preferences.ShadowMapQuality;

        public GameScene() : this(ScenePreferences.Default)
        {
        }

        public GameScene(ScenePreferences preferences)
        {
            _preferences = preferences;
        }

        public GameScene(bool physics) : this(new ScenePreferences
        {
            EnablePhysics = physics,
            Name = "New Scene",
            ShadowMapQuality = 4096,
            MainLightPosition = new Vector3(2, 2, 2),
            MainLightIntensity = 5f,
            MainLightColor = Color.AntiqueWhite,
            CameraStartPosition = new Vector3(0, 2, 5),
            SkyboxTexturePath = "Models/Textures/Sky/SkyClouds.jpg",
            SkyboxScale = new Vector3(50, 50, 50),
            VertexShaderPath = "WinFormsOpenTK.Shaders.Vert.shader.vert",
            FragmentShaderPath = "WinFormsOpenTK.Shaders.Frag.shader.frag"
        }) { }

        internal Renderer InitRender()
        {
            var SkyboxTextures = new SkyboxPaths();
            skyboxTexturePaths = SkyboxTextures.GetPaths();

            var mainLight = SetMainLight();


            var p1 = AddPointLight(new Vector3(5, 3, 0), Color.Red, 2.0f);
            var p2 = AddPointLight(new Vector3(-5, 3, 0), Color.Blue, 2.0f);
            var p3 = AddPointLight(new Vector3(0, 1, 5), Color.Green, 1.5f);
            var p4 = AddPointLight(new Vector3(2, 2, -4), Color.Orange, 3f);

            LightModule[] m = { p1, p2, p3, p4 };
            mainLight.AddLights(m);

            if (_preferences.EnablePhysics)
                _physicsWorld = new PhysicsWorld();

            var modelFactory = new ModelFactory(
                _preferences.VertexShaderPath,
                _preferences.FragmentShaderPath,
                skyboxTexturePaths);
            _sceneInitializer = new SceneInitializer(modelFactory);

            SetCamera();

            _renderer = new Renderer(GetEngineCameraModule(), mainLight, _preferences.ShadowMapQuality);
            return _renderer;
        }

        internal LightManager SetMainLight()
        {
            var _lightManager = new LightManager();

            var lightObject1 = new SceneObject(null, _preferences.MainLightPosition);
            var lightModule1 = ModuleInitializer.AddLightModule(lightObject1, _preferences.MainLightColor);
            lightModule1.SetIntensity(_preferences.MainLightIntensity);
            lightObject1.AddModule(lightModule1);
            lightObject.Add(lightObject1);
            _lightManager.AddLight(lightModule1);

            return _lightManager;
        }

        public LightModule AddPointLight(Vector3 position, Color color, float intensity = 1.0f)
        {
            var lightObj = new SceneObject(null, position);
            var lightModule = ModuleInitializer.AddLightModule(lightObj, color);
            lightModule.SetIntensity(intensity);
            lightObj.AddModule(lightModule);
            lightObject.Add(lightObj);

            return lightModule;
        }

        internal void SetCamera()
        {
            _cameraObject = new SceneObject(null, _preferences.CameraStartPosition);
            var moveLinear = ModuleInitializer.AddMoveableCameraModule(_cameraObject);
            _cameraObject.AddModule(moveLinear);

            UpdateRenderList();
        }

        internal void SetSkyBox()
        {
            Model SkyModel = new Model(skybox: false);
            SkyModel.SetVModel("Models/Sky.obj", ModelFormat.Obj);
            SkyModel.SetTexture(_preferences.SkyboxTexturePath);
            SkyModel.SetEmission(_preferences.SkyboxTexturePath);
            SkyModel.SetShader(_preferences.VertexShaderPath, _preferences.FragmentShaderPath);
            SkyModel.SetupEmission();

            var Transform = new TransformObject(GetEngineCameraModule().Position, _preferences.SkyboxScale);

            _skyboxObject = new SceneObject( SkyModel, Transform);

            var skyModule = new SkyboxModule(GetEngineCameraModule(), _skyboxObject);
            _skyboxObject.AddModule(skyModule);

            UpdateRenderList();
        }

        internal void UpdatePreferences(ScenePreferences newPreferences)
        {
            _preferences = newPreferences;
        }

        internal ScenePreferences GetPreferences()
        {
            return _preferences;
        }

        internal void UpdateRenderList()
        {
            _renderList.Clear();
            _renderList.Add(_cameraObject);
            _renderList.Add(_skyboxObject);
            _renderList.AddRange(_sceneObjects);
        }

        internal CameraModule GetEngineCameraModule()
        {
            return _cameraObject.GetModule<CameraModule>();
        }

        internal void Dispose()
        {
            foreach (var obj in _sceneObjects)
            {
                obj.Model.Shader?.Dispose();
            }

            _physicsWorld?.Dispose();
        }

        internal void UpdatePhysics(float deltaTime)
        {
            if (_preferences.EnablePhysics)
                _physicsWorld.Update(deltaTime);
        }

        internal void StartObjects()
        {
            foreach (var obj in _renderList)
            {
                if (obj != null)
                    obj.Start();
            }
        }

        internal void UpdateObjects(float v)
        {
            foreach (var obj in _renderList)
            {
                if(obj != null)
                    obj.Update(0.008f);
            }
        }

        internal void AddToScene(SceneObject newObj)
        {
            _sceneObjects.Add(newObj);
            UpdateRenderList();
        }

        internal int GetSceneObjectsCount()
        {
            return _sceneObjects.Count;
        }

        internal void DeleteSceneObjectByIndex(int index)
        {
            var obj = _sceneObjects[index];
            obj.Model.Shader?.Dispose();

            _sceneObjects.RemoveAt(index); 
            
            UpdateRenderList();
        }

        public SceneObject GetSceneObjectByIndex(int i)
        {
            return _sceneObjects[i];
        }

        internal void SetEngineCameraOrbitMode(int index)
        {
            var obj = GetSceneObjectByIndex(index);
            GetEngineCameraModule().SetOrbitMode(obj, 8.0f);
        }

        internal List<SceneObject> GetRenderList()
        {
            return _renderList;
        }

        internal SceneObject CreateNewModel(TransformObject transform, string _selectedModelPath, ModelTextures _selectedTexturePath, string _selectedModelFormat)
        {
            var model = _sceneInitializer.CreateModel(_selectedModelPath, _selectedTexturePath, FormatConverter.GetFormatFromString(_selectedModelFormat));

            SceneObject newObj;

            newObj = new SceneObject(model, transform);

            newObj.Start();
            AddToScene(newObj);

            return newObj;
        }

        internal void SetGlobalLightIntensity(float intensity)
        {
            foreach (var obj in lightObject)
            {
                var moduleLight = obj.Modules.Where(a => a is LightModule).First() as LightModule;
                if (moduleLight != null)
                    moduleLight.SetIntensity(intensity);
                else
                {
                    MessageBox.Show("No light");
                }
            }
        }

        internal List<SceneObject> GetSceneObjects()
        {
            return _sceneObjects;
        }

        internal PhysicsWorld GetPhysics()
        {
            return _physicsWorld;
        }
    }

    public class TransformObject
    {
        private Vector3 _position = Vector3.Zero;
        private Vector3 _scale = Vector3.One;
        private Quaternion _rotation = Quaternion.Identity;

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public Vector3 Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        public Quaternion Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public TransformObject() { }

        public TransformObject(Vector3 position)
        {
            _position = position;
        }

        public TransformObject(Vector3 position, Vector3 scale)
        {
            _position = position;
            _scale = scale;
        }

        public TransformObject(Vector3 position, Vector3 scale, Quaternion rotation)
        {
            _position = position;
            _scale = scale;
            _rotation = rotation;
        }
    }
}