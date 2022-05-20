using OpenTK.Mathematics;

namespace GameAddition.Camera
{
    internal class FPSRotation : CameraRotation
    {
        public Yaw Yaw;
        public Pitch Pitch;

        public FPSRotation(Yaw yaw, Pitch pitch)
        {
            Yaw = yaw;
            Pitch = pitch;
        }

        public void IncreaseYaw(float angle)
        {
            Yaw.IncreaseAngle(angle);
        }

        public void DecreaseYaw(float angle)
        {
            Yaw.DecreaseAngle(angle);
        }

        public void IncreasePitch(float angle)
        {
            Pitch.IncreaseAngle(angle);
        }

        public void DecreasePitch(float angle)
        {
            Pitch.DecreaseAngle(angle);
        }

        public override Vector3 DoRotation()
        {
            Vector3 rotation = Yaw.DoRotation() * Pitch.DoRotation();
            return rotation;
        }
    }
}
