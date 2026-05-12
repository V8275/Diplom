using OpenTK.Graphics.OpenGL;
using StbImageSharp;

namespace OpenTKProject
{
    public class Texture
    {
        private string path;
        public string PathToTexture { get { return path; } }
        public int Handle;

        public Texture(string pathTexture, bool generateMipMaps = true,  PixelInternalFormat PixelFormat = PixelInternalFormat.Rgba)
        {
            path = pathTexture;
            Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            StbImage.stbi_set_flip_vertically_on_load(1);

            ImageResult image = ImageResult.FromStream(File.OpenRead(pathTexture), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelFormat, image.Width, image.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            if(generateMipMaps == true)
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
    }
}
