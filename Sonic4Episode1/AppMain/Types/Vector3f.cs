using Microsoft.Xna.Framework;

public partial class AppMain
{
    public struct Vector3f
    {
        public float x;
        public float y;
        public float z;

        public Vector3f(float X, float Y, float Z)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
        }

        public void Assign(float X, float Y, float Z)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
        }

        public static explicit operator Vector3(Vector3f v)
        {
            return new Vector3(v.x, v.y, v.z);
        }
    }
}
