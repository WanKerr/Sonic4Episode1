public partial class AppMain
{
    public class GMS_WATER_SURFACE_DATA
    {
        public AMS_AMB_HEADER amb_header;
        public bool flag_load_object;

        internal void Clear()
        {
            this.amb_header = null;
            this.flag_load_object = false;
        }
    }
}
