public partial class AppMain
{
    public class AMS_VECTOR3I
    {
        public int x;
        public int y;
        public int z;

        public void Assign(AMS_VECTOR3I other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
        }

        public void Assign(VecFx32 v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }
    }
}
