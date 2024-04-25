public partial class AppMain
{
    public static AOS_SPRITE AoActSprCreate(
      A2S_AMA_HEADER ama,
      uint id,
      float frame)
    {
        AOS_SPRITE spr = aoActAllocSprite();
        if (spr == null)
            return null;
        AoActSprApply(spr, ama, id, frame);
        return spr;
    }

    public static void AoActSprDelete(AOS_SPRITE spr)
    {
        aoActFreeSprite(spr);
    }

    public static void AoActSprApply(
      AOS_SPRITE spr,
      A2S_AMA_HEADER ama,
      uint id,
      float frame)
    {
        A2S_AMA_ACT next;
        for (next = ama.act_tbl[(int)id]; next.next != null && next.frm_num <= (double)frame; next = next.next)
            frame -= next.frm_num;
        float frame1 = frame;
        int key1 = -1;
        aoActSearchTrsKey(next, ref frame1, ref key1);
        float frame2 = frame;
        int key2 = -1;
        aoActSearchMtnKey(next, ref frame2, ref key2);
        float frame3 = frame;
        int key3 = -1;
        aoActSearchAnmKey(next, ref frame3, ref key3);
        float frame4 = frame;
        int key4 = -1;
        aoActSearchMatKey(next, ref frame4, ref key4);
        float frame5 = frame;
        int key5 = -1;
        aoActSearchHitKey(next, ref frame5, ref key5);
        spr.flag = next.flag;
        spr.offset.left = next.ofst.left;
        spr.offset.right = next.ofst.right;
        spr.offset.top = next.ofst.top;
        spr.offset.bottom = next.ofst.bottom;
        float scale_x = 0.0f;
        float scale_y = 0.0f;
        if (next.mtn == null)
        {
            spr.center_x = 0.0f;
            spr.center_y = 0.0f;
            spr.prio = 0.0f;
            spr.rotate = 0.0f;
        }
        else
        {
            aoActMakeTrs(next.mtn.trs_key_num, next.mtn.trs_frm_num, next.mtn.trs_key_tbl, next.mtn.trs_tbl, key1, frame1, ref spr.center_x, ref spr.center_y, ref spr.prio);
            aoActMakeMtn(next.mtn.mtn_key_num, next.mtn.mtn_frm_num, next.mtn.mtn_key_tbl, next.mtn.mtn_tbl, key2, frame2, out scale_x, out scale_y, out spr.rotate);
            spr.offset.left *= scale_x;
            spr.offset.right *= scale_x;
            spr.offset.top *= scale_y;
            spr.offset.bottom *= scale_y;
        }
        if (next.anm == null)
        {
            spr.tex_id = -1;
            spr.color.r = byte.MaxValue;
            spr.color.g = byte.MaxValue;
            spr.color.b = byte.MaxValue;
            spr.color.a = byte.MaxValue;
            spr.fade.r = 0;
            spr.fade.g = 0;
            spr.fade.b = 0;
            spr.fade.a = 0;
        }
        else
        {
            aoActMakeAnm(next.anm.anm_key_num, next.anm.anm_frm_num, next.anm.anm_key_tbl, next.anm.anm_tbl, key3, frame3, ref spr.tex_id, ref spr.uv, ref spr.clamp);
            aoActMakeMat(next.anm.mat_key_num, next.anm.mat_frm_num, next.anm.mat_key_tbl, next.anm.mat_tbl, key4, frame4, ref spr.color, ref spr.fade, out spr.blend);
        }
        if (next.hit == null)
        {
            spr.hit.type = AOE_ACT_HIT.AOD_ACT_HIT_NONE;
        }
        else
        {
            aoActMakeHit(next.hit.hit_key_num, next.hit.hit_frm_num, next.hit.hit_key_tbl, next.hit.hit_tbl, key5, frame5, spr.hit);
            spr.hit.scale_x = scale_x;
            spr.hit.scale_y = scale_y;
        }
        aoActAcmSprite(spr);
    }

    public static void AoActSprDraw(AOS_SPRITE spr)
    {
        if (g_ao_act_sys_draw_state_enable)
        {
            aoActDrawSprState(spr);
        }
        else
        {
            AOS_ACT_DRAW aosActDraw = new AOS_ACT_DRAW();
            aosActDraw.count = 1U;
            aosActDraw.sprite = new AOS_SPRITE[1];
            aosActDraw.sprite[0].Assign(spr);
            amDrawMakeTask(new TaskProc(aoActDrawTask), (ushort)g_ao_act_sys_draw_prio, aosActDraw);
        }
    }

}