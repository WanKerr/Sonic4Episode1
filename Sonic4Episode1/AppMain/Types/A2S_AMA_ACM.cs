using System;

public class A2S_AMA_ACM
{
    public uint flag;
    public uint acm_key_num;
    public uint acm_frm_num;
    public int acm_key_tbl_offset;
    public A2S_SUB_KEY[] acm_key_tbl;
    public int acm_tbl_offset;
    public AppMain.A2S_SUB_ACM[] acm_tbl;
    public uint trs_key_num;
    public uint trs_frm_num;
    public int trs_key_tbl_offset;
    public A2S_SUB_KEY[] trs_key_tbl;
    public int trs_tbl_offset;
    public AppMain.A2S_SUB_TRS[] trs_tbl;
    public uint mat_key_num;
    public uint mat_frm_num;
    public int mat_key_tbl_offset;
    public A2S_SUB_KEY[] mat_key_tbl;
    public int mat_tbl_offset;
    public AppMain.A2S_SUB_MAT[] mat_tbl;

    internal void Assign(A2S_AMA_ACM old)
    {
        this.flag = old.flag;
        this.acm_key_num = old.acm_key_num;
        this.acm_frm_num = old.acm_frm_num;
        if (old.acm_key_tbl != null)
        {
            this.acm_key_tbl = new A2S_SUB_KEY[old.acm_key_tbl.Length];
            Array.Copy(old.acm_key_tbl, acm_key_tbl, old.acm_key_tbl.Length);
        }
        if (old.acm_tbl != null)
        {
            this.acm_tbl = new AppMain.A2S_SUB_ACM[old.acm_tbl.Length];
            Array.Copy(old.acm_tbl, acm_tbl, old.acm_tbl.Length);
        }
        this.trs_key_num = old.trs_key_num;
        this.trs_frm_num = old.trs_frm_num;
        if (old.trs_key_tbl != null)
        {
            this.trs_key_tbl = new A2S_SUB_KEY[old.trs_key_tbl.Length];
            Array.Copy(old.trs_key_tbl, trs_key_tbl, old.trs_key_tbl.Length);
        }
        if (old.trs_tbl != null)
        {
            this.trs_tbl = AppMain.New<AppMain.A2S_SUB_TRS>(old.trs_tbl.Length);
            for (int index = 0; index < this.trs_tbl.Length; ++index)
                this.trs_tbl[index].Assign(old.trs_tbl[index]);
        }
        this.mat_key_num = old.mat_key_num;
        this.mat_frm_num = old.mat_frm_num;
        if (old.mat_key_tbl != null)
        {
            this.mat_key_tbl = new A2S_SUB_KEY[old.mat_key_tbl.Length];
            Array.Copy(old.mat_key_tbl, mat_key_tbl, old.mat_key_tbl.Length);
        }
        if (old.mat_tbl == null)
            return;
        this.mat_tbl = AppMain.New<AppMain.A2S_SUB_MAT>(old.mat_tbl.Length);
        for (int index = 0; index < this.mat_tbl.Length; ++index)
            this.mat_tbl[index].Assign(old.mat_tbl[index]);
    }
}
