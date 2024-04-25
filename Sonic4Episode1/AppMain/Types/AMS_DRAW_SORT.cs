public partial class AppMain
{
    public class AMS_DRAW_SORT
    {
        public int key;
        public AMS_COMMAND_HEADER command;

        public AMS_DRAW_SORT Assign(AMS_DRAW_SORT sort)
        {
            this.key = sort.key;
            this.command = sort.command;
            return this;
        }
    }
}
