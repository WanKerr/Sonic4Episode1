using System.IO;

public partial class AppMain
{
    private int amMotionId(int file_id, int motion_id)
    {
        return file_id << 16 | motion_id;
    }

    private static AMS_MOTION amMotionCreate(NNS_OBJECT _object)
    {
        return amMotionCreate(_object, 0);
    }

    private static AMS_MOTION amMotionCreate(NNS_OBJECT _object, int flag)
    {
        return amMotionCreate(_object, 64, 16, flag);
    }

    private static AMS_MOTION amMotionCreate(
      NNS_OBJECT _object,
      int motion_num,
      int mmotion_num)
    {
        return amMotionCreate(_object, motion_num, mmotion_num, 0);
    }

    public static AMS_MOTION amMotionCreate(
      NNS_OBJECT _object,
      int motion_num,
      int mmotion_num,
      int flag)
    {
        motion_num = motion_num + 3 & -4;
        mmotion_num = mmotion_num + 3 & -4;
        int nNode = _object.nNode;
        AMS_MOTION amsMotion = new AMS_MOTION();
        amsMotion.mtnbuf = new NNS_MOTION[motion_num];
        amsMotion.mmtn = new NNS_MOTION[mmotion_num];
        amsMotion.data = New<NNS_TRS>((flag & 1) != 0 ? 4 * nNode : 2 * nNode);
        amsMotion._object = _object;
        amsMotion.node_num = nNode;
        for (int index = 0; index < 4; ++index)
        {
            amsMotion.mtnfile[index].file = null;
            amsMotion.mtnfile[index].motion = null;
            amsMotion.mtnfile[index].motion_num = 0;
        }
        amsMotion.motion_num = motion_num;
        for (int index = 0; index < motion_num; ++index)
            amsMotion.mtnbuf[index] = null;
        ArrayPointer<AMS_MOTION_BUF> mbuf = amsMotion.mbuf;
        int num = 0;
        while (num < 2)
        {
            (~mbuf).motion_id = 0;
            (~mbuf).frame = 0.0f;
            if (num == 0)
                (~mbuf).mbuf = new ArrayPointer<NNS_TRS>(amsMotion.data, nNode);
            else if ((flag & 1) != 0)
            {
                (~mbuf).mbuf = amsMotion.mbuf[0].mbuf + nNode;
                amsMotion.mmbuf = amsMotion.mbuf[1].mbuf + nNode;
                nnCalcTRSList(amsMotion.mbuf[1].mbuf.array, amsMotion.mbuf[1].mbuf.offset, _object);
            }
            else
            {
                (~mbuf).mbuf = null;
                amsMotion.mmbuf = null;
            }
            ++num;
            ++mbuf;
        }
        nnCalcTRSList(amsMotion.mbuf[0].mbuf.array, amsMotion.mbuf[0].mbuf.offset, _object);
        nnCalcTRSList(amsMotion.data, 0, _object);
        amsMotion.mmobject = null;
        amsMotion.mmobj_size = 0U;
        amsMotion.mmotion_num = mmotion_num;
        return amsMotion;
    }

    public static void amMotionDelete(AMS_MOTION motion)
    {
    }

    public static void amMotionRegistFile(
      AMS_MOTION motion,
      int file_id,
      AMS_AMB_HEADER amb)
    {
        int num1 = 0;
        AMS_MOTION_FILE amsMotionFile1 = motion.mtnfile[0];
        ArrayPointer<NNS_MOTION> motion1 = amsMotionFile1.motion + amsMotionFile1.motion_num;
        int index = num1 + 1;
        int num2 = 1;
        while (num2 < 4)
        {
            ArrayPointer<NNS_MOTION> arrayPointer = motion.mtnfile[index].motion + motion.mtnfile[index].motion_num;
            if (motion1 < arrayPointer)
                motion1 = arrayPointer;
            ++num2;
            ++index;
        }
        if (motion1 == null)
            motion1 = new ArrayPointer<NNS_MOTION>(motion.mtnbuf, 0);
        AMS_MOTION_FILE amsMotionFile2 = motion.mtnfile[file_id];
        amsMotionFile2.file = amb;
        amsMotionFile2.motion = motion1;
        amsMotionFile2.motion_num = amMotionSetup(motion1, amb);
    }

