using OpenTK.Mathematics;
using WinFormsOpenTK;

namespace OpenTKProject
{
    public class SceneObject
    {
        public Model Model { get; set; }
        public TransformObject TransformObject { get; set; }

        public List<ObjectModule> Modules { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">Model's geometry</param>
        /// <param name="position">Model' start position</param>
        /// <param name="newmodules">Model's start array of extension code modules</param>
        public SceneObject(Model model, TransformObject transform)
        {
            Model = model;

            TransformObject = transform;
        }

        public SceneObject(Model model, Vector3 position)
        {
            Model = model;

            TransformObject = new TransformObject(position);
        }

        public virtual void Start()
        {
            if(Modules != null)
                foreach(ObjectModule module in Modules)
                {
                    module.Start();
                }
        }

        public virtual void Update(float time)
        {
            if (Modules != null)
                foreach (ObjectModule module in Modules)
                {
                    module.Update(time);
                }
        }

        /// <summary>
        /// Add extension module to object
        /// </summary>
        /// <param name="module"></param>
        public void AddModule(ObjectModule module)
        {
            if (Modules != null) Modules.Add(module);
            else 
            {
                Modules = new List<ObjectModule>();
                Modules.Add(module);
            }
        }

        public void MakeModuleList(List<ObjectModule> newmodules)
        {
            if(Modules == null) Modules = new List<ObjectModule>();

            if (newmodules != null) 
                Modules.AddRange(newmodules);
        }

        public T GetModule<T>() where T : ObjectModule
        {
            if (Modules == null) return null;

            foreach (var module in Modules)
            {
                if (module is T typedModule)
                {
                    return typedModule;
                }
            }

            return null;
        }

        public Matrix4 GetModelMatrix()
        {
            return Matrix4.CreateScale(TransformObject.Scale) *
                   Matrix4.CreateRotationX(TransformObject.Rotation.X) *
                   Matrix4.CreateRotationY(TransformObject.Rotation.Y) *
                   Matrix4.CreateRotationZ(TransformObject.Rotation.Z) *
                   Matrix4.CreateTranslation(TransformObject.Position);
        }
    }
}