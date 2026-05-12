using OpenTK.Mathematics;
using BulletSharp;
using WinFormsOpenTK;

namespace OpenTKProject
{
    public class PhysicsModule : ObjectModule
    {
        private SceneObject _owner;
        private PhysicsWorld _physicsWorld;
        private RigidBody _rigidBody;
        private float _mass;
        private bool _isKinematic;

        public RigidBody RigidBody => _rigidBody;
        public float Mass => _rigidBody != null && _rigidBody.InvMass > 0 ? 1.0f / _rigidBody.InvMass : 0;
        public Vector3 Velocity
        {
            get
            {
                if (_rigidBody == null) return Vector3.Zero;
                return new Vector3(_rigidBody.LinearVelocity.X, _rigidBody.LinearVelocity.Y, _rigidBody.LinearVelocity.Z);
            }
            set
            {
                if (_rigidBody != null && !_rigidBody.IsStaticObject && !_isKinematic)
                {
                    _rigidBody.LinearVelocity = new BulletSharp.Math.Vector3(value.X, value.Y, value.Z);
                }
            }
        }

        public Vector3 HorizontalVelocity
        {
            get
            {
                var v = Velocity;
                return new Vector3(v.X, 0, v.Z);
            }
            set
            {
                var current = Velocity;
                Velocity = new Vector3(value.X, current.Y, value.Z);
            }
        }

        public float VerticalVelocity
        {
            get => Velocity.Y;
            set
            {
                var current = Velocity;
                Velocity = new Vector3(current.X, value, current.Z);
            }
        }

        public float HorizontalSpeed => HorizontalVelocity.Length;
        public bool IsGrounded => Math.Abs(VerticalVelocity) < 0.1f;

        public PhysicsModule(SceneObject owner, PhysicsWorld physicsWorld, float mass = 1.0f, bool isKinematic = false)
        {
            _owner = owner;
            _physicsWorld = physicsWorld;
            _mass = mass;
            _isKinematic = isKinematic;
        }

        public override void Start()
        {
            _rigidBody = _physicsWorld.AddRigidBody(_owner, _mass, _isKinematic);
        }

        public override void Update(float time)
        {
            // Обновление в PhysicsWorld.Update
        }

        public void ApplyForce(Vector3 force)
        {
            if (_rigidBody != null && !_rigidBody.IsStaticObject && !_isKinematic)
            {
                _rigidBody.ApplyCentralForce(new BulletSharp.Math.Vector3(force.X, force.Y, force.Z));
            }
        }

        public void ApplyImpulse(Vector3 impulse)
        {
            if (_rigidBody != null && !_rigidBody.IsStaticObject && !_isKinematic)
            {
                _rigidBody.ApplyCentralImpulse(new BulletSharp.Math.Vector3(impulse.X, impulse.Y, impulse.Z));
            }
        }

        public void ApplyForceToHorizontal(Vector3 force)
        {
            if (_rigidBody != null && !_rigidBody.IsStaticObject && !_isKinematic)
            {
                _rigidBody.ApplyCentralForce(new BulletSharp.Math.Vector3(force.X, 0, force.Z));
            }
        }

        public void ApplyImpulseToHorizontal(Vector3 impulse)
        {
            if (_rigidBody != null && !_rigidBody.IsStaticObject && !_isKinematic)
            {
                _rigidBody.ApplyCentralImpulse(new BulletSharp.Math.Vector3(impulse.X, 0, impulse.Z));
            }
        }

        public void SetVelocity(Vector3 velocity)
        {
            if (_rigidBody != null && !_rigidBody.IsStaticObject && !_isKinematic)
            {
                _rigidBody.LinearVelocity = new BulletSharp.Math.Vector3(velocity.X, velocity.Y, velocity.Z);
            }
        }

