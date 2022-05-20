using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace GameCore.Graphics
{
    internal abstract class Shader
    {
        protected string PathToShader;

        public int ShaderObject { get; protected set; }
        
        protected string _sourceCode => File.ReadAllText(PathToShader);

        public Shader(string fullPathToShader)
        {
            PathToShader = fullPathToShader;
            CompileShader();

            string shaderLogInfo = GL.GetShaderInfoLog(ShaderObject);
            if (shaderLogInfo != String.Empty)
            {
                Console.WriteLine("FROM  -  " + fullPathToShader);
                Console.WriteLine(shaderLogInfo);
            }
        }

        protected abstract void CompileShader();
        
        public void DeleteShader()
        {
            GL.DeleteShader(ShaderObject);
        }
    }


    internal sealed class VertexShader : Shader
    {
        public VertexShader(string fullPathToShader) : base(fullPathToShader) { }

        protected override void CompileShader()
        {
            ShaderObject = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(ShaderObject, _sourceCode);
            GL.CompileShader(ShaderObject);
        }
    }


    internal sealed class FragmentShader : Shader
    {
        public FragmentShader(string fullPathToShader) : base(fullPathToShader) { }
        
        protected override void CompileShader()
        {
            ShaderObject = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(ShaderObject, _sourceCode);
            GL.CompileShader(ShaderObject);
        }
    }


    internal sealed class ShaderProgram
    {
        private int _shaderProgramObject;
        private Shader[] _shaders;

        public ShaderProgram(Shader[] shaders)
        {
            _shaders = shaders;
        }

        public void SetUpShaderProgram()
        {
            _shaderProgramObject = GL.CreateProgram();
            AttachShaders();
            GL.LinkProgram(_shaderProgramObject);
        }

        private void AttachShaders()
        {
            foreach (Shader shader in _shaders)
            {
                GL.AttachShader(_shaderProgramObject, shader.ShaderObject);
            }
        }

        public void DeleteShadersFromMemory()
        {
            foreach(Shader shader in _shaders)
            {
                GL.DetachShader(_shaderProgramObject, shader.ShaderObject);
                shader.DeleteShader();
            }
        }

        public void Use()
        {
            GL.UseProgram(_shaderProgramObject);
        }

        public void SetUniform1(string nameOfUniform, int indexOfTextureUnit)
        {
            int uniformLocation = GL.GetUniformLocation(_shaderProgramObject, nameOfUniform);
            GL.Uniform1(uniformLocation, indexOfTextureUnit);
        }

        public void SetMatrix4(string nameOfVar, Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(_shaderProgramObject, nameOfVar);
            GL.UniformMatrix4(location, true, ref matrix);
        }

        public void DeleteShaderProgram()
        {
            GL.DeleteProgram(_shaderProgramObject);
        }

        public void SetVector3(string nameOfUniform, Vector3 vector)
        {
            int location = GL.GetUniformLocation(_shaderProgramObject, nameOfUniform);
            GL.Uniform3(location, vector);
        }

        public void SetFloat(string nameOfUniform, float value)
        {
            int location = GL.GetUniformLocation(_shaderProgramObject, nameOfUniform);
            GL.Uniform1(location, value);
        }
    }
}