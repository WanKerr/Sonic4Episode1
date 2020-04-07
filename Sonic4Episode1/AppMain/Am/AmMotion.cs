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
    private int amMotionId(int file_id, int motion_id)
    {
        return file_id << 16 | motion_id;
    }

    private static AppMain.AMS_MOTION amMotionCreate(AppMain.NNS_OBJECT _object)
    {
        return AppMain.amMotionCreate(_object, 0);
    }

    private static AppMain.AMS_MOTION amMotionCreate(AppMain.NNS_OBJECT _object, int flag)
    {
        return AppMain.amMotionCreate(_object, 64, 16, flag);
    }

    private static AppMain.AMS_MOTION amMotionCreate(
      AppMain.NNS_OBJECT _object,
      int motion_num,
      int mmotion_num)
    {
        return AppMain.amMotionCreate(_object, motion_num, mmotion_num, 0);
    }

    public static AppMain.AMS_MOTION amMotionCreate(
      AppMain.NNS_OBJECT _object,
      int motion_num,
      int mmotion_num,
      int flag)
    {
        motion_num = motion_num + 3 & -4;
        mmotion_num = mmotion_num + 3 & -4;
        int nNode = _object.nNode;
        AppMain.AMS_MOTION amsMotion = new AppMain.AMS_MOTION();
        amsMotion.mtnbuf = new AppMain.NNS_MOTION[motion_num];
        amsMotion.mmtn = new AppMain.NNS_MOTION[mmotion_num];
        amsMotion.data = AppMain.New<AppMain.NNS_TRS>((flag & 1) != 0 ? 4 * nNode : 2 * nNode);
        amsMotion._object = _object;
        amsMotion.node_num = nNode;
        for (int index = 0; index < 4; ++index)
        {
            amsMotion.mtnfile[index].file = (object)null;
            amsMotion.mtnfile[index].motion = (AppMain.ArrayPointer<AppMain.NNS_MOTION>)(AppMain.NNS_MOTION[])null;
            amsMotion.mtnfile[index].motion_num = 0;
        }
        amsMotion.motion_num = motion_num;
        for (int index = 0; index < motion_num; ++index)
            amsMotion.mtnbuf[index] = (AppMain.NNS_MOTION)null;
        AppMain.ArrayPointer<AppMain.AMS_MOTION_BUF> mbuf = (AppMain.ArrayPointer<AppMain.AMS_MOTION_BUF>)amsMotion.mbuf;
        int num = 0;
        while (num < 2)
        {
            ((AppMain.AMS_MOTION_BUF)~mbuf).motion_id = 0;
            ((AppMain.AMS_MOTION_BUF)~mbuf).frame = 0.0f;
            if (num == 0)
                ((AppMain.AMS_MOTION_BUF)~mbuf).mbuf = new AppMain.ArrayPointer<AppMain.NNS_TRS>(amsMotion.data, nNode);
            else if ((flag & 1) != 0)
            {
                ((AppMain.AMS_MOTION_BUF)~mbuf).mbuf = (AppMain.ArrayPointer<AppMain.NNS_TRS>)(amsMotion.mbuf[0].mbuf + nNode);
                amsMotion.mmbuf = (AppMain.ArrayPointer<AppMain.NNS_TRS>)(amsMotion.mbuf[1].mbuf + nNode);
                AppMain.nnCalcTRSList(amsMotion.mbuf[1].mbuf.array, amsMotion.mbuf[1].mbuf.offset, _object);
            }
            else
            {
                ((AppMain.AMS_MOTION_BUF)~mbuf).mbuf = (AppMain.ArrayPointer<AppMain.NNS_TRS>)(AppMain.NNS_TRS[])null;
                amsMotion.mmbuf = (AppMain.ArrayPointer<AppMain.NNS_TRS>)(AppMain.NNS_TRS[])null;
            }
            ++num;
            ++mbuf;
        }
        AppMain.nnCalcTRSList(amsMotion.mbuf[0].mbuf.array, amsMotion.mbuf[0].mbuf.offset, _object);
        AppMain.nnCalcTRSList(amsMotion.data, 0, _object);
        amsMotion.mmobject = (AppMain.NNS_OBJECT)null;
        amsMotion.mmobj_size = 0U;
        amsMotion.mmotion_num = mmotion_num;
        return amsMotion;
    }

    public static void amMotionDelete(AppMain.AMS_MOTION motion)
    {
    }

    public static void amMotionRegistFile(
      AppMain.AMS_MOTION motion,
      int file_id,
      AppMain.AMS_AMB_HEADER amb)
    {
        int num1 = 0;
        AppMain.AMS_MOTION_FILE amsMotionFile1 = motion.mtnfile[0];
        AppMain.ArrayPointer<AppMain.NNS_MOTION> motion1 = (AppMain.ArrayPointer<AppMain.NNS_MOTION>)(amsMotionFile1.motion + amsMotionFile1.motion_num);
        int index = num1 + 1;
        int num2 = 1;
        while (num2 < 4)
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION> arrayPointer = (AppMain.ArrayPointer<AppMain.NNS_MOTION>)(motion.mtnfile[index].motion + motion.mtnfile[index].motion_num);
            if (motion1 < arrayPointer)
                motion1 = arrayPointer;
            ++num2;
            ++index;
        }
        if (motion1 == (AppMain.ArrayPointer<AppMain.NNS_MOTION>)(AppMain.NNS_MOTION[])null)
            motion1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION>(motion.mtnbuf, 0);
        AppMain.AMS_MOTION_FILE amsMotionFile2 = motion.mtnfile[file_id];
        amsMotionFile2.file = (object)amb;
        amsMotionFile2.motion = motion1;
        amsMotionFile2.motion_num = AppMain.amMotionSetup(motion1, amb);
    }

    public static void amMotionRegistFile(AppMain.AMS_MOTION motion, int file_id, object buf)
    {
        int num1 = 0;
        AppMain.AMS_MOTION_FILE amsMotionFile1 = motion.mtnfile[0];
        AppMain.ArrayPointer<AppMain.NNS_MOTION> motion1 = (AppMain.ArrayPointer<AppMain.NNS_MOTION>)(amsMotionFile1.motion + amsMotionFile1.motion_num);
        int index = num1 + 1;
        int num2 = 1;
        while (num2 < 4)
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION> arrayPointer = (AppMain.ArrayPointer<AppMain.NNS_MOTION>)(motion.mtnfile[index].motion + motion.mtnfile[index].motion_num);
            if (motion1 < arrayPointer)
                motion1 = arrayPointer;
            ++num2;
            ++index;
        }
        if (motion1 == (AppMain.ArrayPointer<AppMain.NNS_MOTION>)(AppMain.NNS_MOTION[])null)
            motion1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION>(motion.mtnbuf, 0);
        AppMain.AMS_MOTION_FILE amsMotionFile2 = motion.mtnfile[file_id];
        amsMotionFile2.file = buf;
        amsMotionFile2.motion = motion1;
        if (buf is AppMain.NNS_MOTION)
        {
            amsMotionFile2.motion_num = 1;
            motion.mtnbuf[0] = (AppMain.NNS_MOTION)buf;
        }
        else
            amsMotionFile2.motion_num = AppMain.amMotionSetup(motion1, buf);
    }

    public static int amMotionSetup(
      AppMain.ArrayPointer<AppMain.NNS_MOTION> motion,
      AppMain.AMS_AMB_HEADER amb)
    {
        if (amb.files.Length == 0)
            return 0;
        AppMain.ArrayPointer<AppMain.NNS_MOTION> arrayPointer = motion;
        int num = 0;
        for (int index = 0; index < amb.file_num; ++index)
        {
            if (amb.buf[index] != null && amb.buf[index] is AppMain.NNS_MOTION)
            {
                arrayPointer.SetPrimitive((AppMain.NNS_MOTION)amb.buf[index]);
                ++arrayPointer;
                ++num;
            }
        }
        return num;
    }

    public static void amMotionSetup(out AppMain.NNS_MOTION motion, AppMain.AmbChunk buf)
    {
        motion = (AppMain.NNS_MOTION)null;
        using (MemoryStream memoryStream = new MemoryStream(buf.array, buf.offset, buf.array.Length - buf.offset))
        {
            BinaryReader reader = new BinaryReader((Stream)memoryStream);
            AppMain.NNS_BINCNK_FILEHEADER bincnkFileheader = AppMain.NNS_BINCNK_FILEHEADER.Read(reader);
            long ofsData;
            reader.BaseStream.Seek(ofsData = (long)bincnkFileheader.OfsData, SeekOrigin.Begin);
            AppMain.NNS_BINCNK_DATAHEADER bincnkDataheader = AppMain.NNS_BINCNK_DATAHEADER.Read(reader);
            long data0Pos = ofsData;
            reader.BaseStream.Seek((long)bincnkFileheader.OfsNOF0, SeekOrigin.Begin);
            AppMain.NNS_BINCNK_NOF0HEADER.Read(reader);
            int nChunk = bincnkFileheader.nChunk;
            while (nChunk > 0)
            {
                switch (bincnkDataheader.Id)
                {
                    case 1095584078:
                    case 1129138510:
                    case 1330465102:
                        reader.BaseStream.Seek(data0Pos + (long)bincnkDataheader.OfsMainData, SeekOrigin.Begin);
                        motion = AppMain.NNS_MOTION.Read(reader, data0Pos);
                        break;
                    case 1145980238:
                        return;
                }
                ++nChunk;
                reader.BaseStream.Seek(ofsData += (long)(8 + bincnkDataheader.OfsNextId), SeekOrigin.Begin);
                bincnkDataheader = AppMain.NNS_BINCNK_DATAHEADER.Read(reader);
            }
        }
    }

    public static int amMotionSetup(AppMain.ArrayPointer<AppMain.NNS_MOTION> motion, object _buf)
    {
        AppMain.AmbChunk ambChunk = (AppMain.AmbChunk)_buf;
        using (MemoryStream memoryStream = new MemoryStream(ambChunk.array, ambChunk.offset, ambChunk.array.Length - ambChunk.offset))
        {
            BinaryReader reader = new BinaryReader((Stream)memoryStream);
            AppMain.ArrayPointer<AppMain.NNS_MOTION> arrayPointer = motion;
            int num = 0;
            arrayPointer.SetPrimitive((AppMain.NNS_MOTION)null);
            AppMain.NNS_BINCNK_FILEHEADER bincnkFileheader = AppMain.NNS_BINCNK_FILEHEADER.Read(reader);
            long ofsData;
            reader.BaseStream.Seek(ofsData = (long)bincnkFileheader.OfsData, SeekOrigin.Begin);
            AppMain.NNS_BINCNK_DATAHEADER bincnkDataheader = AppMain.NNS_BINCNK_DATAHEADER.Read(reader);
            long data0Pos = ofsData;
            reader.BaseStream.Seek((long)bincnkFileheader.OfsNOF0, SeekOrigin.Begin);
            AppMain.NNS_BINCNK_NOF0HEADER.Read(reader);
            int nChunk = bincnkFileheader.nChunk;
            while (nChunk > 0)
            {
                switch (bincnkDataheader.Id)
                {
                    case 1095584078:
                    case 1129138510:
                    case 1330465102:
                        reader.BaseStream.Seek(data0Pos + (long)bincnkDataheader.OfsMainData, SeekOrigin.Begin);
                        arrayPointer.SetPrimitive(AppMain.NNS_MOTION.Read(reader, data0Pos));
                        ++arrayPointer;
                        ++num;
                        break;
                    case 1145980238:
                        goto label_6;
                }
                ++nChunk;
                reader.BaseStream.Seek(ofsData += (long)(8 + bincnkDataheader.OfsNextId), SeekOrigin.Begin);
                bincnkDataheader = AppMain.NNS_BINCNK_DATAHEADER.Read(reader);
            }
        label_6:
            return num;
        }
    }

    public static void amMotionSet(AppMain.AMS_MOTION motion, int mbuf_id, int motion_id)
    {
        AppMain.AMS_MOTION_BUF amsMotionBuf = motion.mbuf[mbuf_id];
        amsMotionBuf.motion_id = motion_id;
        amsMotionBuf.frame = motion.mtnfile[motion_id >> 16].motion[motion_id & (int)ushort.MaxValue].StartFrame;
    }

    public static void amMotionSetFrame(AppMain.AMS_MOTION motion, int mbuf_id, float frame)
    {
        motion.mbuf[mbuf_id].frame = frame;
    }

    public static void amMotionCalc(AppMain.AMS_MOTION motion)
    {
        AppMain.amMotionCalc(motion, -1);
    }

    public static void amMotionCalc(AppMain.AMS_MOTION motion, int mbuf_id)
    {
        int index1 = 0;
        while (index1 < 2)
        {
            if ((mbuf_id & 1) != 0 && !(motion.mbuf[index1].mbuf == (AppMain.ArrayPointer<AppMain.NNS_TRS>)(AppMain.NNS_TRS[])null))
            {
                int motionId = motion.mbuf[index1].motion_id;
                int index2 = motionId >> 16;
                int index3 = motionId & (int)ushort.MaxValue;
                AppMain.nnCalcTRSListMotion(motion.mbuf[index1].mbuf.array, motion.mbuf[index1].mbuf.offset, motion._object, motion.mtnfile[index2].motion[index3], motion.mbuf[index1].frame);
            }
            ++index1;
            mbuf_id >>= 1;
        }
    }

    private void amMotionApply(AppMain.AMS_MOTION motion)
    {
        AppMain.amMotionApply(motion, 0.0f, 1f);
    }

    public static void amMotionApply(AppMain.AMS_MOTION motion, float marge)
    {
        AppMain.amMotionApply(motion, marge, 1f);
    }

    public static void amMotionApply(AppMain.AMS_MOTION motion, float marge, float per)
    {
        AppMain.ArrayPointer<AppMain.NNS_TRS> arrayPointer = motion.mbuf[0].mbuf;
        if ((double)per <= 0.0)
            return;
        if (motion.mbuf[1].mbuf != (AppMain.ArrayPointer<AppMain.NNS_TRS>)(AppMain.NNS_TRS[])null)
        {
            if ((double)marge >= 1.0)
                arrayPointer = motion.mbuf[1].mbuf;
            else if ((double)marge > 0.0)
            {
                if ((double)per < 1.0)
                {
                    arrayPointer = motion.mmbuf;
                    AppMain.nnLinkMotion(arrayPointer, motion.mbuf[0].mbuf, motion.mbuf[1].mbuf, motion.node_num, marge);
                }
                else
                {
                    AppMain.nnLinkMotion((AppMain.ArrayPointer<AppMain.NNS_TRS>)motion.data, motion.mbuf[0].mbuf, motion.mbuf[1].mbuf, motion.node_num, marge);
                    return;
                }
            }
        }
        if ((double)per >= 1.0)
        {
            for (int index = 0; index < motion.node_num; ++index)
                motion.data[index].Assign(arrayPointer[index]);
        }
        else
            AppMain.nnLinkMotion((AppMain.ArrayPointer<AppMain.NNS_TRS>)motion.data, (AppMain.ArrayPointer<AppMain.NNS_TRS>)motion.data, arrayPointer, motion.node_num, per);
    }

    public static void amMotionGet(AppMain.AMS_MOTION motion)
    {
        AppMain.amMotionGet(motion, 0.0f, 1f);
    }

    public static void amMotionGet(AppMain.AMS_MOTION motion, float marge)
    {
        AppMain.amMotionGet(motion, marge, 1f);
    }

    public static void amMotionGet(AppMain.AMS_MOTION motion, float marge, float per)
    {
        AppMain.amMotionCalc(motion);
        AppMain.amMotionApply(motion, marge, per);
    }

    public static void amMotionMaterialRegistFile(
      AppMain.AMS_MOTION motion,
      int file_id,
      AppMain.AMS_AMB_HEADER amb)
    {
        int fileNum = amb.file_num;
        for (int index = 0; index < fileNum; ++index)
            motion.mmtn[file_id + index] = (AppMain.NNS_MOTION)amb.buf[index];
        motion.mmotion_id = file_id;
        motion.mmotion_frame = 0.0f;
        motion.mmobj_size = 0U;
        motion.mmobject = new AppMain.NNS_OBJECT();
        AppMain.nnInitMaterialMotionObject(motion.mmobject, motion._object, motion.mmtn[motion.mmotion_id]);
    }

    private static void amMotionMaterialRegistFile(
      AppMain.AMS_MOTION motion,
      int file_id,
      object file)
    {
        motion.mmtn[file_id] = (AppMain.NNS_MOTION)file;
        motion.mmotion_id = file_id;
        motion.mmotion_frame = 0.0f;
        motion.mmobj_size = 0U;
        motion.mmobject = new AppMain.NNS_OBJECT();
        AppMain.nnInitMaterialMotionObject(motion.mmobject, motion._object, motion.mmtn[motion.mmotion_id]);
    }

    public static void amMotionMaterialSet(AppMain.AMS_MOTION motion, int motion_id)
    {
        motion.mmotion_id = motion_id;
        motion.mmotion_frame = 0.0f;
        AppMain.nnInitMaterialMotionObject(motion.mmobject, motion._object, motion.mmtn[motion.mmotion_id]);
    }

    public static void amMotionMaterialSetFrame(AppMain.AMS_MOTION motion, float frame)
    {
        motion.mmotion_frame = frame;
    }

    public static void amMotionMaterialCalc(AppMain.AMS_MOTION motion)
    {
        if (!AppMain.amThreadCheckDraw())
            return;
        AppMain.nnCalcMaterialMotion(motion.mmobject, motion._object, motion.mmtn[motion.mmotion_id], motion.mmotion_frame);
    }

    private void amMotionDraw(uint state, AppMain.AMS_MOTION motion, AppMain.NNS_TEXLIST texlist)
    {
        this.amMotionDraw(state, motion, texlist, 0U, (AppMain.NNS_MATERIALCALLBACK_FUNC)null);
    }

    private void amMotionDraw(
      uint state,
      AppMain.AMS_MOTION motion,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag)
    {
        this.amMotionDraw(state, motion, texlist, drawflag, (AppMain.NNS_MATERIALCALLBACK_FUNC)null);
    }

    private void amMotionDraw(
      uint state,
      AppMain.AMS_MOTION motion,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag,
      AppMain.NNS_MATERIALCALLBACK_FUNC func)
    {
        int nodeNum = motion.node_num;
        AppMain.AMS_PARAM_DRAW_MOTION_TRS paramDrawMotionTrs = AppMain.amDrawAlloc_AMS_PARAM_DRAW_MOTION_TRS();
        AppMain.NNS_MATRIX dst = paramDrawMotionTrs.mtx = AppMain.amDrawAlloc_NNS_MATRIX();
        AppMain.nnCopyMatrix(dst, AppMain.amMatrixGetCurrent());
        paramDrawMotionTrs._object = motion._object;
        paramDrawMotionTrs.mtx = dst;
        paramDrawMotionTrs.sub_obj_type = 0U;
        paramDrawMotionTrs.flag = drawflag;
        paramDrawMotionTrs.texlist = texlist;
        paramDrawMotionTrs.trslist = new AppMain.NNS_TRS[nodeNum];
        paramDrawMotionTrs.material_func = func;
        for (int index = 0; index < nodeNum; ++index)
        {
            paramDrawMotionTrs.trslist[index] = AppMain.amDrawAlloc_NNS_TRS();
            paramDrawMotionTrs.trslist[index].Assign(motion.data[index]);
        }
        int motionId = motion.mbuf[0].motion_id;
        paramDrawMotionTrs.motion = motion.mtnfile[motionId >> 16].motion[motionId & (int)ushort.MaxValue];
        paramDrawMotionTrs.frame = motion.mbuf[0].frame;
        AppMain.amDrawRegistCommand(state, -11, (object)paramDrawMotionTrs);
    }

    private void amMotionMaterialDraw(
      uint state,
      AppMain.AMS_MOTION motion,
      AppMain.NNS_TEXLIST texlist)
    {
        this.amMotionMaterialDraw(state, motion, texlist, 0U, (AppMain.NNS_MATERIALCALLBACK_FUNC)null);
    }

    private void amMotionMaterialDraw(
      uint state,
      AppMain.AMS_MOTION motion,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag)
    {
        this.amMotionMaterialDraw(state, motion, texlist, drawflag, (AppMain.NNS_MATERIALCALLBACK_FUNC)null);
    }

    private void amMotionMaterialDraw(
      uint state,
      AppMain.AMS_MOTION motion,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag,
      AppMain.NNS_MATERIALCALLBACK_FUNC func)
    {
        if (motion.mmobject == null)
        {
            this.amMotionDraw(state, motion, texlist, drawflag);
        }
        else
        {
            int nodeNum = motion.node_num;
            AppMain.AMS_PARAM_DRAW_MOTION_TRS paramDrawMotionTrs = new AppMain.AMS_PARAM_DRAW_MOTION_TRS();
            AppMain.NNS_MATRIX dst = paramDrawMotionTrs.mtx = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
            AppMain.nnCopyMatrix(dst, AppMain.amMatrixGetCurrent());
            paramDrawMotionTrs.mtx = dst;
            paramDrawMotionTrs.sub_obj_type = 0U;
            paramDrawMotionTrs.flag = drawflag;
            paramDrawMotionTrs.texlist = texlist;
            paramDrawMotionTrs.trslist = new AppMain.NNS_TRS[nodeNum];
            paramDrawMotionTrs.material_func = func;
            for (int index = 0; index < nodeNum; ++index)
                paramDrawMotionTrs.trslist[index] = new AppMain.NNS_TRS(motion.data[index]);
            paramDrawMotionTrs._object = motion._object;
            paramDrawMotionTrs.mmotion = motion.mmtn[motion.mmotion_id];
            paramDrawMotionTrs.mframe = motion.mmotion_frame;
            int motionId = motion.mbuf[0].motion_id;
            if (motion.mtnfile[motionId >> 16].file != null)
            {
                paramDrawMotionTrs.motion = motion.mtnfile[motionId >> 16].motion[motionId & (int)ushort.MaxValue];
                paramDrawMotionTrs.frame = motion.mbuf[0].frame;
            }
            else
            {
                paramDrawMotionTrs.motion = (AppMain.NNS_MOTION)null;
                paramDrawMotionTrs.frame = 0.0f;
            }
            AppMain.amDrawRegistCommand(state, -12, (object)paramDrawMotionTrs);
        }
    }

    private void amMotionDraw(AppMain.AMS_MOTION motion, AppMain.NNS_TEXLIST texlist)
    {
        this.amMotionDraw(motion, texlist, 0U, (AppMain.NNS_MATERIALCALLBACK_FUNC)null);
    }

    private void amMotionDraw(AppMain.AMS_MOTION motion, AppMain.NNS_TEXLIST texlist, uint drawflag)
    {
        this.amMotionDraw(motion, texlist, drawflag, (AppMain.NNS_MATERIALCALLBACK_FUNC)null);
    }

    private void amMotionDraw(
      AppMain.AMS_MOTION motion,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag,
      AppMain.NNS_MATERIALCALLBACK_FUNC func)
    {
        AppMain.AMS_PARAM_DRAW_MOTION_TRS paramDrawMotionTrs;
        AppMain.AMS_COMMAND_HEADER command = new AppMain.AMS_COMMAND_HEADER()
        {
            param = (object)(paramDrawMotionTrs = new AppMain.AMS_PARAM_DRAW_MOTION_TRS()),
            command_id = -11
        };
        command.param = (object)paramDrawMotionTrs;
        paramDrawMotionTrs._object = motion._object;
        paramDrawMotionTrs.mtx = (AppMain.NNS_MATRIX)null;
        paramDrawMotionTrs.sub_obj_type = 0U;
        paramDrawMotionTrs.flag = drawflag;
        paramDrawMotionTrs.texlist = texlist;
        paramDrawMotionTrs.trslist = motion.data;
        paramDrawMotionTrs.material_func = func;
        int motionId = motion.mbuf[0].motion_id;
        paramDrawMotionTrs.motion = motion.mtnfile[motionId >> 16].motion[motionId & (int)ushort.MaxValue];
        paramDrawMotionTrs.frame = motion.mbuf[0].frame;
        AppMain._amDrawMotionTRS(command, drawflag);
    }

    private void amMotionMaterialDraw(AppMain.AMS_MOTION motion, AppMain.NNS_TEXLIST texlist)
    {
        this.amMotionMaterialDraw(motion, texlist, 0U, (AppMain.NNS_MATERIALCALLBACK_FUNC)null);
    }

    private void amMotionMaterialDraw(
      AppMain.AMS_MOTION motion,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag)
    {
        this.amMotionMaterialDraw(motion, texlist, drawflag, (AppMain.NNS_MATERIALCALLBACK_FUNC)null);
    }

    private void amMotionMaterialDraw(
      AppMain.AMS_MOTION motion,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag,
      AppMain.NNS_MATERIALCALLBACK_FUNC func)
    {
        if (motion.mmobject == null)
        {
            this.amMotionDraw(motion, texlist, drawflag);
        }
        else
        {
            AppMain.AMS_PARAM_DRAW_MOTION_TRS paramDrawMotionTrs;
            AppMain.AMS_COMMAND_HEADER command = new AppMain.AMS_COMMAND_HEADER()
            {
                param = (object)(paramDrawMotionTrs = new AppMain.AMS_PARAM_DRAW_MOTION_TRS()),
                command_id = -12
            };
            command.param = (object)paramDrawMotionTrs;
            paramDrawMotionTrs._object = motion.mmobject;
            paramDrawMotionTrs.mtx = (AppMain.NNS_MATRIX)null;
            paramDrawMotionTrs.sub_obj_type = 0U;
            paramDrawMotionTrs.flag = drawflag;
            paramDrawMotionTrs.texlist = texlist;
            paramDrawMotionTrs.trslist = motion.data;
            paramDrawMotionTrs.material_func = func;
            paramDrawMotionTrs.mmotion = (AppMain.NNS_MOTION)null;
            paramDrawMotionTrs.mframe = 0.0f;
            int motionId = motion.mbuf[0].motion_id;
            if (motion.mtnfile[motionId >> 16].file != null)
            {
                paramDrawMotionTrs.motion = motion.mtnfile[motionId >> 16].motion[motionId & (int)ushort.MaxValue];
                paramDrawMotionTrs.frame = motion.mbuf[0].frame;
            }
            else
            {
                paramDrawMotionTrs.motion = (AppMain.NNS_MOTION)null;
                paramDrawMotionTrs.frame = 0.0f;
            }
            AppMain._amDrawMotionTRS(command, drawflag);
        }
    }

    public static float amMotionGetStartFrame(AppMain.AMS_MOTION motion, int motion_id)
    {
        return motion.mtnfile[motion_id >> 16].motion[motion_id & (int)ushort.MaxValue].StartFrame;
    }

    public static float amMotionGetEndFrame(AppMain.AMS_MOTION motion, int motion_id)
    {
        return motion.mtnfile[motion_id >> 16].motion[motion_id & (int)ushort.MaxValue].EndFrame;
    }

    private void amMotionGetFrames(
      AppMain.AMS_MOTION motion,
      int motion_id,
      out float start,
      out float end)
    {
        AppMain.NNS_MOTION nnsMotion = motion.mtnfile[motion_id >> 16].motion[motion_id & (int)ushort.MaxValue];
        start = nnsMotion.StartFrame;
        end = nnsMotion.EndFrame;
    }

    public static float amMotionMaterialGetStartFrame(AppMain.AMS_MOTION motion, int motion_id)
    {
        return motion.mmtn[motion.mmotion_id].StartFrame;
    }

    public static float amMotionMaterialGetEndFrame(AppMain.AMS_MOTION motion, int motion_id)
    {
        return motion.mmtn[motion.mmotion_id].EndFrame;
    }

    private void amMotionMaterialGetFrames(
      AppMain.AMS_MOTION motion,
      int motion_id,
      out float start,
      out float end)
    {
        AppMain.NNS_MOTION nnsMotion = motion.mmtn[motion.mmotion_id];
        start = nnsMotion.StartFrame;
        end = nnsMotion.EndFrame;
    }

}