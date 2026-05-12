using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using WinFormsOpenTK;

namespace OpenTKProject
{
    public class ModelRenderer
    {
        private readonly Dictionary<Model, (int vao, int vbo, int ebo)> _modelBuffers;

        public ModelRenderer()
        {
            _modelBuffers = new Dictionary<Model, (int, int, int)>();
        }

        public void RenderModel(SceneObject sceneObj, CameraModule camera, LightManager lightManager,
                       int shadowMapTexture, Vector2i windowSize)
        {
            EnsureBuffersExist(sceneObj.Model);

            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                (float)windowSize.X / (float)windowSize.Y, 0.1f, 100.0f);

            Matrix4 modelMatrix = sceneObj.GetModelMatrix();
            Matrix4 lightSpaceMatrix = GetMainLightSpaceMatrix(lightManager);

            sceneObj.Model.Shader.Use();

            SetupTextures(sceneObj.Model, shadowMapTexture);

            sceneObj.Model.Shader.SetMatrix4("model", modelMatrix);
            sceneObj.Model.Shader.SetMatrix4("view", view);
            sceneObj.Model.Shader.SetMatrix4("projection", projection);
            sceneObj.Model.Shader.SetMatrix4("lightSpaceMatrix", lightSpaceMatrix);

            SetupLighting(sceneObj.Model, camera, lightManager);

            var buffers = _modelBuffers[sceneObj.Model];
            GL.BindVertexArray(buffers.vao);
            GL.DrawElements(PrimitiveType.Triangles, sceneObj.Model.VModel.Indices.Count,
                           DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);
        }

        private Matrix4 GetMainLightSpaceMatrix(LightManager lightManager)
        {
            var lights = lightManager.GetLights();
            if (lights.Count > 0)
                return lights[0].GetLightSpaceMatrix();
            return Matrix4.Identity;
        }

        private void SetupTextures(Model model, int shadowMapTexture)
        {
            if (model.Texture != null)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, model.Texture.Handle);
                model.Shader.SetInt("texture0", 0);
            }

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, shadowMapTexture);
            model.Shader.SetInt("shadowMap", 1);

            if (model.NormalMap != null)
            {
                GL.ActiveTexture(TextureUnit.Texture2);
                GL.BindTexture(TextureTarget.Texture2D, model.NormalMap.Handle);
                model.Shader.SetInt("normalMap", 2);
                model.Shader.SetInt("hasNormalMap", 1);
            }
            else
            {
                model.Shader.SetInt("hasNormalMap", 0);
            }

            if (model.Metallic != null)
            {
                GL.ActiveTexture(TextureUnit.Texture3);
                GL.BindTexture(TextureTarget.Texture2D, model.Metallic.Handle);
                model.Shader.SetInt("metallicMap", 3);
            }

            if (model.Roughness != null)
            {
                GL.ActiveTexture(TextureUnit.Texture4);
                GL.BindTexture(TextureTarget.Texture2D, model.Roughness.Handle);
                model.Shader.SetInt("roughnessMap", 4);
            }

            if (model.Emission != null)
            {
                GL.ActiveTexture(TextureUnit.Texture5);
                GL.BindTexture(TextureTarget.Texture2D, model.Emission.Handle);
                model.Shader.SetInt("emissionMap", 5);
            }

            if (model.SkyBoxReflection != null)
            {
                GL.ActiveTexture(TextureUnit.Texture6);
                GL.BindTexture(TextureTarget.TextureCubeMap, model.SkyBoxReflection.Handle);
                model.Shader.SetInt("skybox", 6);
            }
        }

        private void SetupLighting(Model model, CameraModule camera, LightManager lightManager)
        {
            var shader = model.Shader;

            shader.SetVector3("viewPos", camera.Position);
            shader.SetVector3("material.ambient", lightManager.GetLights()[0].ColorToVec3());
            shader.SetVector3("material.diffuse", new Vector3(1.0f, 0.5f, 0.31f));
            shader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            shader.SetFloat("material.shininess", 32.0f);
            shader.SetFloat("material.metallic", 0.5f);
            shader.SetFloat("material.roughness", 0.5f);
            shader.SetFloat("material.ao", 1.0f); 
            shader.SetInt("hasMetallicMap", model.Metallic != null ? 1 : 0);
            shader.SetInt("hasRoughnessMap", model.Roughness != null ? 1 : 0);
            shader.SetInt("hasEmission", model.Emission != null ? 1 : 0);
            shader.SetInt("hasSkybox", model.HasSkyBox);

            var lights = lightManager.GetLights();
            shader.SetInt("lightsCount", lights.Count);

            for (int i = 0; i < lights.Count && i < 10; i++)
            {
                string prefix = $"lights[{i}].";
                shader.SetVector3(prefix + "position", lights[i].ParentObject.TransformObject.Position);
                shader.SetVector3(prefix + "ambient", lights[i].Ambient);
                shader.SetVector3(prefix + "diffuse", lights[i].Diffuse);
                shader.SetVector3(prefix + "specular", lights[i].Specular);
            }
        }

        private void EnsureBuffersExist(Model model)
        {
            if (!_modelBuffers.ContainsKey(model))
            {
                SetupModelBuffers(model);
            }
        }

        private void SetupModelBuffers(Model model)
        {
            int vao = GL.GenVertexArray();
            int vbo = GL.GenBuffer();
            int ebo = GL.GenBuffer();

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            SetupModelData(model, vbo, ebo);
            _modelBuffers[model] = (vao, vbo, ebo);
        }

        private void SetupModelData(Model model, int vbo, int ebo)
        {
            int stride = 11;
            int uvOffset = 3 * sizeof(float);
            int normalOffset = 5 * sizeof(float);
            int tangentOffset = 8 * sizeof(float);
            int strideSize = stride * sizeof(float);

            GL.BufferData(BufferTarget.ArrayBuffer, model.VModel.Vertices.Count * sizeof(float),
                          model.VModel.Vertices.ToArray(), BufferUsageHint.StaticDraw);

            GL.BufferData(BufferTarget.ElementArrayBuffer, model.VModel.Indices.Count * sizeof(uint),
                          model.VModel.Indices.ToArray(), BufferUsageHint.StaticDraw);

            // позиция
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, strideSize, 0);

            // UV координаты
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, strideSize, uvOffset);

            // нормали
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, strideSize, normalOffset);

            // касательные
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, strideSize, tangentOffset);
        }

        public void Dispose()
        {
            foreach (var buffers in _modelBuffers.Values)
            {
                GL.DeleteBuffer(buffers.vbo);
                GL.DeleteVertexArray(buffers.vao);
                GL.DeleteBuffer(buffers.ebo);
            }
        }
    }
}