    public static void amMotionRegistFile(AMS_MOTION motion, int file_id, object buf)
    {
        int num1 = 0;
        AMS_MOTION_FILE amsMotionFile1 = motion.mtnfile[0];
        ArrayPointer<NNS_MOTION> motion1 = amsMotionFile1.motion + amsMotionFile1.motion_num;
        int index = num1 + 1;
        int num2 = 1;
        while (num2 < 4)
        {
            ArrayPointer<NNS_MOTION> arrayPointer = motion.mtnfile[index].motion + motion.mtnfile[index].motion_num;
            if (motion1 < arrayPointer)
                motion1 = arrayPointer;
            ++num2;
            ++index;
        }
        if (motion1 == null)
            motion1 = new ArrayPointer<NNS_MOTION>(motion.mtnbuf, 0);
        AMS_MOTION_FILE amsMotionFile2 = motion.mtnfile[file_id];
        amsMotionFile2.file = buf;
        amsMotionFile2.motion = motion1;
        if (buf is NNS_MOTION)
        {
            amsMotionFile2.motion_num = 1;
            motion.mtnbuf[0] = (NNS_MOTION)buf;
        }
        else
            amsMotionFile2.motion_num = amMotionSetup(motion1, buf);
    }

    public static int amMotionSetup(
      ArrayPointer<NNS_MOTION> motion,
      AMS_AMB_HEADER amb)
    {
        if (amb.files.Length == 0)
            return 0;
        ArrayPointer<NNS_MOTION> arrayPointer = motion;
        int num = 0;
        for (int index = 0; index < amb.file_num; ++index)
        {
            if (amb.buf[index] != null && amb.buf[index] is NNS_MOTION)
            {
                arrayPointer.SetPrimitive((NNS_MOTION)amb.buf[index]);
                ++arrayPointer;
                ++num;
            }
        }
        return num;
    }

    public static void amMotionSetup(out NNS_MOTION motion, AmbChunk buf)
    {
        motion = null;
        using (MemoryStream memoryStream = new MemoryStream(buf.array, buf.offset, buf.array.Length - buf.offset))
        {
            BinaryReader reader = new BinaryReader(memoryStream);
            NNS_BINCNK_FILEHEADER bincnkFileheader = NNS_BINCNK_FILEHEADER.Read(reader);
            long ofsData;
            reader.BaseStream.Seek(ofsData = bincnkFileheader.OfsData, SeekOrigin.Begin);
            NNS_BINCNK_DATAHEADER bincnkDataheader = NNS_BINCNK_DATAHEADER.Read(reader);
            long data0Pos = ofsData;
            reader.BaseStream.Seek(bincnkFileheader.OfsNOF0, SeekOrigin.Begin);
            NNS_BINCNK_NOF0HEADER.Read(reader);
            int nChunk = bincnkFileheader.nChunk;
            while (nChunk > 0)
            {
                switch (bincnkDataheader.Id)
                {
                    case 1095584078:
                    case 1129138510:
                    case 1330465102:
                        reader.BaseStream.Seek(data0Pos + bincnkDataheader.OfsMainData, SeekOrigin.Begin);
                        motion = NNS_MOTION.Read(reader, data0Pos);
                        break;
                    case 1145980238:
                        return;
                }
                ++nChunk;
                reader.BaseStream.Seek(ofsData += 8 + bincnkDataheader.OfsNextId, SeekOrigin.Begin);
                bincnkDataheader = NNS_BINCNK_DATAHEADER.Read(reader);
            }
        }
    }

