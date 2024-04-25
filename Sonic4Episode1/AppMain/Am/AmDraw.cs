using System;
using mpp;

public partial class AppMain
{
    public const int AMD_TRAIL_PARTSMAX = 64;
    public const int AMD_TRAIL_MAX = 8;
    public const int AMD_TRAILEF_WORKSIZE = 128;
    public const int AMD_TRAILEF_BUFSIZE = 8;
    public const int AMD_TRAIL_COMP_FRAME2 = 6;
    public const int AMD_ON = 1;
    public const int AMD_OFF = 2;
    public const uint AMD_DRAWSTATE_STACK_NUM = 8;
    public const int AMD_VMODE_STD_NTSC = 0;
    public const int AMD_VMODE_STD_PAL = 1;
    public const int AMD_VMODE_STD_PAL60 = 2;
    public const int AMD_VMODE_STD_PAL60P = 3;
    public const int AMD_VMODE_STD_HIDEF = 4;
    public const int AMD_DISPLAYLIST_NUM = 4;
    public const int AMD_REGISTLIST_NUM = 256;
    public const int AMD_SORTLIST_NUM = 512;
    private const int AMD_ZFUNC_LEQUAL = 515;
    private const int AMD_WRAP_CLAMP = 33071;
    private const int AMD_WRAP_REPEAT = 10497;
    private const int AMD_ZFUNC_DEFAULT = 515;
    public const int AMD_USE_VIRTUAL_RESOLUTION_2D = 1;
    public const int AMD_REGIST_NONE = 0;
    public const int AMD_REGIST_LOAD_TEXTURE = 1;
    public const int AMD_REGIST_RELEASE_TEXTURE = 2;
    public const int AMD_REGIST_VERTEX_BUFFER_OBJECT = 3;
    public const int AMD_REGIST_DELETE_VERTEX_OBJECT = 4;
    public const int AMD_REGIST_LOAD_SHADER_OBJECT = 5;
    public const int AMD_REGIST_RELEASE_STD_SHADER = 6;
    public const int AMD_REGIST_LOAD_SHADER = 7;
    public const int AMD_REGIST_BUILD_SHADER = 8;
    public const int AMD_REGIST_CREATE_SHADER = 9;
    public const int AMD_REGIST_RELEASE_SHADER = 10;
    public const int AMD_REGIST_LOAD_TEXTURE_IMAGE = 11;
    public const int AMD_REGIST_RELEASE_TEXTURE_IMAGE = 12;
    public const int AMD_COMMAND_STATE_MAKE_TASK = 16777216;
    public const int AMD_COMMAND_STATE_DEBUG_PRINT = 33554432;
    public const int AMD_COMMAND_STATE_DEBUG = 50331648;
    public const int AMD_COMMAND_MAKE_TASK = -1;
    public const int AMD_COMMAND_DEBUG_PRINT = -2;
    public const int AMD_COMMAND_DEBUG_COLOR = -3;
    public const int AMD_COMMAND_DEBUG_MEMORY = -4;
    public const int AMD_COMMAND_DEBUG_THREAD = -5;
    public const int AMD_COMMAND_DRAW_OBJECT = -6;
    public const int AMD_COMMAND_DRAW_OBJECT_MATMTN = -7;
    public const int AMD_COMMAND_DRAW_OBJECT_MATERIAL = -8;
    public const int AMD_COMMAND_DRAW_MOTION = -9;
    public const int AMD_COMMAND_DRAW_MOTION_MATMTN = -10;
    public const int AMD_COMMAND_DRAW_MOTION_TRS = -11;
    public const int AMD_COMMAND_DRAW_MOTION_TRS_MATMTN = -12;
    public const int AMD_COMMAND_DRAW_PRIMITIVE_2D = -13;
    public const int AMD_COMMAND_DRAW_PRIMITIVE_3D = -14;
    public const int AMD_COMMAND_SET_DIFFUSE = -15;
    public const int AMD_COMMAND_SET_AMBIENT = -16;
    public const int AMD_COMMAND_SET_ALPHA = -17;
    public const int AMD_COMMAND_SET_SPECULAR = -18;
    public const int AMD_COMMAND_SET_ENVMAP = -19;
    public const int AMD_COMMAND_SET_BLEND = -20;
    public const int AMD_COMMAND_SET_TEXOFFSET = -21;
    public const int AMD_COMMAND_SET_FOG = -22;
    public const int AMD_COMMAND_SET_FOG_COLOR = -23;
    public const int AMD_COMMAND_SET_FOG_RANGE = -24;
    public const int AMD_COMMAND_SET_Z_MODE = -25;
    public const int AMD_COMMAND_SORT_DRAW_OBJECT = -1;
    public const int AMD_COMMAND_SORT_DRAW_OBJECT_MATMTN = -2;
    public const int AMD_COMMAND_SORT_DRAW_PRIMITIVE2D = -3;
    public const int AMD_COMMAND_SORT_DRAW_PRIMITIVE3D = -4;
    private const int AMDRAWE_BLENDTYPE_NORMAL = 0;
    private const int AMDRAWE_BLENDTYPE_ADD = 1;
    private const int AMDRAWE_BLENDTYPE_SUB = 2;
    private const int AMDRAWE_BLENDTYPE_NUM = 3;
    private const int AMD_NNS_PRIM3D_PCT_BUF_SIZE = 16384;
    private const int AMD_NNS_PRIM3D_PCT_ARRAY_COUNT = 1024;

    private static Pool<AMS_PARAM_DRAW_PRIMITIVE> amDraw_AMS_PARAM_DRAW_PRIMITIVE_Pool;
    private static Pool<NNS_MATRIX> amDraw_NNS_MATRIX_Pool;
    private static Pool<AMS_PARAM_MAKE_TASK> amDraw_AMS_PARAM_MAKE_TASK_Pool;
    private static Pool<AMS_DRAWSTATE_FOG> amDraw_AMS_DRAWSTATE_FOG_Pool;
    private static Pool<AMS_DRAWSTATE_FOG_COLOR> amDraw_AMS_DRAWSTATE_FOG_COLOR_Pool;
    private static Pool<AMS_DRAWSTATE_FOG_RANGE> amDraw_AMS_DRAWSTATE_FOG_RANGE_Pool;
    private static Pool<AMS_PARAM_DRAW_OBJECT_MATERIAL> amDraw_AMS_PARAM_DRAW_OBJECT_MATERIAL_Pool;
    private static Pool<GMS_GMK_ITEM_MAT_CB_PARAM> amDraw_GMS_GMK_ITEM_MAT_CB_PARAM_Pool;
    private static Pool<DMAP_PARAM_WATER> amDraw_DMAP_PARAM_WATER_Pool;
    private static ArrayPool<NNS_PRIM3D_PC> amDraw_NNS_PRIM3D_PC_Array_Pool;
    private static ArrayPool<GMS_MAP_PRIM_DRAW_WORK> amDraw_GMS_MAP_PRIM_DRAW_WORK_Array_Pool;
    private static ArrayPoolFast<NNS_TRS> amDraw_NNS_TRS_Array_Pool;
    private static Pool<NNS_MATERIAL_STDSHADER_COLOR> amDraw_NNS_MATERIAL_STDSHADER_COLOR_Pool;
    private static Pool<NNS_MATERIAL_GLES11_DESC> amDraw_NNS_MATERIAL_GLES11_DESC_Pool;
    private static Pool<NNS_MATERIALPTR> amDraw_NNS_MATERIALPTR_Pool;
    private static ArrayPoolFast<NNS_MATERIALPTR> amDraw_NNS_MATERIALPTR_Array_Pool;
    private static ArrayPoolFast<GMS_BS_CMN_CNM_NODE_INFO> amDraw_GMS_BS_CMN_CNM_NODE_INFO_Array_Pool;
    private static Pool<GMS_BS_CMN_CNM_NODE_INFO> amDraw_GMS_BS_CMN_CNM_NODE_INFO_Pool;
    private static Pool<GMS_BS_CMN_CNM_PARAM> amDraw_GMS_BS_CMN_CNM_PARAM_Pool;
    private static Pool<OBS_DRAW_PARAM_3DNN_USER_FUNC> amDraw_OBS_DRAW_PARAM_3DNN_USER_FUNC_Pool;
    private static Pool<OBS_DRAW_PARAM_3DNN_MOTION> amDraw_OBS_DRAW_PARAM_3DNN_MOTION_Pool;
    private static Pool<OBS_DRAW_PARAM_3DNN_DRAW_MOTION> amDraw_OBS_DRAW_PARAM_3DNN_DRAW_MOTION_Pool;
    private static Pool<AMS_PARAM_DRAW_MOTION_TRS> amDraw_AMS_PARAM_DRAW_MOTION_TRS_Pool;
    private static Pool<NNS_TRS> amDraw_NNS_TRS_Pool;
    private static Pool<AOS_ACT_DRAW> amDraw_AOS_ACT_DRAW_Pool;
    private static Pool<NNS_OBJECT> amDraw_NNS_OBJECT_Pool;

    public static uint AMD_FCOLTORGBA8888(float r, float g, float b, float a)
    {
        return (uint)(((int)(uint)(r * (double)byte.MaxValue) & byte.MaxValue) << 24 | ((int)(uint)(g * (double)byte.MaxValue) & byte.MaxValue) << 16 | ((int)(uint)(b * (double)byte.MaxValue) & byte.MaxValue) << 8 | (int)(uint)(a * (double)byte.MaxValue) & byte.MaxValue);
    }

    public static float AMD_DISPLAY_WIDTH
    {
        get => _am_draw_video.disp_width;
        set => _am_draw_video.disp_width = value;
    }

    public static float AMD_DISPLAY_HEIGHT
    {
        get => _am_draw_video.disp_height;
        set => _am_draw_video.disp_height = value;
    }

    public static float AMD_SCREEN_WIDTH
    {
        get => _am_draw_video.draw_width;
        set => _am_draw_video.draw_width = value;
    }

    public static float AMD_SCREEN_HEIGHT
    {
        get => _am_draw_video.draw_height;
        set => _am_draw_video.draw_height = value;
    }

    public static float AMD_SCREEN_2D_WIDTH
    {
        get => _am_draw_video.width_2d;
        set => _am_draw_video.width_2d = value;
    }

