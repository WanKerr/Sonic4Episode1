public partial class AppMain
{
    private static void ObjAction3dNNModelLoad(
     OBS_ACTION3D_NN_WORK obj_3d,
     OBS_DATA_WORK data_work,
     string filename,
     int index,
     AMS_AMB_HEADER archive,
     string filename_tex,
     AMS_AMB_HEADER amb_tex,
     uint drawflag)
    {
        object buf = null;
        string filepath = null;
        obj_3d.command_state = 0U;
        obj_3d.marge = 0.0f;
        obj_3d.per = 1f;
        obj_3d.use_light_flag = g_obj.def_user_light_flag;
        nnMakeUnitMatrix(obj_3d.user_obj_mtx);
        nnMakeUnitMatrix(obj_3d.user_obj_mtx_r);
        for (int index1 = 0; index1 < 2; ++index1)
            obj_3d.speed[index1] = 1f;
        obj_3d.mat_speed = 1f;
        obj_3d.blend_spd = 0.25f;
        obj_3d.drawflag = drawflag;
        obj_3d.draw_state.Assign(g_obj_draw_3dnn_draw_state);
        if (archive != null)
            obj_3d.flag |= 65536U;
        if (filename != null && filename != "")
        {
            buf = ObjDataLoad(data_work, filename, archive);
            if (archive != null && buf == null)
            {
                obj_3d.flag &= 4294901759U;
                buf = ObjDataLoad(data_work, filename, null);
            }
        }
        else if (archive != null)
        {
            buf = ObjDataLoadAmbIndex(data_work, index, archive);
            if (buf == null)
                obj_3d.flag &= 4294901759U;
        }
        else if (data_work != null)
            buf = ObjDataGetInc(data_work);
        if (buf == null)
            return;
        obj_3d.model = buf;
        if (data_work != null)
            obj_3d.model_data_work = data_work;
        if (filename_tex != null && filename_tex != "")
        {
            sFile = filename_tex;
            filepath = sFile;
        }
        else
            sFile = "";
        obj_3d.reg_index = amObjectLoad(out obj_3d._object, out obj_3d.texlist, out obj_3d.texlistbuf, buf, drawflag | g_obj.load_drawflag, filepath, amb_tex);
        if (obj_load_initial_set_flag)
        {
            OBS_LOAD_INITIAL_WORK objLoadInitialWork = obj_load_initial_work;
            if (objLoadInitialWork.obj_num < byte.MaxValue)
            {
                objLoadInitialWork.obj_3d[objLoadInitialWork.obj_num] = obj_3d;
                ++objLoadInitialWork.obj_num;
            }
        }
        obj_3d.flag |= 2147483648U;
        obj_3d.flag &= 3221225471U;
    }

    private static void ObjCopyAction3dNNModel(
      OBS_ACTION3D_NN_WORK src_obj_3d,
      OBS_ACTION3D_NN_WORK dest_obj_3d)
    {
        dest_obj_3d._object = src_obj_3d._object;
        dest_obj_3d.texlist = src_obj_3d.texlist;
        dest_obj_3d.texlistbuf = src_obj_3d.texlistbuf;
        dest_obj_3d.model = src_obj_3d.model;
        dest_obj_3d.model_data_work = src_obj_3d.model_data_work;
        dest_obj_3d.command_state = src_obj_3d.command_state;
        dest_obj_3d.flag = src_obj_3d.flag;
        dest_obj_3d.marge = 0.0f;
        dest_obj_3d.per = 1f;
        dest_obj_3d.use_light_flag = src_obj_3d.use_light_flag;
        for (int index = 0; index < 2; ++index)
            dest_obj_3d.speed[index] = 1f;
        dest_obj_3d.mat_speed = 1f;
        nnMakeUnitMatrix(dest_obj_3d.user_obj_mtx);
        nnMakeUnitMatrix(dest_obj_3d.user_obj_mtx_r);
        dest_obj_3d.blend_spd = 0.25f;
        dest_obj_3d.sub_obj_type = src_obj_3d.sub_obj_type;
        dest_obj_3d.drawflag = src_obj_3d.drawflag;
        dest_obj_3d.draw_state.Assign(g_obj_draw_3dnn_draw_state);
        dest_obj_3d.reg_index = -1;
    }

    private static void ObjObjectCopyAction3dNNModel(
      OBS_OBJECT_WORK obj_work,
      OBS_ACTION3D_NN_WORK src_obj_3d,
      OBS_ACTION3D_NN_WORK dest_obj_3d)
    {
        if (dest_obj_3d == null)
        {
            dest_obj_3d = obj_work.obj_3d == null ? new OBS_ACTION3D_NN_WORK() : obj_work.obj_3d;
            dest_obj_3d.Clear();
            obj_work.flag |= 134217728U;
        }
        obj_work.flag |= 536870912U;
        ObjCopyAction3dNNModel(src_obj_3d, dest_obj_3d);
        obj_work.obj_3d = dest_obj_3d;
    }

    private static void ObjObjectAction3dNNModelReleaseCopy(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.flag & 134217728) != 0)
        {
            obj_work.obj_3d = null;
            obj_work.flag &= 4160749567U;
        }
        obj_work.obj_3d = null;
        obj_work.flag &= 3758096383U;
    }

    private static void ObjObjectAction3dNNModelLoad(
      OBS_OBJECT_WORK obj_work,
      OBS_ACTION3D_NN_WORK obj_3d,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive,
      string filename_tex,
      AMS_AMB_HEADER amb_tex,
      uint drawflag)
    {
        if (obj_3d == null)
        {
            obj_3d = obj_work.obj_3d == null ? new OBS_ACTION3D_NN_WORK() : obj_work.obj_3d;
            obj_3d.Clear();
            obj_work.flag |= 134217728U;
        }
        obj_work.obj_3d = obj_3d;
        ObjAction3dNNModelLoad(obj_3d, data_work, filename, index, archive, filename_tex, amb_tex, drawflag);
    }

    private static void ObjAction3dNNModelLoadTxb(
      OBS_ACTION3D_NN_WORK obj_3d,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive,
      string filename_tex,
      AMS_AMB_HEADER amb_tex,
      uint drawflag,
      TXB_HEADER txb)
    {
        object buf = null;
        string filepath = null;
        obj_3d.command_state = 0U;
        obj_3d.marge = 0.0f;
        obj_3d.per = 1f;
        obj_3d.use_light_flag = g_obj.def_user_light_flag;
        nnMakeUnitMatrix(obj_3d.user_obj_mtx);
        nnMakeUnitMatrix(obj_3d.user_obj_mtx_r);
        for (int index1 = 0; index1 < 2; ++index1)
            obj_3d.speed[index1] = 1f;
        obj_3d.blend_spd = 0.25f;
        obj_3d.drawflag = drawflag;
        obj_3d.draw_state.Assign(g_obj_draw_3dnn_draw_state);
        if (archive != null)
            obj_3d.flag |= 65536U;
        if (filename != null)
        {
            buf = ObjDataLoad(data_work, filename, archive);
            if (archive != null && buf == null)
            {
                obj_3d.flag &= 4294901759U;
                buf = ObjDataLoad(data_work, filename, null);
            }
        }
        else if (archive != null)
        {
            buf = ObjDataLoadAmbIndex(data_work, index, archive);
            if (buf == null)
                obj_3d.flag &= 4294901759U;
        }
        else if (data_work != null)
            buf = ObjDataGetInc(data_work);
        if (buf == null)
            return;
        obj_3d.model = buf;
        if (data_work != null)
            obj_3d.model_data_work = data_work;
        if (filename_tex != null && filename_tex != "")
        {
            sFile = filename_tex;
            filepath = sFile;
        }
        else
            sFile = "";
        obj_3d.reg_index = amObjectLoad(out obj_3d._object, amTxbGetTexFileList(txb), out obj_3d.texlist, out obj_3d.texlistbuf, buf, drawflag | g_obj.load_drawflag, filepath, amb_tex);
        if (obj_load_initial_set_flag)
        {
            OBS_LOAD_INITIAL_WORK objLoadInitialWork = obj_load_initial_work;
            if (objLoadInitialWork.obj_num < byte.MaxValue)
            {
                objLoadInitialWork.obj_3d[objLoadInitialWork.obj_num] = obj_3d;
                ++objLoadInitialWork.obj_num;
            }
        }
        obj_3d.flag |= 2147483648U;
        obj_3d.flag &= 3221225471U;
    }

    private static void ObjObjectAction3dNNModelLoadTxb(
      OBS_OBJECT_WORK obj_work,
      OBS_ACTION3D_NN_WORK obj_3d,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive,
      string filename_tex,
      AMS_AMB_HEADER amb_tex,
      uint drawflag,
      TXB_HEADER txb)
    {
        if (obj_3d == null)
        {
            obj_3d = obj_work.obj_3d == null ? new OBS_ACTION3D_NN_WORK() : obj_work.obj_3d;
            obj_3d.Clear();
            obj_work.flag |= 134217728U;
        }
        obj_work.obj_3d = obj_3d;
        ObjAction3dNNModelLoadTxb(obj_3d, data_work, filename, index, archive, filename_tex, amb_tex, drawflag, txb);
    }

    private static void ObjAction3dNNMotionLoad(
      OBS_ACTION3D_NN_WORK obj_3d,
      int reg_file_id,
      bool marge,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive)
    {
        ObjAction3dNNMotionLoad(obj_3d, reg_file_id, marge, data_work, filename, index, archive, 64, 16);
    }

    private static void ObjAction3dNNMotionLoad(
      OBS_ACTION3D_NN_WORK obj_3d,
      int reg_file_id,
      bool marge,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive,
      int motion_num,
      int mmotion_num)
    {
        object obj = null;
        if (((int)obj_3d.flag & 1073741824) == 0)
        {
            obj_3d.flag |= 536870912U;
            OBS_ACTION3D_MTN_LOAD_SETTING n3DMtnLoadSetting = obj_3d.mtn_load_setting[reg_file_id];
            n3DMtnLoadSetting.enable = true;
            n3DMtnLoadSetting.marge = marge;
            n3DMtnLoadSetting.data_work = data_work;
            n3DMtnLoadSetting.filename = "";
            if (filename != null)
                n3DMtnLoadSetting.filename = filename;
            n3DMtnLoadSetting.index = index;
            n3DMtnLoadSetting.archive = archive;
        }
        else
        {
            if (archive != null)
                obj_3d.flag |= (uint)(131072 << reg_file_id);
            if (filename != null && filename != "")
            {
                obj = ObjDataLoad(data_work, filename, archive);
                if (archive != null && obj == null)
                {
                    obj_3d.flag &= (uint)~(131072 << reg_file_id);
                    obj = ObjDataLoad(data_work, filename, null);
                }
            }
            else if (archive != null)
            {
                obj = ObjDataLoadAmbIndex(data_work, index, archive);
                if (archive != null && obj == null)
                    obj_3d.flag &= (uint)~(131072 << reg_file_id);
            }
            else if (data_work != null)
                obj = ObjDataGetInc(data_work);
            if (obj == null)
                return;
            obj_3d.mtn[reg_file_id] = obj;
            if (data_work != null)
                obj_3d.mtn_data_work[reg_file_id] = data_work;
            if (obj_3d.motion == null)
                obj_3d.motion = amMotionCreate(obj_3d._object, motion_num, mmotion_num, marge ? 1 : 0);
            switch (obj)
            {
                case AMS_AMB_HEADER _:
                case AMS_FS _:
                    AMS_AMB_HEADER amb = readAMBFile(obj);
                    amMotionRegistFile(obj_3d.motion, reg_file_id, amb);
                    break;
                default:
                    amMotionRegistFile(obj_3d.motion, reg_file_id, obj);
                    break;
            }
        }
    }

    private static void ObjObjectAction3dNNMotionLoad(
      OBS_OBJECT_WORK obj_work,
      int reg_file_id,
      bool marge,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive)
    {
        ObjObjectAction3dNNMotionLoad(obj_work, reg_file_id, marge, data_work, filename, index, archive, 64, 16);
    }

    private static void ObjObjectAction3dNNMotionLoad(
      OBS_OBJECT_WORK obj_work,
      int reg_file_id,
      bool marge,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive,
      int motion_num,
      int mmotion_num)
    {
        ObjAction3dNNMotionLoad(obj_work.obj_3d, reg_file_id, marge, data_work, filename, index, archive, motion_num, mmotion_num);
    }

    private static void ObjAction3dNNMaterialMotionLoad(
      OBS_ACTION3D_NN_WORK obj_3d,
      int reg_file_id,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive)
    {
        ObjAction3dNNMaterialMotionLoad(obj_3d, reg_file_id, data_work, filename, index, archive, 64, 16);
    }

    private static void ObjAction3dNNMaterialMotionLoad(
      OBS_ACTION3D_NN_WORK obj_3d,
      int reg_file_id,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive,
      int motion_num,
      int mmotion_num)
    {
        object obj = null;
        if (((int)obj_3d.flag & 1073741824) == 0)
        {
            obj_3d.flag |= 268435456U;
            OBS_ACTION3D_MTN_LOAD_SETTING n3DMtnLoadSetting = obj_3d.mat_mtn_load_setting[reg_file_id];
            n3DMtnLoadSetting.enable = true;
            n3DMtnLoadSetting.marge = false;
            n3DMtnLoadSetting.data_work = data_work;
            n3DMtnLoadSetting.filename = "";
            if (filename != null && filename != "")
                n3DMtnLoadSetting.filename = filename;
            n3DMtnLoadSetting.index = index;
            n3DMtnLoadSetting.archive = archive;
        }
        else
        {
            if (archive != null)
                obj_3d.flag |= (uint)(4096 << reg_file_id);
            if (filename != null && filename != "")
            {
                obj = ObjDataLoad(data_work, filename, archive);
                if (archive != null && obj == null)
                {
                    obj_3d.flag &= (uint)~(4096 << reg_file_id);
                    obj = ObjDataLoad(data_work, filename, null);
                }
            }
            else if (archive != null)
            {
                obj = ObjDataLoadAmbIndex(data_work, index, archive);
                if (archive != null && obj == null)
                    obj_3d.flag &= (uint)~(4096 << reg_file_id);
            }
            else if (data_work != null)
                obj = ObjDataGetInc(data_work);
            if (obj == null)
                return;
            obj_3d.mat_mtn[reg_file_id] = obj;
            if (data_work != null)
                obj_3d.mat_mtn_data_work[reg_file_id] = data_work;
            if (obj_3d.motion == null)
                obj_3d.motion = amMotionCreate(obj_3d._object, motion_num, mmotion_num, 0);
            switch (obj)
            {
                case AMS_AMB_HEADER _:
                case AMS_FS _:
                    AMS_AMB_HEADER amb = readAMBFile(obj);
                    amMotionMaterialRegistFile(obj_3d.motion, reg_file_id, amb);
                    break;
                default:
                    amMotionMaterialRegistFile(obj_3d.motion, reg_file_id, obj);
                    break;
            }
        }
    }

    private static void ObjObjectAction3dNNMaterialMotionLoad(
      OBS_OBJECT_WORK obj_work,
      int reg_file_id,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      object archive)
    {
        ObjObjectAction3dNNMaterialMotionLoad(obj_work, reg_file_id, data_work, filename, index, archive, 64, 16);
    }

    private static void ObjObjectAction3dNNMaterialMotionLoad(
      OBS_OBJECT_WORK obj_work,
      int reg_file_id,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      object archive,
      int motion_num,
      int mmotion_num)
    {
        ObjAction3dNNMaterialMotionLoad(obj_work.obj_3d, reg_file_id, data_work, filename, index, (AMS_AMB_HEADER)archive, motion_num, mmotion_num);
    }

    private static bool ObjAction3dNNModelLoadCheck(OBS_ACTION3D_NN_WORK obj_3d)
    {
        if (((int)obj_3d.flag & int.MinValue) != 0)
        {
            if (amDrawIsRegistComplete(obj_3d.reg_index))
            {
                obj_3d.flag &= int.MaxValue;
                obj_3d.flag |= 1073741824U;
                obj_3d.reg_index = -1;
                if (obj_3d.model_data_work != null)
                {
                    ObjDataRelease(obj_3d.model_data_work);
                    obj_3d.model_data_work = null;
                }
                else if (obj_3d.model != null && ((int)obj_3d.flag & 65536) == 0)
                    obj_3d.model = null;
                obj_3d.flag &= 4294901759U;
                obj_3d.model = null;
                if (((int)obj_3d.flag & 536870912) != 0)
                {
                    int reg_file_id = 0;
                    ArrayPointer<OBS_ACTION3D_MTN_LOAD_SETTING> arrayPointer = new ArrayPointer<OBS_ACTION3D_MTN_LOAD_SETTING>(obj_3d.mtn_load_setting);
                    while (reg_file_id < 4)
                    {
                        if ((~arrayPointer).enable)
                        {
                            ObjAction3dNNMotionLoad(obj_3d, reg_file_id, (~arrayPointer).marge, (~arrayPointer).data_work, (~arrayPointer).filename, (~arrayPointer).index, (~arrayPointer).archive);
                            (~arrayPointer).enable = false;
                        }
                        ++reg_file_id;
                        ++arrayPointer;
                    }
                    obj_3d.flag &= 3758096383U;
                }
                if (((int)obj_3d.flag & 268435456) != 0)
                {
                    int reg_file_id = 0;
                    ArrayPointer<OBS_ACTION3D_MTN_LOAD_SETTING> arrayPointer = new ArrayPointer<OBS_ACTION3D_MTN_LOAD_SETTING>(obj_3d.mat_mtn_load_setting);
                    while (reg_file_id < 4)
                    {
                        if ((~arrayPointer).enable)
                        {
                            ObjAction3dNNMaterialMotionLoad(obj_3d, reg_file_id, (~arrayPointer).data_work, (~arrayPointer).filename, (~arrayPointer).index, (~arrayPointer).archive);
                            (~arrayPointer).enable = false;
                        }
                        ++reg_file_id;
                        ++arrayPointer;
                    }
                    obj_3d.flag &= 4026531839U;
                }
                return true;
            }
        }
        else if (((int)obj_3d.flag & 1073741824) != 0)
            return true;
        return false;
    }

    private static void ObjAction3dNNModelRelease(OBS_ACTION3D_NN_WORK obj_3d)
    {
        obj_3d.reg_index = amObjectRelease(obj_3d._object, obj_3d.texlist);
        obj_3d.flag |= 134217728U;
    }

    private static bool ObjAction3dNNModelReleaseCheck(OBS_ACTION3D_NN_WORK obj_3d)
    {
        if (((int)obj_3d.flag & 1207959552) == 0)
            return true;
        if (!amDrawIsRegistComplete(obj_3d.reg_index))
            return false;
        if (obj_3d._object != null)
            obj_3d._object = null;
        if (obj_3d.texlistbuf != null)
            obj_3d.texlistbuf = null;
        obj_3d.flag &= 3087007743U;
        if (obj_3d.model_data_work != null)
        {
            ObjDataRelease(obj_3d.model_data_work);
            obj_3d.model_data_work = null;
        }
        else if (obj_3d.model != null && ((int)obj_3d.flag & 65536) == 0)
            obj_3d.model = null;
        obj_3d.flag &= 4294901759U;
        return true;
    }

    private static void ObjAction3dNNMotionRelease(OBS_ACTION3D_NN_WORK obj_3d)
    {
        if (obj_3d.motion != null)
        {
            amMotionDelete(obj_3d.motion);
            obj_3d.motion = null;
        }
        for (int index = 0; index < 4; ++index)
        {
            if (obj_3d.mtn_data_work[index] != null)
            {
                ObjDataRelease(obj_3d.mtn_data_work[index]);
                obj_3d.mtn_data_work[index] = null;
            }
            else if (obj_3d.mtn[index] != null)
            {
                int num1 = (int)obj_3d.flag & 131072 << index;
            }
            obj_3d.flag &= (uint)~(131072 << index);
            obj_3d.mtn[index] = null;
            if (obj_3d.mat_mtn_data_work[index] != null)
            {
                ObjDataRelease(obj_3d.mat_mtn_data_work[index]);
                obj_3d.mat_mtn_data_work[index] = null;
            }
            else if (obj_3d.mat_mtn[index] != null)
            {
                int num2 = (int)obj_3d.flag & 4096 << index;
            }
            obj_3d.flag &= (uint)~(4096 << index);
            obj_3d.mat_mtn[index] = null;
        }
    }

    private static void ObjAction3dESEffectLoad(
      OBS_ACTION3D_ES_WORK obj_3des,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive,
      int user_attr,
      int ecb_prio)
    {
        object obj = null;
        obj_3des.command_state = 0U;
        obj_3des.user_attr = user_attr;
        obj_3des.speed = 1f;
        if (archive != null)
            obj_3des.flag |= 65536U;
        if (filename != null && filename != "")
        {
            obj = ObjDataLoad(data_work, filename, archive);
            if (archive != null && obj == null)
            {
                obj_3des.flag &= 4294901759U;
                obj = ObjDataLoad(data_work, filename, null);
            }
        }
        else if (archive != null)
        {
            obj = ObjDataLoadAmbIndex(data_work, index, archive);
            if (obj == null)
                obj_3des.flag &= 4294901759U;
        }
        else if (data_work != null)
            obj = ObjDataGetInc(data_work);
        if (obj == null)
            return;
        obj_3des.eff = obj;
        if (data_work != null)
            obj_3des.eff_data_work = data_work;
        obj_3des.ecb = _amEffectCreate((AMS_AME_HEADER)obj_3des.eff, user_attr, ecb_prio);
    }

    private static void ObjObjectAction3dESEffectLoad(
      OBS_OBJECT_WORK obj_work,
      OBS_ACTION3D_ES_WORK obj_3des,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive)
    {
        ObjObjectAction3dESEffectLoad(obj_work, obj_3des, data_work, filename, index, archive, 0, 0);
    }

    private static void ObjObjectAction3dESEffectLoad(
      OBS_OBJECT_WORK obj_work,
      OBS_ACTION3D_ES_WORK obj_3des,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive,
      int user_attr,
      int ecb_prio)
    {
        if (obj_3des == null)
        {
            obj_3des = obj_work.obj_3des == null ? new OBS_ACTION3D_ES_WORK() : obj_work.obj_3des;
            obj_3des.Clear();
            obj_work.flag |= 268435456U;
        }
        obj_work.obj_3des = obj_3des;
        ObjAction3dESEffectLoad(obj_3des, data_work, filename, index, archive, user_attr, ecb_prio);
    }

    private static void ObjAction3dESEffectRelease(OBS_ACTION3D_ES_WORK obj_3des)
    {
        amEffectDelete(obj_3des.ecb);
        obj_3des.ecb = null;
    }

    private static void ObjAction3dESTextureLoad(
      OBS_ACTION3D_ES_WORK obj_3des,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive,
      bool load_tex)
    {
        AMS_AMB_HEADER amsAmbHeader = null;
        if (archive != null)
            obj_3des.flag |= 131072U;
        if (filename != null && filename != "")
        {
            amsAmbHeader = readAMBFile(ObjDataLoad(data_work, filename, archive));
            if (archive != null && amsAmbHeader == null)
            {
                obj_3des.flag &= 4294836223U;
                amsAmbHeader = readAMBFile(ObjDataLoad(data_work, filename, null));
            }
        }
        else if (archive != null)
        {
            amsAmbHeader = readAMBFile(ObjDataLoadAmbIndex(data_work, index, archive));
            if (amsAmbHeader == null)
                obj_3des.flag &= 4294836223U;
        }
        else if (data_work != null)
            amsAmbHeader = readAMBFile(ObjDataGetInc(data_work));
        if (amsAmbHeader == null)
            return;
        obj_3des.ambtex = amsAmbHeader;
        if (data_work != null)
            obj_3des.ambtex_data_work = data_work;
        if (!load_tex)
            return;
        TXB_HEADER txb = readTXBfile(amBindGet(amsAmbHeader, 0));
        uint count = amTxbGetCount(txb);
        obj_3des.texlistbuf = null;
        nnSetUpTexlist(out obj_3des.texlist, (int)count, ref obj_3des.texlistbuf);
        if (obj_load_initial_set_flag)
        {
            OBS_LOAD_INITIAL_WORK objLoadInitialWork = obj_load_initial_work;
            if (objLoadInitialWork.es_num < byte.MaxValue)
            {
                objLoadInitialWork.obj_3des[objLoadInitialWork.es_num] = obj_3des;
                ++objLoadInitialWork.es_num;
            }
        }
        obj_3des.tex_reg_index = amTextureLoad(obj_3des.texlist, amTxbGetTexFileList(txb), filename, amsAmbHeader);
        obj_3des.flag |= 1073741824U;
    }

    private static void ObjObjectAction3dESTextureLoad(
      OBS_OBJECT_WORK obj_work,
      OBS_ACTION3D_ES_WORK obj_3des,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive,
      bool load_tex)
    {
        ObjAction3dESTextureLoad(obj_3des, data_work, filename, index, archive, load_tex);
    }

    private static void ObjAction3dESTextureRelease(OBS_ACTION3D_ES_WORK obj_3des)
    {
        obj_3des.tex_reg_index = obj_3des.texlist_data_work == null ? amTextureRelease(obj_3des.texlist) : ObjAction3dESTextureReleaseDwork(obj_3des.texlist_data_work);
        if (obj_3des.tex_reg_index != -1)
        {
            obj_3des.flag |= 1073741824U;
        }
        else
        {
            obj_3des.texlist_data_work = null;
            obj_3des.texlist = null;
            obj_3des.texlistbuf = null;
        }
    }

    private static bool ObjAction3dESTextureReleaseCheck(OBS_ACTION3D_ES_WORK obj_3des)
    {
        if (((int)obj_3des.flag & 1073741824) == 0)
            return true;
        if (obj_3des.texlist_data_work != null)
        {
            if (ObjAction3dESTextureReleaseDworkCheck(obj_3des.texlist_data_work, obj_3des.tex_reg_index))
            {
                obj_3des.texlist_data_work = null;
                obj_3des.texlist = null;
                obj_3des.texlistbuf = null;
                obj_3des.tex_reg_index = -1;
                obj_3des.flag &= 3221225471U;
                return true;
            }
        }
        else if (amDrawIsRegistComplete(obj_3des.tex_reg_index))
        {
            obj_3des.texlist = null;
            obj_3des.texlistbuf = null;
            obj_3des.tex_reg_index = -1;
            obj_3des.flag &= 3221225471U;
            return true;
        }
        return false;
    }

    private static int ObjAction3dESTextureLoadToDwork(
      OBS_DATA_WORK texlist_dwork,
      AMS_AMB_HEADER amb_tex,
      ref object texlist_buf)
    {
        int num;
        if (texlist_dwork.pData == null)
        {
            TXB_HEADER txb = readTXBfile(amBindGet(amb_tex, 0));
            uint count = amTxbGetCount(txb);
            texlist_buf = null;
            NNS_TEXLIST texlist;
            nnSetUpTexlist(out texlist, (int)count, ref texlist_buf);
            num = amTextureLoad(texlist, amTxbGetTexFileList(txb), null, amb_tex);
            ObjDataSet(texlist_dwork, texlist);
        }
        else
        {
            ObjDataGetInc(texlist_dwork);
            num = -1;
            texlist_buf = null;
        }
        return num;
    }

    private static int ObjAction3dESTextureReleaseDwork(OBS_DATA_WORK texlist_dwork)
    {
        int num = -1;
        if (texlist_dwork.num != 0 && texlist_dwork.pData != null)
        {
            --texlist_dwork.num;
            if (texlist_dwork.num == 0)
                num = amTextureRelease((NNS_TEXLIST)texlist_dwork.pData);
        }
        return num;
    }

    private static bool ObjAction3dESTextureReleaseDworkCheck(
      OBS_DATA_WORK texlist_dwork,
      int reg_index)
    {
        if (!amDrawIsRegistComplete(reg_index))
            return false;
        texlist_dwork.pData = null;
        return true;
    }

    private static void ObjObjectAction3dESTextureSetByDwork(
      OBS_OBJECT_WORK obj_work,
      OBS_DATA_WORK texlist_dwork)
    {
        OBS_ACTION3D_ES_WORK obj3des = obj_work.obj_3des;
        object texlist_buf = null;
        ObjAction3dESTextureLoadToDwork(texlist_dwork, null, ref texlist_buf);
        obj3des.texlist_data_work = texlist_dwork;
        obj3des.texlist = (NNS_TEXLIST)texlist_dwork.pData;
        obj3des.texlistbuf = texlist_dwork.pData;
    }

    private static void ObjAction3dESModelLoad(
      OBS_ACTION3D_ES_WORK obj_3des,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive,
      uint drawflag,
      bool load_model)
    {
        object buf = null;
        if (archive != null)
            obj_3des.flag |= 262144U;
        if (filename != null && filename != "")
        {
            buf = ObjDataLoad(data_work, filename, archive);
            if (archive != null && buf == null)
            {
                obj_3des.flag &= 4294705151U;
                buf = ObjDataLoad(data_work, filename, null);
            }
        }
        else if (archive != null)
        {
            buf = ObjDataLoadAmbIndex(data_work, index, archive);
            if (buf == null)
                obj_3des.flag &= 4294705151U;
        }
        else if (data_work != null)
            buf = ObjDataGetInc(data_work);
        if (buf == null)
            return;
        obj_3des.model = buf;
        if (data_work != null)
            obj_3des.model_data_work = data_work;
        if (!load_model)
            return;
        NNS_TEXLIST texlist = null;
        object texlistbuf = null;
        obj_3des.model_reg_index = amObjectLoad(out obj_3des._object, out texlist, out texlistbuf, buf, drawflag | g_obj.load_drawflag, null, null);
        obj_3des.flag |= 2147483648U;
        amEffectSetObject(obj_3des.ecb, obj_3des._object, 16);
    }

    private static void ObjObjectAction3dESModelLoad(
      OBS_OBJECT_WORK obj_work,
      OBS_ACTION3D_ES_WORK obj_3des,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive,
      uint drawflag,
      bool load_model)
    {
        ObjAction3dESModelLoad(obj_3des, data_work, filename, index, archive, drawflag, load_model);
    }

    private static void ObjAction3dESModelRelease(OBS_ACTION3D_ES_WORK obj_3des)
    {
        obj_3des.model_reg_index = obj_3des.object_data_work == null ? amObjectRelease(obj_3des._object) : ObjAction3dESModelReleaseDwork(obj_3des.object_data_work);
        if (obj_3des.model_reg_index != -1)
        {
            obj_3des.flag |= 2147483648U;
        }
        else
        {
            obj_3des.object_data_work = null;
            obj_3des._object = null;
        }
    }

    private static bool ObjAction3dESModelReleaseCheck(OBS_ACTION3D_ES_WORK obj_3des)
    {
        if (((int)obj_3des.flag & int.MinValue) == 0)
            return true;
        if (obj_3des.object_data_work != null)
        {
            if (ObjAction3dESModelReleaseDworkCheck(obj_3des.object_data_work, obj_3des.model_reg_index))
            {
                obj_3des.object_data_work = null;
                obj_3des._object = null;
                obj_3des.model_reg_index = -1;
                obj_3des.flag &= int.MaxValue;
                return true;
            }
        }
        else if (amDrawIsRegistComplete(obj_3des.model_reg_index))
        {
            obj_3des._object = null;
            obj_3des.model_reg_index = -1;
            obj_3des.flag &= int.MaxValue;
            return true;
        }
        return false;
    }

    private static int ObjAction3dESModelLoadToDwork(
      OBS_DATA_WORK object_dwork,
      object model,
      uint drawflag)
    {
        int num;
        if (object_dwork.pData == null)
        {
            NNS_TEXLIST texlist = null;
            object texlistbuf = null;
            NNS_OBJECT _object;
            num = amObjectLoad(out _object, out texlist, out texlistbuf, model, drawflag | g_obj.load_drawflag, null, null);
            texlistbuf = null;
            ObjDataSet(object_dwork, _object);
        }
        else
        {
            ObjDataGetInc(object_dwork);
            num = -1;
        }
        return num;
    }

    private static int ObjAction3dESModelReleaseDwork(OBS_DATA_WORK object_dwork)
    {
        int num = -1;
        if (object_dwork.num != 0 && object_dwork.pData != null)
        {
            --object_dwork.num;
            if (object_dwork.num == 0)
                num = amObjectRelease((NNS_OBJECT)object_dwork.pData);
        }
        return num;
    }

    private static bool ObjAction3dESModelReleaseDworkCheck(
      OBS_DATA_WORK object_dwork,
      int reg_index)
    {
        if (!amDrawIsRegistComplete(reg_index))
            return false;
        object_dwork.pData = null;
        return true;
    }

    private static void ObjObjectAction3dESModelSetByDwork(
      OBS_OBJECT_WORK obj_work,
      OBS_DATA_WORK object_dwork)
    {
        OBS_ACTION3D_ES_WORK obj3des = obj_work.obj_3des;
        ObjAction3dESModelLoadToDwork(object_dwork, null, 0U);
        obj3des.object_data_work = object_dwork;
        obj3des._object = (NNS_OBJECT)object_dwork.pData;
        amEffectSetObject(obj3des.ecb, obj3des._object, 16);
    }

    private static bool ObjAction3dESEffectLoadCheck(OBS_ACTION3D_ES_WORK obj_3des)
    {
        bool flag = true;
        if (((int)obj_3des.flag & 1073741824) != 0)
        {
            if (amDrawIsRegistComplete(obj_3des.tex_reg_index))
            {
                obj_3des.flag &= 3221225471U;
                obj_3des.tex_reg_index = -1;
            }
            else
                flag = false;
        }
        if (((int)obj_3des.flag & int.MinValue) != 0)
        {
            if (amDrawIsRegistComplete(obj_3des.model_reg_index))
            {
                obj_3des.flag &= int.MaxValue;
                obj_3des.tex_reg_index = -1;
            }
            else
                flag = false;
        }
        return flag;
    }

    private static void ObjAction2dAMALoad(
      OBS_ACTION2D_AMA_WORK obj_2d,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive,
      AMS_AMB_HEADER amb_tex,
      uint id,
      int type_node)
    {
        A2S_AMA_HEADER a2SAmaHeader = null;
        ObjAction2dAMAWorkInit(obj_2d);
        if (archive != null)
            obj_2d.flag |= 2147483648U;
        if (filename != null)
        {
            a2SAmaHeader = readAMAFile(ObjDataLoad(data_work, filename, archive));
            if (archive != null && a2SAmaHeader == null)
            {
                obj_2d.flag &= int.MaxValue;
                a2SAmaHeader = readAMAFile(ObjDataLoad(data_work, filename, null));
            }
        }
        else if (archive != null)
        {
            a2SAmaHeader = readAMAFile(ObjDataLoadAmbIndex(data_work, index, archive));
            if (a2SAmaHeader == null)
                obj_2d.flag &= int.MaxValue;
        }
        else if (data_work != null)
            a2SAmaHeader = readAMAFile(ObjDataGetInc(data_work));
        if (a2SAmaHeader == null)
            return;
        obj_2d.ama = a2SAmaHeader;
        if (data_work != null)
            obj_2d.ama_data_work = data_work;
        obj_2d.type_node = type_node;
        obj_2d.act_id = id;
        AoTexBuild(obj_2d.ao_tex, amb_tex);
        AoTexLoad(obj_2d.ao_tex);
        obj_2d.flag |= 1073741824U;
        obj_2d.flag &= 3758096383U;
    }

    private static void ObjObjectAction2dAMALoad(
      OBS_OBJECT_WORK obj_work,
      OBS_ACTION2D_AMA_WORK obj_2d,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive,
      AMS_AMB_HEADER amb_tex,
      uint id,
      int type_node)
    {
        if (obj_2d == null)
        {
            obj_2d = obj_work.obj_2d == null ? new OBS_ACTION2D_AMA_WORK() : obj_work.obj_2d;
            obj_2d.Clear();
            obj_work.flag |= 67108864U;
        }
        obj_work.obj_2d = obj_2d;
        ObjAction2dAMALoad(obj_2d, data_work, filename, index, archive, amb_tex, id, type_node);
    }

    private static void ObjAction2dAMALoadSetTexlist(
      OBS_ACTION2D_AMA_WORK obj_2d,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive,
      NNS_TEXLIST texlist,
      uint id,
      int type_node)
    {
        A2S_AMA_HEADER a2SAmaHeader = null;
        ObjAction2dAMAWorkInit(obj_2d);
        if (archive != null)
            obj_2d.flag |= 2147483648U;
        if (filename != null)
        {
            a2SAmaHeader = readAMAFile(ObjDataLoad(data_work, filename, archive));
            if (archive != null && a2SAmaHeader == null)
            {
                obj_2d.flag &= int.MaxValue;
                a2SAmaHeader = readAMAFile(ObjDataLoad(data_work, filename, null));
            }
        }
        else if (archive != null)
        {
            a2SAmaHeader = readAMAFile(ObjDataLoadAmbIndex(data_work, index, archive));
            if (a2SAmaHeader == null)
                obj_2d.flag &= int.MaxValue;
        }
        else if (data_work != null)
            a2SAmaHeader = readAMAFile(ObjDataGetInc(data_work));
        if (a2SAmaHeader == null)
            return;
        obj_2d.ama = a2SAmaHeader;
        if (data_work != null)
            obj_2d.ama_data_work = data_work;
        obj_2d.type_node = type_node;
        obj_2d.act_id = id;
        obj_2d.texlist = texlist;
        obj_2d.flag |= 536870912U;
        ObjAction2dAMACreate(obj_2d);
    }

    private static void ObjObjectAction2dAMALoadSetTexlist(
      OBS_OBJECT_WORK obj_work,
      OBS_ACTION2D_AMA_WORK obj_2d,
      OBS_DATA_WORK data_work,
      string filename,
      int index,
      AMS_AMB_HEADER archive,
      NNS_TEXLIST texlist,
      uint id,
      int type_node)
    {
        if (obj_2d == null)
        {
            obj_2d = obj_work.obj_2d == null ? new OBS_ACTION2D_AMA_WORK() : obj_work.obj_2d;
            obj_2d.Clear();
            obj_work.flag |= 67108864U;
        }
        obj_work.obj_2d = obj_2d;
        ObjAction2dAMALoadSetTexlist(obj_2d, data_work, filename, index, archive, texlist, id, type_node);
    }

    private static void ObjAction2dAMAWorkInit(OBS_ACTION2D_AMA_WORK obj_2d)
    {
        obj_2d.Clear();
        obj_2d.speed = 1f;
        obj_2d.color.a = byte.MaxValue;
        obj_2d.color.r = byte.MaxValue;
        obj_2d.color.g = byte.MaxValue;
        obj_2d.color.b = byte.MaxValue;
        obj_2d.fade.a = 0;
        obj_2d.fade.r = 0;
        obj_2d.fade.g = 0;
        obj_2d.fade.b = 0;
    }

    private static void ObjAction2dAMACreate(OBS_ACTION2D_AMA_WORK obj_2d)
    {
        AoActSetTexture(obj_2d.texlist);
        obj_2d.act = obj_2d.type_node == 0 ? AoActCreate(obj_2d.ama, obj_2d.act_id, 0.0f) : AoActCreateNode(obj_2d.ama, obj_2d.act_id, 0.0f);
        AoActSetTexture(null);
    }

    private static bool ObjAction2dAMALoadCheck(OBS_ACTION2D_AMA_WORK obj_2d)
    {
        if (((int)obj_2d.flag & 1073741824) != 0)
        {
            if (AoTexIsLoaded(obj_2d.ao_tex))
            {
                obj_2d.texlist = AoTexGetTexList(obj_2d.ao_tex);
                obj_2d.flag &= 3221225471U;
                obj_2d.flag |= 536870912U;
                ObjAction2dAMACreate(obj_2d);
                return true;
            }
        }
        else if (((int)obj_2d.flag & 536870912) != 0)
            return true;
        return false;
    }

}