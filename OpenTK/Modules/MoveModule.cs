using OpenTK.Mathematics;
using System.Runtime.InteropServices;
using WinFormsOpenTK;

namespace OpenTKProject
{
    public class MoveModule : ObjectModule
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        private SceneObject _owner;
        private PhysicsModule _physics;
        private float _moveSpeed = 10.0f;
        private float _maxSpeed = 20.0f;
        private float _acceleration = 50.0f;
        private float _damping = 10.0f;
        private float _jumpForce = 10.0f;
        private bool _usePhysics = true;
        private MovementMode _movementMode = MovementMode.Velocity;

        private bool _rotateToMovement = true;
        private float _rotationSpeed = 5.0f;
        private Vector3 _lastMovementDirection = -Vector3.UnitZ;
        private float _targetRotationY = 0;
        private bool _smoothRotation = true;

        public float MoveSpeed
        {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }

        public float MaxSpeed
        {
            get => _maxSpeed;
            set => _maxSpeed = value;
        }

        public float Acceleration
        {
            get => _acceleration;
            set => _acceleration = value;
        }

        public float Damping
        {
            get => _damping;
            set => _damping = value;
        }

        public float JumpForce
        {
            get => _jumpForce;
            set => _jumpForce = value;
        }

        public bool UsePhysics
        {
            get => _usePhysics;
            set => _usePhysics = value;
        }

        public MovementMode CurrentMovementMode
        {
            get => _movementMode;
            set => _movementMode = value;
        }

        public bool IsGrounded => _physics?.IsGrounded ?? false;

        public bool RotateToMovement
        {
            get => _rotateToMovement;
            set => _rotateToMovement = value;
        }

        public float RotationSpeed
        {
            get => _rotationSpeed;
            set => _rotationSpeed = value;
        }

        public bool SmoothRotation
        {
            get => _smoothRotation;
            set => _smoothRotation = value;
        }

        public MoveModule(SceneObject owner, float moveSpeed = 10.0f)
        {
            _owner = owner;
            _moveSpeed = moveSpeed;
        }

        public override void Start()
        {
            TransformObject to = _owner.TransformObject;
            _physics = _owner.GetModule<PhysicsModule>();
            _targetRotationY = to.Rotation.Y;
        }

        public override void Update(float time)
        {
            if (_usePhysics && _physics != null)
            {
                Vector3 inputDirection = GetInputDirection();

                if (inputDirection != Vector3.Zero)
                {
                    MoveWithPhysics(inputDirection, time);
                }
                else
                {
                    _physics.ApplyHorizontalDamping(_damping, time, _movementMode);
                }

                UpdateRotation(time);
            }
        }

        private Vector3 GetInputDirection()
        {
            Vector3 direction = Vector3.Zero;

            if ((GetAsyncKeyState(KeyStates.VK_W) & 0x8000) != 0)
                direction.Z -= 1;
            if ((GetAsyncKeyState(KeyStates.VK_S) & 0x8000) != 0)
                direction.Z += 1;
            if ((GetAsyncKeyState(KeyStates.VK_A) & 0x8000) != 0)
                direction.X -= 1;
            if ((GetAsyncKeyState(KeyStates.VK_D) & 0x8000) != 0)
                direction.X += 1;

            if ((GetAsyncKeyState(KeyStates.VK_Q) & 0x8000) != 0)
                direction.Y -= 1;
            if ((GetAsyncKeyState(KeyStates.VK_E) & 0x8000) != 0)
                direction.Y += 1;
            if ((GetAsyncKeyState(KeyStates.VK_CONTROL) & 0x8000) != 0)
                direction.Y -= 1;

            if ((GetAsyncKeyState(KeyStates.VK_SPACE) & 0x8000) != 0)
            {
                Jump();
            }

            return direction;
        }

        private void UpdateRotation(float deltaTime)
        {
            TransformObject to = _owner.TransformObject;
            if (!_rotateToMovement) return;

            Vector3 moveDirection = GetMovementDirection();

            if (moveDirection.LengthSquared > 0.01f)
            {
                _lastMovementDirection = moveDirection;
                _targetRotationY = (float)Math.Atan2(-moveDirection.X, -moveDirection.Z);
            }

            if (_smoothRotation)
            {
                float currentY = to.Rotation.Y;
                float targetY = _targetRotationY;

                float delta = targetY - currentY;
                if (delta > Math.PI) delta -= 2 * (float)Math.PI;
                if (delta < -Math.PI) delta += 2 * (float)Math.PI;

                float newY = currentY + delta * Math.Clamp(_rotationSpeed * deltaTime, 0, 1);
                to.Rotation = new Quaternion(to.Rotation.X, newY, to.Rotation.Z, to.Rotation.W);
            }
            else
            {
                to.Rotation = new Quaternion(to.Rotation.X, _targetRotationY, to.Rotation.Z, to.Rotation.W);
            }
        }

