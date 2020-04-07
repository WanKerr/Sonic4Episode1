namespace gs.backup
{
    public abstract class SBase
    {
        public bool isDirty;

        public virtual bool GetDirty()
        {
            return isDirty;
        }
    }
}