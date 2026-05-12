using OpenTKProject;
using WinFormsOpenTK;

namespace OpenTKProject
{
    internal class SkyboxModule : ObjectModule
    {
        CameraModule controller;
        SceneObject sceneObject;

        float rotationSpeed = 0;
        bool rotate = false;

        public SkyboxModule(CameraModule cc, SceneObject obj, float rotSpeed = 0)
        {
            controller = cc;
            sceneObject = obj;

            if(rotSpeed != 0)
            {
                rotate = true;
                rotationSpeed = rotSpeed;
            }
        }

        public override void Start() { }

        public override void Update(float time)
        {
            sceneObject.TransformObject.Position = controller.Position;

            if (rotate) RotateSky();
        }

        private void RotateSky()
        {

        }
    }
}
