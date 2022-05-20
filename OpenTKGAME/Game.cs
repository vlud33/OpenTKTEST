using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using GameCore.Graphics;
using System.Diagnostics;
using GameAddition.Camera;
using GameAddition.LoadModel;
using System.Runtime.InteropServices;

namespace GameCore
{
    public class Window : GameWindow
    {
        private int _width;
        private int _height;

        private float _sensativity = 4f;
        private float _speed = 3f;

        private Vector3 _lightPosition = new Vector3(1.2f, 1.0f, 2f);

        private NativeWindowSettings _windowSettings;
        private VertexArrayBuffer _vertexBuffer;
        private VertexArray _vertexArray, _vertexArrayForLight;
        private FPSCamera _fPSCamera;
        private ShaderProgram _shaderProgram, _shaderProgramForLight;
        private FragmentShader _fragmentShader;
        private VertexShader _vertexShader;
        private Stopwatch _time;

        private readonly float[] _vertices = new float[]
        {
        -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
         0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
         0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
         0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
        -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,

        -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
         0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
        -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,

        -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

         0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
         0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
         0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
         0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
         0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
         0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

        -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
         0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
         0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
         0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,

        -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
         0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
        -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
        -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
        };

        private float _aspectRatio => (float)_width / (float)_height;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings windowSettings) : base(gameWindowSettings, windowSettings) 
        {
            _width = windowSettings.Size.X;
            _height = windowSettings.Size.Y;
            _windowSettings = windowSettings;

            CursorVisible = false;
            CursorGrabbed = true;
            CenterWindow();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.5f, 0.2f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            
            // object
            _vertexArray = new VertexArray();
            _vertexArray.Bind();

            _vertexBuffer = new VertexArrayBuffer();
            _vertexBuffer.Bind();

            _vertexBuffer.AddVerticies(_vertices, BufferUsageHint.StaticDraw);

            // 3 - xyz, 7 - xyz + rgba
            VertexAttribPointer.Set(0, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 0);
            VertexAttribPointer.Set(1, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));

            _shaderProgram = GetShaderProgram("Shaders\\Fragment_Shader.frag", "Shaders\\Vertex_Shader.vert");

            //set color 
            _shaderProgram.SetVector3("LightColor", new Vector3(0.9f));
            _shaderProgram.SetVector3("ObjectColor", new Vector3(1f, 0.5f, 0.31f));

            
            //light
            _vertexArrayForLight = new VertexArray();
            _vertexArrayForLight.Bind();

            _shaderProgramForLight = GetShaderProgram("Shaders\\Fragment_Shader_For_Light.frag", "Shaders\\Vertex_Shader.vert");

