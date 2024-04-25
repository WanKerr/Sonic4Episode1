public partial class AppMain
{
    public class GMS_OBJECT_WORK_ATK
    {
        public readonly OBS_OBJECT_WORK obj = OBS_OBJECT_WORK.Create();
        public readonly OBS_RECT_WORK[] rect_work = new OBS_RECT_WORK[2];

        public GMS_OBJECT_WORK_ATK()
        {
            this.rect_work[0] = new OBS_RECT_WORK();
            this.rect_work[1] = new OBS_RECT_WORK();
        }
    }
}
