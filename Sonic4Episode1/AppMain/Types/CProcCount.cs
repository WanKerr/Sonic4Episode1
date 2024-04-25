public partial class AppMain
{
    public class CProcCount : CProc
    {
        private uint m_counter;

        public new void operator_brackets()
        {
            ++this.m_counter;
            base.operator_brackets();
        }

        public uint GetCount()
        {
            return this.m_counter;
        }

        public new void SetProc(ITaskAsv.FProc proc)
        {
            this.ResetCounter();
            base.SetProc(proc);
        }

        public new void SetProc()
        {
            this.ResetCounter();
            base.SetProc();
        }

        public CProcCount()
        {
            this.ResetCounter();
        }

        public CProcCount(object it)
          : base(it)
        {
            this.ResetCounter();
        }

        protected void ResetCounter()
        {
            this.m_counter = uint.MaxValue;
        }
    }
}
