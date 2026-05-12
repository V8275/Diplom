using OpenTKProject;
using static BulletSharp.Dbvt;

namespace WinFormsOpenTK
{
    public static class FormatConverter
    {
        public static ModelFormat GetFormatFromString(string formatType)
        {
            switch (formatType)
            {
                case "obj":
                    return ModelFormat.Obj;
                case "gltf":
                    return ModelFormat.Gltf;
                default:
                    return ModelFormat.Obj;
            }
        }
    }
}