    public static int amMotionSetup(ArrayPointer<NNS_MOTION> motion, object _buf)
    {
        AmbChunk ambChunk = (AmbChunk)_buf;
        using (MemoryStream memoryStream = new MemoryStream(ambChunk.array, ambChunk.offset, ambChunk.array.Length - ambChunk.offset))
        {
            BinaryReader reader = new BinaryReader(memoryStream);
            ArrayPointer<NNS_MOTION> arrayPointer = motion;
            int num = 0;
            arrayPointer.SetPrimitive(null);
            NNS_BINCNK_FILEHEADER bincnkFileheader = NNS_BINCNK_FILEHEADER.Read(reader);
            long ofsData;
            reader.BaseStream.Seek(ofsData = bincnkFileheader.OfsData, SeekOrigin.Begin);
            NNS_BINCNK_DATAHEADER bincnkDataheader = NNS_BINCNK_DATAHEADER.Read(reader);
            long data0Pos = ofsData;
            reader.BaseStream.Seek(bincnkFileheader.OfsNOF0, SeekOrigin.Begin);
            NNS_BINCNK_NOF0HEADER.Read(reader);
            int nChunk = bincnkFileheader.nChunk;
            while (nChunk > 0)
            {
                switch (bincnkDataheader.Id)
                {
                    case 1095584078:
                    case 1129138510:
                    case 1330465102:
                        reader.BaseStream.Seek(data0Pos + bincnkDataheader.OfsMainData, SeekOrigin.Begin);
                        arrayPointer.SetPrimitive(NNS_MOTION.Read(reader, data0Pos));
                        ++arrayPointer;
                        ++num;
                        break;
                    case 1145980238:
                        goto label_6;
                }
                ++nChunk;
                reader.BaseStream.Seek(ofsData += 8 + bincnkDataheader.OfsNextId, SeekOrigin.Begin);
                bincnkDataheader = NNS_BINCNK_DATAHEADER.Read(reader);
            }
        label_6:
            return num;
        }
    }

    public static void amMotionSet(AMS_MOTION motion, int mbuf_id, int motion_id)
    {
        AMS_MOTION_BUF amsMotionBuf = motion.mbuf[mbuf_id];
        amsMotionBuf.motion_id = motion_id;
        amsMotionBuf.frame = motion.mtnfile[motion_id >> 16].motion[motion_id & ushort.MaxValue].StartFrame;
    }

    public static void amMotionSetFrame(AMS_MOTION motion, int mbuf_id, float frame)
    {
        motion.mbuf[mbuf_id].frame = frame;
    }

    public static void amMotionCalc(AMS_MOTION motion)
    {
        amMotionCalc(motion, -1);
    }

    public static void amMotionCalc(AMS_MOTION motion, int mbuf_id)
    {
        int index1 = 0;
        while (index1 < 2)
        {
            if ((mbuf_id & 1) != 0 && !(motion.mbuf[index1].mbuf == null))
            {
                int motionId = motion.mbuf[index1].motion_id;
                int index2 = motionId >> 16;
                int index3 = motionId & ushort.MaxValue;
                nnCalcTRSListMotion(motion.mbuf[index1].mbuf.array, motion.mbuf[index1].mbuf.offset, motion._object, motion.mtnfile[index2].motion[index3], motion.mbuf[index1].frame);
            }
            ++index1;
            mbuf_id >>= 1;
        }
    }

    private void amMotionApply(AMS_MOTION motion)
    {
        amMotionApply(motion, 0.0f, 1f);
    }

    public static void amMotionApply(AMS_MOTION motion, float marge)
    {
        amMotionApply(motion, marge, 1f);
    }

    public static void amMotionApply(AMS_MOTION motion, float marge, float per)
    {
        ArrayPointer<NNS_TRS> arrayPointer = motion.mbuf[0].mbuf;
        if (per <= 0.0)
            return;
        if (motion.mbuf[1].mbuf != null)
        {
            if (marge >= 1.0)
                arrayPointer = motion.mbuf[1].mbuf;
            else if (marge > 0.0)
            {
                if (per < 1.0)
                {
                    arrayPointer = motion.mmbuf;
                    nnLinkMotion(arrayPointer, motion.mbuf[0].mbuf, motion.mbuf[1].mbuf, motion.node_num, marge);
                }
                else
                {
                    nnLinkMotion(motion.data, motion.mbuf[0].mbuf, motion.mbuf[1].mbuf, motion.node_num, marge);
                    return;
                }
            }
        }
        if (per >= 1.0)
        {
            for (int index = 0; index < motion.node_num; ++index)
                motion.data[index].Assign(arrayPointer[index]);
        }
        else
            nnLinkMotion(motion.data, motion.data, arrayPointer, motion.node_num, per);
    }

