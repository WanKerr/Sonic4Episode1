public partial class AppMain
{
    public class AMS_TRAIL_PARTSDATA
    {
        public AMS_TRAIL_PARTS[] parts = New<AMS_TRAIL_PARTS>(64);
        public AMS_TRAIL_PARTS trailHead = new AMS_TRAIL_PARTS();
        public AMS_TRAIL_PARTS trailTail = new AMS_TRAIL_PARTS();

        public void Clear()
        {
            foreach (AMS_TRAIL_PARTS part in this.parts)
                part.Clear();
            this.trailHead.Clear();
            this.trailTail.Clear();
        }
    }
}
