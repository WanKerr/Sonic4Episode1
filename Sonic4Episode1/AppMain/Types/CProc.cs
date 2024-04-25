public partial class AppMain
{
    public class CProc
    {
        private ITaskAsv.FProc m_proc;
        private object m_it;

        public void operator_brackets()
        {
            if (this.m_it == null || this.IsNoneProc())
                return;
            this.m_proc();
        }

        public bool IsNoneProc()
        {
            return this.m_proc == null;
        }

        public bool IsProc(ITaskAsv.FProc proc)
        {
            return this.m_proc == proc;
        }

        public bool IsProc()
        {
            return this.IsNoneProc();
        }

        public ITaskAsv.FProc GetProc()
        {
            return this.m_proc;
        }

        public void SetTarget(object it)
        {
            this.m_it = it;
            this.SetProc();
        }

        public void SetTarget()
        {
            this.m_it = null;
            this.SetProc();
        }

        public void SetProc(ITaskAsv.FProc proc)
        {
            this.m_proc = proc;
        }

        public void SetProc()
        {
            this.m_proc = null;
        }

        public CProc()
        {
            this.m_it = null;
            this.m_proc = null;
        }

        public CProc(object it)
        {
            this.m_it = it;
            this.m_proc = null;
        }
    }
}