    public static float AMD_SCREEN_2D_HEIGHT
    {
        get => _am_draw_video.height_2d;
        set => _am_draw_video.height_2d = value;
    }

    public static float AMD_2D_SCALE_X
    {
        get => _am_draw_video.scale_x_2d;
        set => _am_draw_video.scale_x_2d = value;
    }

    public static float AMD_2D_SCALE_Y
    {
        get => _am_draw_video.scale_y_2d;
        set => _am_draw_video.scale_y_2d = value;
    }

    public static float AMD_2D_BASE_X
    {
        get => _am_draw_video.base_x_2d;
        set => _am_draw_video.base_x_2d = value;
    }

    public static float AMD_2D_BASE_Y
    {
        get => _am_draw_video.base_y_2d;
        set => _am_draw_video.base_y_2d = value;
    }

    public static float AMD_SCREEN_ASPECT
    {
        get => _am_draw_video.draw_aspect;
        set => _am_draw_video.draw_aspect = value;
    }

    public static int AMD_RENDER_WIDTH
    {
        get => _am_render_manager.target_now.width;
        set => _am_render_manager.target_now.width = value;
    }

    public static int AMD_RENDER_HEIGHT
    {
        get => _am_render_manager.target_now.height;
        set => _am_render_manager.target_now.height = value;
    }

    public static float AMD_RENDER_ASPECT
    {
        get => _am_render_manager.target_now.aspect;
        set => _am_render_manager.target_now.aspect = value;
    }

    private static AMS_PARAM_DRAW_PRIMITIVE amDrawAlloc_AMS_PARAM_DRAW_PRIMITIVE()
    {
        return amDraw_AMS_PARAM_DRAW_PRIMITIVE_Pool.Alloc();
    }

    private static NNS_MATRIX amDrawAlloc_NNS_MATRIX()
    {
        return amDraw_NNS_MATRIX_Pool.Alloc();
    }

    private static AMS_DRAWSTATE_FOG amDrawAlloc_AMS_DRAWSTATE_FOG()
    {
        return amDraw_AMS_DRAWSTATE_FOG_Pool.Alloc();
    }

    private static AMS_DRAWSTATE_FOG_COLOR amDrawAlloc_AMS_DRAWSTATE_FOG_COLOR()
    {
        return amDraw_AMS_DRAWSTATE_FOG_COLOR_Pool.Alloc();
    }

    private static AMS_DRAWSTATE_FOG_RANGE amDrawAlloc_AMS_DRAWSTATE_FOG_RANGE()
    {
        return amDraw_AMS_DRAWSTATE_FOG_RANGE_Pool.Alloc();
    }

    private static AMS_PARAM_MAKE_TASK amDrawAlloc_AMS_PARAM_MAKE_TASK()
    {
        return amDraw_AMS_PARAM_MAKE_TASK_Pool.Alloc();
    }

    private static AMS_PARAM_DRAW_OBJECT_MATERIAL amDrawAlloc_AMS_PARAM_DRAW_OBJECT_MATERIAL()
    {
        return amDraw_AMS_PARAM_DRAW_OBJECT_MATERIAL_Pool.Alloc();
    }

    private static GMS_GMK_ITEM_MAT_CB_PARAM amDrawAlloc_GMS_GMK_ITEM_MAT_CB_PARAM()
    {
        return amDraw_GMS_GMK_ITEM_MAT_CB_PARAM_Pool.Alloc();
    }

    private static DMAP_PARAM_WATER amDrawAlloc_DMAP_PARAM_WATER()
    {
        return amDraw_DMAP_PARAM_WATER_Pool.Alloc();
    }

    private static NNS_PRIM3D_PCT_ARRAY amDrawAlloc_NNS_PRIM3D_PCT(int count)
    {
        ++NNS_PRIM3D_PCT_ALLOC_CNT;
        if (NNS_PRIM3D_PCT_buf_size + count >= 16384)
            NNS_PRIM3D_PCT_buf_size = 0;
        if (NNS_PRIM3D_PCT_arrays_count >= 1024)
            NNS_PRIM3D_PCT_arrays_count = 0;
        NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = NNS_PRIM3D_PCT_arrays[NNS_PRIM3D_PCT_arrays_count];
        nnsPriM3DPctArray.buffer = NNS_PRIM3D_PCT_buf;
        nnsPriM3DPctArray.offset = NNS_PRIM3D_PCT_buf_size;
        ++NNS_PRIM3D_PCT_arrays_count;
        NNS_PRIM3D_PCT_buf_size += count;
        return nnsPriM3DPctArray;
    }

    private static NNS_PRIM3D_PC[] amDrawAlloc_NNS_PRIM3D_PC(int count)
    {
        return amDraw_NNS_PRIM3D_PC_Array_Pool.AllocArray(count);
    }

    private static NNS_TRS[] amDrawAlloc_NNS_TRS(int count)
    {
        return amDraw_NNS_TRS_Array_Pool.AllocArray(count);
    }

    private static NNS_MATERIALPTR[] amDrawAlloc_NNS_MATERIALPTR(int count)
    {
        return amDraw_NNS_MATERIALPTR_Array_Pool.AllocArray(count);
    }

    private static GMS_BS_CMN_CNM_NODE_INFO[] amDrawAlloc_GMS_BS_CMN_CNM_NODE_INFO(
      int count)
    {
        return amDraw_GMS_BS_CMN_CNM_NODE_INFO_Array_Pool.AllocArray(count);
    }

    private static GMS_BS_CMN_CNM_NODE_INFO amDrawAlloc_GMS_BS_CMN_CNM_NODE_INFO()
    {
        return amDraw_GMS_BS_CMN_CNM_NODE_INFO_Pool.Alloc();
    }

    private static GMS_BS_CMN_CNM_PARAM amDrawAlloc_GMS_BS_CMN_CNM_PARAM()
    {
        return amDraw_GMS_BS_CMN_CNM_PARAM_Pool.Alloc();
    }

    private static NNS_MATERIAL_STDSHADER_COLOR amDrawAlloc_NNS_MATERIAL_STDSHADER_COLOR()
    {
        return amDraw_NNS_MATERIAL_STDSHADER_COLOR_Pool.Alloc();
    }

    private static NNS_MATERIAL_GLES11_DESC amDrawAlloc_NNS_MATERIAL_GLES11_DESC()
    {
        return amDraw_NNS_MATERIAL_GLES11_DESC_Pool.Alloc();
    }

    private static NNS_MATERIALPTR amDrawAlloc_NNS_MATERIALPTR()
    {
        return amDraw_NNS_MATERIALPTR_Pool.Alloc();
    }

    private static OBS_DRAW_PARAM_3DNN_USER_FUNC amDrawAlloc_OBS_DRAW_PARAM_3DNN_USER_FUNC()
    {
        return amDraw_OBS_DRAW_PARAM_3DNN_USER_FUNC_Pool.Alloc();
    }

    private static OBS_DRAW_PARAM_3DNN_MOTION amDrawAlloc_OBS_DRAW_PARAM_3DNN_MOTION()
    {
        return amDraw_OBS_DRAW_PARAM_3DNN_MOTION_Pool.Alloc();
    }

    private static OBS_DRAW_PARAM_3DNN_DRAW_MOTION amDrawAlloc_OBS_DRAW_PARAM_3DNN_DRAW_MOTION()
    {
        return amDraw_OBS_DRAW_PARAM_3DNN_DRAW_MOTION_Pool.Alloc();
    }

    private static AMS_PARAM_DRAW_MOTION_TRS amDrawAlloc_AMS_PARAM_DRAW_MOTION_TRS()
    {
        return amDraw_AMS_PARAM_DRAW_MOTION_TRS_Pool.Alloc();
    }

    private static NNS_TRS amDrawAlloc_NNS_TRS()
    {
        return amDraw_NNS_TRS_Pool.Alloc();
    }

    private static AOS_ACT_DRAW amDrawAlloc_AOS_ACT_DRAW()
    {
        return amDraw_AOS_ACT_DRAW_Pool.Alloc();
    }

    private static NNS_OBJECT amDrawAlloc_NNS_OBJECT()
    {
        return amDraw_NNS_OBJECT_Pool.Alloc();
    }

    private static void amDrawResetCache()
    {
        amDraw_AMS_PARAM_DRAW_PRIMITIVE_Pool.ReleaseUsedObjects();
        amDraw_NNS_MATRIX_Pool.ReleaseUsedObjects();
        amDraw_AMS_PARAM_MAKE_TASK_Pool.ReleaseUsedObjects();
        amDraw_AMS_DRAWSTATE_FOG_Pool.ReleaseUsedObjects();
        amDraw_AMS_PARAM_DRAW_OBJECT_MATERIAL_Pool.ReleaseUsedObjects();
        amDraw_AMS_DRAWSTATE_FOG_COLOR_Pool.ReleaseUsedObjects();
        amDraw_AMS_DRAWSTATE_FOG_RANGE_Pool.ReleaseUsedObjects();
        amDraw_NNS_PRIM3D_PC_Array_Pool.ReleaseUsedArrays();
        amDraw_GMS_MAP_PRIM_DRAW_WORK_Array_Pool.ReleaseUsedArrays();
        amDraw_OBS_DRAW_PARAM_3DNN_USER_FUNC_Pool.ReleaseUsedObjects();
        amDraw_OBS_DRAW_PARAM_3DNN_MOTION_Pool.ReleaseUsedObjects();
        amDraw_OBS_DRAW_PARAM_3DNN_DRAW_MOTION_Pool.ReleaseUsedObjects();
        amDraw_AMS_PARAM_DRAW_MOTION_TRS_Pool.ReleaseUsedObjects();
        amDraw_GMS_GMK_ITEM_MAT_CB_PARAM_Pool.ReleaseUsedObjects();
        amDraw_DMAP_PARAM_WATER_Pool.ReleaseUsedObjects();
        amDraw_NNS_TRS_Pool.ReleaseUsedObjects();
        amDraw_AOS_ACT_DRAW_Pool.ReleaseUsedObjects();
        amDraw_NNS_OBJECT_Pool.ReleaseUsedObjects();
        amDraw_NNS_TRS_Array_Pool.ReleaseUsedArrays();
        amDraw_GMS_BS_CMN_CNM_NODE_INFO_Array_Pool.ReleaseUsedArrays();
        amDraw_GMS_BS_CMN_CNM_NODE_INFO_Pool.ReleaseUsedObjects();
        amDraw_GMS_BS_CMN_CNM_PARAM_Pool.ReleaseUsedObjects();
        amDraw_NNS_MATERIAL_STDSHADER_COLOR_Pool.ReleaseUsedObjects();
        amDraw_NNS_MATERIAL_GLES11_DESC_Pool.ReleaseUsedObjects();
        amDraw_NNS_MATERIALPTR_Pool.ReleaseUsedObjects();
        amDraw_NNS_MATERIALPTR_Array_Pool.ReleaseUsedArrays();
        NNS_PRIM3D_PCT_buf_size = 0;
        NNS_PRIM3D_PCT_arrays_count = 0;
    }

