using OpenTK.Mathematics;

namespace GameAddition.Camera
{
    internal sealed class Pitch : AngleRotation
    {
        protected override float Angle { get; set; }

        public Pitch(float angle = 0) : base(angle) { }

        public override void ChangeAngle(float angle)
        {
            Angle = Math.Clamp(angle, 0f, 90f);
        }

        public override void IncreaseAngle(float newAngle)
        {
            Angle += Math.Clamp(newAngle, -89f, 89f);
        }

        public override void DecreaseAngle(float newAngle)
        {
            Angle -= Math.Clamp(newAngle, -89f, 89f);
        }

        public override Vector3 DoRotation()
        {
            Vector3 rotation = Vector3.One;
            float angleInDegrees = GetAngleInRadians();
            rotation.X = (float)Math.Cos(angleInDegrees);
            rotation.Y = (float)Math.Sin(angleInDegrees);
            rotation.Z = (float)Math.Cos(angleInDegrees);
            return rotation;
        }


    }
}
