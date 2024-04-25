using System;

public partial class AppMain
{
    public class A2S_AMA_HIT
    {
        public uint flag;
        public uint hit_key_num;
        public uint hit_frm_num;
        public int hit_key_tbl_offset;
        public A2S_SUB_KEY[] hit_key_tbl;
        public int hit_tbl_offset;
        public A2S_SUB_HIT[] hit_tbl;

        internal void Assign(A2S_AMA_HIT old)
        {
            this.flag = old.flag;
            this.hit_key_num = old.hit_key_num;
            this.hit_frm_num = old.hit_frm_num;
            if (old.hit_key_tbl != null)
            {
                this.hit_key_tbl = new A2S_SUB_KEY[old.hit_key_tbl.Length];
                Array.Copy(old.hit_key_tbl, hit_key_tbl, old.hit_key_tbl.Length);
            }
            if (old.hit_tbl == null)
                return;
            this.hit_tbl = New<A2S_SUB_HIT>(old.hit_tbl.Length);
            for (int index = 0; index < this.hit_tbl.Length; ++index)
                this.hit_tbl[index].Assign(old.hit_tbl[index]);
        }
    }
}