    private static void amDrawCreateBuffer(int command_size, int data_size, int work_size)
    {
        _am_draw_command_buf_max = command_size;
        _am_draw_data_buf_max = data_size;
        for (int index = 0; index < 4; ++index)
        {
            _am_draw_command_buf[index] = new object[command_size];
            _am_draw_data_buf[index] = new object[data_size];
        }
    }

    private static void amDrawDeleteBuffer()
    {
    }

    public static void amDrawSetDrawCommandFunc(
      _am_draw_command_delegate func,
      _am_draw_command_delegate sort)
    {
        _am_draw_command_func = func;
        _am_draw_command_sort = sort;
    }

    private static void amDrawSetShaderCompile(int flag)
    {
        mppAssertNotImpl();
    }

    private static void amDrawSetVSyncAlarm(AMS_ALARM alarm)
    {
        mppAssertNotImpl();
    }

    private static void amDrawWaitVSync()
    {
        mppAssertNotImpl();
    }

    public bool amDrawBegin()
    {
        return this.amDrawBegin(null, AMD_RENDER_CLEAR_COLOR | AMD_RENDER_CLEAR_DEPTH, null, 1f, 0);
    }

    public bool amDrawBegin(
      AMS_RENDER_TARGET target,
      uint flag,
      NNS_RGBA_U8 color,
      float depth,
      int stencil)
    {
        _am_draw_offset_x = 0;
        amRenderSetTarget(target, flag, color, depth, stencil);
        if (_am_draw_in_scene != 0 || flag == 0U)
            return true;
        uint mask = 0;
        if (((int)flag & (int)AMD_RENDER_CLEAR_COLOR) != 0)
        {
            if (color != null)
            {
                _am_draw_bg_color.r = color.r;
                _am_draw_bg_color.g = color.g;
                _am_draw_bg_color.b = color.b;
                _am_draw_bg_color.a = color.a;
            }
            OpenGL.glClearColor(_am_draw_bg_color.r / (float)byte.MaxValue, _am_draw_bg_color.g / (float)byte.MaxValue, _am_draw_bg_color.b / (float)byte.MaxValue, _am_draw_bg_color.a / (float)byte.MaxValue);
            mask |= 16384U;
        }
        if (((int)flag & (int)AMD_RENDER_CLEAR_DEPTH) != 0)
        {
            OpenGL.glClearDepthf(depth);
            mask |= 256U;
        }
        OpenGL.glClear(mask);
        return true;
    }

    private static void amDrawEnd()
    {
        amDrawEnd(null, 1f, 0);
    }

    private static void amDrawEnd(NNS_RGBA_U8 color, float z, int stencil)
    {
        if (_am_draw_in_scene == 0)
            return;
        _am_draw_in_scene = 0;
    }

    private static void amDrawSetBGColor(NNS_RGBA_U8 bgColor)
    {
        _am_draw_bg_color = bgColor;
    }

    private static NNS_RGBA_U8 amDrawGetBGColor()
    {
        mppAssertNotImpl();
        return _am_draw_bg_color;
    }

    private static void amDrawBeginScene()
    {
        _am_displaylist_manager.sort_num = 0;
    }

    public static void amDrawEndScene()
    {
        AMS_DISPLAYLIST_MANAGER displaylistManager = _am_displaylist_manager;
        int sortNum = displaylistManager.sort_num;
        displaylistManager.sort_num = 0;
        if (sortNum == 0)
            return;
        AMS_DRAW_SORT sort = new AMS_DRAW_SORT();
        ArrayPointer<AMS_DRAW_SORT> arrayPointer1 = new ArrayPointer<AMS_DRAW_SORT>(displaylistManager.sortlist);
        int num1 = sortNum - 1;
        while (num1 > 0)
        {
            ArrayPointer<AMS_DRAW_SORT> arrayPointer2 = arrayPointer1;
            ArrayPointer<AMS_DRAW_SORT> arrayPointer3 = arrayPointer1 + 1;
            int num2 = (~arrayPointer2).key;
            int num3 = num1;
            while (num3 > 0)
            {
                int key = (~arrayPointer3).key;
                if (key >= num2)
                {
                    num2 = key;
                    arrayPointer2 = arrayPointer3;
                }
                --num3;
                ++arrayPointer3;
            }
            sort.Assign(arrayPointer1);
            (~arrayPointer1).Assign(~arrayPointer2);
            (~arrayPointer2).Assign(sort);
            --num1;
            ++arrayPointer1;
        }
        amDrawPushState();
        arrayPointer1 = new ArrayPointer<AMS_DRAW_SORT>(displaylistManager.sortlist);
        int num4 = sortNum;
        while (num4 > 0)
        {
            AMS_COMMAND_HEADER command = (~arrayPointer1).command;
            if (command.command_id >= 0)
                _am_draw_command_sort(command, 0U);
            else
                _am_draw_sort_system_exec[-command.command_id](command, 0U);
            --num4;
            ++arrayPointer1;
        }
        amDrawPopState();
    }

    private static void amDrawBuildShader()
    {
        mppAssertNotImpl();
    }

    private static void amDrawDisplay()
    {
        amDrawDisplay(null, 0);
    }

    private static void amDrawDisplay(AMS_RENDER_TARGET target, int index)
    {
        AMS_RENDER_TARGET amsRenderTarget = amRenderSetTarget(null);
        if (target == null)
            target = amsRenderTarget;
        if (target != null)
            amRenderSetTexture(0, target, index);
        _am_draw_offset_x = 0;
        
    }

    private static void amDrawInitDisplayList()
    {
        amDrawInitDisplayList(0);
    }

    private static void amDrawInitDisplayList(int user_header_size)
    {
        _am_draw_task = amTaskInitSystem(256, 64, 1);
        AMS_DISPLAYLIST_MANAGER displaylistManager = _am_displaylist_manager;
        displaylistManager.write_index = 0;
        displaylistManager.last_index = -1;
        displaylistManager.read_index = -1;
        AMS_DISPLAYLIST[] displaylist = displaylistManager.displaylist;
        for (int index = 0; index < 4; ++index)
        {
            displaylist[index].counter = -1;
            displaylist[index].command_buf = _am_draw_command_buf[index];
            displaylist[index].data_buf = _am_draw_data_buf[index];
            displaylist[index].command_buf_size = 0;
            displaylist[index].data_buf_size = 0;
        }
        displaylistManager.regist_num = 0;
        displaylistManager.reg_read_index = 0;
        displaylistManager.reg_end_index = 0;
        displaylistManager.reg_write_index = 0;
        displaylistManager.reg_write_num = 0;
        amDrawOpenDisplayList();
    }

    private static void amDrawExitDisplayList()
    {
        amTaskExitSystem(_am_draw_task);
    }

    private static void amDrawOpenDisplayList()
    {
        AMS_DISPLAYLIST_MANAGER displaylistManager = _am_displaylist_manager;
        AMS_DISPLAYLIST amsDisplaylist = displaylistManager.displaylist[displaylistManager.write_index];
        amsDisplaylist.command_buf_size = 1;
        amsDisplaylist.data_buf_size = 0;
        amsDisplaylist.counter = 0;
        ArrayPointer<object> commandBuf = amsDisplaylist.command_buf;
        AMS_COMMAND_BUFFER_HEADER commandBufferHeader;
        if (commandBuf[0] == null)
        {
            commandBufferHeader = new AMS_COMMAND_BUFFER_HEADER();
            commandBuf.SetPrimitive(commandBufferHeader);
        }
        else
            commandBufferHeader = (AMS_COMMAND_BUFFER_HEADER)commandBuf[0];
        displaylistManager.write_header = commandBufferHeader;
        Array.Copy(_am_system_flag, commandBufferHeader.system_flag, 4);
        Array.Copy(_am_debug_flag, commandBufferHeader.debug_flag, 4);
        commandBufferHeader.display_flag = 0;
        commandBufferHeader.icon_alpha = 0.0f;
        displaylistManager.command_buf_ptr = commandBuf + 1;
    }

    private static void amDrawCloseDisplayList()
    {
        AMS_DISPLAYLIST_MANAGER displaylistManager = _am_displaylist_manager;
        for (int index = 0; index < 4; ++index)
        {
            if (displaylistManager.displaylist[index].counter >= 0)
                ++displaylistManager.displaylist[index].counter;
        }
        displaylistManager.last_index = displaylistManager.write_index;
        displaylistManager.write_index = (displaylistManager.write_index + 1) % 4;
        if (displaylistManager.write_index == displaylistManager.read_index)
            displaylistManager.write_index = (displaylistManager.write_index + 1) % 4;
        displaylistManager.regist_num += displaylistManager.reg_write_num;
        displaylistManager.reg_end_index = displaylistManager.reg_write_index;
        displaylistManager.reg_write_num = 0;
        amDrawOpenDisplayList();
    }

    private static int amDrawGetDisplayList()
    {
        AMS_DISPLAYLIST_MANAGER displaylistManager = _am_displaylist_manager;
        int index1 = displaylistManager.last_index;
        if (displaylistManager.read_index != -1)
        {
            int index2 = displaylistManager.read_index;
            int counter1 = displaylistManager.displaylist[index2].counter;
            int num = 0;
            for (int index3 = 0; index3 < 3; ++index3)
            {
                index2 = (index2 + 1) % 4;
                int counter2 = displaylistManager.displaylist[index2].counter;
                if (counter2 > num && counter2 < counter1)
                {
                    index1 = index2;
                    num = counter2;
                }
            }
        }
        displaylistManager.read_index = index1;
        AMS_COMMAND_BUFFER_HEADER commandBufferHeader = (AMS_COMMAND_BUFFER_HEADER)~displaylistManager.displaylist[index1].command_buf;
        displaylistManager.read_header = commandBufferHeader;
        return index1;
    }

