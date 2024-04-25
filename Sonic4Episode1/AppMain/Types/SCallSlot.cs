using System.Reflection;

public partial class AppMain
{
    public struct SCallSlot
    {
        public MethodInfo m_info;
        public bool m_bNoCast;

        public SCallSlot(MethodInfo info, bool bNoCast)
        {
            this.m_info = info;
            this.m_bNoCast = bNoCast;
        }
    }
}
