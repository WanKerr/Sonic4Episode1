using System;

public partial class AppMain
{
    public class A2S_AMA_USR
    {
        public uint flag;
        public uint usr_key_num;
        public uint usr_frm_num;
        public int usr_key_tbl_offset;
        public A2S_SUB_KEY[] usr_key_tbl;
        public int usr_tbl_offset;
        public A2S_SUB_USR[] usr_tbl;

        internal void Assign(A2S_AMA_USR old)
        {
            this.flag = old.flag;
            this.usr_key_num = old.usr_key_num;
            this.usr_frm_num = old.usr_frm_num;
            if (old.usr_key_tbl != null)
            {
                this.usr_key_tbl = new A2S_SUB_KEY[old.usr_key_tbl.Length];
                Array.Copy(old.usr_key_tbl, usr_key_tbl, old.usr_key_tbl.Length);
            }
            if (old.usr_tbl == null)
                return;
            this.usr_tbl = new A2S_SUB_USR[old.usr_tbl.Length];
            Array.Copy(old.usr_tbl, usr_tbl, old.usr_tbl.Length);
        }
    }
}