    private static void amDrawRegistCommand(uint state, int command_id)
    {
        amDrawRegistCommand(state, command_id, null);
    }

    public static void amDrawRegistCommand(uint state, int command_id, object param)
    {
        AMS_DISPLAYLIST_MANAGER displaylistManager = _am_displaylist_manager;
        ++displaylistManager.displaylist[displaylistManager.write_index].command_buf_size;
        AMS_COMMAND_HEADER amsCommandHeader = (AMS_COMMAND_HEADER)~displaylistManager.command_buf_ptr ?? new AMS_COMMAND_HEADER();
        displaylistManager.command_buf_ptr.SetPrimitive(amsCommandHeader);
        amsCommandHeader.state = state;
        amsCommandHeader.command_id = command_id;
        amsCommandHeader.param = param;
        displaylistManager.command_buf_ptr += 1;
    }

    private static int amDrawRegistCommand(int command_id)
    {
        return amDrawRegistCommand(command_id, null);
    }

    public static int amDrawRegistCommand(int command_id, object param)
    {
        AMS_DISPLAYLIST_MANAGER displaylistManager = _am_displaylist_manager;
        int regWriteIndex = displaylistManager.reg_write_index;
        AMS_REGISTLIST amsRegistlist = displaylistManager.registlist[regWriteIndex];
        amsRegistlist.command_id = command_id;
        amsRegistlist.param = param;
        displaylistManager.reg_write_index = (regWriteIndex + 1) % 256;
        ++displaylistManager.reg_write_num;
        return regWriteIndex;
    }

    public static bool amDrawIsRegistComplete(int index)
    {
        return _am_displaylist_manager.registlist[index].command_id == 0;
    }

    private static byte[] amDrawGetDataBuffer()
    {
        mppAssertNotImpl();
        return null;
    }

    public static void amDrawInitState()
    {
        _am_draw_state.drawflag = 0U;
        _am_draw_state.diffuse.mode = 3;
        _am_draw_state.diffuse.r = 1f;
        _am_draw_state.diffuse.g = 1f;
        _am_draw_state.diffuse.b = 1f;
        _am_draw_state.ambient.mode = 3;
        _am_draw_state.ambient.r = 1f;
        _am_draw_state.ambient.g = 1f;
        _am_draw_state.ambient.b = 1f;
        _am_draw_state.alpha.mode = 3;
        _am_draw_state.alpha.alpha = 1f;
        _am_draw_state.specular.mode = 3;
        _am_draw_state.specular.r = 1f;
        _am_draw_state.specular.g = 1f;
        _am_draw_state.specular.b = 1f;
        _am_draw_state.blend.mode = 0;
        _am_draw_state.envmap.texsrc = 1;
        _am_draw_state.zmode.compare = 1U;
        _am_draw_state.zmode.func = 515;
        _am_draw_state.zmode.update = 1U;
        NNS_MATRIX texmtx = _am_draw_state.envmap.texmtx;
        nnMakeUnitMatrix(texmtx);
        nnTranslateMatrix(texmtx, texmtx, 0.5f, 0.5f, 0.0f);
        nnScaleMatrix(texmtx, texmtx, 0.5f, 0.5f, 0.0f);
        for (int index = 0; index < 4; ++index)
        {
            _am_draw_state.texoffset[index].mode = 2;
            _am_draw_state.texoffset[index].u = 0.0f;
            _am_draw_state.texoffset[index].v = 0.0f;
        }
        amDrawSetState(_am_draw_state);
    }

    private static void amDrawPushState()
    {
        _am_draw_state_stack[_am_draw_state_stack_num++].Assign(_am_draw_state);
    }

    private static void amDrawPopState()
    {
        --_am_draw_state_stack_num;
        amDrawSetState(_am_draw_state_stack[_am_draw_state_stack_num]);
    }

    private static void amDrawSetState(AMS_DRAWSTATE state)
    {
        _am_draw_state.Assign(state);
        nnSetMaterialControlDiffuse(state.diffuse.mode, state.diffuse.r, state.diffuse.g, state.diffuse.b);
        nnSetMaterialControlAmbient(state.ambient.mode, state.ambient.r, state.ambient.g, state.ambient.b);
        nnSetMaterialControlAlpha(state.alpha.mode, state.alpha.alpha);
        nnSetMaterialControlBlendMode(state.blend.mode);
        nnSetMaterialControlEnvTexMatrix(state.envmap.texsrc, _am_draw_state.envmap.texmtx);
        for (int slot = 0; slot < 4; ++slot)
            nnSetMaterialControlTextureOffset(slot, state.texoffset[slot].mode, state.texoffset[slot].u, state.texoffset[slot].v);
        nnSetFogSwitch(state.fog.flag != 0);
        nnSetFogColor(state.fog_color.r, state.fog_color.g, state.fog_color.b);
        nnSetFogRange(state.fog_range.fnear, state.fog_range.ffar);
    }

    private static AMS_DRAWSTATE amDrawGetState(AMS_DRAWSTATE state)
    {
        state?.Assign(_am_draw_state);
        return _am_draw_state;
    }

    private static AMS_DRAWSTATE amDrawGetState()
    {
        return _am_draw_state;
    }

    public static void amDrawSetProjection(NNS_MATRIX proj_mtx, int proj_type)
    {
        _am_draw_proj_mtx.Assign(proj_mtx);
        _am_draw_proj_type = proj_type;
        nnSetProjection(proj_mtx, proj_type);
    }

    private static NNS_MATRIX amDrawGetProjectionMatrix()
    {
        return _am_draw_proj_mtx;
    }

    private static int amDrawGetProjectionType()
    {
        return _am_draw_proj_type;
    }

    private static void amDrawExecute()
    {
        amTaskExecute(_am_draw_task);
        amTaskReset(_am_draw_task);
    }

    private static void amDrawExecCommand(uint state)
    {
        amDrawExecCommand(state, 0U);
    }

    public static void amDrawExecCommand(uint state, uint drawflag)
    {
        AMS_DISPLAYLIST_MANAGER displaylistManager = _am_displaylist_manager;
        AMS_DISPLAYLIST amsDisplaylist = displaylistManager.displaylist[displaylistManager.read_index];
        object[] array = amsDisplaylist.command_buf.array;
        AMS_COMMAND_BUFFER_HEADER commandBufferHeader = (AMS_COMMAND_BUFFER_HEADER)array[0];
        int num = amsDisplaylist.command_buf_size - 1;
        for (int index = 0; index < num; ++index)
        {
            AMS_COMMAND_HEADER ch = (AMS_COMMAND_HEADER)array[index + 1];
            if ((int)ch.state == (int)state)
            {
                if (ch.command_id >= 0)
                    _am_draw_command_func(ch, drawflag);
                else
                    _am_draw_system_exec[-ch.command_id](ch, drawflag);
            }
        }
    }

    private static void amDrawExecRegist()
    {
        AMS_DISPLAYLIST_MANAGER displaylistManager = _am_displaylist_manager;
        int registNum = displaylistManager.regist_num;
        int index = displaylistManager.reg_read_index;
        int regEndIndex = displaylistManager.reg_end_index;
        if (registNum == 0)
            return;
        int num = 0;
        while (index != regEndIndex)
        {
            AMS_REGISTLIST l = displaylistManager.registlist[index];
            index = (index + 1) % 256;
            ++num;
            _am_draw_regist_func[l.command_id](l);
        }
        displaylistManager.regist_num -= num;
        displaylistManager.reg_read_index = index;
    }

    private static void amDrawAddSort(AMS_COMMAND_HEADER command, int key)
    {
        AMS_DISPLAYLIST_MANAGER displaylistManager = _am_displaylist_manager;
        if (displaylistManager.sort_num >= 512)
        {
            amSystemLog("[WARN] sort_num over.\n");
        }
        else
        {
            AMS_DRAW_SORT amsDrawSort = displaylistManager.sortlist[displaylistManager.sort_num++];
            amsDrawSort.key = key;
            amsDrawSort.command = command;
        }
    }

    private static object _amDrawConvVertex2D(NNS_PRIM2D_P vs, int count)
    {
        mppAssertNotImpl();
        return null;
    }

    private static object _amDrawConvVertex2D(NNS_PRIM2D_PC[] vs, int count)
    {
        NNS_PRIM2D_PC[] nnsPriM2DPcArray = New<NNS_PRIM2D_PC>(count);
        for (int index = 0; index < count; ++index)
        {
            amDrawConv2D(nnsPriM2DPcArray[index].Pos, vs[index].Pos);
            nnsPriM2DPcArray[index].Col = vs[index].Col;
        }
        return nnsPriM2DPcArray;
    }

    private static void amDrawConv2D(NNS_VECTOR2D vd, NNS_VECTOR2D vs)
    {
        vd.Assign(vs);
    }

    private static object _amDrawConvVertex2D(NNS_PRIM2D_PCT vs, int count)
    {
        mppAssertNotImpl();
        return null;
    }

    private static void amDrawPrimitive2D(int format, int type, object vtx, int count, float pri)
    {
        switch (format)
        {
            case 1:
                vtx = _amDrawConvVertex2D((NNS_PRIM2D_PC[])vtx, count);
                break;
            case 2:
                vtx = _amDrawConvVertex2D((NNS_PRIM2D_PCT)vtx, count);
                break;
        }
        nnDrawPrimitive2D(type, vtx, count, pri);
    }

    private static void amDrawPrimitiveLine2D(
      NNE_PRIM_LINE type,
      object vtx,
      int count,
      float pri)
    {
        mppAssertNotImpl();
    }

    private static void amDrawPrimitivePoint2D(int format, object vtx, int count, float pri)
    {
        mppAssertNotImpl();
    }

    public static void amDrawMakeTask(TaskProc proc, ushort prio)
    {
        amDrawMakeTask(proc, prio, null);
    }

    public static void amDrawMakeTask(TaskProc proc, ushort prio, object data)
    {
        AMS_PARAM_MAKE_TASK amsParamMakeTask = amDraw_AMS_PARAM_MAKE_TASK_Pool.Alloc();
        amsParamMakeTask.prio = prio;
        amsParamMakeTask.proc = proc;
        amsParamMakeTask.work_data = data;
        amDrawRegistCommand(16777216U, -1, amsParamMakeTask);
    }

