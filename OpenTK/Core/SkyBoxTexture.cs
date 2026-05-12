using OpenTK.Graphics.OpenGL;
using StbImageSharp;

namespace OpenTKProject
{
    public class SkyBoxTexture
    {
        private Texture[] sides = new Texture[6];
        private int cubemapHandle;
        public int Handle { get { return cubemapHandle; } }

        public SkyBoxTexture(string[] texturePaths, bool generateMipMaps = true)
        {
            if (texturePaths.Length != 6)
                throw new ArgumentException("Skybox requires exactly 6 textures");

            cubemapHandle = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, cubemapHandle);

            StbImage.stbi_set_flip_vertically_on_load(0);

            for (int i = 0; i < 6; i++)
            {
                ImageResult image = ImageResult.FromStream(
                    File.OpenRead(texturePaths[i]),
                    ColorComponents.RedGreenBlueAlpha
                );

                GL.TexImage2D(
                    TextureTarget.TextureCubeMapPositiveX + i,  // +X, -X, +Y, -Y, +Z, -Z
                    0,
                    PixelInternalFormat.Rgba,
                    image.Width,
                    image.Height,
                    0,
                    PixelFormat.Rgba,
                    PixelType.UnsignedByte,
                    image.Data
                );
            }

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter,
                generateMipMaps ? (int)TextureMinFilter.LinearMipmapLinear : (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS,
                (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT,
                (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR,
                (int)TextureWrapMode.ClampToEdge);

            if (generateMipMaps)
                GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);
        }

        public Texture GetSide(int index)
        {
            return sides[index];
        }
    }
}
