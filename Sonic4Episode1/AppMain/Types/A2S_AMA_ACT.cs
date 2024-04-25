public partial class AppMain
{
}

public class A2S_AMA_ACT
{
    public int _off;
    public uint flag;
    public uint id;
    public uint frm_num;
    public uint pad1;
    public AppMain.A2S_SUB_RECT ofst;
    public int mtn_offset;
    public AppMain.A2S_AMA_MTN mtn;
    public int anm_offset;
    public AppMain.A2S_AMA_ANM anm;
    public int acm_offset;
    public A2S_AMA_ACM acm;
    public int usr_offset;
    public AppMain.A2S_AMA_USR usr;
    public int hit_offset;
    public AppMain.A2S_AMA_HIT hit;
    public int next_offset;
    public A2S_AMA_ACT next;

    public void Assign(A2S_AMA_ACT old)
    {
        this.flag = old.flag;
        this.id = old.flag;
        this.frm_num = old.frm_num;
        this.pad1 = old.pad1;
        this.ofst = old.ofst;
        if (old.mtn != null)
        {
            this.mtn = new AppMain.A2S_AMA_MTN();
            this.mtn.Assign(old.mtn);
        }
        if (old.anm != null)
        {
            this.anm = new AppMain.A2S_AMA_ANM();
            this.anm.Assign(old.anm);
        }
        if (old.acm != null)
        {
            this.acm = new A2S_AMA_ACM();
            this.acm.Assign(old.acm);
        }
        if (old.usr != null)
        {
            this.usr = new AppMain.A2S_AMA_USR();
            this.usr.Assign(old.usr);
        }
        if (old.hit == null)
            return;
        this.hit = new AppMain.A2S_AMA_HIT();
        this.hit.Assign(old.hit);
    }
}