            VertexAttribPointer.Set(0, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 0);
            VertexAttribPointer.Set(1, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));

            _time = new Stopwatch();
            _time.Start();

            _fPSCamera = new FPSCamera(_aspectRatio, new FPSRotation(new Yaw(0), new Pitch()));

            CleanBuffers();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            _shaderProgram.Use();

            Matrix4 model = CreateModelTransformation(/*_time.Elapsed.TotalMilliseconds*/0, 0, 0, 0);

            _shaderProgram.SetMatrix4("model", model);
            _shaderProgram.SetMatrix4("view", _fPSCamera.GetViewMatrix());
            _shaderProgram.SetMatrix4("projection", _fPSCamera.GetProjection());
            _shaderProgram.SetVector3("CameraPosition", _fPSCamera.GetCameraPosition());

            Vector3 lightColor = new Vector3();
            float time = DateTime.Now.Second + DateTime.Now.Millisecond / 1000f;
            lightColor.X = (float)MathF.Sin(time * 2f);
            lightColor.Y = (float)MathF.Sin(time * 0.7f);
            lightColor.Z = (float)MathF.Sin(time * 1.3f);

            _shaderProgram.SetVector3("light.position", _lightPosition);
            _shaderProgram.SetVector3("light.ambient", new Vector3(0.2f) * lightColor);
            _shaderProgram.SetVector3("light.diffuse", new Vector3(0.7f) * lightColor);
            _shaderProgram.SetVector3("light.specular", new Vector3(1.0f));

            _shaderProgram.SetVector3("material.ambient", new Vector3(1f, 0.5f, 0.31f));
            _shaderProgram.SetVector3("material.diffuse", new Vector3(1f, 0.5f, 0.31f));
            _shaderProgram.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _shaderProgram.SetFloat("material.shininess", 32f);

            _vertexArray.Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            _shaderProgramForLight.Use();

            Matrix4 light = CreateModelTransformation(0, _lightPosition.X, _lightPosition.Y, _lightPosition.Z);

            //_lightPosition.X += 1 * ((float)e.Time);

            light *= Matrix4.CreateTranslation(_lightPosition);

            _shaderProgramForLight.SetMatrix4("model", light);
            _shaderProgramForLight.SetMatrix4("view", _fPSCamera.GetViewMatrix());
            _shaderProgramForLight.SetMatrix4("projection", _fPSCamera.GetProjection());

            _vertexArrayForLight.Bind();

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (IsFocused == false)
            {
                return;
            }

            _fPSCamera.DecreasePitch(MouseState.Delta.Y * _sensativity * (float)args.Time);
            _fPSCamera.IncreseYaw(MouseState.Delta.X * _sensativity * (float)args.Time);

            _fPSCamera.IncreaseDirectionByRotation();

            if (KeyboardState.IsKeyDown(Keys.W))
            {
                _fPSCamera.IncreasePosition(_fPSCamera.GetDirection() * _speed * (float)args.Time);
            }

            if (KeyboardState.IsKeyDown(Keys.S))
            {
                _fPSCamera.DecreasePosition(_fPSCamera.GetDirection() * _speed * (float)args.Time);
            }

            if (KeyboardState.IsKeyDown(Keys.F12))
            {
                unsafe
                {
                    GLFW.SetWindowMonitor(WindowPtr, CurrentMonitor.ToUnsafePtr<OpenTK.Windowing.GraphicsLibraryFramework.Monitor>(), 600, 600, 1920, 1080, 60);
                }
                GL.Viewport(0, 0, 1920, 1080);
                _width = 1920;
                _height = 1080;
                _fPSCamera.SetAspectRatio(_aspectRatio);
            }

            if (KeyboardState.IsKeyDown(Keys.A))
            {
                _fPSCamera.DecreasePosition(Vector3.Normalize(Vector3.Cross(_fPSCamera.GetDirection(), _fPSCamera.Up)) * _speed * (float)args.Time);
            }

            if (KeyboardState.IsKeyDown(Keys.D))
            {
                _fPSCamera.IncreasePosition(Vector3.Normalize(Vector3.Cross(_fPSCamera.GetDirection(), _fPSCamera.Up)) * _speed * (float)args.Time);
            }

            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                _fPSCamera.IncreasePosition(_fPSCamera.Up * _speed * (float)args.Time);
            }

            if (KeyboardState.IsKeyDown(Keys.LeftShift))
            {
                _fPSCamera.DecreasePosition(_fPSCamera.Up * _speed * (float)args.Time);
            }

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (KeyboardState.IsKeyDown(Keys.F2))
            {
                CursorVisible = true;
            }

            if (KeyboardState.IsKeyDown(Keys.F1))
            {
                CursorVisible = false;
            }

            _shaderProgram.SetMatrix4("view", _fPSCamera.GetViewMatrix());
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _fPSCamera.DecreaseFOV(e.OffsetY);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            _shaderProgram.DeleteShaderProgram();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            _width = e.Width;
            _height = e.Height;
            _fPSCamera.SetAspectRatio(_aspectRatio);
        }

        private Matrix4 CreateModelTransformation(double totalMilliseconds=1.5, float positionX = 4f, float positionY = 0f, float positionZ = 2f)
        {
            Matrix4 model = Matrix4.Identity/* * Matrix4.CreateRotationY(MathF.Sin((float)totalMilliseconds * 0.00001f) * 100)*/;
            //model *= Matrix4.CreateRotationZ((float)Math.PI / 6);
            model *= Matrix4.CreateTranslation(positionX, positionY, positionZ);
            return model;
        }

        private ShaderProgram GetShaderProgram(string pathToFragmentShader, string pathToVertexShader)
        {

            if (File.Exists(pathToVertexShader) == false)
                throw new ArgumentException($"{pathToVertexShader}  -  Doesn't exists");
            
            if (File.Exists(pathToFragmentShader) == false)
                throw new ArgumentException($"{pathToFragmentShader}  -  Doesn't exists");

            _fragmentShader = new FragmentShader(pathToFragmentShader);
            _vertexShader = new VertexShader(pathToVertexShader);

            // create shader program in order to combine those shaders
            ShaderProgram program = new ShaderProgram(new Shader[] { _vertexShader, _fragmentShader });
            program.SetUpShaderProgram();
            program.DeleteShadersFromMemory();
            program.Use();
            return program;
        }

        private void CleanBuffers()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
    }
}