        public void SetHorizontalVelocity(Vector3 horizontalVelocity)
        {
            if (_rigidBody != null && !_rigidBody.IsStaticObject && !_isKinematic)
            {
                var current = _rigidBody.LinearVelocity;
                _rigidBody.LinearVelocity = new BulletSharp.Math.Vector3(
                    horizontalVelocity.X,
                    current.Y,
                    horizontalVelocity.Z
                );
            }
        }

        public void SetAngularVelocity(Vector3 angularVelocity)
        {
            if (_rigidBody != null && !_rigidBody.IsStaticObject && !_isKinematic)
            {
                _rigidBody.AngularVelocity = new BulletSharp.Math.Vector3(angularVelocity.X, angularVelocity.Y, angularVelocity.Z);
            }
        }

        public void StopHorizontalMovement()
        {
            if (_rigidBody != null && !_rigidBody.IsStaticObject && !_isKinematic)
            {
                var current = _rigidBody.LinearVelocity;
                _rigidBody.LinearVelocity = new BulletSharp.Math.Vector3(0, current.Y, 0);
            }
        }

        public void Stop()
        {
            if (_rigidBody != null && !_rigidBody.IsStaticObject && !_isKinematic)
            {
                _rigidBody.LinearVelocity = BulletSharp.Math.Vector3.Zero;
                _rigidBody.AngularVelocity = BulletSharp.Math.Vector3.Zero;
            }
        }

        public void ClampHorizontalSpeed(float maxSpeed)
        {
            var horizontal = HorizontalVelocity;
            if (horizontal.Length > maxSpeed)
            {
                horizontal = Vector3.Normalize(horizontal) * maxSpeed;
                SetHorizontalVelocity(horizontal);
            }
        }

        public void ApplyHorizontalDamping(float damping, float deltaTime, MovementMode mode)
        {
            float horizontalSpeed = HorizontalSpeed;
            if (horizontalSpeed <= 0.01f) return;

            switch (mode)
            {
                case MovementMode.Force:
                    float dampingForce = damping * Mass;
                    float dampingSpeed = damping * deltaTime / Mass;

                    if (dampingSpeed >= horizontalSpeed)
                    {
                        StopHorizontalMovement();
                    }
                    else
                    {
                        Vector3 dampingDir = -HorizontalVelocity.Normalized() * dampingForce;
                        ApplyForceToHorizontal(dampingDir);
                    }
                    break;

                case MovementMode.Impulse:
                case MovementMode.Velocity:
                    float newSpeed = Math.Max(0, horizontalSpeed - damping * deltaTime);
                    if (newSpeed > 0)
                    {
                        Vector3 newVelocity = Vector3.Normalize(HorizontalVelocity) * newSpeed;
                        SetHorizontalVelocity(newVelocity);
                    }
                    else
                    {
                        StopHorizontalMovement();
                    }
                    break;
            }
        }

        public void MoveKinematic(Vector3 deltaPosition)
        {
            if (_rigidBody == null || !_rigidBody.IsKinematicObject) return;

            TransformObject to = _owner.TransformObject;

            var newPos = to.Position + deltaPosition;
            _rigidBody.WorldTransform = BulletSharp.Math.Matrix.Translation(
                newPos.X, newPos.Y, newPos.Z);
            to.Position = newPos;
        }

        public void Teleport(Vector3 newPosition)
        {
            _owner.TransformObject.Position = newPosition;

            if (_rigidBody != null)
            {
                _rigidBody.WorldTransform = BulletSharp.Math.Matrix.Translation(
                    newPosition.X, newPosition.Y, newPosition.Z);
                _rigidBody.LinearVelocity = BulletSharp.Math.Vector3.Zero;
                _rigidBody.AngularVelocity = BulletSharp.Math.Vector3.Zero;
            }
        }
    }

    // Режимы движения
    public enum MovementMode
    {
        Direct,        // Прямое изменение позиции
        Force,         // Через физику
        Impulse,       // Через импульс
        Velocity       // Через прямую установку скорости
    }
}