public partial class AppMain
{
    public class AMS_COMMAND_HEADER
    {
        public uint state;
        public int command_id;
        public object param;

        public AMS_COMMAND_HEADER(AMS_COMMAND_HEADER pHeader)
        {
            this.Assign(pHeader);
        }

        public AMS_COMMAND_HEADER()
        {
        }

        public void Assign(AMS_COMMAND_HEADER pHeader)
        {
            this.state = pHeader.state;
            this.command_id = pHeader.command_id;
            this.param = pHeader.param;
        }
    }
}