    public static void amDrawSetWorldViewMatrix(NNS_MATRIX mtx)
    {
        if (mtx == null)
            mtx = amMatrixGetCurrent();
        nnCopyMatrix(_am_draw_world_view_matrix, mtx);
    }

    private static void amDrawMakeTask(TaskProc proc, ushort prio, uint data)
    {
        amDrawRegistCommand(16777216U, -1, new AMS_PARAM_MAKE_TASK()
        {
            prio = prio,
            proc = proc,
            work_data = data
        });
    }

    private static void amDrawPrintColor(uint rgba)
    {
        mppAssertNotImpl();
    }

    private static void amDrawObject(
      uint state,
      NNS_OBJECT obj,
      NNS_TEXLIST texlist,
      uint drawflag,
      NNS_MATERIALCALLBACK_FUNC func)
    {
        mppAssertNotImpl();
    }

    private static void amDrawObjectMaterialMotion(
      uint state,
      NNS_OBJECT _object,
      NNS_TEXLIST texlist,
      uint drawflag,
      NNS_MATERIALCALLBACK_FUNC func)
    {
        mppAssertNotImpl();
    }

    public static void amDrawObjectSetMaterial(
      uint state,
      NNS_OBJECT _object,
      NNS_TEXLIST texlist,
      NNS_VECTOR scale,
      ref NNS_RGBA color,
      float u,
      float v,
      int blend,
      uint drawflag)
    {
        amDrawObjectSetMaterial(state, _object, texlist, scale, ref color, u, v, blend, drawflag, null);
    }

    public static void amDrawObjectSetMaterial(
      uint state,
      NNS_OBJECT _object,
      NNS_TEXLIST texlist,
      NNS_VECTOR scale,
      ref NNS_RGBA color,
      float u,
      float v,
      int blend,
      uint drawflag,
      NNS_MATERIALCALLBACK_FUNC func)
    {
        AMS_PARAM_DRAW_OBJECT_MATERIAL drawObjectMaterial = amDrawAlloc_AMS_PARAM_DRAW_OBJECT_MATERIAL();
        NNS_MATRIX dst = amDrawAlloc_NNS_MATRIX();
        nnCopyMatrix(dst, amMatrixGetCurrent());
        drawObjectMaterial._object = _object;
        drawObjectMaterial.mtx = dst;
        drawObjectMaterial.sub_obj_type = 0U;
        drawObjectMaterial.flag = drawflag;
        drawObjectMaterial.texlist = texlist;
        drawObjectMaterial.scaleZ = -scale.z;
        nnCopyVector(drawObjectMaterial.scale, scale);
        drawObjectMaterial.color = color;
        drawObjectMaterial.scroll_u = u;
        drawObjectMaterial.scroll_v = v;
        drawObjectMaterial.blend = blend;
        drawObjectMaterial.material_func = func;
        amDrawRegistCommand(state, -8, drawObjectMaterial);
    }

    private static void amDrawMotion(
      uint state,
      NNS_MOTION motion,
      float frame,
      NNS_OBJECT _object,
      NNS_TEXLIST texlist,
      uint drawflag,
      NNS_MATERIALCALLBACK_FUNC func)
    {
        mppAssertNotImpl();
    }

    private static void amDrawMotionMaterialMotion(
      uint state,
      NNS_MOTION motion,
      float frame,
      NNS_OBJECT _object,
      NNS_TEXLIST texlist,
      uint drawflag,
      NNS_MATERIALCALLBACK_FUNC func)
    {
        mppAssertNotImpl();
    }

    public static void amDrawPrimitive3D(uint state, AMS_PARAM_DRAW_PRIMITIVE setParam)
    {
        AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive = amDrawAlloc_AMS_PARAM_DRAW_PRIMITIVE();
        paramDrawPrimitive.Assign(setParam);
        if (paramDrawPrimitive.mtx == null)
        {
            paramDrawPrimitive.mtx = amDraw_NNS_MATRIX_Pool.Alloc();
            paramDrawPrimitive.mtx.Assign(amMatrixGetCurrent());
        }
        amDrawRegistCommand(state, -14, paramDrawPrimitive);
    }

    private static void amDrawPrim2D(uint state, AMS_PARAM_DRAW_PRIMITIVE setParam)
    {
        AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive = GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        paramDrawPrimitive.Assign(setParam);
        NNS_MATRIX dst = GlobalPool<NNS_MATRIX>.Alloc();
        nnCopyMatrix(dst, amMatrixGetCurrent());
        paramDrawPrimitive.mtx = dst;
        amDrawRegistCommand(state, -13, paramDrawPrimitive);
    }

    private static void amDrawGetPrimBlendParam(int type, AMS_PARAM_DRAW_PRIMITIVE setParam)
    {
        switch (type)
        {
            case 0:
                setParam.bldSrc = 770;
                setParam.bldDst = 771;
                setParam.bldMode = 32774;
                break;
            case 1:
                setParam.bldSrc = 770;
                setParam.bldDst = 1;
                setParam.bldMode = 32774;
                break;
            case 2:
                setParam.bldSrc = 770;
                setParam.bldDst = 1;
                setParam.bldMode = 32779;
                break;
        }
    }

    private static void amDrawSetMaterialDiffuse(uint state, int mode, float r, float g, float b)
    {
        mppAssertNotImpl();
    }

    private static void amDrawSetMaterialAmbient(uint state, int mode, float r, float g, float b)
    {
        mppAssertNotImpl();
    }

    private static void amDrawSetMaterialAlpha(uint state, int mode, float alpha)
    {
        mppAssertNotImpl();
    }

    private static void amDrawSetMaterialSpecular(uint state, int mode, float r, float g, float b)
    {
        mppAssertNotImpl();
    }

    private static void amDrawSetMaterialBlendMode(uint state, int mode)
    {
        mppAssertNotImpl();
    }

    private static void amDrawSetMaterialTexOffset(
      uint state,
      int slot,
      int mode,
      float u,
      float v)
    {
        mppAssertNotImpl();
    }

    public static void amDrawSetFog(uint state, int flag)
    {
        AMS_DRAWSTATE_FOG amsDrawstateFog = amDrawAlloc_AMS_DRAWSTATE_FOG();
        amsDrawstateFog.flag = flag & 1;
        amDrawRegistCommand(state, -22, amsDrawstateFog);
    }

    public static void amDrawSetFogColor(uint state, float r, float g, float b)
    {
        AMS_DRAWSTATE_FOG_COLOR drawstateFogColor = amDrawAlloc_AMS_DRAWSTATE_FOG_COLOR();
        drawstateFogColor.r = r;
        drawstateFogColor.g = g;
        drawstateFogColor.b = b;
        amDrawRegistCommand(state, -23, drawstateFogColor);
    }

    public static void amDrawSetFogRange(uint state, float fnear, float ffar)
    {
        AMS_DRAWSTATE_FOG_RANGE drawstateFogRange = amDrawAlloc_AMS_DRAWSTATE_FOG_RANGE();
        drawstateFogRange.fnear = fnear;
        drawstateFogRange.ffar = ffar;
        amDrawRegistCommand(state, -24, drawstateFogRange);
    }

    private static void amDrawSetZMode(uint state, bool compare, int func, bool update)
    {
        mppAssertNotImpl();
    }

    private static void amDrawObject(
      NNS_OBJECT _object,
      NNS_TEXLIST texlist,
      uint drawflag,
      NNS_MATERIALCALLBACK_FUNC func)
    {
        mppAssertNotImpl();
    }

    private static void amDrawObjectMaterialMotion(
      NNS_OBJECT _object,
      NNS_TEXLIST texlist,
      uint drawflag,
      NNS_MATERIALCALLBACK_FUNC func)
    {
        mppAssertNotImpl();
    }

    private static void amDrawMotion(
      NNS_MOTION motion,
      float frame,
      NNS_OBJECT _object,
      NNS_TEXLIST texlist,
      uint drawflag,
      NNS_MATERIALCALLBACK_FUNC func)
    {
        mppAssertNotImpl();
    }

    private static void amDrawMotionMaterialMotion(
      NNS_MOTION motion,
      float frame,
      NNS_OBJECT _object,
      NNS_TEXLIST texlist,
      uint drawflag,
      NNS_MATERIALCALLBACK_FUNC func)
    {
        AMS_COMMAND_HEADER command = new AMS_COMMAND_HEADER();
        AMS_PARAM_DRAW_MOTION amsParamDrawMotion = new AMS_PARAM_DRAW_MOTION();
        command.command_id = -10;
        command.param = amsParamDrawMotion;
        amsParamDrawMotion._object = _object;
        amsParamDrawMotion.mtx = null;
        amsParamDrawMotion.sub_obj_type = 0U;
        amsParamDrawMotion.flag = drawflag;
        amsParamDrawMotion.texlist = texlist;
        amsParamDrawMotion.motion = motion;
        amsParamDrawMotion.frame = frame;
        amsParamDrawMotion.material_func = func;
        _amDrawMotion(command, drawflag);
    }

    private static void amDrawSetMaterialDiffuse(int mode, float r, float g, float b)
    {
        if (mode != -1)
        {
            nnSetMaterialControlDiffuse(mode, r, g, b);
            _am_draw_state.drawflag |= 1048576U;
        }
        else
            _am_draw_state.drawflag &= 4293918719U;
        _am_draw_state.diffuse.mode = mode;
        _am_draw_state.diffuse.r = r;
        _am_draw_state.diffuse.g = g;
        _am_draw_state.diffuse.b = b;
    }

    private static void amDrawSetMaterialAmbient(int mode, float r, float g, float b)
    {
        if (mode != -1)
        {
            nnSetMaterialControlAmbient(mode, r, g, b);
            _am_draw_state.drawflag |= 2097152U;
        }
        else
            _am_draw_state.drawflag &= 4292870143U;
        _am_draw_state.ambient.mode = mode;
        _am_draw_state.ambient.r = r;
        _am_draw_state.ambient.g = g;
        _am_draw_state.ambient.b = b;
    }

    private static void amDrawSetMaterialAlpha(int mode, float alpha)
    {
        if (mode != -1)
        {
            nnSetMaterialControlAlpha(mode, alpha);
            _am_draw_state.drawflag |= 8388608U;
        }
        else
            _am_draw_state.drawflag &= 4286578687U;
        _am_draw_state.alpha.mode = mode;
        _am_draw_state.alpha.alpha = alpha;
    }

