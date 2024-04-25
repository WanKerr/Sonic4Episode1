public partial class AppMain
{
    public class NNS_LIGHT_TARGET_DIRECTIONAL
    {
        public readonly NNS_VECTOR Position = new NNS_VECTOR();
        public readonly NNS_VECTOR Target = new NNS_VECTOR();
        public uint User;
        public NNS_RGBA Color;
        public float Intensity;
        public float InnerRange;
        public float OuterRange;
        public float FallOffStart;
        public float FallOffEnd;
        public float dummy;

        public static explicit operator NNS_LIGHT_PARALLEL(
          NNS_LIGHT_TARGET_DIRECTIONAL light)
        {
            return new NNS_LIGHT_PARALLEL(light);
        }

        public static explicit operator NNS_LIGHT_POINT(
          NNS_LIGHT_TARGET_DIRECTIONAL light)
        {
            return new NNS_LIGHT_POINT(light);
        }

        public static explicit operator NNS_LIGHT_TARGET_SPOT(
          NNS_LIGHT_TARGET_DIRECTIONAL light)
        {
            return new NNS_LIGHT_TARGET_SPOT(light);
        }

        public static explicit operator NNS_LIGHT_ROTATION_SPOT(
          NNS_LIGHT_TARGET_DIRECTIONAL light)
        {
            return new NNS_LIGHT_ROTATION_SPOT(light);
        }
    }
}
