using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTKProject;
using WinFormsOpenTK;

public class Renderer
{
    private readonly CameraModule _cameraController;
    private readonly ShadowMapRenderer _shadowMapRenderer;
    private readonly ModelRenderer _modelRenderer;
    private readonly LightManager _lightManager;

    public Renderer(CameraModule cameraController, LightManager lightManager, int shadowMapSize = 2048)
    {
        _cameraController = cameraController;
        _lightManager = lightManager;
        _shadowMapRenderer = new ShadowMapRenderer(shadowMapSize, shadowMapSize);
        _modelRenderer = new ModelRenderer();
    }

    public void Initialize()
    {
        _shadowMapRenderer.Initialize();
    }

    public void Render(List<SceneObject> renderO, Vector2i windowSize)
    {
        var sceneObjects = renderO.Where(a => a.Model != null).ToList();

        var mainLight = _lightManager.GetLights().FirstOrDefault();
        if (mainLight != null)
        {
            _shadowMapRenderer.RenderShadowMap(sceneObjects, mainLight);
        }

        GL.Viewport(0, 0, windowSize.X, windowSize.Y);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        foreach (var obj in sceneObjects)
        {
            _modelRenderer.RenderModel(obj, _cameraController, _lightManager,
                _shadowMapRenderer.ShadowMapTexture, windowSize);
        }
    }
}