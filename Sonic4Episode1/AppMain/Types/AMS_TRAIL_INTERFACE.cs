public partial class AppMain
{
    public class AMS_TRAIL_INTERFACE
    {
        public AMS_TRAIL_PARTSDATA[] trailData = New<AMS_TRAIL_PARTSDATA>(8);
        public AMS_TRAIL_EFFECT[] trailEffect = new AMS_TRAIL_EFFECT[8];
        public short trailId;
        public short trailNum;
        public short trailState;
    }
}
