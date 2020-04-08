using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static void ObjAction3dNNModelLoad(
     AppMain.OBS_ACTION3D_NN_WORK obj_3d,
     AppMain.OBS_DATA_WORK data_work,
     string filename,
     int index,
     AppMain.AMS_AMB_HEADER archive,
     string filename_tex,
     AppMain.AMS_AMB_HEADER amb_tex,
     uint drawflag)
    {
        object buf = (object)null;
        string filepath = (string)null;
        obj_3d.command_state = 0U;
        obj_3d.marge = 0.0f;
        obj_3d.per = 1f;
        obj_3d.use_light_flag = AppMain.g_obj.def_user_light_flag;
        AppMain.nnMakeUnitMatrix(obj_3d.user_obj_mtx);
        AppMain.nnMakeUnitMatrix(obj_3d.user_obj_mtx_r);
        for (int index1 = 0; index1 < 2; ++index1)
            obj_3d.speed[index1] = 1f;
        obj_3d.mat_speed = 1f;
        obj_3d.blend_spd = 0.25f;
        obj_3d.drawflag = drawflag;
        obj_3d.draw_state.Assign(AppMain.g_obj_draw_3dnn_draw_state);
        if (archive != null)
            obj_3d.flag |= 65536U;
        if (filename != null && filename != "")
        {
            buf = (object)AppMain.ObjDataLoad(data_work, filename, (object)archive);
            if (archive != null && buf == null)
            {
                obj_3d.flag &= 4294901759U;
                buf = (object)AppMain.ObjDataLoad(data_work, filename, (object)null);
            }
        }
        else if (archive != null)
        {
            buf = AppMain.ObjDataLoadAmbIndex(data_work, index, archive);
            if (buf == null)
                obj_3d.flag &= 4294901759U;
        }
        else if (data_work != null)
            buf = AppMain.ObjDataGetInc(data_work);
        if (buf == null)
            return;
        obj_3d.model = buf;
        if (data_work != null)
            obj_3d.model_data_work = data_work;
        if (filename_tex != null && filename_tex != "")
        {
            AppMain.sFile = filename_tex;
            filepath = AppMain.sFile;
        }
        else
            AppMain.sFile = "";
        obj_3d.reg_index = AppMain.amObjectLoad(out obj_3d._object, out obj_3d.texlist, out obj_3d.texlistbuf, buf, drawflag | AppMain.g_obj.load_drawflag, filepath, amb_tex);
        if (AppMain.obj_load_initial_set_flag)
        {
            AppMain.OBS_LOAD_INITIAL_WORK objLoadInitialWork = AppMain.obj_load_initial_work;
            if (objLoadInitialWork.obj_num < (int)byte.MaxValue)
            {
                objLoadInitialWork.obj_3d[objLoadInitialWork.obj_num] = obj_3d;
                ++objLoadInitialWork.obj_num;
            }
        }
        obj_3d.flag |= 2147483648U;
        obj_3d.flag &= 3221225471U;
    }

    private static void ObjCopyAction3dNNModel(
      AppMain.OBS_ACTION3D_NN_WORK src_obj_3d,
      AppMain.OBS_ACTION3D_NN_WORK dest_obj_3d)
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
        AppMain.nnMakeUnitMatrix(dest_obj_3d.user_obj_mtx);
        AppMain.nnMakeUnitMatrix(dest_obj_3d.user_obj_mtx_r);
        dest_obj_3d.blend_spd = 0.25f;
        dest_obj_3d.sub_obj_type = src_obj_3d.sub_obj_type;
        dest_obj_3d.drawflag = src_obj_3d.drawflag;
        dest_obj_3d.draw_state.Assign(AppMain.g_obj_draw_3dnn_draw_state);
        dest_obj_3d.reg_index = -1;
    }

    private static void ObjObjectCopyAction3dNNModel(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.OBS_ACTION3D_NN_WORK src_obj_3d,
      AppMain.OBS_ACTION3D_NN_WORK dest_obj_3d)
    {
        if (dest_obj_3d == null)
        {
            dest_obj_3d = obj_work.obj_3d == null ? new AppMain.OBS_ACTION3D_NN_WORK() : obj_work.obj_3d;
            dest_obj_3d.Clear();
            obj_work.flag |= 134217728U;
        }
        obj_work.flag |= 536870912U;
        AppMain.ObjCopyAction3dNNModel(src_obj_3d, dest_obj_3d);
        obj_work.obj_3d = dest_obj_3d;
    }

    private static void ObjObjectAction3dNNModelReleaseCopy(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.flag & 134217728) != 0)
        {
            obj_work.obj_3d = (AppMain.OBS_ACTION3D_NN_WORK)null;
            obj_work.flag &= 4160749567U;
        }
        obj_work.obj_3d = (AppMain.OBS_ACTION3D_NN_WORK)null;
        obj_work.flag &= 3758096383U;
    }

    private static void ObjObjectAction3dNNModelLoad(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.OBS_ACTION3D_NN_WORK obj_3d,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive,
      string filename_tex,
      AppMain.AMS_AMB_HEADER amb_tex,
      uint drawflag)
    {
        if (obj_3d == null)
        {
            obj_3d = obj_work.obj_3d == null ? new AppMain.OBS_ACTION3D_NN_WORK() : obj_work.obj_3d;
            obj_3d.Clear();
            obj_work.flag |= 134217728U;
        }
        obj_work.obj_3d = obj_3d;
        AppMain.ObjAction3dNNModelLoad(obj_3d, data_work, filename, index, archive, filename_tex, amb_tex, drawflag);
    }

    private static void ObjAction3dNNModelLoadTxb(
      AppMain.OBS_ACTION3D_NN_WORK obj_3d,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive,
      string filename_tex,
      AppMain.AMS_AMB_HEADER amb_tex,
      uint drawflag,
      AppMain.TXB_HEADER txb)
    {
        object buf = (object)null;
        string filepath = (string)null;
        obj_3d.command_state = 0U;
        obj_3d.marge = 0.0f;
        obj_3d.per = 1f;
        obj_3d.use_light_flag = AppMain.g_obj.def_user_light_flag;
        AppMain.nnMakeUnitMatrix(obj_3d.user_obj_mtx);
        AppMain.nnMakeUnitMatrix(obj_3d.user_obj_mtx_r);
        for (int index1 = 0; index1 < 2; ++index1)
            obj_3d.speed[index1] = 1f;
        obj_3d.blend_spd = 0.25f;
        obj_3d.drawflag = drawflag;
        obj_3d.draw_state.Assign(AppMain.g_obj_draw_3dnn_draw_state);
        if (archive != null)
            obj_3d.flag |= 65536U;
        if (filename != null)
        {
            buf = (object)AppMain.ObjDataLoad(data_work, filename, (object)archive);
            if (archive != null && buf == null)
            {
                obj_3d.flag &= 4294901759U;
                buf = (object)AppMain.ObjDataLoad(data_work, filename, (object)null);
            }
        }
        else if (archive != null)
        {
            buf = AppMain.ObjDataLoadAmbIndex(data_work, index, archive);
            if (buf == null)
                obj_3d.flag &= 4294901759U;
        }
        else if (data_work != null)
            buf = AppMain.ObjDataGetInc(data_work);
        if (buf == null)
            return;
        obj_3d.model = buf;
        if (data_work != null)
            obj_3d.model_data_work = data_work;
        if (filename_tex != null && filename_tex != "")
        {
            AppMain.sFile = filename_tex;
            filepath = AppMain.sFile;
        }
        else
            AppMain.sFile = "";
        obj_3d.reg_index = AppMain.amObjectLoad(out obj_3d._object, AppMain.amTxbGetTexFileList(txb), out obj_3d.texlist, out obj_3d.texlistbuf, buf, drawflag | AppMain.g_obj.load_drawflag, filepath, amb_tex);
        if (AppMain.obj_load_initial_set_flag)
        {
            AppMain.OBS_LOAD_INITIAL_WORK objLoadInitialWork = AppMain.obj_load_initial_work;
            if (objLoadInitialWork.obj_num < (int)byte.MaxValue)
            {
                objLoadInitialWork.obj_3d[objLoadInitialWork.obj_num] = obj_3d;
                ++objLoadInitialWork.obj_num;
            }
        }
        obj_3d.flag |= 2147483648U;
        obj_3d.flag &= 3221225471U;
    }

    private static void ObjObjectAction3dNNModelLoadTxb(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.OBS_ACTION3D_NN_WORK obj_3d,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive,
      string filename_tex,
      AppMain.AMS_AMB_HEADER amb_tex,
      uint drawflag,
      AppMain.TXB_HEADER txb)
    {
        if (obj_3d == null)
        {
            obj_3d = obj_work.obj_3d == null ? new AppMain.OBS_ACTION3D_NN_WORK() : obj_work.obj_3d;
            obj_3d.Clear();
            obj_work.flag |= 134217728U;
        }
        obj_work.obj_3d = obj_3d;
        AppMain.ObjAction3dNNModelLoadTxb(obj_3d, data_work, filename, index, archive, filename_tex, amb_tex, drawflag, txb);
    }

    private static void ObjAction3dNNMotionLoad(
      AppMain.OBS_ACTION3D_NN_WORK obj_3d,
      int reg_file_id,
      bool marge,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive)
    {
        AppMain.ObjAction3dNNMotionLoad(obj_3d, reg_file_id, marge, data_work, filename, index, archive, 64, 16);
    }

    private static void ObjAction3dNNMotionLoad(
      AppMain.OBS_ACTION3D_NN_WORK obj_3d,
      int reg_file_id,
      bool marge,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive,
      int motion_num,
      int mmotion_num)
    {
        object obj = (object)null;
        if (((int)obj_3d.flag & 1073741824) == 0)
        {
            obj_3d.flag |= 536870912U;
            AppMain.OBS_ACTION3D_MTN_LOAD_SETTING n3DMtnLoadSetting = obj_3d.mtn_load_setting[reg_file_id];
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
                obj = (object)AppMain.ObjDataLoad(data_work, filename, (object)archive);
                if (archive != null && obj == null)
                {
                    obj_3d.flag &= (uint)~(131072 << reg_file_id);
                    obj = (object)AppMain.ObjDataLoad(data_work, filename, (object)null);
                }
            }
            else if (archive != null)
            {
                obj = AppMain.ObjDataLoadAmbIndex(data_work, index, archive);
                if (archive != null && obj == null)
                    obj_3d.flag &= (uint)~(131072 << reg_file_id);
            }
            else if (data_work != null)
                obj = AppMain.ObjDataGetInc(data_work);
            if (obj == null)
                return;
            obj_3d.mtn[reg_file_id] = obj;
            if (data_work != null)
                obj_3d.mtn_data_work[reg_file_id] = data_work;
            if (obj_3d.motion == null)
                obj_3d.motion = AppMain.amMotionCreate(obj_3d._object, motion_num, mmotion_num, marge ? 1 : 0);
            switch (obj)
            {
                case AppMain.AMS_AMB_HEADER _:
                case AppMain.AMS_FS _:
                    AppMain.AMS_AMB_HEADER amb = AppMain.readAMBFile(obj);
                    AppMain.amMotionRegistFile(obj_3d.motion, reg_file_id, amb);
                    break;
                default:
                    AppMain.amMotionRegistFile(obj_3d.motion, reg_file_id, obj);
                    break;
            }
        }
    }

    private static void ObjObjectAction3dNNMotionLoad(
      AppMain.OBS_OBJECT_WORK obj_work,
      int reg_file_id,
      bool marge,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive)
    {
        AppMain.ObjObjectAction3dNNMotionLoad(obj_work, reg_file_id, marge, data_work, filename, index, archive, 64, 16);
    }

    private static void ObjObjectAction3dNNMotionLoad(
      AppMain.OBS_OBJECT_WORK obj_work,
      int reg_file_id,
      bool marge,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive,
      int motion_num,
      int mmotion_num)
    {
        AppMain.ObjAction3dNNMotionLoad(obj_work.obj_3d, reg_file_id, marge, data_work, filename, index, archive, motion_num, mmotion_num);
    }

    private static void ObjAction3dNNMaterialMotionLoad(
      AppMain.OBS_ACTION3D_NN_WORK obj_3d,
      int reg_file_id,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive)
    {
        AppMain.ObjAction3dNNMaterialMotionLoad(obj_3d, reg_file_id, data_work, filename, index, archive, 64, 16);
    }

    private static void ObjAction3dNNMaterialMotionLoad(
      AppMain.OBS_ACTION3D_NN_WORK obj_3d,
      int reg_file_id,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive,
      int motion_num,
      int mmotion_num)
    {
        object obj = (object)null;
        if (((int)obj_3d.flag & 1073741824) == 0)
        {
            obj_3d.flag |= 268435456U;
            AppMain.OBS_ACTION3D_MTN_LOAD_SETTING n3DMtnLoadSetting = obj_3d.mat_mtn_load_setting[reg_file_id];
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
                obj = (object)AppMain.ObjDataLoad(data_work, filename, (object)archive);
                if (archive != null && obj == null)
                {
                    obj_3d.flag &= (uint)~(4096 << reg_file_id);
                    obj = (object)AppMain.ObjDataLoad(data_work, filename, (object)null);
                }
            }
            else if (archive != null)
            {
                obj = AppMain.ObjDataLoadAmbIndex(data_work, index, archive);
                if (archive != null && obj == null)
                    obj_3d.flag &= (uint)~(4096 << reg_file_id);
            }
            else if (data_work != null)
                obj = AppMain.ObjDataGetInc(data_work);
            if (obj == null)
                return;
            obj_3d.mat_mtn[reg_file_id] = obj;
            if (data_work != null)
                obj_3d.mat_mtn_data_work[reg_file_id] = data_work;
            if (obj_3d.motion == null)
                obj_3d.motion = AppMain.amMotionCreate(obj_3d._object, motion_num, mmotion_num, 0);
            switch (obj)
            {
                case AppMain.AMS_AMB_HEADER _:
                case AppMain.AMS_FS _:
                    AppMain.AMS_AMB_HEADER amb = AppMain.readAMBFile(obj);
                    AppMain.amMotionMaterialRegistFile(obj_3d.motion, reg_file_id, amb);
                    break;
                default:
                    AppMain.amMotionMaterialRegistFile(obj_3d.motion, reg_file_id, obj);
                    break;
            }
        }
    }

    private static void ObjObjectAction3dNNMaterialMotionLoad(
      AppMain.OBS_OBJECT_WORK obj_work,
      int reg_file_id,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      object archive)
    {
        AppMain.ObjObjectAction3dNNMaterialMotionLoad(obj_work, reg_file_id, data_work, filename, index, archive, 64, 16);
    }

    private static void ObjObjectAction3dNNMaterialMotionLoad(
      AppMain.OBS_OBJECT_WORK obj_work,
      int reg_file_id,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      object archive,
      int motion_num,
      int mmotion_num)
    {
        AppMain.ObjAction3dNNMaterialMotionLoad(obj_work.obj_3d, reg_file_id, data_work, filename, index, (AppMain.AMS_AMB_HEADER)archive, motion_num, mmotion_num);
    }

    private static bool ObjAction3dNNModelLoadCheck(AppMain.OBS_ACTION3D_NN_WORK obj_3d)
    {
        if (((int)obj_3d.flag & int.MinValue) != 0)
        {
            if (AppMain.amDrawIsRegistComplete(obj_3d.reg_index))
            {
                obj_3d.flag &= (uint)int.MaxValue;
                obj_3d.flag |= 1073741824U;
                obj_3d.reg_index = -1;
                if (obj_3d.model_data_work != null)
                {
                    AppMain.ObjDataRelease(obj_3d.model_data_work);
                    obj_3d.model_data_work = (AppMain.OBS_DATA_WORK)null;
                }
                else if (obj_3d.model != null && ((int)obj_3d.flag & 65536) == 0)
                    obj_3d.model = (object)null;
                obj_3d.flag &= 4294901759U;
                obj_3d.model = (object)null;
                if (((int)obj_3d.flag & 536870912) != 0)
                {
                    int reg_file_id = 0;
                    AppMain.ArrayPointer<AppMain.OBS_ACTION3D_MTN_LOAD_SETTING> arrayPointer = new AppMain.ArrayPointer<AppMain.OBS_ACTION3D_MTN_LOAD_SETTING>(obj_3d.mtn_load_setting);
                    while (reg_file_id < 4)
                    {
                        if (((AppMain.OBS_ACTION3D_MTN_LOAD_SETTING)~arrayPointer).enable)
                        {
                            AppMain.ObjAction3dNNMotionLoad(obj_3d, reg_file_id, ((AppMain.OBS_ACTION3D_MTN_LOAD_SETTING)~arrayPointer).marge, ((AppMain.OBS_ACTION3D_MTN_LOAD_SETTING)~arrayPointer).data_work, ((AppMain.OBS_ACTION3D_MTN_LOAD_SETTING)~arrayPointer).filename, ((AppMain.OBS_ACTION3D_MTN_LOAD_SETTING)~arrayPointer).index, ((AppMain.OBS_ACTION3D_MTN_LOAD_SETTING)~arrayPointer).archive);
                            ((AppMain.OBS_ACTION3D_MTN_LOAD_SETTING)~arrayPointer).enable = false;
                        }
                        ++reg_file_id;
                        ++arrayPointer;
                    }
                    obj_3d.flag &= 3758096383U;
                }
                if (((int)obj_3d.flag & 268435456) != 0)
                {
                    int reg_file_id = 0;
                    AppMain.ArrayPointer<AppMain.OBS_ACTION3D_MTN_LOAD_SETTING> arrayPointer = new AppMain.ArrayPointer<AppMain.OBS_ACTION3D_MTN_LOAD_SETTING>(obj_3d.mat_mtn_load_setting);
                    while (reg_file_id < 4)
                    {
                        if (((AppMain.OBS_ACTION3D_MTN_LOAD_SETTING)~arrayPointer).enable)
                        {
                            AppMain.ObjAction3dNNMaterialMotionLoad(obj_3d, reg_file_id, ((AppMain.OBS_ACTION3D_MTN_LOAD_SETTING)~arrayPointer).data_work, ((AppMain.OBS_ACTION3D_MTN_LOAD_SETTING)~arrayPointer).filename, ((AppMain.OBS_ACTION3D_MTN_LOAD_SETTING)~arrayPointer).index, ((AppMain.OBS_ACTION3D_MTN_LOAD_SETTING)~arrayPointer).archive);
                            ((AppMain.OBS_ACTION3D_MTN_LOAD_SETTING)~arrayPointer).enable = false;
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

    private static void ObjAction3dNNModelRelease(AppMain.OBS_ACTION3D_NN_WORK obj_3d)
    {
        obj_3d.reg_index = AppMain.amObjectRelease(obj_3d._object, obj_3d.texlist);
        obj_3d.flag |= 134217728U;
    }

    private static bool ObjAction3dNNModelReleaseCheck(AppMain.OBS_ACTION3D_NN_WORK obj_3d)
    {
        if (((int)obj_3d.flag & 1207959552) == 0)
            return true;
        if (!AppMain.amDrawIsRegistComplete(obj_3d.reg_index))
            return false;
        if (obj_3d._object != null)
            obj_3d._object = (AppMain.NNS_OBJECT)null;
        if (obj_3d.texlistbuf != null)
            obj_3d.texlistbuf = (object)null;
        obj_3d.flag &= 3087007743U;
        if (obj_3d.model_data_work != null)
        {
            AppMain.ObjDataRelease(obj_3d.model_data_work);
            obj_3d.model_data_work = (AppMain.OBS_DATA_WORK)null;
        }
        else if (obj_3d.model != null && ((int)obj_3d.flag & 65536) == 0)
            obj_3d.model = (object)null;
        obj_3d.flag &= 4294901759U;
        return true;
    }

    private static void ObjAction3dNNMotionRelease(AppMain.OBS_ACTION3D_NN_WORK obj_3d)
    {
        if (obj_3d.motion != null)
        {
            AppMain.amMotionDelete(obj_3d.motion);
            obj_3d.motion = (AppMain.AMS_MOTION)null;
        }
        for (int index = 0; index < 4; ++index)
        {
            if (obj_3d.mtn_data_work[index] != null)
            {
                AppMain.ObjDataRelease(obj_3d.mtn_data_work[index]);
                obj_3d.mtn_data_work[index] = (AppMain.OBS_DATA_WORK)null;
            }
            else if (obj_3d.mtn[index] != null)
            {
                int num1 = (int)obj_3d.flag & 131072 << index;
            }
            obj_3d.flag &= (uint)~(131072 << index);
            obj_3d.mtn[index] = (object)null;
            if (obj_3d.mat_mtn_data_work[index] != null)
            {
                AppMain.ObjDataRelease(obj_3d.mat_mtn_data_work[index]);
                obj_3d.mat_mtn_data_work[index] = (AppMain.OBS_DATA_WORK)null;
            }
            else if (obj_3d.mat_mtn[index] != null)
            {
                int num2 = (int)obj_3d.flag & 4096 << index;
            }
            obj_3d.flag &= (uint)~(4096 << index);
            obj_3d.mat_mtn[index] = (object)null;
        }
    }

    private static void ObjAction3dESEffectLoad(
      AppMain.OBS_ACTION3D_ES_WORK obj_3des,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive,
      int user_attr,
      int ecb_prio)
    {
        object obj = (object)null;
        obj_3des.command_state = 0U;
        obj_3des.user_attr = user_attr;
        obj_3des.speed = 1f;
        if (archive != null)
            obj_3des.flag |= 65536U;
        if (filename != null && filename != "")
        {
            obj = (object)AppMain.ObjDataLoad(data_work, filename, (object)archive);
            if (archive != null && obj == null)
            {
                obj_3des.flag &= 4294901759U;
                obj = (object)AppMain.ObjDataLoad(data_work, filename, (object)null);
            }
        }
        else if (archive != null)
        {
            obj = AppMain.ObjDataLoadAmbIndex(data_work, index, archive);
            if (obj == null)
                obj_3des.flag &= 4294901759U;
        }
        else if (data_work != null)
            obj = AppMain.ObjDataGetInc(data_work);
        if (obj == null)
            return;
        obj_3des.eff = obj;
        if (data_work != null)
            obj_3des.eff_data_work = data_work;
        obj_3des.ecb = AppMain._amEffectCreate((AppMain.AMS_AME_HEADER)obj_3des.eff, user_attr, ecb_prio);
    }

    private static void ObjObjectAction3dESEffectLoad(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.OBS_ACTION3D_ES_WORK obj_3des,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive)
    {
        AppMain.ObjObjectAction3dESEffectLoad(obj_work, obj_3des, data_work, filename, index, archive, 0, 0);
    }

    private static void ObjObjectAction3dESEffectLoad(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.OBS_ACTION3D_ES_WORK obj_3des,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive,
      int user_attr,
      int ecb_prio)
    {
        if (obj_3des == null)
        {
            obj_3des = obj_work.obj_3des == null ? new AppMain.OBS_ACTION3D_ES_WORK() : obj_work.obj_3des;
            obj_3des.Clear();
            obj_work.flag |= 268435456U;
        }
        obj_work.obj_3des = obj_3des;
        AppMain.ObjAction3dESEffectLoad(obj_3des, data_work, filename, index, archive, user_attr, ecb_prio);
    }

    private static void ObjAction3dESEffectRelease(AppMain.OBS_ACTION3D_ES_WORK obj_3des)
    {
        AppMain.amEffectDelete(obj_3des.ecb);
        obj_3des.ecb = (AppMain.AMS_AME_ECB)null;
    }

    private static void ObjAction3dESTextureLoad(
      AppMain.OBS_ACTION3D_ES_WORK obj_3des,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive,
      bool load_tex)
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = (AppMain.AMS_AMB_HEADER)null;
        if (archive != null)
            obj_3des.flag |= 131072U;
        if (filename != null && filename != "")
        {
            amsAmbHeader = AppMain.readAMBFile((object)AppMain.ObjDataLoad(data_work, filename, (object)archive));
            if (archive != null && amsAmbHeader == null)
            {
                obj_3des.flag &= 4294836223U;
                amsAmbHeader = AppMain.readAMBFile((object)AppMain.ObjDataLoad(data_work, filename, (object)null));
            }
        }
        else if (archive != null)
        {
            amsAmbHeader = AppMain.readAMBFile(AppMain.ObjDataLoadAmbIndex(data_work, index, archive));
            if (amsAmbHeader == null)
                obj_3des.flag &= 4294836223U;
        }
        else if (data_work != null)
            amsAmbHeader = AppMain.readAMBFile(AppMain.ObjDataGetInc(data_work));
        if (amsAmbHeader == null)
            return;
        obj_3des.ambtex = (object)amsAmbHeader;
        if (data_work != null)
            obj_3des.ambtex_data_work = data_work;
        if (!load_tex)
            return;
        AppMain.TXB_HEADER txb = AppMain.readTXBfile(AppMain.amBindGet(amsAmbHeader, 0));
        uint count = AppMain.amTxbGetCount(txb);
        obj_3des.texlistbuf = (object)null;
        AppMain.nnSetUpTexlist(out obj_3des.texlist, (int)count, ref obj_3des.texlistbuf);
        if (AppMain.obj_load_initial_set_flag)
        {
            AppMain.OBS_LOAD_INITIAL_WORK objLoadInitialWork = AppMain.obj_load_initial_work;
            if (objLoadInitialWork.es_num < (int)byte.MaxValue)
            {
                objLoadInitialWork.obj_3des[objLoadInitialWork.es_num] = obj_3des;
                ++objLoadInitialWork.es_num;
            }
        }
        obj_3des.tex_reg_index = AppMain.amTextureLoad(obj_3des.texlist, AppMain.amTxbGetTexFileList(txb), filename, amsAmbHeader);
        obj_3des.flag |= 1073741824U;
    }

    private static void ObjObjectAction3dESTextureLoad(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.OBS_ACTION3D_ES_WORK obj_3des,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive,
      bool load_tex)
    {
        AppMain.ObjAction3dESTextureLoad(obj_3des, data_work, filename, index, archive, load_tex);
    }

    private static void ObjAction3dESTextureRelease(AppMain.OBS_ACTION3D_ES_WORK obj_3des)
    {
        obj_3des.tex_reg_index = obj_3des.texlist_data_work == null ? AppMain.amTextureRelease(obj_3des.texlist) : AppMain.ObjAction3dESTextureReleaseDwork(obj_3des.texlist_data_work);
        if (obj_3des.tex_reg_index != -1)
        {
            obj_3des.flag |= 1073741824U;
        }
        else
        {
            obj_3des.texlist_data_work = (AppMain.OBS_DATA_WORK)null;
            obj_3des.texlist = (AppMain.NNS_TEXLIST)null;
            obj_3des.texlistbuf = (object)null;
        }
    }

    private static bool ObjAction3dESTextureReleaseCheck(AppMain.OBS_ACTION3D_ES_WORK obj_3des)
    {
        if (((int)obj_3des.flag & 1073741824) == 0)
            return true;
        if (obj_3des.texlist_data_work != null)
        {
            if (AppMain.ObjAction3dESTextureReleaseDworkCheck(obj_3des.texlist_data_work, obj_3des.tex_reg_index))
            {
                obj_3des.texlist_data_work = (AppMain.OBS_DATA_WORK)null;
                obj_3des.texlist = (AppMain.NNS_TEXLIST)null;
                obj_3des.texlistbuf = (object)null;
                obj_3des.tex_reg_index = -1;
                obj_3des.flag &= 3221225471U;
                return true;
            }
        }
        else if (AppMain.amDrawIsRegistComplete(obj_3des.tex_reg_index))
        {
            obj_3des.texlist = (AppMain.NNS_TEXLIST)null;
            obj_3des.texlistbuf = (object)null;
            obj_3des.tex_reg_index = -1;
            obj_3des.flag &= 3221225471U;
            return true;
        }
        return false;
    }

    private static int ObjAction3dESTextureLoadToDwork(
      AppMain.OBS_DATA_WORK texlist_dwork,
      AppMain.AMS_AMB_HEADER amb_tex,
      ref object texlist_buf)
    {
        int num;
        if (texlist_dwork.pData == null)
        {
            AppMain.TXB_HEADER txb = AppMain.readTXBfile(AppMain.amBindGet(amb_tex, 0));
            uint count = AppMain.amTxbGetCount(txb);
            texlist_buf = (object)null;
            AppMain.NNS_TEXLIST texlist;
            AppMain.nnSetUpTexlist(out texlist, (int)count, ref texlist_buf);
            num = AppMain.amTextureLoad(texlist, AppMain.amTxbGetTexFileList(txb), (string)null, amb_tex);
            AppMain.ObjDataSet(texlist_dwork, (object)texlist);
        }
        else
        {
            AppMain.ObjDataGetInc(texlist_dwork);
            num = -1;
            texlist_buf = (object)null;
        }
        return num;
    }

    private static int ObjAction3dESTextureReleaseDwork(AppMain.OBS_DATA_WORK texlist_dwork)
    {
        int num = -1;
        if (texlist_dwork.num != (ushort)0 && texlist_dwork.pData != null)
        {
            --texlist_dwork.num;
            if (texlist_dwork.num == (ushort)0)
                num = AppMain.amTextureRelease((AppMain.NNS_TEXLIST)texlist_dwork.pData);
        }
        return num;
    }

    private static bool ObjAction3dESTextureReleaseDworkCheck(
      AppMain.OBS_DATA_WORK texlist_dwork,
      int reg_index)
    {
        if (!AppMain.amDrawIsRegistComplete(reg_index))
            return false;
        texlist_dwork.pData = (object)null;
        return true;
    }

    private static void ObjObjectAction3dESTextureSetByDwork(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.OBS_DATA_WORK texlist_dwork)
    {
        AppMain.OBS_ACTION3D_ES_WORK obj3des = obj_work.obj_3des;
        object texlist_buf = (object)null;
        AppMain.ObjAction3dESTextureLoadToDwork(texlist_dwork, (AppMain.AMS_AMB_HEADER)null, ref texlist_buf);
        obj3des.texlist_data_work = texlist_dwork;
        obj3des.texlist = (AppMain.NNS_TEXLIST)texlist_dwork.pData;
        obj3des.texlistbuf = texlist_dwork.pData;
    }

    private static void ObjAction3dESModelLoad(
      AppMain.OBS_ACTION3D_ES_WORK obj_3des,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive,
      uint drawflag,
      bool load_model)
    {
        object buf = (object)null;
        if (archive != null)
            obj_3des.flag |= 262144U;
        if (filename != null && filename != "")
        {
            buf = (object)AppMain.ObjDataLoad(data_work, filename, (object)archive);
            if (archive != null && buf == null)
            {
                obj_3des.flag &= 4294705151U;
                buf = (object)AppMain.ObjDataLoad(data_work, filename, (object)null);
            }
        }
        else if (archive != null)
        {
            buf = AppMain.ObjDataLoadAmbIndex(data_work, index, archive);
            if (buf == null)
                obj_3des.flag &= 4294705151U;
        }
        else if (data_work != null)
            buf = AppMain.ObjDataGetInc(data_work);
        if (buf == null)
            return;
        obj_3des.model = buf;
        if (data_work != null)
            obj_3des.model_data_work = data_work;
        if (!load_model)
            return;
        AppMain.NNS_TEXLIST texlist = (AppMain.NNS_TEXLIST)null;
        object texlistbuf = (object)null;
        obj_3des.model_reg_index = AppMain.amObjectLoad(out obj_3des._object, out texlist, out texlistbuf, buf, drawflag | AppMain.g_obj.load_drawflag, (string)null, (AppMain.AMS_AMB_HEADER)null);
        obj_3des.flag |= 2147483648U;
        AppMain.amEffectSetObject(obj_3des.ecb, obj_3des._object, 16);
    }

    private static void ObjObjectAction3dESModelLoad(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.OBS_ACTION3D_ES_WORK obj_3des,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive,
      uint drawflag,
      bool load_model)
    {
        AppMain.ObjAction3dESModelLoad(obj_3des, data_work, filename, index, archive, drawflag, load_model);
    }

    private static void ObjAction3dESModelRelease(AppMain.OBS_ACTION3D_ES_WORK obj_3des)
    {
        obj_3des.model_reg_index = obj_3des.object_data_work == null ? AppMain.amObjectRelease(obj_3des._object) : AppMain.ObjAction3dESModelReleaseDwork(obj_3des.object_data_work);
        if (obj_3des.model_reg_index != -1)
        {
            obj_3des.flag |= 2147483648U;
        }
        else
        {
            obj_3des.object_data_work = (AppMain.OBS_DATA_WORK)null;
            obj_3des._object = (AppMain.NNS_OBJECT)null;
        }
    }

    private static bool ObjAction3dESModelReleaseCheck(AppMain.OBS_ACTION3D_ES_WORK obj_3des)
    {
        if (((int)obj_3des.flag & int.MinValue) == 0)
            return true;
        if (obj_3des.object_data_work != null)
        {
            if (AppMain.ObjAction3dESModelReleaseDworkCheck(obj_3des.object_data_work, obj_3des.model_reg_index))
            {
                obj_3des.object_data_work = (AppMain.OBS_DATA_WORK)null;
                obj_3des._object = (AppMain.NNS_OBJECT)null;
                obj_3des.model_reg_index = -1;
                obj_3des.flag &= (uint)int.MaxValue;
                return true;
            }
        }
        else if (AppMain.amDrawIsRegistComplete(obj_3des.model_reg_index))
        {
            obj_3des._object = (AppMain.NNS_OBJECT)null;
            obj_3des.model_reg_index = -1;
            obj_3des.flag &= (uint)int.MaxValue;
            return true;
        }
        return false;
    }

    private static int ObjAction3dESModelLoadToDwork(
      AppMain.OBS_DATA_WORK object_dwork,
      object model,
      uint drawflag)
    {
        int num;
        if (object_dwork.pData == null)
        {
            AppMain.NNS_TEXLIST texlist = (AppMain.NNS_TEXLIST)null;
            object texlistbuf = (object)null;
            AppMain.NNS_OBJECT _object;
            num = AppMain.amObjectLoad(out _object, out texlist, out texlistbuf, model, drawflag | AppMain.g_obj.load_drawflag, (string)null, (AppMain.AMS_AMB_HEADER)null);
            texlistbuf = (object)null;
            AppMain.ObjDataSet(object_dwork, (object)_object);
        }
        else
        {
            AppMain.ObjDataGetInc(object_dwork);
            num = -1;
        }
        return num;
    }

    private static int ObjAction3dESModelReleaseDwork(AppMain.OBS_DATA_WORK object_dwork)
    {
        int num = -1;
        if (object_dwork.num != (ushort)0 && object_dwork.pData != null)
        {
            --object_dwork.num;
            if (object_dwork.num == (ushort)0)
                num = AppMain.amObjectRelease((AppMain.NNS_OBJECT)object_dwork.pData);
        }
        return num;
    }

    private static bool ObjAction3dESModelReleaseDworkCheck(
      AppMain.OBS_DATA_WORK object_dwork,
      int reg_index)
    {
        if (!AppMain.amDrawIsRegistComplete(reg_index))
            return false;
        object_dwork.pData = (object)null;
        return true;
    }

    private static void ObjObjectAction3dESModelSetByDwork(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.OBS_DATA_WORK object_dwork)
    {
        AppMain.OBS_ACTION3D_ES_WORK obj3des = obj_work.obj_3des;
        AppMain.ObjAction3dESModelLoadToDwork(object_dwork, (object)null, 0U);
        obj3des.object_data_work = object_dwork;
        obj3des._object = (AppMain.NNS_OBJECT)object_dwork.pData;
        AppMain.amEffectSetObject(obj3des.ecb, obj3des._object, 16);
    }

    private static bool ObjAction3dESEffectLoadCheck(AppMain.OBS_ACTION3D_ES_WORK obj_3des)
    {
        bool flag = true;
        if (((int)obj_3des.flag & 1073741824) != 0)
        {
            if (AppMain.amDrawIsRegistComplete(obj_3des.tex_reg_index))
            {
                obj_3des.flag &= 3221225471U;
                obj_3des.tex_reg_index = -1;
            }
            else
                flag = false;
        }
        if (((int)obj_3des.flag & int.MinValue) != 0)
        {
            if (AppMain.amDrawIsRegistComplete(obj_3des.model_reg_index))
            {
                obj_3des.flag &= (uint)int.MaxValue;
                obj_3des.tex_reg_index = -1;
            }
            else
                flag = false;
        }
        return flag;
    }

    private static void ObjAction2dAMALoad(
      AppMain.OBS_ACTION2D_AMA_WORK obj_2d,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive,
      AppMain.AMS_AMB_HEADER amb_tex,
      uint id,
      int type_node)
    {
        AppMain.A2S_AMA_HEADER a2SAmaHeader = (AppMain.A2S_AMA_HEADER)null;
        AppMain.ObjAction2dAMAWorkInit(obj_2d);
        if (archive != null)
            obj_2d.flag |= 2147483648U;
        if (filename != null)
        {
            a2SAmaHeader = AppMain.readAMAFile((object)AppMain.ObjDataLoad(data_work, filename, (object)archive));
            if (archive != null && a2SAmaHeader == null)
            {
                obj_2d.flag &= (uint)int.MaxValue;
                a2SAmaHeader = AppMain.readAMAFile((object)AppMain.ObjDataLoad(data_work, filename, (object)null));
            }
        }
        else if (archive != null)
        {
            a2SAmaHeader = AppMain.readAMAFile(AppMain.ObjDataLoadAmbIndex(data_work, index, archive));
            if (a2SAmaHeader == null)
                obj_2d.flag &= (uint)int.MaxValue;
        }
        else if (data_work != null)
            a2SAmaHeader = AppMain.readAMAFile(AppMain.ObjDataGetInc(data_work));
        if (a2SAmaHeader == null)
            return;
        obj_2d.ama = a2SAmaHeader;
        if (data_work != null)
            obj_2d.ama_data_work = data_work;
        obj_2d.type_node = type_node;
        obj_2d.act_id = id;
        AppMain.AoTexBuild(obj_2d.ao_tex, amb_tex);
        AppMain.AoTexLoad(obj_2d.ao_tex);
        obj_2d.flag |= 1073741824U;
        obj_2d.flag &= 3758096383U;
    }

    private static void ObjObjectAction2dAMALoad(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.OBS_ACTION2D_AMA_WORK obj_2d,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive,
      AppMain.AMS_AMB_HEADER amb_tex,
      uint id,
      int type_node)
    {
        if (obj_2d == null)
        {
            obj_2d = obj_work.obj_2d == null ? new AppMain.OBS_ACTION2D_AMA_WORK() : obj_work.obj_2d;
            obj_2d.Clear();
            obj_work.flag |= 67108864U;
        }
        obj_work.obj_2d = obj_2d;
        AppMain.ObjAction2dAMALoad(obj_2d, data_work, filename, index, archive, amb_tex, id, type_node);
    }

    private static void ObjAction2dAMALoadSetTexlist(
      AppMain.OBS_ACTION2D_AMA_WORK obj_2d,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive,
      AppMain.NNS_TEXLIST texlist,
      uint id,
      int type_node)
    {
        AppMain.A2S_AMA_HEADER a2SAmaHeader = (AppMain.A2S_AMA_HEADER)null;
        AppMain.ObjAction2dAMAWorkInit(obj_2d);
        if (archive != null)
            obj_2d.flag |= 2147483648U;
        if (filename != null)
        {
            a2SAmaHeader = AppMain.readAMAFile((object)AppMain.ObjDataLoad(data_work, filename, (object)archive));
            if (archive != null && a2SAmaHeader == null)
            {
                obj_2d.flag &= (uint)int.MaxValue;
                a2SAmaHeader = AppMain.readAMAFile((object)AppMain.ObjDataLoad(data_work, filename, (object)null));
            }
        }
        else if (archive != null)
        {
            a2SAmaHeader = AppMain.readAMAFile(AppMain.ObjDataLoadAmbIndex(data_work, index, archive));
            if (a2SAmaHeader == null)
                obj_2d.flag &= (uint)int.MaxValue;
        }
        else if (data_work != null)
            a2SAmaHeader = AppMain.readAMAFile(AppMain.ObjDataGetInc(data_work));
        if (a2SAmaHeader == null)
            return;
        obj_2d.ama = a2SAmaHeader;
        if (data_work != null)
            obj_2d.ama_data_work = data_work;
        obj_2d.type_node = type_node;
        obj_2d.act_id = id;
        obj_2d.texlist = texlist;
        obj_2d.flag |= 536870912U;
        AppMain.ObjAction2dAMACreate(obj_2d);
    }

    private static void ObjObjectAction2dAMALoadSetTexlist(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.OBS_ACTION2D_AMA_WORK obj_2d,
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      int index,
      AppMain.AMS_AMB_HEADER archive,
      AppMain.NNS_TEXLIST texlist,
      uint id,
      int type_node)
    {
        if (obj_2d == null)
        {
            obj_2d = obj_work.obj_2d == null ? new AppMain.OBS_ACTION2D_AMA_WORK() : obj_work.obj_2d;
            obj_2d.Clear();
            obj_work.flag |= 67108864U;
        }
        obj_work.obj_2d = obj_2d;
        AppMain.ObjAction2dAMALoadSetTexlist(obj_2d, data_work, filename, index, archive, texlist, id, type_node);
    }

    private static void ObjAction2dAMAWorkInit(AppMain.OBS_ACTION2D_AMA_WORK obj_2d)
    {
        obj_2d.Clear();
        obj_2d.speed = 1f;
        obj_2d.color.a = byte.MaxValue;
        obj_2d.color.r = byte.MaxValue;
        obj_2d.color.g = byte.MaxValue;
        obj_2d.color.b = byte.MaxValue;
        obj_2d.fade.a = (byte)0;
        obj_2d.fade.r = (byte)0;
        obj_2d.fade.g = (byte)0;
        obj_2d.fade.b = (byte)0;
    }

    private static void ObjAction2dAMACreate(AppMain.OBS_ACTION2D_AMA_WORK obj_2d)
    {
        AppMain.AoActSetTexture(obj_2d.texlist);
        obj_2d.act = obj_2d.type_node == 0 ? AppMain.AoActCreate(obj_2d.ama, obj_2d.act_id, 0.0f) : AppMain.AoActCreateNode(obj_2d.ama, obj_2d.act_id, 0.0f);
        AppMain.AoActSetTexture((AppMain.NNS_TEXLIST)null);
    }

    private static bool ObjAction2dAMALoadCheck(AppMain.OBS_ACTION2D_AMA_WORK obj_2d)
    {
        if (((int)obj_2d.flag & 1073741824) != 0)
        {
            if (AppMain.AoTexIsLoaded(obj_2d.ao_tex))
            {
                obj_2d.texlist = AppMain.AoTexGetTexList(obj_2d.ao_tex);
                obj_2d.flag &= 3221225471U;
                obj_2d.flag |= 536870912U;
                AppMain.ObjAction2dAMACreate(obj_2d);
                return true;
            }
        }
        else if (((int)obj_2d.flag & 536870912) != 0)
            return true;
        return false;
    }

}