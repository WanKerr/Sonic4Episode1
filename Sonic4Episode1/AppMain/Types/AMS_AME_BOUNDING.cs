public partial class AppMain
{
    public class AMS_AME_BOUNDING
    {
        public readonly NNS_VECTOR4D center = GlobalPool<NNS_VECTOR4D>.Alloc();
        public float radius;
        public float radius2;

        public AMS_AME_BOUNDING Assign(AMS_AME_BOUNDING bound)
        {
            this.center.Assign(bound.center);
            this.radius = bound.radius;
            this.radius2 = bound.radius2;
            return this;
        }

        public void Clear()
        {
            this.center.Clear();
            this.radius = 0.0f;
            this.radius2 = 0.0f;
        }
    }
}