    private static void amDrawSetMaterialSpecular(int mode, float r, float g, float b)
    {
        if (mode != -1)
            _am_draw_state.drawflag |= 4194304U;
        else
            _am_draw_state.drawflag &= 4290772991U;
        _am_draw_state.specular.mode = mode;
        _am_draw_state.specular.r = r;
        _am_draw_state.specular.g = g;
        _am_draw_state.specular.b = b;
    }

    private static void amDrawSetMaterialEnvMap(uint state, int texsrc, NNS_MATRIX texmtx)
    {
        mppAssertNotImpl();
    }

    private static void amDrawSetMaterialBlendMode(int mode)
    {
        if (mode != -1)
        {
            nnSetMaterialControlBlendMode(mode);
            _am_draw_state.drawflag |= 33554432U;
        }
        else
            _am_draw_state.drawflag &= 4261412863U;
        _am_draw_state.blend.mode = mode;
    }

    private static void amDrawSetMaterialTexOffset(int slot, int mode, float u, float v)
    {
        if (slot != -1)
        {
            if (mode != -1)
            {
                nnSetMaterialControlTextureOffset(slot, mode, u, v);
                _am_draw_state.drawflag |= 268435456U;
            }
            else
                _am_draw_state.drawflag &= 4026531839U;
            ArrayPointer<AMS_DRAWSTATE_TEXOFFSET> arrayPointer = new ArrayPointer<AMS_DRAWSTATE_TEXOFFSET>(_am_draw_state.texoffset, slot);
            (~arrayPointer).mode = mode;
            (~arrayPointer).u = u;
            (~arrayPointer).v = v;
        }
        else
        {
            ArrayPointer<AMS_DRAWSTATE_TEXOFFSET> arrayPointer = new ArrayPointer<AMS_DRAWSTATE_TEXOFFSET>(_am_draw_state.texoffset, 0);
            if (mode != -1)
            {
                int slot1 = 0;
                while (slot1 < 4)
                {
                    nnSetMaterialControlTextureOffset(slot1, mode, u, v);
                    _am_draw_state.drawflag |= 268435456U;
                    (~arrayPointer).mode = mode;
                    (~arrayPointer).u = u;
                    (~arrayPointer).v = v;
                    ++slot1;
                    ++arrayPointer;
                }
            }
            else
            {
                int num = 0;
                while (num < 4)
                {
                    _am_draw_state.drawflag &= 4026531839U;
                    (~arrayPointer).mode = mode;
                    (~arrayPointer).u = u;
                    (~arrayPointer).v = v;
                    ++num;
                    ++arrayPointer;
                }
            }
        }
    }

    private static void amDrawSetFog(int flag)
    {
        flag &= 1;
        nnSetFogSwitch(flag != 0);
        _am_draw_state.fog.flag = flag;
    }

    private static void amDrawSetFogColor(float r, float g, float b)
    {
        nnSetFogColor(r, g, b);
        _am_draw_state.fog_color.r = r;
        _am_draw_state.fog_color.g = g;
        _am_draw_state.fog_color.b = b;
    }

    private static void amDrawSetFogRange(float fnear, float ffar)
    {
        nnSetFogRange(fnear, ffar);
        _am_draw_state.fog_range.fnear = fnear;
        _am_draw_state.fog_range.ffar = ffar;
    }

    private static void amDrawSetZMode(bool compare, int func, bool update)
    {
        mppAssertNotImpl();
        _am_draw_state.zmode.compare = compare ? 1U : 0U;
        _am_draw_state.zmode.func = func;
        _am_draw_state.zmode.update = update ? 1U : 0U;
    }

    private static void _amDrawTaskMake(AMS_COMMAND_HEADER command, uint drawflag)
    {
        AMS_PARAM_MAKE_TASK amsParamMakeTask = (AMS_PARAM_MAKE_TASK)command.param;
        amTaskMake(_am_draw_task, amsParamMakeTask.proc, null, (uint)amsParamMakeTask.prio, 0U, 0U, "DRAW").work = amsParamMakeTask.work_data;
    }

    private static void _amDrawPrintf(AMS_COMMAND_HEADER command, uint drawflag)
    {
        mppAssertNotImpl();
    }

    private static void _amDrawPrintColor(AMS_COMMAND_HEADER command, uint drawflag)
    {
        mppAssertNotImpl();
    }

    private static void _amDrawHeapMap(AMS_COMMAND_HEADER command, uint drawflag)
    {
        mppAssertNotImpl();
    }

    private static void _amDrawThreadMap(AMS_COMMAND_HEADER command, uint drawflag)
    {
        mppAssertNotImpl();
    }

    private static void _amDrawObject(AMS_COMMAND_HEADER command, uint drawflag)
    {
        NNS_MATRIX nnsMatrix = amDrawAlloc_NNS_MATRIX();
        amMatrixPush();
        AMS_PARAM_DRAW_OBJECT amsParamDrawObject = (AMS_PARAM_DRAW_OBJECT)command.param;
        int nNode = amsParamDrawObject._object.nNode;
        if (__amDrawObject.plt_mtx == null || __amDrawObject.plt_mtx.Length < nNode)
        {
            __amDrawObject.nstat = new uint[nNode];
            __amDrawObject.plt_mtx = new NNS_MATRIX[nNode];
            for (int index = 0; index < nNode; ++index)
                __amDrawObject.plt_mtx[index] = new NNS_MATRIX();
        }
        NNS_MATRIX[] pltMtx = __amDrawObject.plt_mtx;
        uint[] nstat = __amDrawObject.nstat;
        if (amsParamDrawObject.mtx != null)
        {
            nnMultiplyMatrix(nnsMatrix, amMatrixGetCurrent(), amsParamDrawObject.mtx);
            nnMultiplyMatrix(nnsMatrix, _am_draw_world_view_matrix, nnsMatrix);
        }
        else
            nnMultiplyMatrix(nnsMatrix, _am_draw_world_view_matrix, amMatrixGetCurrent());
        nnSetUpNodeStatusList(nstat, nNode, 0U);
        nnCalcMatrixPalette(pltMtx, nstat, amsParamDrawObject._object, nnsMatrix, _am_default_stack, 1U);
        if (amsParamDrawObject.texlist != null)
            nnSetTextureList(amsParamDrawObject.texlist);
        nnSetMaterialCallback(amsParamDrawObject.material_func);
        if (command.command_id == -6)
            nnDrawObject(amsParamDrawObject._object, pltMtx, nstat, (uint)((int)amsParamDrawObject.sub_obj_type | 256 | 512 | 7), amsParamDrawObject.flag | drawflag | _am_draw_state.drawflag, 0U);
        else
            nnDrawMaterialMotionObject(amsParamDrawObject._object, pltMtx, nstat, (uint)((int)amsParamDrawObject.sub_obj_type | 256 | 512 | 7), amsParamDrawObject.flag | drawflag | _am_draw_state.drawflag);
        if (amsParamDrawObject.material_func != null)
            nnSetMaterialCallback(null);
        amMatrixPop();
    }

    private static void _amDrawObjectSetMaterial(AMS_COMMAND_HEADER command, uint drawflag)
    {
        AMS_PARAM_DRAW_OBJECT_MATERIAL drawObjectMaterial = (AMS_PARAM_DRAW_OBJECT_MATERIAL)command.param;
        amDrawPushState();
        if (((int)drawObjectMaterial.flag & 3145728) != 0)
        {
            amDrawSetMaterialDiffuse(3, drawObjectMaterial.color.r, drawObjectMaterial.color.g, drawObjectMaterial.color.b);
            amDrawSetMaterialAmbient(3, drawObjectMaterial.color.r, drawObjectMaterial.color.g, drawObjectMaterial.color.b);
        }
        if (((int)drawObjectMaterial.flag & 8388608) != 0)
            amDrawSetMaterialAlpha(3, drawObjectMaterial.color.a);
        if (((int)drawObjectMaterial.flag & 268435456) != 0)
            amDrawSetMaterialTexOffset(0, 1, drawObjectMaterial.scroll_u, drawObjectMaterial.scroll_v);
        command.command_id = -6;
        _amDrawObject(command, drawflag);
        amDrawPopState();
    }

    private static void _amDrawMotion(AMS_COMMAND_HEADER command, uint drawflag)
    {
        mppAssertNotImpl();
    }

    private static void _amDrawMotionTRS(AMS_COMMAND_HEADER command, uint drawflag)
    {
        mppAssertNotImpl();
    }

    private static void _amDrawPrimitive2D(AMS_COMMAND_HEADER command, uint drawflag)
    {
        AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive1 = (AMS_PARAM_DRAW_PRIMITIVE)command.param;
        if (paramDrawPrimitive1.texlist != null)
        {
            nnSetPrimitiveTexNum(paramDrawPrimitive1.texlist, paramDrawPrimitive1.texId);
            nnSetPrimitiveTexState(0, 0, paramDrawPrimitive1.uwrap, paramDrawPrimitive1.vwrap);
        }
        if (paramDrawPrimitive1.noSort != 0 || paramDrawPrimitive1.ablend == 0)
        {
            _amDrawSetPrimitive2DParam(command, drawflag);
            nnBeginDrawPrimitive2D(paramDrawPrimitive1.format2D, paramDrawPrimitive1.ablend);
            switch (paramDrawPrimitive1.format2D)
            {
                case 1:
                    amDrawPrimitive2D(paramDrawPrimitive1.format2D, paramDrawPrimitive1.type, paramDrawPrimitive1.vtxPC2D, paramDrawPrimitive1.count, paramDrawPrimitive1.zOffset);
                    break;
                case 2:
                    amDrawPrimitive2D(paramDrawPrimitive1.format2D, paramDrawPrimitive1.type, paramDrawPrimitive1.vtxPCT2D, paramDrawPrimitive1.count, paramDrawPrimitive1.zOffset);
                    break;
            }
            nnEndDrawPrimitive2D();
        }
        else
        {
            AMS_COMMAND_HEADER command1 = new AMS_COMMAND_HEADER(command);
            command1.command_id = -3;
            AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive2 = GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
            NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
            nnsMatrix.Assign(amMatrixGetCurrent());
            paramDrawPrimitive2.Assign((AMS_PARAM_DRAW_PRIMITIVE)command.param);
            paramDrawPrimitive2.mtx = nnsMatrix;
            command1.param = paramDrawPrimitive2;
            amDrawAddSort(command1, (int)(paramDrawPrimitive2.zOffset * 100.0));
        }
    }