    public static void amMotionGet(AMS_MOTION motion)
    {
        amMotionGet(motion, 0.0f, 1f);
    }

    public static void amMotionGet(AMS_MOTION motion, float marge)
    {
        amMotionGet(motion, marge, 1f);
    }

    public static void amMotionGet(AMS_MOTION motion, float marge, float per)
    {
        amMotionCalc(motion);
        amMotionApply(motion, marge, per);
    }

    public static void amMotionMaterialRegistFile(
      AMS_MOTION motion,
      int file_id,
      AMS_AMB_HEADER amb)
    {
        int fileNum = amb.file_num;
        for (int index = 0; index < fileNum; ++index)
            motion.mmtn[file_id + index] = (NNS_MOTION)amb.buf[index];
        motion.mmotion_id = file_id;
        motion.mmotion_frame = 0.0f;
        motion.mmobj_size = 0U;
        motion.mmobject = new NNS_OBJECT();
        nnInitMaterialMotionObject(motion.mmobject, motion._object, motion.mmtn[motion.mmotion_id]);
    }

    private static void amMotionMaterialRegistFile(
      AMS_MOTION motion,
      int file_id,
      object file)
    {
        motion.mmtn[file_id] = (NNS_MOTION)file;
        motion.mmotion_id = file_id;
        motion.mmotion_frame = 0.0f;
        motion.mmobj_size = 0U;
        motion.mmobject = new NNS_OBJECT();
        nnInitMaterialMotionObject(motion.mmobject, motion._object, motion.mmtn[motion.mmotion_id]);
    }

    public static void amMotionMaterialSet(AMS_MOTION motion, int motion_id)
    {
        motion.mmotion_id = motion_id;
        motion.mmotion_frame = 0.0f;
        nnInitMaterialMotionObject(motion.mmobject, motion._object, motion.mmtn[motion.mmotion_id]);
    }

    public static void amMotionMaterialSetFrame(AMS_MOTION motion, float frame)
    {
        motion.mmotion_frame = frame;
    }

    public static void amMotionMaterialCalc(AMS_MOTION motion)
    {
        //if (!AppMain.amThreadCheckDraw())
        //    return;
        nnCalcMaterialMotion(motion.mmobject, motion._object, motion.mmtn[motion.mmotion_id], motion.mmotion_frame);
    }

    private void amMotionDraw(uint state, AMS_MOTION motion, NNS_TEXLIST texlist)
    {
        this.amMotionDraw(state, motion, texlist, 0U, null);
    }

    private void amMotionDraw(
      uint state,
      AMS_MOTION motion,
      NNS_TEXLIST texlist,
      uint drawflag)
    {
        this.amMotionDraw(state, motion, texlist, drawflag, null);
    }

    private void amMotionDraw(
      uint state,
      AMS_MOTION motion,
      NNS_TEXLIST texlist,
      uint drawflag,
      NNS_MATERIALCALLBACK_FUNC func)
    {
        int nodeNum = motion.node_num;
        AMS_PARAM_DRAW_MOTION_TRS paramDrawMotionTrs = amDrawAlloc_AMS_PARAM_DRAW_MOTION_TRS();
        NNS_MATRIX dst = paramDrawMotionTrs.mtx = amDrawAlloc_NNS_MATRIX();
        nnCopyMatrix(dst, amMatrixGetCurrent());
        paramDrawMotionTrs._object = motion._object;
        paramDrawMotionTrs.mtx = dst;
        paramDrawMotionTrs.sub_obj_type = 0U;
        paramDrawMotionTrs.flag = drawflag;
        paramDrawMotionTrs.texlist = texlist;
        paramDrawMotionTrs.trslist = new NNS_TRS[nodeNum];
        paramDrawMotionTrs.material_func = func;
        for (int index = 0; index < nodeNum; ++index)
        {
            paramDrawMotionTrs.trslist[index] = amDrawAlloc_NNS_TRS();
            paramDrawMotionTrs.trslist[index].Assign(motion.data[index]);
        }
        int motionId = motion.mbuf[0].motion_id;
        paramDrawMotionTrs.motion = motion.mtnfile[motionId >> 16].motion[motionId & ushort.MaxValue];
        paramDrawMotionTrs.frame = motion.mbuf[0].frame;
        amDrawRegistCommand(state, -11, paramDrawMotionTrs);
    }

