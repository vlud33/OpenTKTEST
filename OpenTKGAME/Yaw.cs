using OpenTK.Mathematics;

namespace GameAddition.Camera
{
    internal sealed class Yaw : AngleRotation
    {
        protected override float Angle { get; set; }

        public Yaw(float angle = 270) : base(angle) { }

        public override void ChangeAngle(float angle)
        {
            Angle = angle;
        }

        public override void IncreaseAngle(float newAngle)
        {
            Angle += newAngle;
        }

        public override void DecreaseAngle(float newAngle)
        {
            Angle -= newAngle;
        }

        public override Vector3 DoRotation()
        {
            Vector3 rotation = Vector3.One;
            float angleInDegrees = GetAngleInRadians();
            rotation.X = (float)Math.Cos(angleInDegrees);
            rotation.Z = (float)Math.Sin(angleInDegrees);
            return rotation;
        }


    }
}