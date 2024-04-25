public partial class AppMain
{
    private static int dwaterInit()
    {
        if (_dmap_water == null)
            _dmap_water = new DMAP_WATER();
        _dmap_water.repeat_u = 1f;
        _dmap_water.repeat_v = 1f;
        _dmap_water.speed_u = 1f / 1000f;
        _dmap_water.speed_v = -1f / 1000f;
        _dmap_water.speed_surface = 1f / 1000f;
        _dmap_water.regist_index = -1;
        _dmap_water.repeat_pos_x = 280f;
        dwaterSetColor(0.65f, 1f, 0.75f);
        return _dmap_water.regist_index;
    }

    private static void dwaterExit()
    {
        if (_dmap_water != null)
        {
            for (int index = 0; index < 2; ++index)
            {
                if (_dmap_water._object[index]._object != null)
                    _dmap_water._object[index]._object = null;
                if (_dmap_water._object[index].texlistbuf != null)
                    _dmap_water._object[index].texlistbuf = null;
                if (_dmap_water._object[index].motion != null)
                    amMotionDelete(_dmap_water._object[index].motion);
            }
        }
        _dmap_water = null;
    }

    private static void dwaterSetObjectAMB(
      AMS_AMB_HEADER amb_obj,
      AMS_AMB_HEADER amb_tex)
    {
        _dmap_water.amb_object = amb_obj;
        _dmap_water.amb_texture = amb_tex;
    }

    private static int dwaterLoadObject(uint objflag)
    {
        int num1 = 1;
        ArrayPointer<DMAP_WATER_OBJ> arrayPointer = new ArrayPointer<DMAP_WATER_OBJ>(_dmap_water._object);
        if (_dmap_water.regist_index != -1)
        {
            if (!amDrawIsRegistComplete(_dmap_water.regist_index))
                return 0;
            _dmap_water.regist_index = -1;
            if ((~arrayPointer)._object != null)
            {
                int num2 = 0;
                while (num2 < 2)
                {
                    object file = _dmap_water.amb_object.buf[2 + num2];
                    (~arrayPointer).motion = amMotionCreate((~arrayPointer)._object);
                    amMotionMaterialRegistFile((~arrayPointer).motion, 0, file);
                    amMotionMaterialSet((~arrayPointer).motion, 0);
                    amMotionMaterialSetFrame((~arrayPointer).motion, amMotionMaterialGetStartFrame((~arrayPointer).motion, 0));
                    ++num2;
                    ++arrayPointer;
                }
                return 1;
            }
        }
        int index = 0;
        while (index < 2)
        {
            object buf = amBindGet(_dmap_water.amb_object, index);
            _dmap_water.regist_index = amObjectLoad(out (~arrayPointer)._object, out (~arrayPointer).texlist, out (~arrayPointer).texlistbuf, buf, objflag, null, _dmap_water.amb_texture);
            num1 = 0;
            ++index;
            ++arrayPointer;
        }
        return num1;
    }

    private static int dwaterLoadTexture(
      object water_image,
      int water_size,
      object color_image,
      int color_size)
    {
        return 0;
    }

    private static int dwaterRelease()
    {
        int num = -1;
        if (_dmap_water != null)
        {
            for (int index = 0; index < 2; ++index)
            {
                if (_dmap_water._object[index]._object != null)
                    num = amObjectRelease(_dmap_water._object[index]._object, _dmap_water._object[index].texlist);
            }
        }
        return num;
    }

    private static void dwaterSetColor(float r, float g, float b)
    {
    }

    private static void dwaterUpdate(
      float speed,
      float pos_x,
      float pos_y,
      float dy,
      int rot_z,
      float scale)
    {
        float num1 = 0.006666667f;
        _dmap_water.speed_surface += 0.0005f;
        if (_dmap_water.speed_surface > 1.0)
            --_dmap_water.speed_surface;
        _dmap_water.pos_x = pos_x;
        _dmap_water.pos_y = pos_y;
        _dmap_water.pos_dy = dy;
        _dmap_water.rot_z = rot_z;
        _dmap_water.scale = scale;
        float f1 = _dmap_water.ofst_u + _dmap_water.speed_u * speed;
        float num2 = f1 < 0.0 ? (float)(1.0 - (-f1 - (double)floorf(-f1))) : f1 - floorf(f1);
        _dmap_water.ofst_u = num2;
        float f2 = _dmap_water.ofst_v + _dmap_water.speed_v * speed;
        float num3 = f2 < 0.0 ? (float)(1.0 - (-f2 - (double)floorf(-f2))) : f2 - floorf(f2);
        _dmap_water.ofst_v = num3;
        float num4 = (float)(num1 * (double)_dmap_water.repeat_u / 1.14999997615814);
        float f3 = _dmap_water.ofst_u + pos_x * num4;
        float num5 = f3 < 0.0 ? (float)(1.0 - (-f3 - (double)floorf(-f3))) : f3 - floorf(f3);
        _dmap_water.draw_u = num5;
        _dmap_water.draw_v = _dmap_water.ofst_v;
        ArrayPointer<DMAP_WATER_OBJ> arrayPointer = new ArrayPointer<DMAP_WATER_OBJ>(_dmap_water._object);
        int num6 = 0;
        while (num6 < 2)
        {
            AMS_MOTION motion = (~arrayPointer).motion;
            float num7 = (~arrayPointer).frame + speed;
            float startFrame = amMotionMaterialGetStartFrame(motion, 0);
            float endFrame = amMotionMaterialGetEndFrame(motion, 0);
            while (num7 >= (double)endFrame)
                num7 = startFrame + (num7 - endFrame);
            (~arrayPointer).frame = num7;
            ++num6;
            ++arrayPointer;
        }
    }

    private static void dwaterGetParam(DMAP_PARAM_WATER param)
    {
        param.frame[0] = _dmap_water._object[0].frame;
        param.frame[1] = _dmap_water._object[1].frame;
        param.draw_u = _dmap_water.draw_u;
        param.draw_v = _dmap_water.draw_v;
        param.scale = _dmap_water.scale;
        param.pos_x = _dmap_water.pos_x;
        param.pos_y = _dmap_water.pos_y;
        param.pos_dy = _dmap_water.pos_dy;
        param.repeat_u = _dmap_water.repeat_u;
        param.repeat_v = _dmap_water.repeat_v;
        param.rot_z = _dmap_water.rot_z;
        param.color = _dmap_water.color;
    }

    private static void dwaterSetParam()
    {
        DMAP_PARAM_WATER dmapParamWater = amDrawAlloc_DMAP_PARAM_WATER();
        dwaterGetParam(dmapParamWater);
        amDrawMakeTask(new TaskProc(_dwaterSetParam), 0, dmapParamWater);
    }

    private static void dwaterDrawReflection(uint state, uint drawflag)
    {
        amMatrixPush();
        NNS_MATRIX current = amMatrixGetCurrent();
        DMAP_WATER_OBJ dmapWaterObj = _dmap_water._object[1];
        AMS_MOTION motion = dmapWaterObj.motion;
        amMotionMaterialSetFrame(motion, _dmap_water._object[1].frame);
        amMotionMaterialCalc(motion);
        float x = floorf(_dmap_water.pos_x / (_dmap_water.repeat_pos_x * _dmap_water.scale) - 0.5f) * (_dmap_water.repeat_pos_x * _dmap_water.scale);
        float y = _dmap_water.pos_y + _dmap_water.pos_dy;
        for (int index = 0; index < 2; ++index)
        {
            nnMakeTranslateMatrix(current, x, y, FX_FX32_TO_F32(-1179648));
            nnScaleMatrix(current, current, _dmap_water.scale, _dmap_water.scale, 1f);
            ObjDraw3DNNMotionMaterialMotion(motion, dmapWaterObj.texlist, drawflag, 0U, null, null, state, null, null, null, null, null, 1U);
            x += _dmap_water.repeat_pos_x * _dmap_water.scale;
        }
        amMatrixPop();
    }

    private static void dwaterDrawSurface(uint state, uint drawflag)
    {
        amMatrixPush();
        NNS_MATRIX current = amMatrixGetCurrent();
        DMAP_WATER_OBJ dmapWaterObj = _dmap_water._object[0];
        AMS_MOTION motion = dmapWaterObj.motion;
        amMotionMaterialSetFrame(motion, _dmap_water._object[0].frame);
        amMotionMaterialCalc(motion);
        float x = floorf(_dmap_water.pos_x / (_dmap_water.repeat_pos_x * _dmap_water.scale) - 0.5f) * (_dmap_water.repeat_pos_x * _dmap_water.scale);
        float y = _dmap_water.pos_y + _dmap_water.pos_dy;
        for (int index = 0; index < 2; ++index)
        {
            nnMakeTranslateMatrix(current, x, y, FX_FX32_TO_F32(917504));
            nnScaleMatrix(current, current, _dmap_water.scale, _dmap_water.scale * 2f, 1f);
            ObjDraw3DNNMotionMaterialMotion(motion, dmapWaterObj.texlist, drawflag, 0U, null, null, state, null, null, null, null, null, 1U);
            x += _dmap_water.repeat_pos_x * _dmap_water.scale;
        }
        amMatrixPop();
    }

    private static void dwaterDrawWater(AMS_RENDER_TARGET texture)
    {
        DMAP_PARAM_WATER drawParam = _dmap_water.draw_param;
        amDrawPushState();
        NNS_VECTOR nnsVector = new NNS_VECTOR(0.0f, drawParam.pos_dy, -0.5f);
        nnTransformVector(nnsVector, amDrawGetProjectionMatrix(), nnsVector);
        double drawU = drawParam.draw_u;
        double drawV = drawParam.draw_v;
        double repeatU = drawParam.repeat_u;
        double scale1 = drawParam.scale;
        double repeatV = drawParam.repeat_v;
        double scale2 = drawParam.scale;
        double y = nnsVector.y;
        int rotZ = drawParam.rot_z;
        int color = (int)drawParam.color;
        amDrawPopState();
    }

    private static void _dwaterSetParam(AMS_TCB tcbp)
    {
        DMAP_PARAM_WATER work = (DMAP_PARAM_WATER)amTaskGetWork(tcbp);
        _dmap_water.draw_param = work;
    }

}