        private Vector3 GetMovementDirection()
        {
            if (_usePhysics && _physics != null)
            {
                Vector3 velocity = _physics.Velocity;
                Vector3 horizontalVelocity = new Vector3(velocity.X, 0, velocity.Z);

                if (horizontalVelocity.LengthSquared > 0.01f)
                {
                    return Vector3.Normalize(horizontalVelocity);
                }
            }

            return _lastMovementDirection;
        }

        private void Jump()
        {
            if (!IsGrounded || _physics?.RigidBody == null) return;

            var rb = _physics.RigidBody;
            if (rb.IsStaticObject || rb.IsKinematicObject) return;

            switch (_movementMode)
            {
                case MovementMode.Force:
                    Vector3 jumpForce = new Vector3(0, _jumpForce * _physics.Mass, 0);
                    _physics.ApplyForce(jumpForce);
                    break;

                case MovementMode.Impulse:
                    Vector3 jumpImpulse = new Vector3(0, _jumpForce * _physics.Mass, 0);
                    _physics.ApplyImpulse(jumpImpulse);
                    break;

                case MovementMode.Velocity:
                    _physics.VerticalVelocity = _jumpForce;
                    break;
            }
        }

        private void MoveWithPhysics(Vector3 direction, float deltaTime)
        {
            var rb = _physics.RigidBody;
            if (rb == null) return;

            if (rb.IsKinematicObject)
            {
                _physics.MoveKinematic(direction * _moveSpeed * deltaTime);
            }
            else
            {
                switch (_movementMode)
                {
                    case MovementMode.Force:
                        // F = m * a
                        Vector3 force = direction * _acceleration * _physics.Mass;
                        _physics.ApplyForce(force);
                        _physics.ClampHorizontalSpeed(_maxSpeed);
                        break;

                    case MovementMode.Impulse:
                        Vector3 impulse = direction * _moveSpeed * _physics.Mass;
                        _physics.ApplyImpulse(impulse);
                        _physics.ClampHorizontalSpeed(_maxSpeed);
                        break;

                    case MovementMode.Velocity:
                        Vector3 targetVelocity = direction * _moveSpeed;
                        Vector3 currentHorizontal = _physics.HorizontalVelocity;

                        float t = Math.Clamp(_acceleration * deltaTime, 0, 1);
                        Vector3 newHorizontal = Vector3.Lerp(currentHorizontal,
                            new Vector3(targetVelocity.X, 0, targetVelocity.Z), t);

                        if (Math.Abs(direction.Y) > 0.1f)
                        {
                            _physics.VerticalVelocity = direction.Y * _moveSpeed;
                        }

                        _physics.SetHorizontalVelocity(newHorizontal);
                        break;
                }
            }
        }

        /// <summary>
        /// Установить направление взгляда вручную
        /// </summary>
        public void SetLookDirection(Vector3 direction)
        {
            if (direction.LengthSquared > 0.01f)
            {
                direction = Vector3.Normalize(new Vector3(direction.X, 0, direction.Z));
                _targetRotationY = (float)Math.Atan2(direction.X, -direction.Z);
                _lastMovementDirection = direction;
            }
        }

        /// <summary>
        /// Повернуть модель к указанной точке
        /// </summary>
        public void LookAt(Vector3 targetPoint)
        {
            TransformObject to = _owner.TransformObject;
            Vector3 direction = targetPoint - to.Position;
            SetLookDirection(direction);
        }

        /// <summary>
        /// Установить угол поворота вручную (в радианах)
        /// </summary>
        public void SetRotationY(float angleY)
        {
            TransformObject to = _owner.TransformObject;
            _targetRotationY = angleY;
            to.Rotation = new Quaternion(to.Rotation.X, angleY, to.Rotation.Z);

            _lastMovementDirection = new Vector3(
                (float)Math.Sin(angleY),
                0,
                -(float)Math.Cos(angleY)
            );
        }

        /// <summary>
        /// Выполнить прыжок
        /// </summary>
        public void PerformJump()
        {
            Jump();
        }

        /// <summary>
        /// Движение в определенном направлении
        /// </summary>
        public void MoveInDirection(Vector3 direction, float deltaTime)
        {
            if (_usePhysics && _physics != null)
            {
                if (direction.LengthSquared > 0)
                {
                    Vector3 horizontalDir = new Vector3(direction.X, 0, direction.Z);
                    if (horizontalDir.LengthSquared > 0)
                        horizontalDir = Vector3.Normalize(horizontalDir);

                    Vector3 finalDirection = new Vector3(
                        horizontalDir.X,
                        direction.Y,
                        horizontalDir.Z
                    );

                    MoveWithPhysics(finalDirection, deltaTime);
                }
                else
                {
                    _physics.ApplyHorizontalDamping(_damping, deltaTime, _movementMode);
                }
            }
        }

        /// <summary>
        /// Мгновенное перемещение
        /// </summary>
        public void Teleport(Vector3 newPosition)
        {
            _physics?.Teleport(newPosition);
        }

        /// <summary>
        /// Остановить движение
        /// </summary>
        public void Stop()
        {
            _physics?.Stop();
        }
    }
}