    private void amMotionMaterialDraw(
      uint state,
      AMS_MOTION motion,
      NNS_TEXLIST texlist)
    {
        this.amMotionMaterialDraw(state, motion, texlist, 0U, null);
    }

    private void amMotionMaterialDraw(
      uint state,
      AMS_MOTION motion,
      NNS_TEXLIST texlist,
      uint drawflag)
    {
        this.amMotionMaterialDraw(state, motion, texlist, drawflag, null);
    }

    private void amMotionMaterialDraw(
      uint state,
      AMS_MOTION motion,
      NNS_TEXLIST texlist,
      uint drawflag,
      NNS_MATERIALCALLBACK_FUNC func)
    {
        if (motion.mmobject == null)
        {
            this.amMotionDraw(state, motion, texlist, drawflag);
        }
        else
        {
            int nodeNum = motion.node_num;
            AMS_PARAM_DRAW_MOTION_TRS paramDrawMotionTrs = new AMS_PARAM_DRAW_MOTION_TRS();
            NNS_MATRIX dst = paramDrawMotionTrs.mtx = GlobalPool<NNS_MATRIX>.Alloc();
            nnCopyMatrix(dst, amMatrixGetCurrent());
            paramDrawMotionTrs.mtx = dst;
            paramDrawMotionTrs.sub_obj_type = 0U;
            paramDrawMotionTrs.flag = drawflag;
            paramDrawMotionTrs.texlist = texlist;
            paramDrawMotionTrs.trslist = new NNS_TRS[nodeNum];
            paramDrawMotionTrs.material_func = func;
            for (int index = 0; index < nodeNum; ++index)
                paramDrawMotionTrs.trslist[index] = new NNS_TRS(motion.data[index]);
            paramDrawMotionTrs._object = motion._object;
            paramDrawMotionTrs.mmotion = motion.mmtn[motion.mmotion_id];
            paramDrawMotionTrs.mframe = motion.mmotion_frame;
            int motionId = motion.mbuf[0].motion_id;
            if (motion.mtnfile[motionId >> 16].file != null)
            {
                paramDrawMotionTrs.motion = motion.mtnfile[motionId >> 16].motion[motionId & ushort.MaxValue];
                paramDrawMotionTrs.frame = motion.mbuf[0].frame;
            }
            else
            {
                paramDrawMotionTrs.motion = null;
                paramDrawMotionTrs.frame = 0.0f;
            }
            amDrawRegistCommand(state, -12, paramDrawMotionTrs);
        }
    }

    private void amMotionDraw(AMS_MOTION motion, NNS_TEXLIST texlist)
    {
        this.amMotionDraw(motion, texlist, 0U, null);
    }

    private void amMotionDraw(AMS_MOTION motion, NNS_TEXLIST texlist, uint drawflag)
    {
        this.amMotionDraw(motion, texlist, drawflag, null);
    }

    private void amMotionDraw(
      AMS_MOTION motion,
      NNS_TEXLIST texlist,
      uint drawflag,
      NNS_MATERIALCALLBACK_FUNC func)
    {
        AMS_PARAM_DRAW_MOTION_TRS paramDrawMotionTrs;
        AMS_COMMAND_HEADER command = new AMS_COMMAND_HEADER()
        {
            param = paramDrawMotionTrs = new AMS_PARAM_DRAW_MOTION_TRS(),
            command_id = -11
        };
        command.param = paramDrawMotionTrs;
        paramDrawMotionTrs._object = motion._object;
        paramDrawMotionTrs.mtx = null;
        paramDrawMotionTrs.sub_obj_type = 0U;
        paramDrawMotionTrs.flag = drawflag;
        paramDrawMotionTrs.texlist = texlist;
        paramDrawMotionTrs.trslist = motion.data;
        paramDrawMotionTrs.material_func = func;
        int motionId = motion.mbuf[0].motion_id;
        paramDrawMotionTrs.motion = motion.mtnfile[motionId >> 16].motion[motionId & ushort.MaxValue];
        paramDrawMotionTrs.frame = motion.mbuf[0].frame;
        _amDrawMotionTRS(command, drawflag);
    }

