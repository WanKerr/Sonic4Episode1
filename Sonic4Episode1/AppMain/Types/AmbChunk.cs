public partial class AppMain
{
    public class AmbChunk
    {
        public AMS_AMB_HEADER amb;
        public byte[] array;
        public int offset;
        public int length;

        public AmbChunk()
        {
        }

        public AmbChunk(byte[] array, int offset, int length, AMS_AMB_HEADER parent)
        {
            this.array = array;
            this.offset = offset;
            this.length = length;
            this.amb = parent;
        }
    }
}
