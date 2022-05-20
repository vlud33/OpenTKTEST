using OpenTK.Mathematics;

namespace GameAddition.Camera
{
    abstract internal class Camera
    {
    }

    internal sealed class FPSCamera : Camera
    {
        private Vector3 _position = new Vector3(0, 0, 3f);
        private Vector3 _direction = new Vector3(0, 0, -1f);
        public readonly Vector3 Up = Vector3.UnitY;

        // (width / height)
        private float _aspectRatio;
        private float _depthOfNearPlane = 0.01f;
        private float _depthOfFarPlane = 100f;
        private float _fov = 45f;

        private FPSRotation _fPSRotation;
        
        public FPSCamera(float aspectRatio, FPSRotation fPSRotation) 
        {
            _aspectRatio = aspectRatio;
            _fPSRotation = fPSRotation;
        }

        public Vector3 GetCameraPosition()
        {
            return _position;
        }

        public void SetAspectRatio(float aspectRatio)
        {
            _aspectRatio = aspectRatio;
        }

        public void IncreasePosition(Vector3 newPosition)
        {
            _position += newPosition;
        }

        public void DecreasePosition(Vector3 newPosition)
        {
            _position -= newPosition;
        }

        public void SetDepthOfNearPlane(float depth)
        {
            _depthOfNearPlane = Math.Clamp(depth, 0.01f, 1f);
        }

        public void SetDepthOfFarPlane(float depth)
        {
            _depthOfFarPlane = Math.Clamp(depth, 100f, 150f);
        }

        public void SetFov(float fov)
        {
            _fov = Math.Clamp(fov, 1f, 90f);
        }

        public void IncreseYaw(float angle)
        {
            _fPSRotation.IncreaseYaw(angle);
        }

        public void DecreaseYaw(float angle)
        {
            _fPSRotation.DecreaseYaw(angle);
        }

        public void IncreasePitch(float angle)
        {
            _fPSRotation.IncreasePitch(angle);
        }

        public void DecreasePitch(float angle)
        {
            _fPSRotation.DecreasePitch(angle);
        }

        public void IncreaseDirectionByRotation()
        {
            _direction += _fPSRotation.DoRotation();
            //_direction.X += MathF.Cos(MathHelper.DegreesToRadians(_fPSRotation.Pitch.GetAngleInRadians()) * MathF.Cos(MathHelper.DegreesToRadians(_yaw));
            //_direction.Y += (float)MathF.Sin(MathHelper.DegreesToRadians(_pitch));
            //_direction.Z += MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Sin(MathHelper.DegreesToRadians(_yaw));
        }

        public void DecreaseDirection()
        {
            _direction -= _fPSRotation.DoRotation();
        }

        public void IncreaseFOV(float fov)
        {
            _fov += fov;
            _fov = Math.Clamp(fov, 1f, 90f);
        }

        public void DecreaseFOV(float fov)
        {
            _fov -= fov;
            _fov = Math.Clamp(_fov, 1f, 90f);
        }

        public Matrix4 GetViewMatrix()
        {
            _direction = Vector3.Normalize(_direction);
            return Matrix4.LookAt(_position, _position + _direction, Up);
        }

        public Matrix4 GetProjection()
        {
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(_fov), _aspectRatio, _depthOfNearPlane, _depthOfFarPlane); 
        }

        public Vector3 GetDirection()
        {
            return _direction;
        }
    }
}
