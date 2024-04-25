public partial class AppMain
{
    public class AMS_AMB_HEADER
    {
        public char[] file_id = new char[4]
        {
      '#',
      'A',
      'M',
      'B'
        };
        public string dir = "";
        public sbyte[] flag;
        public int file_num;
        public string[] files;
        public sbyte[] types;
        public object[] buf;
        public int[] offsets;
        public int[] lengths;
        public byte[] data;
        public AMS_AMB_HEADER parent;
        public bool is_real_amb = false;
    }
}
