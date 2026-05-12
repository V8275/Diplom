using OpenTK.Mathematics;

namespace WinFormsOpenTK
{
    public struct ScenePreferences
    {
        public string Name;
        public bool EnablePhysics;
        public int ShadowMapQuality;

        public Vector3 MainLightPosition;
        public float MainLightIntensity;
        public Color MainLightColor;

        public Vector3 CameraStartPosition;

        public string SkyboxTexturePath;
        public Vector3 SkyboxScale;

        public string VertexShaderPath;
        public string FragmentShaderPath;

        public static ScenePreferences Default => new ScenePreferences
        {
            Name = "Default Scene",
            EnablePhysics = true,
            ShadowMapQuality = 4096,
            MainLightPosition = new Vector3(2, 2, 2),
            MainLightIntensity = 5f,
            MainLightColor = Color.AntiqueWhite,
            CameraStartPosition = new Vector3(0, 2, 5),
            SkyboxTexturePath = "Models/Textures/Sky/SkyClouds.jpg",
            SkyboxScale = new Vector3(50, 50, 50),
            VertexShaderPath = "DiplomOpenTK.Shaders.Vert.shader.vert",
            FragmentShaderPath = "DiplomOpenTK.Shaders.Frag.shader.frag"
        };
    }
}