    private static void _amDrawPrimitive3D(AMS_COMMAND_HEADER command, uint drawflag)
    {
        AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive1 = (AMS_PARAM_DRAW_PRIMITIVE)command.param;
        if (paramDrawPrimitive1.texlist != null && paramDrawPrimitive1.texId != -1)
        {
            nnSetPrimitiveTexNum(paramDrawPrimitive1.texlist, paramDrawPrimitive1.texId);
            nnSetPrimitiveTexState(0, 0, paramDrawPrimitive1.uwrap, paramDrawPrimitive1.vwrap);
        }
        if (paramDrawPrimitive1.noSort != 0 || paramDrawPrimitive1.ablend == 0)
        {
            _amDrawSetPrimitive3DParam(command, drawflag);
            nnBeginDrawPrimitive3D(paramDrawPrimitive1.format3D, paramDrawPrimitive1.ablend, 0, 0);
            switch (paramDrawPrimitive1.format3D)
            {
                case 2:
                    nnDrawPrimitive3D(paramDrawPrimitive1.type, paramDrawPrimitive1.vtxPC3D, paramDrawPrimitive1.count);
                    break;
                case 4:
                    nnDrawPrimitive3D(paramDrawPrimitive1.type, paramDrawPrimitive1.vtxPCT3D, paramDrawPrimitive1.count);
                    break;
            }
            nnEndDrawPrimitive3D();
        }
        else
        {
            AMS_COMMAND_HEADER command1 = command;
            command1.command_id = -4;
            AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive2 = amDrawAlloc_AMS_PARAM_DRAW_PRIMITIVE();
            NNS_MATRIX dst = amDrawAlloc_NNS_MATRIX();
            nnCopyMatrix(dst, amMatrixGetCurrent());
            paramDrawPrimitive2.Assign((AMS_PARAM_DRAW_PRIMITIVE)command.param);
            paramDrawPrimitive2.mtx = dst;
            command1.param = paramDrawPrimitive2;
            amDrawAddSort(command1, (int)(paramDrawPrimitive2.sortZ * 100.0));
        }
    }

    private static void _amDrawSortObject(AMS_COMMAND_HEADER command, uint drawflag)
    {
        mppAssertNotImpl();
    }

    private static NNS_MATRIX amDrawGetWorldViewMatrix()
    {
        return _am_draw_world_view_matrix;
    }

    private static void _amDrawSortPrimitive3D(AMS_COMMAND_HEADER command, uint drawflag)
    {
        NNS_MATRIX primitive3DBaseMtx = _amDrawSortPrimitive3D_base_mtx;
        AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive = (AMS_PARAM_DRAW_PRIMITIVE)command.param;
        if (paramDrawPrimitive.texlist != null && paramDrawPrimitive.texId != -1)
        {
            nnSetPrimitiveTexNum(paramDrawPrimitive.texlist, paramDrawPrimitive.texId);
            nnSetPrimitiveTexState(0, 0, 0, 0);
        }
        nnCopyMatrix(primitive3DBaseMtx, paramDrawPrimitive.mtx);
        nnMultiplyMatrix(primitive3DBaseMtx, amDrawGetWorldViewMatrix(), primitive3DBaseMtx);
        nnSetPrimitive3DMatrix(primitive3DBaseMtx);
        _amDrawSetPrimitive3DParam(command, drawflag);
        nnBeginDrawPrimitive3D(paramDrawPrimitive.format3D, paramDrawPrimitive.ablend, 0, 0);
        switch (paramDrawPrimitive.format3D)
        {
            case 2:
                nnDrawPrimitive3D(paramDrawPrimitive.type, paramDrawPrimitive.vtxPC3D, paramDrawPrimitive.count);
                break;
            case 4:
                nnDrawPrimitive3D(paramDrawPrimitive.type, paramDrawPrimitive.vtxPCT3D, paramDrawPrimitive.count);
                break;
        }
        nnEndDrawPrimitive3D();
    }

    private static void _amDrawSortPrimitive2D(AMS_COMMAND_HEADER command, uint drawflag)
    {
        AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive = (AMS_PARAM_DRAW_PRIMITIVE)command.param;
        NNS_MATRIX sortPrimitive2DMtx = _amDrawSortPrimitive2D_mtx;
        nnMakeOrthoMatrix(sortPrimitive2DMtx, 0.0f, 720f, 1080f, 0.0f, 1f, 3f);
        amDrawSetProjection(sortPrimitive2DMtx, 1);
        if (paramDrawPrimitive.texlist != null && paramDrawPrimitive.texId != -1)
        {
            nnSetPrimitiveTexNum(paramDrawPrimitive.texlist, paramDrawPrimitive.texId);
            nnSetPrimitiveTexState(0, 0, 0, 0);
        }
        _amDrawSetPrimitive2DParam(command, drawflag);
        nnBeginDrawPrimitive2D(paramDrawPrimitive.format2D, paramDrawPrimitive.ablend);
        switch (paramDrawPrimitive.format2D)
        {
            case 1:
                nnDrawPrimitive2D(paramDrawPrimitive.type, paramDrawPrimitive.vtxPC2D, paramDrawPrimitive.count, paramDrawPrimitive.zOffset);
                break;
            case 2:
                nnDrawPrimitive2D(paramDrawPrimitive.type, paramDrawPrimitive.vtxPCT2D, paramDrawPrimitive.count, paramDrawPrimitive.zOffset);
                break;
        }
        nnEndDrawPrimitive2D();
    }

    private static void _amDrawSetDiffuse(AMS_COMMAND_HEADER command, uint drawflag)
    {
        AMS_DRAWSTATE_DIFFUSE drawstateDiffuse = (AMS_DRAWSTATE_DIFFUSE)command.param;
        amDrawSetMaterialDiffuse(drawstateDiffuse.mode, drawstateDiffuse.r, drawstateDiffuse.g, drawstateDiffuse.b);
    }

    private static void _amDrawSetAmbient(AMS_COMMAND_HEADER command, uint drawflag)
    {
        AMS_DRAWSTATE_AMBIENT drawstateAmbient = (AMS_DRAWSTATE_AMBIENT)command.param;
        amDrawSetMaterialAmbient(drawstateAmbient.mode, drawstateAmbient.r, drawstateAmbient.g, drawstateAmbient.b);
    }

    private static void _amDrawSetAlpha(AMS_COMMAND_HEADER command, uint drawflag)
    {
        AMS_DRAWSTATE_ALPHA amsDrawstateAlpha = (AMS_DRAWSTATE_ALPHA)command.param;
        amDrawSetMaterialAlpha(amsDrawstateAlpha.mode, amsDrawstateAlpha.alpha);
    }

    private static void _amDrawSetSpecular(AMS_COMMAND_HEADER command, uint drawflag)
    {
        AMS_DRAWSTATE_SPECULAR drawstateSpecular = (AMS_DRAWSTATE_SPECULAR)command.param;
        amDrawSetMaterialSpecular(drawstateSpecular.mode, drawstateSpecular.r, drawstateSpecular.g, drawstateSpecular.b);
    }

    private static void _amDrawSetEnvMap(AMS_COMMAND_HEADER command, uint drawflag)
    {
        mppAssertNotImpl();
    }

    private static void _amDrawSetBlend(AMS_COMMAND_HEADER command, uint drawflag)
    {
        amDrawSetMaterialBlendMode(((AMS_DRAWSTATE_BLEND)command.param).mode);
    }

    private static void _amDrawSetTexOffset(AMS_COMMAND_HEADER command, uint drawflag)
    {
        AMS_PARAM_SET_TEXOFFSET paramSetTexoffset = (AMS_PARAM_SET_TEXOFFSET)command.param;
        amDrawSetMaterialTexOffset(paramSetTexoffset.slot, paramSetTexoffset.texoffset.mode, paramSetTexoffset.texoffset.u, paramSetTexoffset.texoffset.v);
    }

    private static void _amDrawSetFog(AMS_COMMAND_HEADER command, uint drawflag)
    {
        amDrawSetFog(((AMS_DRAWSTATE_FOG)command.param).flag);
    }

    private static void _amDrawSetFogColor(AMS_COMMAND_HEADER command, uint drawflag)
    {
        AMS_DRAWSTATE_FOG_COLOR drawstateFogColor = (AMS_DRAWSTATE_FOG_COLOR)command.param;
        amDrawSetFogColor(drawstateFogColor.r, drawstateFogColor.g, drawstateFogColor.b);
    }

    private static void _amDrawSetFogRange(AMS_COMMAND_HEADER command, uint drawflag)
    {
        AMS_DRAWSTATE_FOG_RANGE drawstateFogRange = (AMS_DRAWSTATE_FOG_RANGE)command.param;
        amDrawSetFogRange(drawstateFogRange.fnear, drawstateFogRange.ffar);
    }

    private static void _amDrawSetZMode(AMS_COMMAND_HEADER command, uint drawflag)
    {
        AMS_DRAWSTATE_Z_MODE amsDrawstateZMode = (AMS_DRAWSTATE_Z_MODE)command.param;
        amDrawSetZMode(0U != amsDrawstateZMode.compare, amsDrawstateZMode.func, 0U != amsDrawstateZMode.update);
    }

    private static void _amDrawSetPrimitive3DParam(AMS_COMMAND_HEADER command, uint drawflag)
    {
        AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive = (AMS_PARAM_DRAW_PRIMITIVE)command.param;
        if (paramDrawPrimitive.aTest != 0)
            nnSetPrimitive3DAlphaFuncGL(516U, 0.5f);
        else
            nnSetPrimitive3DAlphaFuncGL(519U, 0.5f);
        if (paramDrawPrimitive.zMask != 0)
            nnSetPrimitive3DDepthMaskGL(false);
        else
            nnSetPrimitive3DDepthMaskGL(true);
        if (paramDrawPrimitive.zTest != 0)
            nnSetPrimitive3DDepthFuncGL(515U);
        else
            nnSetPrimitive3DDepthFuncGL(519U);
        if (paramDrawPrimitive.ablend == 0 || paramDrawPrimitive.bldMode != 32774)
            return;
        switch ((uint)paramDrawPrimitive.bldDst)
        {
            case 1:
                nnSetPrimitiveBlend(0);
                break;
            case 771:
                nnSetPrimitiveBlend(1);
                break;
            default:
                nnSetPrimitiveBlend(1);
                break;
        }
    }

