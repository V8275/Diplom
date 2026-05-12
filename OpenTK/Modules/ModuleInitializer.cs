using OpenTK.Mathematics;
using OpenTKProject;

namespace WinFormsOpenTK
{
    public static class ModuleInitializer
    {
        public static PhysicsModule AddPhysicsModule(SceneObject sceneObject, PhysicsWorld world, bool isKinematic, float mass = 10)
        {
            return new PhysicsModule(sceneObject, world, mass, isKinematic);
        }

        public static CollisionModule AddCollisionModule(SceneObject sceneObject, PhysicsWorld world, Vector3 vector3)
        {
            return new CollisionModule(sceneObject, world, vector3);
        }

        public static MoveModule AddMoveModule(SceneObject sceneObject, float speed = 10)
        {
            return new MoveModule(sceneObject, speed);
        }

        public static LightModule AddLightModule(SceneObject sceneObject, Color color)
        {
            return new LightModule(sceneObject, color);
        }

        public static CameraModule AddMoveableCameraModule(SceneObject sceneObject, float speed = 1.5f)
        {
            return new CameraModule(sceneObject, speed);
        }
    }
}
