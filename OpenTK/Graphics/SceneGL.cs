using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;

namespace WinFormsOpenTK
{
    public class SceneGL
    {
        public event Action OpenGLInitialized;

        private GLControl _glControl;
        private Renderer _renderer;
        private CameraModule _linearMoveModule;

        private bool _isMouseCaptured;
        private bool _firstMove;
        private int _lastX;
        private int _lastY;

        public SceneGL(ref GLControl control, ref Renderer renderer)
        {
            _glControl = control;
            _renderer = renderer;
        }

        public void SetCamera(CameraModule lMM)
        {
            _linearMoveModule = lMM;
        }

        public void InitializeOpenGL()
        {
            _glControl.Load += GlControl_Load;
            _glControl.Resize += GlControl_Resize;
            _glControl.MouseDown += GlControl_MouseDown;
            _glControl.MouseMove += GlControl_MouseMove;
            _glControl.LostFocus += GlControl_LostFocus;
            _glControl.MouseWheel += GlControl_MouseWheel;
            _glControl.TabStop = true;
        }

        private void GlControl_Load(object sender, EventArgs e)
        {
            _glControl.MakeCurrent();

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _renderer.Initialize();

            OpenGLInitialized?.Invoke();
            Console.WriteLine("OpenGL initialized");
        }

        private void GlControl_Resize(object sender, EventArgs e)
        {
            if (_glControl.IsDisposed) return;
            _glControl.MakeCurrent();
            GL.Viewport(0, 0, _glControl.Width, _glControl.Height);
        }

        private void GlControl_MouseDown(object sender, MouseEventArgs e)
        {
            _glControl.Focus();
            _isMouseCaptured = true;
            _firstMove = true;
            Cursor.Hide();
        }

        private void GlControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isMouseCaptured) return;

            if (_firstMove)
            {
                _lastX = e.X;
                _lastY = e.Y;
                _firstMove = false;
                return;
            }

            float xOffset = e.X - _lastX;
            float yOffset = _lastY - e.Y;

            _lastX = e.X;
            _lastY = e.Y;

            _linearMoveModule.RotateCamera(xOffset, yOffset);

            if (_isMouseCaptured)
            {
                var centerX = _glControl.Width / 2;
                var centerY = _glControl.Height / 2;

                if (Math.Abs(e.X - centerX) > 100 || Math.Abs(e.Y - centerY) > 100)
                {
                    Cursor.Position = _glControl.PointToScreen(new Point(centerX, centerY));
                    _lastX = centerX;
                    _lastY = centerY;
                }
            }
        }

        private void GlControl_LostFocus(object sender, EventArgs e)
        {
            _isMouseCaptured = false;
            Cursor.Show();
        }

        private void GlControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (_isMouseCaptured)
            {
                // e.Delta равно 120 за один шаг колесика
                float delta = e.Delta / 120.0f;
                _linearMoveModule.Zoom(delta);
            }
        }
    }
}