    private static void _amDrawSetPrimitive2DParam(AMS_COMMAND_HEADER command, uint drawflag)
    {
        AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive = (AMS_PARAM_DRAW_PRIMITIVE)command.param;
        if (paramDrawPrimitive.aTest != 0)
            nnSetPrimitive2DAlphaFuncGL(516U, 0.5f);
        else
            nnSetPrimitive2DAlphaFuncGL(519U, 0.5f);
        if (paramDrawPrimitive.zMask != 0)
            nnSetPrimitive3DDepthMaskGL(false);
        else
            nnSetPrimitive3DDepthMaskGL(true);
        if (paramDrawPrimitive.zTest != 0)
            nnSetPrimitive3DDepthFuncGL(515U);
        else
            nnSetPrimitive3DDepthFuncGL(519U);
        if (paramDrawPrimitive.ablend == 0 || paramDrawPrimitive.bldMode != 32774)
            return;
        switch (paramDrawPrimitive.bldDst)
        {
            case 1:
                nnSetPrimitiveBlend(0);
                break;
            case 771:
                nnSetPrimitiveBlend(1);
                break;
            default:
                nnSetPrimitiveBlend(1);
                break;
        }
    }

    private static void _amDrawRegistNop(AMS_REGISTLIST regist)
    {
        mppAssertNotImpl();
        regist.command_id = 0;
    }

    private static void _amDrawLoadTexture(AMS_REGISTLIST regist)
    {
        AMS_PARAM_LOAD_TEXTURE paramLoadTexture = (AMS_PARAM_LOAD_TEXTURE)regist.param;
        OpenGL.glGenTexture(out paramLoadTexture.pTexInfo.TexName);
        OpenGL.glBindTexture(3553U, paramLoadTexture.pTexInfo.TexName);
        OpenGL.mppglTexImage2D(paramLoadTexture.tex);
        _amSetTextureAttribute(paramLoadTexture);
        paramLoadTexture.buf_delete = null;
        regist.command_id = 0;
    }

    private static void _amSetTextureAttribute(AMS_PARAM_LOAD_TEXTURE param)
    {
        param.pTexInfo.Bank = param.bank;
        param.pTexInfo.GlobalIndex = param.globalIndex;
        param.pTexInfo.Flag = 0U;
    }

    private static void _amDrawReleaseTexture(AMS_REGISTLIST regist)
    {
        AMS_PARAM_RELEASE_TEXTURE paramReleaseTexture = (AMS_PARAM_RELEASE_TEXTURE)regist.param;
        int nTex = paramReleaseTexture.texlist.nTex;
        ArrayPointer<NNS_TEXINFO> pTexInfoList = paramReleaseTexture.texlist.pTexInfoList;
        int num = nTex - 1;
        ArrayPointer<NNS_TEXINFO> arrayPointer = pTexInfoList + (nTex - 1);
        while (num >= 0)
        {
            if ((~arrayPointer).TexName != 0U)
                OpenGL.glDeleteTextures(1, new uint[1] { (~arrayPointer).TexName });
            --num;
            --arrayPointer;
        }
        paramReleaseTexture.texlist.nTex = -1;
        paramReleaseTexture.texlist.pTexInfoList = null;
        paramReleaseTexture.texlist = null;
        regist.command_id = 0;
    }

    private static void _amDrawVertexBufferObject(AMS_REGISTLIST regist)
    {
        AMS_PARAM_VERTEX_BUFFER_OBJECT vertexBufferObject = (AMS_PARAM_VERTEX_BUFFER_OBJECT)regist.param;
        int num = (int)nnBindBufferObjectGL(vertexBufferObject.obj, vertexBufferObject.srcobj, vertexBufferObject.bindflag);
        regist.command_id = 0;
    }

    private static void _amDrawDeleteVertexObject(AMS_REGISTLIST regist)
    {
        nnDeleteBufferObjectGL(((AMS_PARAM_DELETE_VERTEX_OBJECT)regist.param).obj);
        regist.command_id = 0;
    }

    private static void _amDrawLoadShaderObject(AMS_REGISTLIST regist)
    {
        mppAssertNotImpl();
        regist.command_id = 0;
    }

    private static void _amDrawReleaseStdShader(AMS_REGISTLIST regist)
    {
        mppAssertNotImpl();
        regist.command_id = 0;
    }

    private static void _amDrawLoadShader(AMS_REGISTLIST regist)
    {
        mppAssertNotImpl();
        regist.command_id = 0;
    }

    private static void _amDrawBuildShader(AMS_REGISTLIST regist)
    {
        mppAssertNotImpl();
        regist.command_id = 0;
    }

    private static void _amDrawCreateShader(AMS_REGISTLIST regist)
    {
        mppAssertNotImpl();
        regist.command_id = 0;
    }

    private static void _amDrawReleaseShader(AMS_REGISTLIST regist)
    {
        mppAssertNotImpl();
    }

    private static void _amDrawLoadTextureImage(AMS_REGISTLIST regist)
    {
        mppAssertNotImpl();
        AMS_PARAM_LOAD_TEXTURE_IMAGE loadTextureImage = (AMS_PARAM_LOAD_TEXTURE_IMAGE)regist.param;
        regist.command_id = 0;
    }

    private static void _amDrawReleaseTextureImage(AMS_REGISTLIST regist)
    {
        mppAssertNotImpl();
        AMS_PARAM_RELEASE_TEXTURE_IMAGE releaseTextureImage = (AMS_PARAM_RELEASE_TEXTURE_IMAGE)regist.param;
        if (releaseTextureImage.texture != null)
        {
            releaseTextureImage.texture.Dispose();
            releaseTextureImage.texture = null;
        }
        regist.command_id = 0;
    }

    public static void amMatrixPush(ref SNNS_MATRIX mtx)
    {
        nnPushMatrix(_amMatrixGetCurrentStack(), ref mtx);
    }

    public static void amMatrixPush(NNS_MATRIX mtx)
    {
        nnPushMatrix(_amMatrixGetCurrentStack(), mtx);
    }

    public static void amMatrixPush()
    {
        nnPushMatrix(_amMatrixGetCurrentStack());
    }

    public static void amMatrixPop()
    {
        nnPopMatrix(_amMatrixGetCurrentStack());
    }

    public static NNS_MATRIX amMatrixGetCurrent()
    {
        return nnGetCurrentMatrix(_amMatrixGetCurrentStack());
    }

    public static void amMatrixSetCurrent(NNS_MATRIX m)
    {
        nnSetCurrentMatrix(_amMatrixGetCurrentStack(), m);
    }

    public static void amMatrixClearStack()
    {
        nnClearMatrixStack(_amMatrixGetCurrentStack());
    }

    public static void amMatrixCalcPoint(
      ref SNNS_VECTOR4D pDst,
      ref SNNS_VECTOR4D pSrc)
    {
        NNS_MATRIX current = amMatrixGetCurrent();
        nnTransformVector(ref pDst, current, ref pSrc);
        pDst.w = pSrc.w;
    }

    public static void amMatrixCalcPoint(NNS_VECTOR4D pDst, NNS_VECTOR4D pSrc)
    {
        NNS_MATRIX current = amMatrixGetCurrent();
        nnTransformVector(pDst, current, pSrc);
        pDst.w = pSrc.w;
    }

    public static void amMatrixCalcPoint(NNS_VECTOR pDst, NNS_VECTOR pSrc)
    {
        NNS_MATRIX current = amMatrixGetCurrent();
        nnTransformVector(pDst, current, pSrc);
    }

    public static void amMatrixCalcVector(NNS_VECTOR pDst, NNS_VECTOR pSrc)
    {
        NNS_MATRIX current = amMatrixGetCurrent();
        nnTransformNormalVector(pDst, current, pSrc);
    }

    public static void amMatrixCalcVector(NNS_VECTOR4D pDst, NNS_VECTOR4D pSrc)
    {
        NNS_MATRIX current = amMatrixGetCurrent();
        nnTransformNormalVector(pDst, current, pSrc);
    }

    public static void amMatrixCalcVector(
      ref SNNS_VECTOR4D pDst,
      ref SNNS_VECTOR4D pSrc)
    {
        NNS_MATRIX current = amMatrixGetCurrent();
        nnTransformNormalVector(ref pDst, current, ref pSrc);
    }

    public static void amMatrixCalcVector(ref SNNS_VECTOR4D pDst, NNS_VECTOR4D pSrc)
    {
        NNS_MATRIX current = amMatrixGetCurrent();
        nnTransformNormalVector(ref pDst, current, pSrc);
    }

    public static NNS_MATRIXSTACK _amMatrixGetCurrentStack()
    {
        return _am_default_stack;
    }

    private static void amRenderInit()
    {
        AMS_RENDER_MANAGER amsRenderManager = new AMS_RENDER_MANAGER();
        AMS_RENDER_TARGET amsRenderTarget = new AMS_RENDER_TARGET();
        AMS_RENDER_TARGET amRenderDefault = _am_render_default;
        AMS_RENDER_MANAGER amRenderManager = _am_render_manager;
        amRenderManager.targetp = _am_render_default;
        amRenderManager.target_now = _am_render_default;
    }

    private static AMS_RENDER_TARGET amRenderSetTarget(
      AMS_RENDER_TARGET target,
      uint flag,
      NNS_RGBA_U8 color)
    {
        return amRenderSetTarget(target, flag, color, 1f, 0);
    }

    private static AMS_RENDER_TARGET amRenderSetTarget(AMS_RENDER_TARGET target)
    {
        return amRenderSetTarget(target, 0U, null, 1f, 0);
    }

    private static AMS_RENDER_TARGET amRenderSetTarget(
      AMS_RENDER_TARGET target,
      uint flag,
      NNS_RGBA_U8 color,
      float z,
      int stencil)
    {
        return null;
    }

    private static void amRenderSetTexture(int slot, AMS_RENDER_TARGET target, int index)
    {
    }

    private static void amRenderCopyTarget(
      AMS_RENDER_TARGET target,
      NNS_RGBA_U8 color)
    {
    }
}