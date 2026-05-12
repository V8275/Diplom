using OpenTK.Mathematics;

namespace OpenTKProject
{
    public class LightModule : ObjectModule
    {
        public SceneObject ParentObject { get; set; }
        public Color Color { get; set; }
        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }

        public float Intensity { get; set; }

        public LightModule(SceneObject parentObject, Color color, float intensity = 1.0f)
        {
            ParentObject = parentObject;
            Color = color;
            Intensity = intensity;

            Vector3 colorVec = ColorToVec3();

            Ambient = colorVec * 0.5f * intensity;
            Diffuse = colorVec * 10.0f * intensity;
            Specular = colorVec * 5.0f * intensity;
        }

        public void SetIntensity(float intensity)
        {
            Intensity = intensity;

            float baseAmbient = 0.5f;
            float baseDiffuse = 10.0f;
            float baseSpecular = 5.0f;

            Vector3 colorVec = ColorToVec3();

            Ambient = new Vector3(baseAmbient, baseAmbient, baseAmbient) * intensity * colorVec;
            Diffuse = new Vector3(baseDiffuse, baseDiffuse, baseDiffuse) * intensity * colorVec;
            Specular = new Vector3(baseSpecular, baseSpecular, baseSpecular) * intensity * colorVec;
        }

        public Matrix4 GetLightSpaceMatrix(float size = 10.0f, float nearPlane = 1.0f, float farPlane = 50.0f)
        {
            Matrix4 lightProjection = Matrix4.CreateOrthographicOffCenter(
                -size, size, -size, size, nearPlane, farPlane);
            Matrix4 lightView = Matrix4.LookAt(ParentObject.TransformObject.Position, Vector3.Zero, Vector3.UnitY);
            return lightView * lightProjection;
        }

        public Vector3 ColorToVec3()
        {
            return new Vector3(Color.R / 255f, Color.G / 255f, Color.B / 255f);
        }

        public override void Start()
        {
        }

        public override void Update(float time)
        {
        }
    }
}