    private void amMotionMaterialDraw(AMS_MOTION motion, NNS_TEXLIST texlist)
    {
        this.amMotionMaterialDraw(motion, texlist, 0U, null);
    }

    private void amMotionMaterialDraw(
      AMS_MOTION motion,
      NNS_TEXLIST texlist,
      uint drawflag)
    {
        this.amMotionMaterialDraw(motion, texlist, drawflag, null);
    }

    private void amMotionMaterialDraw(
      AMS_MOTION motion,
      NNS_TEXLIST texlist,
      uint drawflag,
      NNS_MATERIALCALLBACK_FUNC func)
    {
        if (motion.mmobject == null)
        {
            this.amMotionDraw(motion, texlist, drawflag);
        }
        else
        {
            AMS_PARAM_DRAW_MOTION_TRS paramDrawMotionTrs;
            AMS_COMMAND_HEADER command = new AMS_COMMAND_HEADER()
            {
                param = paramDrawMotionTrs = new AMS_PARAM_DRAW_MOTION_TRS(),
                command_id = -12
            };
            command.param = paramDrawMotionTrs;
            paramDrawMotionTrs._object = motion.mmobject;
            paramDrawMotionTrs.mtx = null;
            paramDrawMotionTrs.sub_obj_type = 0U;
            paramDrawMotionTrs.flag = drawflag;
            paramDrawMotionTrs.texlist = texlist;
            paramDrawMotionTrs.trslist = motion.data;
            paramDrawMotionTrs.material_func = func;
            paramDrawMotionTrs.mmotion = null;
            paramDrawMotionTrs.mframe = 0.0f;
            int motionId = motion.mbuf[0].motion_id;
            if (motion.mtnfile[motionId >> 16].file != null)
            {
                paramDrawMotionTrs.motion = motion.mtnfile[motionId >> 16].motion[motionId & ushort.MaxValue];
                paramDrawMotionTrs.frame = motion.mbuf[0].frame;
            }
            else
            {
                paramDrawMotionTrs.motion = null;
                paramDrawMotionTrs.frame = 0.0f;
            }
            _amDrawMotionTRS(command, drawflag);
        }
    }

    public static float amMotionGetStartFrame(AMS_MOTION motion, int motion_id)
    {
        return motion.mtnfile[motion_id >> 16].motion[motion_id & ushort.MaxValue].StartFrame;
    }

    public static float amMotionGetEndFrame(AMS_MOTION motion, int motion_id)
    {
        return motion.mtnfile[motion_id >> 16].motion[motion_id & ushort.MaxValue].EndFrame;
    }

    private void amMotionGetFrames(
      AMS_MOTION motion,
      int motion_id,
      out float start,
      out float end)
    {
        NNS_MOTION nnsMotion = motion.mtnfile[motion_id >> 16].motion[motion_id & ushort.MaxValue];
        start = nnsMotion.StartFrame;
        end = nnsMotion.EndFrame;
    }

    public static float amMotionMaterialGetStartFrame(AMS_MOTION motion, int motion_id)
    {
        return motion.mmtn[motion.mmotion_id].StartFrame;
    }

    public static float amMotionMaterialGetEndFrame(AMS_MOTION motion, int motion_id)
    {
        return motion.mmtn[motion.mmotion_id].EndFrame;
    }

    private void amMotionMaterialGetFrames(
      AMS_MOTION motion,
      int motion_id,
      out float start,
      out float end)
    {
        NNS_MOTION nnsMotion = motion.mmtn[motion.mmotion_id];
        start = nnsMotion.StartFrame;
        end = nnsMotion.EndFrame;
    }

}