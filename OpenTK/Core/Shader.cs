using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace OpenTKProject
{
    public class Shader : IDisposable
    {

        private string vPath;
        public string PathToVecShader { get { return vPath; } }


        private string fPath;
        public string PathToFragShader { get { return fPath; } }

        int Handle;

        int VertexShader;
        int FragmentShader;

        private bool disposedValue = false;

        public Shader(string vertexPath, string fragmentPath)
        {
            vPath = vertexPath;
            fPath = fragmentPath;

            string VertexShaderSource = LoadStringFromResource(vertexPath);
            string FragmentShaderSource = LoadStringFromResource(fragmentPath);

            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            CompileShaders();
            CombineShaders();
            ClearMemory();
        }

        public string LoadStringFromResource(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string Result = "";

            try
            {
                using (Stream stream = assembly.GetManifestResourceStream(path))
                using (StreamReader reader = new StreamReader(stream))
                {
                    Result = reader.ReadToEnd();
                }
            }
            catch (ArgumentNullException)
            {
                Result = File.ReadAllText(path);
            }

            return Result;
        }

        public void CompileShaders()
        {
            GL.CompileShader(VertexShader);

            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int VertSuccess);
            if (VertSuccess == 0)
            {
                string infoLog = GL.GetShaderInfoLog(VertexShader);
                File.WriteAllText("VertSuccess.txt", infoLog);
            }

            GL.CompileShader(FragmentShader);

            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int fragSuccess);
            string fragLog = GL.GetShaderInfoLog(FragmentShader);
            File.WriteAllText("fragLog.txt", fragLog);

            if (fragSuccess == 0)
            {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                File.WriteAllText("fragLog.txt", infoLog);
            }
        }

        public void CombineShaders()
        {
            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);
            }
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        public int GetUniformLocation(string uniformName)
        {
            return GL.GetUniformLocation(Handle, uniformName);
        }

        public void SetInt(string name, int value)
        {
            int location = GL.GetUniformLocation(Handle, name);
            GL.Uniform1(location, value);
        }

        public void ClearMemory()
        {
            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(Handle, name);
            GL.UniformMatrix4(location, false, ref matrix);
        }

        public void SetVector3(string name, Vector3 v3)
        {
            int location = GL.GetUniformLocation(Handle, name);
            GL.Uniform3(location, v3);
        }

        public void SetFloat(string name, float f)
        {
            int location = GL.GetUniformLocation(Handle, name);
            GL.Uniform1(location, f);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            if (disposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
