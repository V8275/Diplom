using OpenTK.Graphics.OpenGL;

namespace OpenTKProject
{
    public class ShadowMap
    {
        public int DepthMapFBO { get; private set; }
        public int DepthMapTexture { get; private set; }
        public int ShadowWidth { get; private set; }
        public int ShadowHeight { get; private set; }

        private bool _isInitialized = false;

        public ShadowMap(int width = 4096, int height = 4096)
        {
            ShadowWidth = width;
            ShadowHeight = height;
        }

        public void Initialize()
        {
            if (_isInitialized)
                return;

            DepthMapFBO = GL.GenFramebuffer();

            DepthMapTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, DepthMapTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent24,
                          ShadowWidth, ShadowHeight, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            float[] borderColor = { 1.0f, 1.0f, 1.0f, 1.0f };
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, DepthMapFBO);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment,
                                    TextureTarget.Texture2D, DepthMapTexture, 0);

            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                throw new Exception("Framebuffer is not complete!");
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            _isInitialized = true;
        }

        public void Use()
        {
            if (!_isInitialized)
                Initialize();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, DepthMapFBO);
            GL.Viewport(0, 0, ShadowWidth, ShadowHeight);
            GL.Clear(ClearBufferMask.DepthBufferBit);
        }

        public void UnUse()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Dispose()
        {
            if (_isInitialized)
            {
                GL.DeleteFramebuffer(DepthMapFBO);
                GL.DeleteTexture(DepthMapTexture);
            }
        }
    }
}