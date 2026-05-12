using System.Runtime.InteropServices;
using OpenTK.Mathematics;
using OpenTKProject;

namespace WinFormsOpenTK
{
    public class CameraModule : ObjectModule
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        private SceneObject _parent;
        private float _zoomSpeed = 2.0f;
        private RotateMode _currentMode = RotateMode.Free;
        private float _speed;
        private SceneObject _targetObject = null;
        private float _orbitDistance = 0.5f;
        private float _minDistance = 0.5f;
        private float _maxDistance = 10.0f;

        public Vector3 Position
        {
            get => _parent.TransformObject.Position;
            private set => _parent.TransformObject.Position = value;
        }
        public Vector3 Front { get; private set; }
        public Vector3 Up { get; private set; }
        public Vector3 Right { get; private set; }
        public float Yaw { get; private set; } = -90.0f;
        public float Pitch { get; private set; } = 0.0f;
        public float MouseSensitivity { get; set; } = 0.1f;
        public RotateMode CurrentMode => _currentMode;

        public void MoveForward(float deltaTime) => _parent.TransformObject.Position += Front * _speed * deltaTime;
        public void MoveBackward(float deltaTime) => _parent.TransformObject.Position -= Front * _speed * deltaTime;
        public void MoveLeft(float deltaTime) => _parent.TransformObject.Position -= Right * _speed * deltaTime;
        public void MoveRight(float deltaTime) => _parent.TransformObject.Position += Right * _speed * deltaTime;
        public void MoveUp(float deltaTime) => _parent.TransformObject.Position += Up * _speed * deltaTime;
        public void MoveDown(float deltaTime) => _parent.TransformObject.Position -= Up * _speed * deltaTime;

        public CameraModule(SceneObject parent, float sp = 1.5f)
        {
            _parent = parent;
            _speed = sp;

            UpdateVectors();
        }

        public override void Start()
        {
        }

        public override void Update(float time)
        {
            Move(time);
        }

        private void Move(float time)
        {
            if (CurrentMode == RotateMode.Free)
            {
                if ((GetAsyncKeyState(KeyStates.VK_W) & 0x8000) != 0) MoveForward(time);
                if ((GetAsyncKeyState(KeyStates.VK_S) & 0x8000) != 0) MoveBackward(time);
                if ((GetAsyncKeyState(KeyStates.VK_A) & 0x8000) != 0) MoveLeft(time);
                if ((GetAsyncKeyState(KeyStates.VK_D) & 0x8000) != 0) MoveRight(time);
            }

            // Эти клавиши работают в обоих режимах
            if ((GetAsyncKeyState(KeyStates.VK_SPACE) & 0x8000) != 0) MoveUp(time);
            if ((GetAsyncKeyState(KeyStates.VK_CONTROL) & 0x8000) != 0) MoveDown(time);

            if ((GetAsyncKeyState(KeyStates.VK_ESCAPE) & 0x8001) != 0)
            {
                Cursor.Show();
            }
        }

        public void Zoom(float delta)
        {
            if (_currentMode == RotateMode.Free)
            {
                _speed += delta * 0.1f;
                _speed = MathHelper.Clamp(_speed, 0.5f, 20.0f);
            }
            else if (_currentMode == RotateMode.Orbit && _targetObject != null)
            {
                _orbitDistance -= delta * _zoomSpeed * 0.1f;
                _orbitDistance = MathHelper.Clamp(_orbitDistance, _minDistance, _maxDistance);
                UpdateOrbitPosition();
            }
        }

        public Matrix4 GetViewMatrix()
        {
            if (_currentMode == RotateMode.Orbit && _targetObject != null)
            {
                UpdateOrbitPosition();
                return Matrix4.LookAt(Position, _targetObject.TransformObject.Position, Up);
            }
            return Matrix4.LookAt(Position, Position + Front, Up);
        }

        public void RotateCamera(float xOffset, float yOffset)
        {
            xOffset *= MouseSensitivity;
            yOffset *= MouseSensitivity;

            Yaw += xOffset;
            Pitch += yOffset;
            Pitch = MathHelper.Clamp(Pitch, -89.0f, 89.0f);

            if (_currentMode == RotateMode.Free)
                UpdateVectors();
            else if (_currentMode == RotateMode.Orbit && _targetObject != null)
                UpdateOrbitPosition();
        }

        private void UpdateVectors()
        {
            Front = Vector3.Normalize(new Vector3(
                MathF.Cos(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch)),
                MathF.Sin(MathHelper.DegreesToRadians(Pitch)),
                MathF.Sin(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch))
            ));
            Right = Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));
        }

        private void UpdateOrbitPosition()
        {
            if (_targetObject == null) return;

            TransformObject to = _targetObject.TransformObject;

            Position = to.Position + new Vector3(
                MathF.Cos(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch)) * _orbitDistance,
                -MathF.Sin(MathHelper.DegreesToRadians(Pitch)) * _orbitDistance,
                MathF.Sin(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch)) * _orbitDistance
            );

            Front = Vector3.Normalize(to.Position - Position);
            Right = Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));
        }

        public void SetFreeMode() => _currentMode = RotateMode.Free;

        public void SetOrbitMode(SceneObject target, float distance = 5.0f)
        {
            if (target == null) return;
            _currentMode = RotateMode.Orbit;
            _targetObject = target;
            _orbitDistance = MathHelper.Clamp(distance, _minDistance, _maxDistance);
            UpdateOrbitPosition();
        }

        public void SetDistanceLimits(float min, float max)
        {
            _minDistance = min;
            _maxDistance = max;
        }
    }
}
