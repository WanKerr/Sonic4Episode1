using Microsoft.Xna.Framework;

public partial class AppMain
{
    private static void GmGmkPulleyBuild()
    {
        gm_gmk_pulley_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(819), GmGameDatGetGimmickData(820), 0U);
    }

    private static void GmGmkPulleyFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(819);
        GmGameDBuildRegFlushModel(gm_gmk_pulley_obj_3d_list, gimmickData.file_num);
    }

    private static OBS_OBJECT_WORK GmGmkPulleyBaseInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        pos_y -= 24576;
        OBS_OBJECT_WORK rideWork = GMM_ENEMY_CREATE_RIDE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_PULLEY_WORK(), "GMK_PULLEY_BASE");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)rideWork;
        ((GMS_GMK_PULLEY_WORK)rideWork).se_handle = null;
        mtTaskChangeTcbDestructor(rideWork.tcb, new GSF_TASK_PROCEDURE(gmGmkPulleyBaseExit));
        ObjObjectCopyAction3dNNModel(rideWork, gm_gmk_pulley_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(rideWork, 0, false, ObjDataGet(821), null, 0, null);
        ObjDrawObjectActionSet(rideWork, 0);
        rideWork.move_flag |= 8448U;
        rideWork.disp_flag |= 4194308U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
        OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[2];
        pRec.ppHit = null;
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkPulleyDefFunc);
        ObjRectAtkSet(pRec, 0, 0);
        ObjRectDefSet(pRec, 65534, 0);
        ObjRectWorkSet(pRec, -4, 9, 4, 24);
        pRec.flag |= 1024U;
        rideWork.pos.z = 0;
        rideWork.ppFunc = null;
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_EFFECT_3DNN_WORK(), rideWork, 0, "GMK_PULLEY_ROT");
        GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork = (GMS_EFFECT_3DNN_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_pulley_obj_3d_list[1], gmsEffect3DnnWork.obj_3d);
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.disp_flag &= 4294967039U;
        work.flag |= 16U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPulleyRotMain);
        ((GMS_GMK_PULLEY_WORK)rideWork).efct_work = null;
        return rideWork;
    }

    private static OBS_OBJECT_WORK GmGmkPulleyPoleLInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        pos_y -= 24576;
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_PULLEY_POLE_L");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_pulley_obj_3d_list[3], gmsEnemy3DWork.obj_3d);
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.flag |= 2U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        work.pos.z = -131072;
        work.ppFunc = null;
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPulleyDrawSetPoleL);
        return work;
    }

    private static OBS_OBJECT_WORK GmGmkPulleyPoleRInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        pos_y -= 24576;
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_PULLEY_POLE_R");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_pulley_obj_3d_list[2], gmsEnemy3DWork.obj_3d);
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.flag |= 2U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        work.pos.z = -131072;
        work.ppFunc = null;
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPulleyDrawSetPoleR);
        return work;
    }

    private static OBS_OBJECT_WORK GmGmkPulleyRopeFInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        pos_y -= 24576;
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_PULLEY_ROPE_F");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_pulley_obj_3d_list[4], gmsEnemy3DWork.obj_3d);
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.flag |= 2U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        work.pos.z = -131072;
        work.ppFunc = null;
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPulleyDrawSetRopeN);
        return work;
    }

    private static OBS_OBJECT_WORK GmGmkPulleyRopeTInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        pos_y -= 24576;
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_PULLEY_ROPE_T");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_pulley_obj_3d_list[5], gmsEnemy3DWork.obj_3d);
        if (eve_rec.id == 95)
            work.dir.y = 32768;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.flag |= 2U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        work.pos.z = -131072;
        work.ppFunc = null;
        work.ppOut = eve_rec.id != 95 ? new MPP_VOID_OBS_OBJECT_WORK(gmGmkPulleyDrawSetRopeTR) : new MPP_VOID_OBS_OBJECT_WORK(gmGmkPulleyDrawSetRopeTL);
        return work;
    }

    public static void GmGmkPulleyDrawServerMain()
    {
        if (!GmMainIsDrawEnable())
            return;
        GMS_GMK_PULLEY_MANAGER gmkPulleyManager = gm_gmk_pulley_manager;
        if (gmkPulleyManager.num <= 0U)
            return;
        AMS_PARAM_DRAW_PRIMITIVE prim = GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        prim.type = 1;
        prim.ablend = 0;
        prim.bldSrc = 768;
        prim.bldDst = 774;
        prim.bldMode = 32774;
        prim.aTest = 1;
        prim.zMask = 0;
        prim.zTest = 1;
        prim.noSort = 1;
        prim.texlist = gmkPulleyManager.texlist;
        prim.texId = (int)gmkPulleyManager.tex_id;
        uint lightColor = GmMainGetLightColor();
        prim.uwrap = 1;
        prim.vwrap = 1;
        prim.count = (int)gmkPulleyManager.num * 6 - 2;
        prim.vtxPCT3D = amDrawAlloc_NNS_PRIM3D_PCT(prim.count);
        NNS_PRIM3D_PCT[] buffer = prim.vtxPCT3D.buffer;
        int offset = prim.vtxPCT3D.offset;
        prim.format3D = 4;
        NNS_MATRIX nnsMatrix1 = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_VECTOR dst = GlobalPool<NNS_VECTOR>.Alloc();
        for (int index1 = 0; index1 < gmkPulleyManager.num; ++index1)
        {
            int index2 = offset + index1 * 6;
            int index3 = offset + index1 * 6 - 1;
            int index4 = offset + index1 * 6 + 4;
            GMS_GMK_PULLEY_REGISTER gmkPulleyRegister = gmkPulleyManager.reg[index1];
            Vector3 vector3;
            vector3.X = FX_FX32_TO_F32(gmkPulleyRegister.vec.x);
            vector3.Y = FX_FX32_TO_F32(gmkPulleyRegister.vec.y);
            vector3.Z = FX_FX32_TO_F32(gmkPulleyRegister.vec.z);
            NNS_VECTOR[] gmGmkPulleyPo = gm_gmk_pulley_pos[gmkPulleyRegister.type];
            NNS_TEXCOORD[] nnsTexcoordArray = gm_gmk_pulley_tex[gmkPulleyRegister.type];
            if (gmkPulleyRegister.flip != 0)
                nnMakeRotateYMatrix(nnsMatrix1, gmkPulleyRegister.flip);
            float num = 0.0f;
            if (gmkPulleyRegister.type == 2)
                num = 2f;
            for (int index5 = 0; index5 < 4; ++index5)
            {
                if (gmkPulleyRegister.flip != 0)
                    nnTransformVector(dst, nnsMatrix1, gmGmkPulleyPo[index5]);
                else
                    nnCopyVector(dst, gmGmkPulleyPo[index5]);
                int index6 = index2 + index5;
                buffer[index6].Pos.x = dst.x + vector3.X;
                buffer[index6].Pos.y = dst.y - vector3.Y + num;
                buffer[index6].Pos.z = dst.z + vector3.Z;
                buffer[index6].Tex.u = nnsTexcoordArray[index5].u;
                buffer[index6].Tex.v = nnsTexcoordArray[index5].v;
                buffer[index6].Col = lightColor;
            }
            if (index1 != 0)
                buffer[index3] = buffer[index2];
            if (index1 != gmkPulleyManager.num - 1U)
                buffer[index4] = buffer[index2 + 3];
        }
        GlobalPool<NNS_VECTOR>.Release(dst);
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix1);
        NNS_MATRIX nnsMatrix2 = GlobalPool<NNS_MATRIX>.Alloc();
        nnMakeUnitMatrix(nnsMatrix2);
        amMatrixPush(nnsMatrix2);
        ObjDraw3DNNDrawPrimitive(prim, 0U, 0, 0);
        amMatrixPop();
        GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Release(prim);
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix2);
        gmkPulleyManager.num = 0U;
    }

    private static void gmGmkPulleyBaseExit(MTS_TASK_TCB tcb)
    {
        GMS_GMK_PULLEY_WORK tcbWork = (GMS_GMK_PULLEY_WORK)mtTaskGetTcbWork(tcb);
        if (tcbWork.se_handle != null)
        {
            GsSoundStopSeHandle(tcbWork.se_handle);
            GsSoundFreeSeHandle(tcbWork.se_handle);
            tcbWork.se_handle = null;
        }
        GmEnemyDefaultExit(tcb);
    }

    private static void gmGmkPulleyDefFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        GMS_ENEMY_COM_WORK parentObj1 = (GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || (parentObj2.obj_work.obj_type != 1 || parentObj2.gmk_obj == (OBS_OBJECT_WORK)parentObj1))
            return;
        GmPlySeqInitPulley(parentObj2, parentObj1);
        ObjDrawObjectActionSet3DNN(parentObj1.obj_work, 3, 0);
        parentObj1.obj_work.dir.y = ((int)parentObj2.obj_work.disp_flag & 1) == 0 ? (ushort)0 : (ushort)32768;
        parentObj1.obj_work.user_flag = (uint)(parentObj1.obj_work.user_flag & 18446744073709518847UL);
        ((OBS_OBJECT_WORK)parentObj1).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPulleyMove);
        ObjRectWorkSet(parentObj1.rect_work[2], -32, 9, 32, 24);
    }

    private static void gmGmkPulleyMove(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GMS_PLAYER_WORK ply_work = g_gm_main_system.ply_work[0];
        if (ply_work.gmk_obj != obj_work)
        {
            gmGmkPulleySecedeSet(obj_work, 0);
            obj_work.flag |= 2U;
            obj_work.user_timer = 36;
        }
        else
        {
            if (obj_work.spd.x > 0)
            {
                obj_work.spd.x -= 64;
                if (obj_work.spd.x < 0)
                    obj_work.spd.x = 0;
            }
            else
            {
                obj_work.spd.x += 64;
                if (obj_work.spd.x > 0)
                    obj_work.spd.x = 0;
            }
            if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 2) != 0)
            {
                if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 1) != 0)
                    obj_work.spd.x -= 128;
                else
                    obj_work.spd.x += 128;
            }
            if (((int)g_gs_main_sys_info.game_flag & 1) != 0)
            {
                int num = MTM_MATH_CLIP(GmPlayerKeyGetGimmickRotZ(ply_work), -24576, 24576) * 160 / 24576;
                obj_work.spd.x += num;
            }
            else
            {
                int num = MTM_MATH_CLIP(ply_work.key_rot_z, -24576, 24576) * 160 / 24576;
                obj_work.spd.x += num;
            }
            if (ply_work.act_state != 59 && obj_work.spd.x > -256 && obj_work.spd.x < 256)
                obj_work.user_flag |= 32768U;
            if (ply_work.act_state != 59 && ply_work.act_state != 66 || ((int)ply_work.obj_work.disp_flag & 8) != 0)
            {
                int act_state;
                int id;
                if (((int)ply_work.obj_work.disp_flag & 1) != 0)
                {
                    act_state = 56;
                    id = 0;
                }
                else
                {
                    act_state = 56;
                    id = 0;
                }
                if ((obj_work.spd.x < -256 || obj_work.spd.x > 256) && ((int)obj_work.user_flag & 32768) != 0)
                {
                    GmPlayerActionChange(ply_work, 59);
                    ObjDrawObjectActionSet3DNN(obj_work, 4, 0);
                    obj_work.user_flag = (uint)(obj_work.user_flag & 18446744073709518847UL);
                }
                else if (ply_work.act_state != act_state)
                {
                    GmPlayerActionChange(ply_work, act_state);
                    ply_work.obj_work.disp_flag |= 4U;
                    ObjDrawObjectActionSet3DNN(obj_work, id, 0);
                    obj_work.disp_flag |= 4U;
                }
            }
            obj_work.dir.z = (ushort)MTM_MATH_CLIP((short)(obj_work.spd.x / 4), -10240, 10240);
            gmsEnemy3DWork.ene_com.target_dp_dir.z = obj_work.dir.z;
            int pos_x1;
            int pos_x2;
            if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 1) != 0)
            {
                pos_x1 = gmsEnemy3DWork.ene_com.born_pos_x - gmsEnemy3DWork.ene_com.eve_rec.left * 64 * 4096;
                pos_x2 = gmsEnemy3DWork.ene_com.born_pos_x;
            }
            else
            {
                pos_x1 = gmsEnemy3DWork.ene_com.born_pos_x;
                pos_x2 = gmsEnemy3DWork.ene_com.born_pos_x + gmsEnemy3DWork.ene_com.eve_rec.left * 64 * 4096;
            }
            if (obj_work.pos.x < pos_x1)
            {
                if (ply_work.obj_work.pos.x > pos_x1 + 32768)
                    ply_work.obj_work.pos.x = pos_x1 + 32768;
                gmGmkPulleySonicTakeOffSet(ply_work, obj_work.spd.x);
                gmGmkPulleySecedeSet(obj_work, pos_x1);
                obj_work.user_timer = 0;
            }
            else if (obj_work.pos.x > pos_x2)
            {
                if (ply_work.obj_work.pos.x < pos_x2 - 32768)
                    ply_work.obj_work.pos.x = pos_x2 - 32768;
                gmGmkPulleySonicTakeOffSet(ply_work, obj_work.spd.x);
                gmGmkPulleySecedeSet(obj_work, pos_x2);
                obj_work.user_timer = 0;
            }
            if (obj_work.ppFunc == new MPP_VOID_OBS_OBJECT_WORK(gmGmkPulleyMove) && MTM_MATH_ABS(obj_work.spd.x) > 4096)
                gmGmkPulleySparkInit(obj_work);
            else
                gmGmkPulleySparkKill(obj_work);
            ObjObjectMove(obj_work);
            if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 2) == 0)
                return;
            int num1 = MTM_MATH_ABS(gmsEnemy3DWork.ene_com.born_pos_x - obj_work.pos.x) / 2;
            obj_work.pos.y = gmsEnemy3DWork.ene_com.born_pos_y + num1;
        }
    }

    private static void gmGmkPulleySonicTakeOffSet(GMS_PLAYER_WORK ply_work, int spd_x)
    {
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd.z = 0;
        ply_work.obj_work.dir.z = 0;
        ply_work.obj_work.spd_m = spd_x;
        ply_work.obj_work.spd.y = -12288;
        GmPlySeqChangeSequence(ply_work, 17);
        if (spd_x > 0)
        {
            if (ply_work.obj_work.spd.x < 16384)
                ply_work.obj_work.spd.x = 16384;
            else if (ply_work.obj_work.spd.x > 24576)
                ply_work.obj_work.spd.x = 24576;
        }
        else if (ply_work.obj_work.spd.x > -16384)
            ply_work.obj_work.spd.x = -16384;
        else if (ply_work.obj_work.spd.x < -24576)
            ply_work.obj_work.spd.x = -24576;
        ply_work.obj_work.spd.x /= 2;
        ply_work.obj_work.spd.y = -12288;
        GmPlySeqSetJumpState(ply_work, 0, 7U);
    }

    private static void gmGmkPulleySecedeSet(OBS_OBJECT_WORK obj_work, int pos_x)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        if (pos_x != 0)
            obj_work.pos.x = pos_x;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
        obj_work.spd_m = 0;
        obj_work.dir.z = 0;
        gmsEnemy3DWork.ene_com.target_dp_dir.z = obj_work.dir.z;
        ObjDrawObjectActionSet3DNN(obj_work, 5, 0);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPulleySecede);
        ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[2], -4, 9, 4, 24);
        gmGmkPulleySparkKill(obj_work);
        GMS_GMK_PULLEY_WORK gmsGmkPulleyWork = (GMS_GMK_PULLEY_WORK)obj_work;
        if (gmsGmkPulleyWork.se_handle == null)
            return;
        GsSoundStopSeHandle(gmsGmkPulleyWork.se_handle);
        GsSoundFreeSeHandle(gmsGmkPulleyWork.se_handle);
        gmsGmkPulleyWork.se_handle = null;
    }

    private static void gmGmkPulleySecede(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
            if (obj_work.user_timer == 0)
                obj_work.flag &= 4294967293U;
        }
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        ObjDrawObjectActionSet3DNN(obj_work, 0, 0);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = null;
    }

    private static void gmGmkPulleyRotMain(OBS_OBJECT_WORK obj_work)
    {
        OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        obj_work.pos.Assign(parentObj.pos);
        ushort num = (ushort)(parentObj.spd.x >> 1);
        obj_work.dir.z += num;
    }

    private static void gmGmkPulleySparkInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GMS_GMK_PULLEY_WORK gmsGmkPulleyWork1 = (GMS_GMK_PULLEY_WORK)obj_work;
        if (gmsGmkPulleyWork1.efct_work != null)
            return;
        short dir_y = 0;
        short dir_z = 0;
        gmsGmkPulleyWork1.efct_work = GmEfctZoneEsCreate(obj_work, 0, 6);
        if (obj_work.spd.x < 0)
        {
            dir_z = -16384;
            GmComEfctAddDispOffsetF(gmsGmkPulleyWork1.efct_work, 3f, 0.0f, 0.0f);
        }
        if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 2) != 0)
        {
            if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 1) != 0)
                dir_z += 4836;
            else
                dir_z += -4836;
        }
        GmComEfctAddDispRotationS(gmsGmkPulleyWork1.efct_work, 0, dir_y, dir_z);
        GMS_GMK_PULLEY_WORK gmsGmkPulleyWork2 = (GMS_GMK_PULLEY_WORK)obj_work;
        if (gmsGmkPulleyWork2.se_handle == null || gmsGmkPulleyWork2.se_handle.au_player.sound[0] == null)
        {
            gmsGmkPulleyWork2.se_handle = GsSoundAllocSeHandle();
            GmSoundPlaySE("Pulley", gmsGmkPulleyWork2.se_handle);
        }
        else
            gmsGmkPulleyWork2.se_handle.snd_ctrl_param.volume = 1f;
    }

    private static void gmGmkPulleySparkKill(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PULLEY_WORK gmsGmkPulleyWork = (GMS_GMK_PULLEY_WORK)obj_work;
        if (gmsGmkPulleyWork.efct_work == null)
            return;
        ObjDrawKillAction3DES((OBS_OBJECT_WORK)gmsGmkPulleyWork.efct_work);
        gmsGmkPulleyWork.efct_work = null;
        ((GMS_GMK_PULLEY_WORK)obj_work).se_handle.snd_ctrl_param.volume = 0.0f;
    }

    private static void gmGmkPulleyDrawSetRopeN(OBS_OBJECT_WORK obj_work)
    {
        gmGmkPulleyDrawSetObject(obj_work, 0);
    }

    private static void gmGmkPulleyDrawSetRopeTL(OBS_OBJECT_WORK obj_work)
    {
        gmGmkPulleyDrawSetObject(obj_work, 1);
    }

    private static void gmGmkPulleyDrawSetRopeTR(OBS_OBJECT_WORK obj_work)
    {
        gmGmkPulleyDrawSetObject(obj_work, 2);
    }

    private static void gmGmkPulleyDrawSetPoleL(OBS_OBJECT_WORK obj_work)
    {
        gmGmkPulleyDrawSetObject(obj_work, 3);
    }

    private static void gmGmkPulleyDrawSetPoleR(OBS_OBJECT_WORK obj_work)
    {
        gmGmkPulleyDrawSetObject(obj_work, 4);
    }

    private static void gmGmkPulleyDrawSetObject(OBS_OBJECT_WORK obj_work, int type)
    {
        if (!GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        GMS_GMK_PULLEY_MANAGER gmkPulleyManager = gm_gmk_pulley_manager;
        gmkPulleyManager.texlist = obj_work.obj_3d.texlist;
        gmkPulleyManager.tex_id = 0U;
        GMS_GMK_PULLEY_REGISTER gmkPulleyRegister = gmkPulleyManager.reg[(int)gmkPulleyManager.num];
        gmkPulleyRegister.type = (ushort)type;
        gmkPulleyRegister.flip = obj_work.dir.y;
        gmkPulleyRegister.vec.Assign(obj_work.pos);
        ++gmkPulleyManager.num;
    }

}