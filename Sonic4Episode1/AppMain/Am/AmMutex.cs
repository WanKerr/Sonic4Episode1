using System.Threading;

public partial class AppMain
{
    private static void amMutexCreate(object mutex)
    {
    }

    private static int amMutexDelete(object mutex)
    {
        return 1;
    }

    private static void amMutexLock(object mutex)
    {
        amAssert(mutex);
        Monitor.Enter(mutex);
    }

    private int amMutexTrylock(object mutex)
    {
        amAssert(mutex);
        return !Monitor.TryEnter(mutex) ? 0 : 1;
    }

    private static void amMutexUnlock(object mutex)
    {
        amAssert(mutex);
        Monitor.Exit(mutex);
    }
}