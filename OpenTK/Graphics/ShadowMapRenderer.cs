using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace OpenTKProject
{
    public class ShadowMapRenderer
    {
        private readonly ShadowMap _shadowMap;
        private Shader _shadowShader;
        private readonly Dictionary<Model, (int vao, int vbo, int ebo)> _modelBuffers;
        private bool _isInitialized = false;

        public int ShadowMapTexture => _shadowMap.DepthMapTexture;

        public ShadowMapRenderer(int width, int height)
        {
            _shadowMap = new ShadowMap(width, height);
            _modelBuffers = new Dictionary<Model, (int, int, int)>();
        }

        public void Initialize()
        {
            if (_isInitialized)
                return;

            // »нициализаци€ только после создани€ OpenGL контекста
            _shadowShader = new Shader(
                "D:\\Projects\\VSProjects\\OpenTKProject\\Shaders\\Vert\\shadow.vert",//"D:\\Development\\OpenTKProject\\Shaders\\Vert\\shadow.vert",//
                "D:\\Projects\\VSProjects\\OpenTKProject\\Shaders\\Frag\\shadow.frag"//"D:\\Development\\OpenTKProject\\Shaders\\Frag\\shadow.frag"//
                );

            _isInitialized = true;
        }

        public void RenderShadowMap(List<SceneObject> sceneObjects, LightModule light)
        {
            if (!_isInitialized)
                Initialize();

            _shadowMap.Use();

            Matrix4 lightSpaceMatrix = light.GetLightSpaceMatrix();
            _shadowShader.Use();
            _shadowShader.SetMatrix4("lightSpaceMatrix", lightSpaceMatrix);

            foreach (var obj in sceneObjects)
            {
                EnsureBuffersExist(obj.Model);

                Matrix4 modelMatrix = obj.GetModelMatrix();
                _shadowShader.SetMatrix4("model", modelMatrix);

                var buffers = _modelBuffers[obj.Model];
                GL.BindVertexArray(buffers.vao);
                GL.DrawElements(PrimitiveType.Triangles, obj.Model.VModel.Indices.Count,
                               DrawElementsType.UnsignedInt, 0);
            }

            GL.CullFace(CullFaceMode.Back);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
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

            GL.BufferData(BufferTarget.ArrayBuffer, model.VModel.Vertices.Count() * sizeof(float),
                          model.VModel.Vertices.ToArray(), BufferUsageHint.StaticDraw);

            GL.BufferData(BufferTarget.ElementArrayBuffer, model.VModel.Indices.Count() * sizeof(uint),
                          model.VModel.Indices.ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, strideSize, 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, strideSize, uvOffset);

            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, strideSize, normalOffset);

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
            _shadowShader?.Dispose();
        }
    }
}