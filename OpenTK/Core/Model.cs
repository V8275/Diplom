using OpenTK.Graphics.OpenGL;

namespace OpenTKProject
{
    public class Model
    {
        VisualModel vModel;
        Texture texture;
        Texture normalMap;
        Texture metallic;
        Texture roughness;
        Texture emission;
        SkyBoxTexture skyBoxReflection;
        Shader shader;
        int hasSkyBox;

        public VisualModel VModel { get { return vModel; } }
        public Texture Texture { get { return texture; } }
        public Texture NormalMap { get { return normalMap; } }
        public Texture Metallic { get { return metallic; } }
        public Texture Roughness { get { return roughness; } }
        public Texture Emission { get { return emission; } }
        public SkyBoxTexture SkyBoxReflection { get { return skyBoxReflection; } }
        public Shader Shader { get { return shader; } }
        public int HasSkyBox { get { return hasSkyBox; } }

        public Model() 
        {
            hasSkyBox = 1;
        }

        public Model(bool skybox = true)
        {
            SetSkybox(skybox);
        }

        public Model(VisualModel mod, Texture tex, Shader shad, bool skybox = true)
        {
            vModel = mod;
            texture = tex;
            shader = shad;

            SetSkybox(skybox);
        }

        public Model(Model m, ModelFormat modelFormat = ModelFormat.Obj, bool skybox = true)
        {
            SetVModel(m.vModel.PathToModel, modelFormat);
            if(m.Texture != null)
                SetTexture(m.Texture.PathToTexture);
            SetShader(m.shader.PathToVecShader, m.shader.PathToFragShader);

            SetSkybox(skybox);
        }

        private void SetSkybox(bool hasSB)
        {
            if (hasSB == true)
            {
                hasSkyBox = 1;
            }
            else
            {
                hasSkyBox = 0;
            }
        }

        public void SetSkyTexture(string[] paths)
        {
            skyBoxReflection = new SkyBoxTexture(paths, true);

            SetSkybox(true);
        }

        public void SetVModel(string path, ModelFormat modelFormat)
        {
            vModel = new VisualModel(path, modelFormat);
        }

        public void SetTexture(string path)
        {
            texture = new Texture(path, true, PixelInternalFormat.SrgbAlpha);
        }

        public void SetNormalMap(string path)
        {
            normalMap = new Texture(path);
        }

        public void SetMetallic(string path)
        {
            metallic = new Texture(path, true);
        }

        public void SetRoughness(string path)
        {
            roughness = new Texture(path, true);
        }

        public void SetEmission(string path)
        {
            emission = new Texture(path, true, PixelInternalFormat.Rgb);
        }

        public void SetupEmission()
        {
            shader.SetInt("hasEmission", Emission != null ? 1 : 0);

            if (Emission != null)
            {
                GL.ActiveTexture(TextureUnit.Texture5);
                GL.BindTexture(TextureTarget.Texture2D, Emission.Handle);
                Shader.SetInt("emissionMap", 5);
            }
        }

        public void SetShader(string vertPath, string fragPath)
        {
            shader = new Shader(vertPath, fragPath);
        }
    }
}
