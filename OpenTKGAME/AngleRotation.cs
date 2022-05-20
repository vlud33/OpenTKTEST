using OpenTK.Mathematics; 

namespace GameAddition.Camera
{
    internal abstract class AngleRotation
    {
        protected abstract float Angle { get; set; }

        public AngleRotation(float angle)
        {
            ChangeAngle(angle);
        }

        public float GetAngleInRadians()
        {
            return MathHelper.DegreesToRadians(Angle);
        }
        
        public abstract void ChangeAngle(float angle);

        public abstract void IncreaseAngle(float newAngle);

        public abstract void DecreaseAngle(float newAngle);

        public abstract Vector3 DoRotation();
    }
}
