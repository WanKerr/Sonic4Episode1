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

    public static uint AMD_FCOLTORGBA8888(float r, float g, float b, float a)
    {
        return (uint)(((int)(uint)((double)r * (double)byte.MaxValue) & (int)byte.MaxValue) << 24 | ((int)(uint)((double)g * (double)byte.MaxValue) & (int)byte.MaxValue) << 16 | ((int)(uint)((double)b * (double)byte.MaxValue) & (int)byte.MaxValue) << 8 | (int)(uint)((double)a * (double)byte.MaxValue) & (int)byte.MaxValue);
    }

    public static float AMD_DISPLAY_WIDTH
    {
        get
        {
            return AppMain._am_draw_video.disp_width;
        }
        set
        {
            AppMain._am_draw_video.disp_width = value;
        }
    }

    public static float AMD_DISPLAY_HEIGHT
    {
        get
        {
            return AppMain._am_draw_video.disp_height;
        }
        set
        {
            AppMain._am_draw_video.disp_height = value;
        }
    }

    public static float AMD_SCREEN_WIDTH
    {
        get
        {
            return AppMain._am_draw_video.draw_width;
        }
        set
        {
            AppMain._am_draw_video.draw_width = value;
        }
    }

    public static float AMD_SCREEN_HEIGHT
    {
        get
        {
            return AppMain._am_draw_video.draw_height;
        }
        set
        {
            AppMain._am_draw_video.draw_height = value;
        }
    }

    public static float AMD_SCREEN_2D_WIDTH
    {
        get
        {
            return AppMain._am_draw_video.width_2d;
        }
        set
        {
            AppMain._am_draw_video.width_2d = value;
        }
    }

    public static float AMD_SCREEN_2D_HEIGHT
    {
        get
        {
            return AppMain._am_draw_video.height_2d;
        }
        set
        {
            AppMain._am_draw_video.height_2d = value;
        }
    }

    public static float AMD_2D_SCALE_X
    {
        get
        {
            return AppMain._am_draw_video.scale_x_2d;
        }
        set
        {
            AppMain._am_draw_video.scale_x_2d = value;
        }
    }

    public static float AMD_2D_SCALE_Y
    {
        get
        {
            return AppMain._am_draw_video.scale_y_2d;
        }
        set
        {
            AppMain._am_draw_video.scale_y_2d = value;
        }
    }

    public static float AMD_2D_BASE_X
    {
        get
        {
            return AppMain._am_draw_video.base_x_2d;
        }
        set
        {
            AppMain._am_draw_video.base_x_2d = value;
        }
    }

    public static float AMD_2D_BASE_Y
    {
        get
        {
            return AppMain._am_draw_video.base_y_2d;
        }
        set
        {
            AppMain._am_draw_video.base_y_2d = value;
        }
    }

    public static float AMD_SCREEN_ASPECT
    {
        get
        {
            return AppMain._am_draw_video.draw_aspect;
        }
        set
        {
            AppMain._am_draw_video.draw_aspect = value;
        }
    }

    public static int AMD_RENDER_WIDTH
    {
        get
        {
            return AppMain._am_render_manager.target_now.width;
        }
        set
        {
            AppMain._am_render_manager.target_now.width = value;
        }
    }

    public static int AMD_RENDER_HEIGHT
    {
        get
        {
            return AppMain._am_render_manager.target_now.height;
        }
        set
        {
            AppMain._am_render_manager.target_now.height = value;
        }
    }

    public static float AMD_RENDER_ASPECT
    {
        get
        {
            return AppMain._am_render_manager.target_now.aspect;
        }
        set
        {
            AppMain._am_render_manager.target_now.aspect = value;
        }
    }

    private static AppMain.AMS_PARAM_DRAW_PRIMITIVE amDrawAlloc_AMS_PARAM_DRAW_PRIMITIVE()
    {
        return AppMain.amDraw_AMS_PARAM_DRAW_PRIMITIVE_Pool.Alloc();
    }

    private static AppMain.NNS_MATRIX amDrawAlloc_NNS_MATRIX()
    {
        return AppMain.amDraw_NNS_MATRIX_Pool.Alloc();
    }

    private static AppMain.AMS_DRAWSTATE_FOG amDrawAlloc_AMS_DRAWSTATE_FOG()
    {
        return AppMain.amDraw_AMS_DRAWSTATE_FOG_Pool.Alloc();
    }

    private static AppMain.AMS_DRAWSTATE_FOG_COLOR amDrawAlloc_AMS_DRAWSTATE_FOG_COLOR()
    {
        return AppMain.amDraw_AMS_DRAWSTATE_FOG_COLOR_Pool.Alloc();
    }

    private static AppMain.AMS_DRAWSTATE_FOG_RANGE amDrawAlloc_AMS_DRAWSTATE_FOG_RANGE()
    {
        return AppMain.amDraw_AMS_DRAWSTATE_FOG_RANGE_Pool.Alloc();
    }

    private static AppMain.AMS_PARAM_MAKE_TASK amDrawAlloc_AMS_PARAM_MAKE_TASK()
    {
        return AppMain.amDraw_AMS_PARAM_MAKE_TASK_Pool.Alloc();
    }

    private static AppMain.AMS_PARAM_DRAW_OBJECT_MATERIAL amDrawAlloc_AMS_PARAM_DRAW_OBJECT_MATERIAL()
    {
        return AppMain.amDraw_AMS_PARAM_DRAW_OBJECT_MATERIAL_Pool.Alloc();
    }

    private static AppMain.GMS_GMK_ITEM_MAT_CB_PARAM amDrawAlloc_GMS_GMK_ITEM_MAT_CB_PARAM()
    {
        return AppMain.amDraw_GMS_GMK_ITEM_MAT_CB_PARAM_Pool.Alloc();
    }

    private static AppMain.DMAP_PARAM_WATER amDrawAlloc_DMAP_PARAM_WATER()
    {
        return AppMain.amDraw_DMAP_PARAM_WATER_Pool.Alloc();
    }

    private static AppMain.NNS_PRIM3D_PCT_ARRAY amDrawAlloc_NNS_PRIM3D_PCT(int count)
    {
        ++AppMain.NNS_PRIM3D_PCT_ALLOC_CNT;
        if (AppMain.NNS_PRIM3D_PCT_buf_size + count >= 16384)
            AppMain.NNS_PRIM3D_PCT_buf_size = 0;
        if (AppMain.NNS_PRIM3D_PCT_arrays_count >= 1024)
            AppMain.NNS_PRIM3D_PCT_arrays_count = 0;
        AppMain.NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = AppMain.NNS_PRIM3D_PCT_arrays[AppMain.NNS_PRIM3D_PCT_arrays_count];
        nnsPriM3DPctArray.buffer = AppMain.NNS_PRIM3D_PCT_buf;
        nnsPriM3DPctArray.offset = AppMain.NNS_PRIM3D_PCT_buf_size;
        ++AppMain.NNS_PRIM3D_PCT_arrays_count;
        AppMain.NNS_PRIM3D_PCT_buf_size += count;
        return nnsPriM3DPctArray;
    }

    private static AppMain.NNS_PRIM3D_PC[] amDrawAlloc_NNS_PRIM3D_PC(int count)
    {
        return AppMain.amDraw_NNS_PRIM3D_PC_Array_Pool.AllocArray(count);
    }

    private static AppMain.NNS_TRS[] amDrawAlloc_NNS_TRS(int count)
    {
        return AppMain.amDraw_NNS_TRS_Array_Pool.AllocArray(count);
    }

    private static AppMain.NNS_MATERIALPTR[] amDrawAlloc_NNS_MATERIALPTR(int count)
    {
        return AppMain.amDraw_NNS_MATERIALPTR_Array_Pool.AllocArray(count);
    }

    private static AppMain.GMS_BS_CMN_CNM_NODE_INFO[] amDrawAlloc_GMS_BS_CMN_CNM_NODE_INFO(
      int count)
    {
        return AppMain.amDraw_GMS_BS_CMN_CNM_NODE_INFO_Array_Pool.AllocArray(count);
    }

    private static AppMain.GMS_BS_CMN_CNM_NODE_INFO amDrawAlloc_GMS_BS_CMN_CNM_NODE_INFO()
    {
        return AppMain.amDraw_GMS_BS_CMN_CNM_NODE_INFO_Pool.Alloc();
    }

    private static AppMain.GMS_BS_CMN_CNM_PARAM amDrawAlloc_GMS_BS_CMN_CNM_PARAM()
    {
        return AppMain.amDraw_GMS_BS_CMN_CNM_PARAM_Pool.Alloc();
    }

    private static AppMain.NNS_MATERIAL_STDSHADER_COLOR amDrawAlloc_NNS_MATERIAL_STDSHADER_COLOR()
    {
        return AppMain.amDraw_NNS_MATERIAL_STDSHADER_COLOR_Pool.Alloc();
    }

    private static AppMain.NNS_MATERIAL_GLES11_DESC amDrawAlloc_NNS_MATERIAL_GLES11_DESC()
    {
        return AppMain.amDraw_NNS_MATERIAL_GLES11_DESC_Pool.Alloc();
    }

    private static AppMain.NNS_MATERIALPTR amDrawAlloc_NNS_MATERIALPTR()
    {
        return AppMain.amDraw_NNS_MATERIALPTR_Pool.Alloc();
    }

    private static AppMain.OBS_DRAW_PARAM_3DNN_USER_FUNC amDrawAlloc_OBS_DRAW_PARAM_3DNN_USER_FUNC()
    {
        return AppMain.amDraw_OBS_DRAW_PARAM_3DNN_USER_FUNC_Pool.Alloc();
    }

    private static AppMain.OBS_DRAW_PARAM_3DNN_MOTION amDrawAlloc_OBS_DRAW_PARAM_3DNN_MOTION()
    {
        return AppMain.amDraw_OBS_DRAW_PARAM_3DNN_MOTION_Pool.Alloc();
    }

    private static AppMain.OBS_DRAW_PARAM_3DNN_DRAW_MOTION amDrawAlloc_OBS_DRAW_PARAM_3DNN_DRAW_MOTION()
    {
        return AppMain.amDraw_OBS_DRAW_PARAM_3DNN_DRAW_MOTION_Pool.Alloc();
    }

    private static AppMain.AMS_PARAM_DRAW_MOTION_TRS amDrawAlloc_AMS_PARAM_DRAW_MOTION_TRS()
    {
        return AppMain.amDraw_AMS_PARAM_DRAW_MOTION_TRS_Pool.Alloc();
    }

    private static AppMain.NNS_TRS amDrawAlloc_NNS_TRS()
    {
        return AppMain.amDraw_NNS_TRS_Pool.Alloc();
    }

    private static AppMain.AOS_ACT_DRAW amDrawAlloc_AOS_ACT_DRAW()
    {
        return AppMain.amDraw_AOS_ACT_DRAW_Pool.Alloc();
    }

    private static AppMain.NNS_OBJECT amDrawAlloc_NNS_OBJECT()
    {
        return AppMain.amDraw_NNS_OBJECT_Pool.Alloc();
    }

    private static void amDrawResetCache()
    {
        AppMain.amDraw_AMS_PARAM_DRAW_PRIMITIVE_Pool.ReleaseUsedObjects();
        AppMain.amDraw_NNS_MATRIX_Pool.ReleaseUsedObjects();
        AppMain.amDraw_AMS_PARAM_MAKE_TASK_Pool.ReleaseUsedObjects();
        AppMain.amDraw_AMS_DRAWSTATE_FOG_Pool.ReleaseUsedObjects();
        AppMain.amDraw_AMS_PARAM_DRAW_OBJECT_MATERIAL_Pool.ReleaseUsedObjects();
        AppMain.amDraw_AMS_DRAWSTATE_FOG_COLOR_Pool.ReleaseUsedObjects();
        AppMain.amDraw_AMS_DRAWSTATE_FOG_RANGE_Pool.ReleaseUsedObjects();
        AppMain.amDraw_NNS_PRIM3D_PC_Array_Pool.ReleaseUsedArrays();
        AppMain.amDraw_GMS_MAP_PRIM_DRAW_WORK_Array_Pool.ReleaseUsedArrays();
        AppMain.amDraw_OBS_DRAW_PARAM_3DNN_USER_FUNC_Pool.ReleaseUsedObjects();
        AppMain.amDraw_OBS_DRAW_PARAM_3DNN_MOTION_Pool.ReleaseUsedObjects();
        AppMain.amDraw_OBS_DRAW_PARAM_3DNN_DRAW_MOTION_Pool.ReleaseUsedObjects();
        AppMain.amDraw_AMS_PARAM_DRAW_MOTION_TRS_Pool.ReleaseUsedObjects();
        AppMain.amDraw_GMS_GMK_ITEM_MAT_CB_PARAM_Pool.ReleaseUsedObjects();
        AppMain.amDraw_DMAP_PARAM_WATER_Pool.ReleaseUsedObjects();
        AppMain.amDraw_NNS_TRS_Pool.ReleaseUsedObjects();
        AppMain.amDraw_AOS_ACT_DRAW_Pool.ReleaseUsedObjects();
        AppMain.amDraw_NNS_OBJECT_Pool.ReleaseUsedObjects();
        AppMain.amDraw_NNS_TRS_Array_Pool.ReleaseUsedArrays();
        AppMain.amDraw_GMS_BS_CMN_CNM_NODE_INFO_Array_Pool.ReleaseUsedArrays();
        AppMain.amDraw_GMS_BS_CMN_CNM_NODE_INFO_Pool.ReleaseUsedObjects();
        AppMain.amDraw_GMS_BS_CMN_CNM_PARAM_Pool.ReleaseUsedObjects();
        AppMain.amDraw_NNS_MATERIAL_STDSHADER_COLOR_Pool.ReleaseUsedObjects();
        AppMain.amDraw_NNS_MATERIAL_GLES11_DESC_Pool.ReleaseUsedObjects();
        AppMain.amDraw_NNS_MATERIALPTR_Pool.ReleaseUsedObjects();
        AppMain.amDraw_NNS_MATERIALPTR_Array_Pool.ReleaseUsedArrays();
        AppMain.NNS_PRIM3D_PCT_buf_size = 0;
        AppMain.NNS_PRIM3D_PCT_arrays_count = 0;
    }

    private static void amDrawCreateBuffer(int command_size, int data_size, int work_size)
    {
        AppMain._am_draw_command_buf_max = command_size;
        AppMain._am_draw_data_buf_max = data_size;
        for (int index = 0; index < 4; ++index)
        {
            AppMain._am_draw_command_buf[index] = new object[command_size];
            AppMain._am_draw_data_buf[index] = new object[data_size];
        }
    }

    private static void amDrawDeleteBuffer()
    {
    }

    public static void amDrawSetDrawCommandFunc(
      AppMain._am_draw_command_delegate func,
      AppMain._am_draw_command_delegate sort)
    {
        AppMain._am_draw_command_func = func;
        AppMain._am_draw_command_sort = sort;
    }

    private static void amDrawSetShaderCompile(int flag)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void amDrawSetVSyncAlarm(AppMain.AMS_ALARM alarm)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void amDrawWaitVSync()
    {
        AppMain.mppAssertNotImpl();
    }

    public bool amDrawBegin()
    {
        return this.amDrawBegin((AppMain.AMS_RENDER_TARGET)null, AppMain.AMD_RENDER_CLEAR_COLOR | AppMain.AMD_RENDER_CLEAR_DEPTH, (AppMain.NNS_RGBA_U8)null, 1f, 0);
    }

    public bool amDrawBegin(
      AppMain.AMS_RENDER_TARGET target,
      uint flag,
      AppMain.NNS_RGBA_U8 color,
      float depth,
      int stencil)
    {
        AppMain._am_draw_offset_x = 0;
        AppMain.amRenderSetTarget(target, flag, color, depth, stencil);
        if (AppMain._am_draw_in_scene != 0 || flag == 0U)
            return true;
        uint mask = 0;
        if (((int)flag & (int)AppMain.AMD_RENDER_CLEAR_COLOR) != 0)
        {
            if (color != null)
            {
                AppMain._am_draw_bg_color.r = color.r;
                AppMain._am_draw_bg_color.g = color.g;
                AppMain._am_draw_bg_color.b = color.b;
                AppMain._am_draw_bg_color.a = color.a;
            }
            OpenGL.glClearColor((float)AppMain._am_draw_bg_color.r / (float)byte.MaxValue, (float)AppMain._am_draw_bg_color.g / (float)byte.MaxValue, (float)AppMain._am_draw_bg_color.b / (float)byte.MaxValue, (float)AppMain._am_draw_bg_color.a / (float)byte.MaxValue);
            mask |= 16384U;
        }
        if (((int)flag & (int)AppMain.AMD_RENDER_CLEAR_DEPTH) != 0)
        {
            OpenGL.glClearDepthf(depth);
            mask |= 256U;
        }
        OpenGL.glClear(mask);
        return true;
    }

    private static void amDrawEnd()
    {
        AppMain.amDrawEnd((AppMain.NNS_RGBA_U8)null, 1f, 0);
    }

    private static void amDrawEnd(AppMain.NNS_RGBA_U8 color, float z, int stencil)
    {
        if (AppMain._am_draw_in_scene == 0)
            return;
        AppMain._am_draw_in_scene = 0;
    }

    private static void amDrawSetBGColor(AppMain.NNS_RGBA_U8 bgColor)
    {
        AppMain._am_draw_bg_color = bgColor;
    }

    private static AppMain.NNS_RGBA_U8 amDrawGetBGColor()
    {
        AppMain.mppAssertNotImpl();
        return AppMain._am_draw_bg_color;
    }

    private static void amDrawBeginScene()
    {
        AppMain._am_displaylist_manager.sort_num = 0;
    }

    public static void amDrawEndScene()
    {
        AppMain.AMS_DISPLAYLIST_MANAGER displaylistManager = AppMain._am_displaylist_manager;
        int sortNum = displaylistManager.sort_num;
        displaylistManager.sort_num = 0;
        if (sortNum == 0)
            return;
        AppMain.AMS_DRAW_SORT sort = new AppMain.AMS_DRAW_SORT();
        AppMain.ArrayPointer<AppMain.AMS_DRAW_SORT> arrayPointer1 = new AppMain.ArrayPointer<AppMain.AMS_DRAW_SORT>(displaylistManager.sortlist);
        int num1 = sortNum - 1;
        while (num1 > 0)
        {
            AppMain.ArrayPointer<AppMain.AMS_DRAW_SORT> arrayPointer2 = arrayPointer1;
            AppMain.ArrayPointer<AppMain.AMS_DRAW_SORT> arrayPointer3 = (AppMain.ArrayPointer<AppMain.AMS_DRAW_SORT>)(arrayPointer1 + 1);
            int num2 = ((AppMain.AMS_DRAW_SORT)~arrayPointer2).key;
            int num3 = num1;
            while (num3 > 0)
            {
                int key = ((AppMain.AMS_DRAW_SORT)~arrayPointer3).key;
                if (key >= num2)
                {
                    num2 = key;
                    arrayPointer2 = arrayPointer3;
                }
                --num3;
                ++arrayPointer3;
            }
            sort.Assign((AppMain.AMS_DRAW_SORT)arrayPointer1);
            ((AppMain.AMS_DRAW_SORT)~arrayPointer1).Assign((AppMain.AMS_DRAW_SORT)~arrayPointer2);
            ((AppMain.AMS_DRAW_SORT)~arrayPointer2).Assign(sort);
            --num1;
            ++arrayPointer1;
        }
        AppMain.amDrawPushState();
        arrayPointer1 = new AppMain.ArrayPointer<AppMain.AMS_DRAW_SORT>(displaylistManager.sortlist);
        int num4 = sortNum;
        while (num4 > 0)
        {
            AppMain.AMS_COMMAND_HEADER command = ((AppMain.AMS_DRAW_SORT)~arrayPointer1).command;
            if (command.command_id >= 0)
                AppMain._am_draw_command_sort(command, 0U);
            else
                AppMain._am_draw_sort_system_exec[-command.command_id](command, 0U);
            --num4;
            ++arrayPointer1;
        }
        AppMain.amDrawPopState();
    }

    private static void amDrawBuildShader()
    {
        AppMain.mppAssertNotImpl();
    }

    private static void amDrawDisplay()
    {
        AppMain.amDrawDisplay((AppMain.AMS_RENDER_TARGET)null, 0);
    }

    private static void amDrawDisplay(AppMain.AMS_RENDER_TARGET target, int index)
    {
        AppMain.AMS_RENDER_TARGET amsRenderTarget = AppMain.amRenderSetTarget((AppMain.AMS_RENDER_TARGET)null);
        if (target == null)
            target = amsRenderTarget;
        if (target != null)
            AppMain.amRenderSetTexture(0, target, index);
        AppMain._am_draw_offset_x = 0;
    }

    private static void amDrawInitDisplayList()
    {
        AppMain.amDrawInitDisplayList(0);
    }

    private static void amDrawInitDisplayList(int user_header_size)
    {
        AppMain._am_draw_task = AppMain.amTaskInitSystem(256, 64, 1);
        AppMain.AMS_DISPLAYLIST_MANAGER displaylistManager = AppMain._am_displaylist_manager;
        displaylistManager.write_index = 0;
        displaylistManager.last_index = -1;
        displaylistManager.read_index = -1;
        AppMain.AMS_DISPLAYLIST[] displaylist = displaylistManager.displaylist;
        for (int index = 0; index < 4; ++index)
        {
            displaylist[index].counter = -1;
            displaylist[index].command_buf = (AppMain.ArrayPointer<object>)AppMain._am_draw_command_buf[index];
            displaylist[index].data_buf = (AppMain.ArrayPointer<object>)AppMain._am_draw_data_buf[index];
            displaylist[index].command_buf_size = 0;
            displaylist[index].data_buf_size = 0;
        }
        displaylistManager.regist_num = 0;
        displaylistManager.reg_read_index = 0;
        displaylistManager.reg_end_index = 0;
        displaylistManager.reg_write_index = 0;
        displaylistManager.reg_write_num = 0;
        AppMain.amDrawOpenDisplayList();
    }

    private static void amDrawExitDisplayList()
    {
        AppMain.amTaskExitSystem(AppMain._am_draw_task);
    }

    private static void amDrawOpenDisplayList()
    {
        AppMain.AMS_DISPLAYLIST_MANAGER displaylistManager = AppMain._am_displaylist_manager;
        AppMain.AMS_DISPLAYLIST amsDisplaylist = displaylistManager.displaylist[displaylistManager.write_index];
        amsDisplaylist.command_buf_size = 1;
        amsDisplaylist.data_buf_size = 0;
        amsDisplaylist.counter = 0;
        AppMain.ArrayPointer<object> commandBuf = amsDisplaylist.command_buf;
        AppMain.AMS_COMMAND_BUFFER_HEADER commandBufferHeader;
        if (commandBuf[0] == null)
        {
            commandBufferHeader = new AppMain.AMS_COMMAND_BUFFER_HEADER();
            commandBuf.SetPrimitive((object)commandBufferHeader);
        }
        else
            commandBufferHeader = (AppMain.AMS_COMMAND_BUFFER_HEADER)commandBuf[0];
        displaylistManager.write_header = commandBufferHeader;
        Array.Copy((Array)AppMain._am_system_flag, (Array)commandBufferHeader.system_flag, 4);
        Array.Copy((Array)AppMain._am_debug_flag, (Array)commandBufferHeader.debug_flag, 4);
        commandBufferHeader.display_flag = (ushort)0;
        commandBufferHeader.icon_alpha = 0.0f;
        displaylistManager.command_buf_ptr = (AppMain.ArrayPointer<object>)(commandBuf + 1);
    }

    private static void amDrawCloseDisplayList()
    {
        AppMain.AMS_DISPLAYLIST_MANAGER displaylistManager = AppMain._am_displaylist_manager;
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
        AppMain.amDrawOpenDisplayList();
    }

    private static int amDrawGetDisplayList()
    {
        AppMain.AMS_DISPLAYLIST_MANAGER displaylistManager = AppMain._am_displaylist_manager;
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
        AppMain.AMS_COMMAND_BUFFER_HEADER commandBufferHeader = (AppMain.AMS_COMMAND_BUFFER_HEADER)(object)~displaylistManager.displaylist[index1].command_buf;
        displaylistManager.read_header = commandBufferHeader;
        return index1;
    }

    private static void amDrawRegistCommand(uint state, int command_id)
    {
        AppMain.amDrawRegistCommand(state, command_id, (object)null);
    }

    public static void amDrawRegistCommand(uint state, int command_id, object param)
    {
        AppMain.AMS_DISPLAYLIST_MANAGER displaylistManager = AppMain._am_displaylist_manager;
        ++displaylistManager.displaylist[displaylistManager.write_index].command_buf_size;
        AppMain.AMS_COMMAND_HEADER amsCommandHeader = (AppMain.AMS_COMMAND_HEADER)(object)~displaylistManager.command_buf_ptr ?? new AppMain.AMS_COMMAND_HEADER();
        displaylistManager.command_buf_ptr.SetPrimitive((object)amsCommandHeader);
        amsCommandHeader.state = state;
        amsCommandHeader.command_id = command_id;
        amsCommandHeader.param = param;
        displaylistManager.command_buf_ptr += 1;
    }

    private static int amDrawRegistCommand(int command_id)
    {
        return AppMain.amDrawRegistCommand(command_id, (object)null);
    }

    public static int amDrawRegistCommand(int command_id, object param)
    {
        AppMain.AMS_DISPLAYLIST_MANAGER displaylistManager = AppMain._am_displaylist_manager;
        int regWriteIndex = displaylistManager.reg_write_index;
        AppMain.AMS_REGISTLIST amsRegistlist = displaylistManager.registlist[regWriteIndex];
        amsRegistlist.command_id = command_id;
        amsRegistlist.param = param;
        displaylistManager.reg_write_index = (regWriteIndex + 1) % 256;
        ++displaylistManager.reg_write_num;
        return regWriteIndex;
    }

    public static bool amDrawIsRegistComplete(int index)
    {
        return AppMain._am_displaylist_manager.registlist[index].command_id == 0;
    }

    private static byte[] amDrawGetDataBuffer()
    {
        AppMain.mppAssertNotImpl();
        return (byte[])null;
    }

    public static void amDrawInitState()
    {
        AppMain._am_draw_state.drawflag = 0U;
        AppMain._am_draw_state.diffuse.mode = 3;
        AppMain._am_draw_state.diffuse.r = 1f;
        AppMain._am_draw_state.diffuse.g = 1f;
        AppMain._am_draw_state.diffuse.b = 1f;
        AppMain._am_draw_state.ambient.mode = 3;
        AppMain._am_draw_state.ambient.r = 1f;
        AppMain._am_draw_state.ambient.g = 1f;
        AppMain._am_draw_state.ambient.b = 1f;
        AppMain._am_draw_state.alpha.mode = 3;
        AppMain._am_draw_state.alpha.alpha = 1f;
        AppMain._am_draw_state.specular.mode = 3;
        AppMain._am_draw_state.specular.r = 1f;
        AppMain._am_draw_state.specular.g = 1f;
        AppMain._am_draw_state.specular.b = 1f;
        AppMain._am_draw_state.blend.mode = 0;
        AppMain._am_draw_state.envmap.texsrc = 1;
        AppMain._am_draw_state.zmode.compare = 1U;
        AppMain._am_draw_state.zmode.func = 515;
        AppMain._am_draw_state.zmode.update = 1U;
        AppMain.NNS_MATRIX texmtx = AppMain._am_draw_state.envmap.texmtx;
        AppMain.nnMakeUnitMatrix(texmtx);
        AppMain.nnTranslateMatrix(texmtx, texmtx, 0.5f, 0.5f, 0.0f);
        AppMain.nnScaleMatrix(texmtx, texmtx, 0.5f, 0.5f, 0.0f);
        for (int index = 0; index < 4; ++index)
        {
            AppMain._am_draw_state.texoffset[index].mode = 2;
            AppMain._am_draw_state.texoffset[index].u = 0.0f;
            AppMain._am_draw_state.texoffset[index].v = 0.0f;
        }
        AppMain.amDrawSetState(AppMain._am_draw_state);
    }

    private static void amDrawPushState()
    {
        AppMain._am_draw_state_stack[AppMain._am_draw_state_stack_num++].Assign(AppMain._am_draw_state);
    }

    private static void amDrawPopState()
    {
        --AppMain._am_draw_state_stack_num;
        AppMain.amDrawSetState(AppMain._am_draw_state_stack[AppMain._am_draw_state_stack_num]);
    }

    private static void amDrawSetState(AppMain.AMS_DRAWSTATE state)
    {
        AppMain._am_draw_state.Assign(state);
        AppMain.nnSetMaterialControlDiffuse(state.diffuse.mode, state.diffuse.r, state.diffuse.g, state.diffuse.b);
        AppMain.nnSetMaterialControlAmbient(state.ambient.mode, state.ambient.r, state.ambient.g, state.ambient.b);
        AppMain.nnSetMaterialControlAlpha(state.alpha.mode, state.alpha.alpha);
        AppMain.nnSetMaterialControlBlendMode(state.blend.mode);
        AppMain.nnSetMaterialControlEnvTexMatrix(state.envmap.texsrc, AppMain._am_draw_state.envmap.texmtx);
        for (int slot = 0; slot < 4; ++slot)
            AppMain.nnSetMaterialControlTextureOffset(slot, state.texoffset[slot].mode, state.texoffset[slot].u, state.texoffset[slot].v);
        AppMain.nnSetFogSwitch(state.fog.flag != 0);
        AppMain.nnSetFogColor(state.fog_color.r, state.fog_color.g, state.fog_color.b);
        AppMain.nnSetFogRange(state.fog_range.fnear, state.fog_range.ffar);
    }

    private static AppMain.AMS_DRAWSTATE amDrawGetState(AppMain.AMS_DRAWSTATE state)
    {
        state?.Assign(AppMain._am_draw_state);
        return AppMain._am_draw_state;
    }

    private static AppMain.AMS_DRAWSTATE amDrawGetState()
    {
        return AppMain._am_draw_state;
    }

    public static void amDrawSetProjection(AppMain.NNS_MATRIX proj_mtx, int proj_type)
    {
        AppMain._am_draw_proj_mtx.Assign(proj_mtx);
        AppMain._am_draw_proj_type = proj_type;
        AppMain.nnSetProjection(proj_mtx, proj_type);
    }

    private static AppMain.NNS_MATRIX amDrawGetProjectionMatrix()
    {
        return AppMain._am_draw_proj_mtx;
    }

    private static int amDrawGetProjectionType()
    {
        return AppMain._am_draw_proj_type;
    }

    private static void amDrawExecute()
    {
        AppMain.amTaskExecute(AppMain._am_draw_task);
        AppMain.amTaskReset(AppMain._am_draw_task);
    }

    private static void amDrawExecCommand(uint state)
    {
        AppMain.amDrawExecCommand(state, 0U);
    }

    public static void amDrawExecCommand(uint state, uint drawflag)
    {
        AppMain.AMS_DISPLAYLIST_MANAGER displaylistManager = AppMain._am_displaylist_manager;
        AppMain.AMS_DISPLAYLIST amsDisplaylist = displaylistManager.displaylist[displaylistManager.read_index];
        object[] array = amsDisplaylist.command_buf.array;
        AppMain.AMS_COMMAND_BUFFER_HEADER commandBufferHeader = (AppMain.AMS_COMMAND_BUFFER_HEADER)array[0];
        int num = amsDisplaylist.command_buf_size - 1;
        for (int index = 0; index < num; ++index)
        {
            AppMain.AMS_COMMAND_HEADER ch = (AppMain.AMS_COMMAND_HEADER)array[index + 1];
            if ((int)ch.state == (int)state)
            {
                if (ch.command_id >= 0)
                    AppMain._am_draw_command_func(ch, drawflag);
                else
                    AppMain._am_draw_system_exec[-ch.command_id](ch, drawflag);
            }
        }
    }

    private static void amDrawExecRegist()
    {
        AppMain.AMS_DISPLAYLIST_MANAGER displaylistManager = AppMain._am_displaylist_manager;
        int registNum = displaylistManager.regist_num;
        int index = displaylistManager.reg_read_index;
        int regEndIndex = displaylistManager.reg_end_index;
        if (registNum == 0)
            return;
        int num = 0;
        while (index != regEndIndex)
        {
            AppMain.AMS_REGISTLIST l = displaylistManager.registlist[index];
            index = (index + 1) % 256;
            ++num;
            AppMain._am_draw_regist_func[l.command_id](l);
        }
        displaylistManager.regist_num -= num;
        displaylistManager.reg_read_index = index;
    }

    private static void amDrawAddSort(AppMain.AMS_COMMAND_HEADER command, int key)
    {
        AppMain.AMS_DISPLAYLIST_MANAGER displaylistManager = AppMain._am_displaylist_manager;
        if (displaylistManager.sort_num >= 512)
        {
            AppMain.amSystemLog("[WARN] sort_num over.\n");
        }
        else
        {
            AppMain.AMS_DRAW_SORT amsDrawSort = displaylistManager.sortlist[displaylistManager.sort_num++];
            amsDrawSort.key = key;
            amsDrawSort.command = command;
        }
    }

    private static object _amDrawConvVertex2D(AppMain.NNS_PRIM2D_P vs, int count)
    {
        AppMain.mppAssertNotImpl();
        return (object)null;
    }

    private static object _amDrawConvVertex2D(AppMain.NNS_PRIM2D_PC[] vs, int count)
    {
        AppMain.NNS_PRIM2D_PC[] nnsPriM2DPcArray = AppMain.New<AppMain.NNS_PRIM2D_PC>(count);
        for (int index = 0; index < count; ++index)
        {
            AppMain.amDrawConv2D(nnsPriM2DPcArray[index].Pos, vs[index].Pos);
            nnsPriM2DPcArray[index].Col = vs[index].Col;
        }
        return (object)nnsPriM2DPcArray;
    }

    private static void amDrawConv2D(AppMain.NNS_VECTOR2D vd, AppMain.NNS_VECTOR2D vs)
    {
        vd.Assign(vs);
    }

    private static object _amDrawConvVertex2D(AppMain.NNS_PRIM2D_PCT vs, int count)
    {
        AppMain.mppAssertNotImpl();
        return (object)null;
    }

    private static void amDrawPrimitive2D(int format, int type, object vtx, int count, float pri)
    {
        switch (format)
        {
            case 1:
                vtx = AppMain._amDrawConvVertex2D((AppMain.NNS_PRIM2D_PC[])vtx, count);
                break;
            case 2:
                vtx = AppMain._amDrawConvVertex2D((AppMain.NNS_PRIM2D_PCT)vtx, count);
                break;
        }
        AppMain.nnDrawPrimitive2D(type, vtx, count, pri);
    }

    private static void amDrawPrimitiveLine2D(
      AppMain.NNE_PRIM_LINE type,
      object vtx,
      int count,
      float pri)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void amDrawPrimitivePoint2D(int format, object vtx, int count, float pri)
    {
        AppMain.mppAssertNotImpl();
    }

    public static void amDrawMakeTask(AppMain.TaskProc proc, ushort prio)
    {
        AppMain.amDrawMakeTask(proc, prio, (object)null);
    }

    public static void amDrawMakeTask(AppMain.TaskProc proc, ushort prio, object data)
    {
        AppMain.AMS_PARAM_MAKE_TASK amsParamMakeTask = AppMain.amDraw_AMS_PARAM_MAKE_TASK_Pool.Alloc();
        amsParamMakeTask.prio = (int)prio;
        amsParamMakeTask.proc = proc;
        amsParamMakeTask.work_data = data;
        AppMain.amDrawRegistCommand(16777216U, -1, (object)amsParamMakeTask);
    }

    public static void amDrawSetWorldViewMatrix(AppMain.NNS_MATRIX mtx)
    {
        if (mtx == null)
            mtx = AppMain.amMatrixGetCurrent();
        AppMain.nnCopyMatrix(AppMain._am_draw_world_view_matrix, mtx);
    }

    private static void amDrawMakeTask(AppMain.TaskProc proc, ushort prio, uint data)
    {
        AppMain.amDrawRegistCommand(16777216U, -1, (object)new AppMain.AMS_PARAM_MAKE_TASK()
        {
            prio = (int)prio,
            proc = proc,
            work_data = (object)data
        });
    }

    private static void amDrawPrintColor(uint rgba)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void amDrawObject(
      uint state,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag,
      AppMain.NNS_MATERIALCALLBACK_FUNC func)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void amDrawObjectMaterialMotion(
      uint state,
      AppMain.NNS_OBJECT _object,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag,
      AppMain.NNS_MATERIALCALLBACK_FUNC func)
    {
        AppMain.mppAssertNotImpl();
    }

    public static void amDrawObjectSetMaterial(
      uint state,
      AppMain.NNS_OBJECT _object,
      AppMain.NNS_TEXLIST texlist,
      AppMain.NNS_VECTOR scale,
      ref AppMain.NNS_RGBA color,
      float u,
      float v,
      int blend,
      uint drawflag)
    {
        AppMain.amDrawObjectSetMaterial(state, _object, texlist, scale, ref color, u, v, blend, drawflag, (AppMain.NNS_MATERIALCALLBACK_FUNC)null);
    }

    public static void amDrawObjectSetMaterial(
      uint state,
      AppMain.NNS_OBJECT _object,
      AppMain.NNS_TEXLIST texlist,
      AppMain.NNS_VECTOR scale,
      ref AppMain.NNS_RGBA color,
      float u,
      float v,
      int blend,
      uint drawflag,
      AppMain.NNS_MATERIALCALLBACK_FUNC func)
    {
        AppMain.AMS_PARAM_DRAW_OBJECT_MATERIAL drawObjectMaterial = AppMain.amDrawAlloc_AMS_PARAM_DRAW_OBJECT_MATERIAL();
        AppMain.NNS_MATRIX dst = AppMain.amDrawAlloc_NNS_MATRIX();
        AppMain.nnCopyMatrix(dst, AppMain.amMatrixGetCurrent());
        drawObjectMaterial._object = _object;
        drawObjectMaterial.mtx = dst;
        drawObjectMaterial.sub_obj_type = 0U;
        drawObjectMaterial.flag = drawflag;
        drawObjectMaterial.texlist = texlist;
        drawObjectMaterial.scaleZ = -scale.z;
        AppMain.nnCopyVector(drawObjectMaterial.scale, scale);
        drawObjectMaterial.color = color;
        drawObjectMaterial.scroll_u = u;
        drawObjectMaterial.scroll_v = v;
        drawObjectMaterial.blend = blend;
        drawObjectMaterial.material_func = func;
        AppMain.amDrawRegistCommand(state, -8, (object)drawObjectMaterial);
    }

    private static void amDrawMotion(
      uint state,
      AppMain.NNS_MOTION motion,
      float frame,
      AppMain.NNS_OBJECT _object,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag,
      AppMain.NNS_MATERIALCALLBACK_FUNC func)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void amDrawMotionMaterialMotion(
      uint state,
      AppMain.NNS_MOTION motion,
      float frame,
      AppMain.NNS_OBJECT _object,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag,
      AppMain.NNS_MATERIALCALLBACK_FUNC func)
    {
        AppMain.mppAssertNotImpl();
    }

    public static void amDrawPrimitive3D(uint state, AppMain.AMS_PARAM_DRAW_PRIMITIVE setParam)
    {
        AppMain.AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive = AppMain.amDrawAlloc_AMS_PARAM_DRAW_PRIMITIVE();
        paramDrawPrimitive.Assign(setParam);
        if (paramDrawPrimitive.mtx == null)
        {
            paramDrawPrimitive.mtx = AppMain.amDraw_NNS_MATRIX_Pool.Alloc();
            paramDrawPrimitive.mtx.Assign(AppMain.amMatrixGetCurrent());
        }
        AppMain.amDrawRegistCommand(state, -14, (object)paramDrawPrimitive);
    }

    private static void amDrawPrim2D(uint state, AppMain.AMS_PARAM_DRAW_PRIMITIVE setParam)
    {
        AppMain.AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        paramDrawPrimitive.Assign(setParam);
        AppMain.NNS_MATRIX dst = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.nnCopyMatrix(dst, AppMain.amMatrixGetCurrent());
        paramDrawPrimitive.mtx = dst;
        AppMain.amDrawRegistCommand(state, -13, (object)paramDrawPrimitive);
    }

    private static void amDrawGetPrimBlendParam(int type, AppMain.AMS_PARAM_DRAW_PRIMITIVE setParam)
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
        AppMain.mppAssertNotImpl();
    }

    private static void amDrawSetMaterialAmbient(uint state, int mode, float r, float g, float b)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void amDrawSetMaterialAlpha(uint state, int mode, float alpha)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void amDrawSetMaterialSpecular(uint state, int mode, float r, float g, float b)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void amDrawSetMaterialBlendMode(uint state, int mode)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void amDrawSetMaterialTexOffset(
      uint state,
      int slot,
      int mode,
      float u,
      float v)
    {
        AppMain.mppAssertNotImpl();
    }

    public static void amDrawSetFog(uint state, int flag)
    {
        AppMain.AMS_DRAWSTATE_FOG amsDrawstateFog = AppMain.amDrawAlloc_AMS_DRAWSTATE_FOG();
        amsDrawstateFog.flag = flag & 1;
        AppMain.amDrawRegistCommand(state, -22, (object)amsDrawstateFog);
    }

    public static void amDrawSetFogColor(uint state, float r, float g, float b)
    {
        AppMain.AMS_DRAWSTATE_FOG_COLOR drawstateFogColor = AppMain.amDrawAlloc_AMS_DRAWSTATE_FOG_COLOR();
        drawstateFogColor.r = r;
        drawstateFogColor.g = g;
        drawstateFogColor.b = b;
        AppMain.amDrawRegistCommand(state, -23, (object)drawstateFogColor);
    }

    public static void amDrawSetFogRange(uint state, float fnear, float ffar)
    {
        AppMain.AMS_DRAWSTATE_FOG_RANGE drawstateFogRange = AppMain.amDrawAlloc_AMS_DRAWSTATE_FOG_RANGE();
        drawstateFogRange.fnear = fnear;
        drawstateFogRange.ffar = ffar;
        AppMain.amDrawRegistCommand(state, -24, (object)drawstateFogRange);
    }

    private static void amDrawSetZMode(uint state, bool compare, int func, bool update)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void amDrawObject(
      AppMain.NNS_OBJECT _object,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag,
      AppMain.NNS_MATERIALCALLBACK_FUNC func)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void amDrawObjectMaterialMotion(
      AppMain.NNS_OBJECT _object,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag,
      AppMain.NNS_MATERIALCALLBACK_FUNC func)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void amDrawMotion(
      AppMain.NNS_MOTION motion,
      float frame,
      AppMain.NNS_OBJECT _object,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag,
      AppMain.NNS_MATERIALCALLBACK_FUNC func)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void amDrawMotionMaterialMotion(
      AppMain.NNS_MOTION motion,
      float frame,
      AppMain.NNS_OBJECT _object,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag,
      AppMain.NNS_MATERIALCALLBACK_FUNC func)
    {
        AppMain.AMS_COMMAND_HEADER command = new AppMain.AMS_COMMAND_HEADER();
        AppMain.AMS_PARAM_DRAW_MOTION amsParamDrawMotion = new AppMain.AMS_PARAM_DRAW_MOTION();
        command.command_id = -10;
        command.param = (object)amsParamDrawMotion;
        amsParamDrawMotion._object = _object;
        amsParamDrawMotion.mtx = (AppMain.NNS_MATRIX)null;
        amsParamDrawMotion.sub_obj_type = 0U;
        amsParamDrawMotion.flag = drawflag;
        amsParamDrawMotion.texlist = texlist;
        amsParamDrawMotion.motion = motion;
        amsParamDrawMotion.frame = frame;
        amsParamDrawMotion.material_func = func;
        AppMain._amDrawMotion(command, drawflag);
    }

    private static void amDrawSetMaterialDiffuse(int mode, float r, float g, float b)
    {
        if (mode != -1)
        {
            AppMain.nnSetMaterialControlDiffuse(mode, r, g, b);
            AppMain._am_draw_state.drawflag |= 1048576U;
        }
        else
            AppMain._am_draw_state.drawflag &= 4293918719U;
        AppMain._am_draw_state.diffuse.mode = mode;
        AppMain._am_draw_state.diffuse.r = r;
        AppMain._am_draw_state.diffuse.g = g;
        AppMain._am_draw_state.diffuse.b = b;
    }

    private static void amDrawSetMaterialAmbient(int mode, float r, float g, float b)
    {
        if (mode != -1)
        {
            AppMain.nnSetMaterialControlAmbient(mode, r, g, b);
            AppMain._am_draw_state.drawflag |= 2097152U;
        }
        else
            AppMain._am_draw_state.drawflag &= 4292870143U;
        AppMain._am_draw_state.ambient.mode = mode;
        AppMain._am_draw_state.ambient.r = r;
        AppMain._am_draw_state.ambient.g = g;
        AppMain._am_draw_state.ambient.b = b;
    }

    private static void amDrawSetMaterialAlpha(int mode, float alpha)
    {
        if (mode != -1)
        {
            AppMain.nnSetMaterialControlAlpha(mode, alpha);
            AppMain._am_draw_state.drawflag |= 8388608U;
        }
        else
            AppMain._am_draw_state.drawflag &= 4286578687U;
        AppMain._am_draw_state.alpha.mode = mode;
        AppMain._am_draw_state.alpha.alpha = alpha;
    }

    private static void amDrawSetMaterialSpecular(int mode, float r, float g, float b)
    {
        if (mode != -1)
            AppMain._am_draw_state.drawflag |= 4194304U;
        else
            AppMain._am_draw_state.drawflag &= 4290772991U;
        AppMain._am_draw_state.specular.mode = mode;
        AppMain._am_draw_state.specular.r = r;
        AppMain._am_draw_state.specular.g = g;
        AppMain._am_draw_state.specular.b = b;
    }

    private static void amDrawSetMaterialEnvMap(uint state, int texsrc, AppMain.NNS_MATRIX texmtx)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void amDrawSetMaterialBlendMode(int mode)
    {
        if (mode != -1)
        {
            AppMain.nnSetMaterialControlBlendMode(mode);
            AppMain._am_draw_state.drawflag |= 33554432U;
        }
        else
            AppMain._am_draw_state.drawflag &= 4261412863U;
        AppMain._am_draw_state.blend.mode = mode;
    }

    private static void amDrawSetMaterialTexOffset(int slot, int mode, float u, float v)
    {
        if (slot != -1)
        {
            if (mode != -1)
            {
                AppMain.nnSetMaterialControlTextureOffset(slot, mode, u, v);
                AppMain._am_draw_state.drawflag |= 268435456U;
            }
            else
                AppMain._am_draw_state.drawflag &= 4026531839U;
            AppMain.ArrayPointer<AppMain.AMS_DRAWSTATE_TEXOFFSET> arrayPointer = new AppMain.ArrayPointer<AppMain.AMS_DRAWSTATE_TEXOFFSET>(AppMain._am_draw_state.texoffset, slot);
            ((AppMain.AMS_DRAWSTATE_TEXOFFSET)~arrayPointer).mode = mode;
            ((AppMain.AMS_DRAWSTATE_TEXOFFSET)~arrayPointer).u = u;
            ((AppMain.AMS_DRAWSTATE_TEXOFFSET)~arrayPointer).v = v;
        }
        else
        {
            AppMain.ArrayPointer<AppMain.AMS_DRAWSTATE_TEXOFFSET> arrayPointer = new AppMain.ArrayPointer<AppMain.AMS_DRAWSTATE_TEXOFFSET>(AppMain._am_draw_state.texoffset, 0);
            if (mode != -1)
            {
                int slot1 = 0;
                while (slot1 < 4)
                {
                    AppMain.nnSetMaterialControlTextureOffset(slot1, mode, u, v);
                    AppMain._am_draw_state.drawflag |= 268435456U;
                    ((AppMain.AMS_DRAWSTATE_TEXOFFSET)~arrayPointer).mode = mode;
                    ((AppMain.AMS_DRAWSTATE_TEXOFFSET)~arrayPointer).u = u;
                    ((AppMain.AMS_DRAWSTATE_TEXOFFSET)~arrayPointer).v = v;
                    ++slot1;
                    ++arrayPointer;
                }
            }
            else
            {
                int num = 0;
                while (num < 4)
                {
                    AppMain._am_draw_state.drawflag &= 4026531839U;
                    ((AppMain.AMS_DRAWSTATE_TEXOFFSET)~arrayPointer).mode = mode;
                    ((AppMain.AMS_DRAWSTATE_TEXOFFSET)~arrayPointer).u = u;
                    ((AppMain.AMS_DRAWSTATE_TEXOFFSET)~arrayPointer).v = v;
                    ++num;
                    ++arrayPointer;
                }
            }
        }
    }

    private static void amDrawSetFog(int flag)
    {
        flag &= 1;
        AppMain.nnSetFogSwitch(flag != 0);
        AppMain._am_draw_state.fog.flag = flag;
    }

    private static void amDrawSetFogColor(float r, float g, float b)
    {
        AppMain.nnSetFogColor(r, g, b);
        AppMain._am_draw_state.fog_color.r = r;
        AppMain._am_draw_state.fog_color.g = g;
        AppMain._am_draw_state.fog_color.b = b;
    }

    private static void amDrawSetFogRange(float fnear, float ffar)
    {
        AppMain.nnSetFogRange(fnear, ffar);
        AppMain._am_draw_state.fog_range.fnear = fnear;
        AppMain._am_draw_state.fog_range.ffar = ffar;
    }

    private static void amDrawSetZMode(bool compare, int func, bool update)
    {
        AppMain.mppAssertNotImpl();
        AppMain._am_draw_state.zmode.compare = compare ? 1U : 0U;
        AppMain._am_draw_state.zmode.func = func;
        AppMain._am_draw_state.zmode.update = update ? 1U : 0U;
    }

    private static void _amDrawTaskMake(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.AMS_PARAM_MAKE_TASK amsParamMakeTask = (AppMain.AMS_PARAM_MAKE_TASK)command.param;
        AppMain.amTaskMake(AppMain._am_draw_task, amsParamMakeTask.proc, (AppMain.TaskProc)null, (uint)amsParamMakeTask.prio, 0U, 0U, "DRAW").work = amsParamMakeTask.work_data;
    }

    private static void _amDrawPrintf(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void _amDrawPrintColor(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void _amDrawHeapMap(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void _amDrawThreadMap(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void _amDrawObject(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.NNS_MATRIX nnsMatrix = AppMain.amDrawAlloc_NNS_MATRIX();
        AppMain.amMatrixPush();
        AppMain.AMS_PARAM_DRAW_OBJECT amsParamDrawObject = (AppMain.AMS_PARAM_DRAW_OBJECT)command.param;
        int nNode = amsParamDrawObject._object.nNode;
        if (AppMain.__amDrawObject.plt_mtx == null || AppMain.__amDrawObject.plt_mtx.Length < nNode)
        {
            AppMain.__amDrawObject.nstat = new uint[nNode];
            AppMain.__amDrawObject.plt_mtx = new AppMain.NNS_MATRIX[nNode];
            for (int index = 0; index < nNode; ++index)
                AppMain.__amDrawObject.plt_mtx[index] = new AppMain.NNS_MATRIX();
        }
        AppMain.NNS_MATRIX[] pltMtx = AppMain.__amDrawObject.plt_mtx;
        uint[] nstat = AppMain.__amDrawObject.nstat;
        if (amsParamDrawObject.mtx != null)
        {
            AppMain.nnMultiplyMatrix(nnsMatrix, AppMain.amMatrixGetCurrent(), amsParamDrawObject.mtx);
            AppMain.nnMultiplyMatrix(nnsMatrix, AppMain._am_draw_world_view_matrix, nnsMatrix);
        }
        else
            AppMain.nnMultiplyMatrix(nnsMatrix, AppMain._am_draw_world_view_matrix, AppMain.amMatrixGetCurrent());
        AppMain.nnSetUpNodeStatusList(nstat, nNode, 0U);
        AppMain.nnCalcMatrixPalette(pltMtx, nstat, amsParamDrawObject._object, nnsMatrix, AppMain._am_default_stack, 1U);
        if (amsParamDrawObject.texlist != null)
            AppMain.nnSetTextureList(amsParamDrawObject.texlist);
        AppMain.nnSetMaterialCallback(amsParamDrawObject.material_func);
        if (command.command_id == -6)
            AppMain.nnDrawObject(amsParamDrawObject._object, pltMtx, nstat, (uint)((int)amsParamDrawObject.sub_obj_type | 256 | 512 | 7), amsParamDrawObject.flag | drawflag | AppMain._am_draw_state.drawflag, 0U);
        else
            AppMain.nnDrawMaterialMotionObject(amsParamDrawObject._object, pltMtx, nstat, (uint)((int)amsParamDrawObject.sub_obj_type | 256 | 512 | 7), amsParamDrawObject.flag | drawflag | AppMain._am_draw_state.drawflag);
        if (amsParamDrawObject.material_func != null)
            AppMain.nnSetMaterialCallback((AppMain.NNS_MATERIALCALLBACK_FUNC)null);
        AppMain.amMatrixPop();
    }

    private static void _amDrawObjectSetMaterial(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.AMS_PARAM_DRAW_OBJECT_MATERIAL drawObjectMaterial = (AppMain.AMS_PARAM_DRAW_OBJECT_MATERIAL)command.param;
        AppMain.amDrawPushState();
        if (((int)drawObjectMaterial.flag & 3145728) != 0)
        {
            AppMain.amDrawSetMaterialDiffuse(3, drawObjectMaterial.color.r, drawObjectMaterial.color.g, drawObjectMaterial.color.b);
            AppMain.amDrawSetMaterialAmbient(3, drawObjectMaterial.color.r, drawObjectMaterial.color.g, drawObjectMaterial.color.b);
        }
        if (((int)drawObjectMaterial.flag & 8388608) != 0)
            AppMain.amDrawSetMaterialAlpha(3, drawObjectMaterial.color.a);
        if (((int)drawObjectMaterial.flag & 268435456) != 0)
            AppMain.amDrawSetMaterialTexOffset(0, 1, drawObjectMaterial.scroll_u, drawObjectMaterial.scroll_v);
        command.command_id = -6;
        AppMain._amDrawObject(command, drawflag);
        AppMain.amDrawPopState();
    }

    private static void _amDrawMotion(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void _amDrawMotionTRS(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void _amDrawPrimitive2D(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive1 = (AppMain.AMS_PARAM_DRAW_PRIMITIVE)command.param;
        if (paramDrawPrimitive1.texlist != null)
        {
            AppMain.nnSetPrimitiveTexNum(paramDrawPrimitive1.texlist, paramDrawPrimitive1.texId);
            AppMain.nnSetPrimitiveTexState(0, 0, paramDrawPrimitive1.uwrap, paramDrawPrimitive1.vwrap);
        }
        if (paramDrawPrimitive1.noSort != (short)0 || paramDrawPrimitive1.ablend == 0)
        {
            AppMain._amDrawSetPrimitive2DParam(command, drawflag);
            AppMain.nnBeginDrawPrimitive2D(paramDrawPrimitive1.format2D, paramDrawPrimitive1.ablend);
            switch (paramDrawPrimitive1.format2D)
            {
                case 1:
                    AppMain.amDrawPrimitive2D(paramDrawPrimitive1.format2D, paramDrawPrimitive1.type, (object)paramDrawPrimitive1.vtxPC2D, paramDrawPrimitive1.count, paramDrawPrimitive1.zOffset);
                    break;
                case 2:
                    AppMain.amDrawPrimitive2D(paramDrawPrimitive1.format2D, paramDrawPrimitive1.type, (object)paramDrawPrimitive1.vtxPCT2D, paramDrawPrimitive1.count, paramDrawPrimitive1.zOffset);
                    break;
            }
            AppMain.nnEndDrawPrimitive2D();
        }
        else
        {
            AppMain.AMS_COMMAND_HEADER command1 = new AppMain.AMS_COMMAND_HEADER(command);
            command1.command_id = -3;
            AppMain.AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive2 = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
            AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
            nnsMatrix.Assign(AppMain.amMatrixGetCurrent());
            paramDrawPrimitive2.Assign((AppMain.AMS_PARAM_DRAW_PRIMITIVE)command.param);
            paramDrawPrimitive2.mtx = nnsMatrix;
            command1.param = (object)paramDrawPrimitive2;
            AppMain.amDrawAddSort(command1, (int)((double)paramDrawPrimitive2.zOffset * 100.0));
        }
    }

    private static void _amDrawPrimitive3D(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive1 = (AppMain.AMS_PARAM_DRAW_PRIMITIVE)command.param;
        if (paramDrawPrimitive1.texlist != null && paramDrawPrimitive1.texId != -1)
        {
            AppMain.nnSetPrimitiveTexNum(paramDrawPrimitive1.texlist, paramDrawPrimitive1.texId);
            AppMain.nnSetPrimitiveTexState(0, 0, paramDrawPrimitive1.uwrap, paramDrawPrimitive1.vwrap);
        }
        if (paramDrawPrimitive1.noSort != (short)0 || paramDrawPrimitive1.ablend == 0)
        {
            AppMain._amDrawSetPrimitive3DParam(command, drawflag);
            AppMain.nnBeginDrawPrimitive3D(paramDrawPrimitive1.format3D, paramDrawPrimitive1.ablend, 0, 0);
            switch (paramDrawPrimitive1.format3D)
            {
                case 2:
                    AppMain.nnDrawPrimitive3D(paramDrawPrimitive1.type, (object)paramDrawPrimitive1.vtxPC3D, paramDrawPrimitive1.count);
                    break;
                case 4:
                    AppMain.nnDrawPrimitive3D(paramDrawPrimitive1.type, (object)paramDrawPrimitive1.vtxPCT3D, paramDrawPrimitive1.count);
                    break;
            }
            AppMain.nnEndDrawPrimitive3D();
        }
        else
        {
            AppMain.AMS_COMMAND_HEADER command1 = command;
            command1.command_id = -4;
            AppMain.AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive2 = AppMain.amDrawAlloc_AMS_PARAM_DRAW_PRIMITIVE();
            AppMain.NNS_MATRIX dst = AppMain.amDrawAlloc_NNS_MATRIX();
            AppMain.nnCopyMatrix(dst, AppMain.amMatrixGetCurrent());
            paramDrawPrimitive2.Assign((AppMain.AMS_PARAM_DRAW_PRIMITIVE)command.param);
            paramDrawPrimitive2.mtx = dst;
            command1.param = (object)paramDrawPrimitive2;
            AppMain.amDrawAddSort(command1, (int)((double)paramDrawPrimitive2.sortZ * 100.0));
        }
    }

    private static void _amDrawSortObject(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.mppAssertNotImpl();
    }

    private static AppMain.NNS_MATRIX amDrawGetWorldViewMatrix()
    {
        return AppMain._am_draw_world_view_matrix;
    }

    private static void _amDrawSortPrimitive3D(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.NNS_MATRIX primitive3DBaseMtx = AppMain._amDrawSortPrimitive3D_base_mtx;
        AppMain.AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive = (AppMain.AMS_PARAM_DRAW_PRIMITIVE)command.param;
        if (paramDrawPrimitive.texlist != null && paramDrawPrimitive.texId != -1)
        {
            AppMain.nnSetPrimitiveTexNum(paramDrawPrimitive.texlist, paramDrawPrimitive.texId);
            AppMain.nnSetPrimitiveTexState(0, 0, 0, 0);
        }
        AppMain.nnCopyMatrix(primitive3DBaseMtx, paramDrawPrimitive.mtx);
        AppMain.nnMultiplyMatrix(primitive3DBaseMtx, AppMain.amDrawGetWorldViewMatrix(), primitive3DBaseMtx);
        AppMain.nnSetPrimitive3DMatrix(primitive3DBaseMtx);
        AppMain._amDrawSetPrimitive3DParam(command, drawflag);
        AppMain.nnBeginDrawPrimitive3D(paramDrawPrimitive.format3D, paramDrawPrimitive.ablend, 0, 0);
        switch (paramDrawPrimitive.format3D)
        {
            case 2:
                AppMain.nnDrawPrimitive3D(paramDrawPrimitive.type, (object)paramDrawPrimitive.vtxPC3D, paramDrawPrimitive.count);
                break;
            case 4:
                AppMain.nnDrawPrimitive3D(paramDrawPrimitive.type, (object)paramDrawPrimitive.vtxPCT3D, paramDrawPrimitive.count);
                break;
        }
        AppMain.nnEndDrawPrimitive3D();
    }

    private static void _amDrawSortPrimitive2D(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive = (AppMain.AMS_PARAM_DRAW_PRIMITIVE)command.param;
        AppMain.NNS_MATRIX sortPrimitive2DMtx = AppMain._amDrawSortPrimitive2D_mtx;
        AppMain.nnMakeOrthoMatrix(sortPrimitive2DMtx, 0.0f, 720f, 1080f, 0.0f, 1f, 3f);
        AppMain.amDrawSetProjection(sortPrimitive2DMtx, 1);
        if (paramDrawPrimitive.texlist != null && paramDrawPrimitive.texId != -1)
        {
            AppMain.nnSetPrimitiveTexNum(paramDrawPrimitive.texlist, paramDrawPrimitive.texId);
            AppMain.nnSetPrimitiveTexState(0, 0, 0, 0);
        }
        AppMain._amDrawSetPrimitive2DParam(command, drawflag);
        AppMain.nnBeginDrawPrimitive2D(paramDrawPrimitive.format2D, paramDrawPrimitive.ablend);
        switch (paramDrawPrimitive.format2D)
        {
            case 1:
                AppMain.nnDrawPrimitive2D(paramDrawPrimitive.type, (object)paramDrawPrimitive.vtxPC2D, paramDrawPrimitive.count, paramDrawPrimitive.zOffset);
                break;
            case 2:
                AppMain.nnDrawPrimitive2D(paramDrawPrimitive.type, (object)paramDrawPrimitive.vtxPCT2D, paramDrawPrimitive.count, paramDrawPrimitive.zOffset);
                break;
        }
        AppMain.nnEndDrawPrimitive2D();
    }

    private static void _amDrawSetDiffuse(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.AMS_DRAWSTATE_DIFFUSE drawstateDiffuse = (AppMain.AMS_DRAWSTATE_DIFFUSE)command.param;
        AppMain.amDrawSetMaterialDiffuse(drawstateDiffuse.mode, drawstateDiffuse.r, drawstateDiffuse.g, drawstateDiffuse.b);
    }

    private static void _amDrawSetAmbient(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.AMS_DRAWSTATE_AMBIENT drawstateAmbient = (AppMain.AMS_DRAWSTATE_AMBIENT)command.param;
        AppMain.amDrawSetMaterialAmbient(drawstateAmbient.mode, drawstateAmbient.r, drawstateAmbient.g, drawstateAmbient.b);
    }

    private static void _amDrawSetAlpha(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.AMS_DRAWSTATE_ALPHA amsDrawstateAlpha = (AppMain.AMS_DRAWSTATE_ALPHA)command.param;
        AppMain.amDrawSetMaterialAlpha(amsDrawstateAlpha.mode, amsDrawstateAlpha.alpha);
    }

    private static void _amDrawSetSpecular(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.AMS_DRAWSTATE_SPECULAR drawstateSpecular = (AppMain.AMS_DRAWSTATE_SPECULAR)command.param;
        AppMain.amDrawSetMaterialSpecular(drawstateSpecular.mode, drawstateSpecular.r, drawstateSpecular.g, drawstateSpecular.b);
    }

    private static void _amDrawSetEnvMap(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void _amDrawSetBlend(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.amDrawSetMaterialBlendMode(((AppMain.AMS_DRAWSTATE_BLEND)command.param).mode);
    }

    private static void _amDrawSetTexOffset(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.AMS_PARAM_SET_TEXOFFSET paramSetTexoffset = (AppMain.AMS_PARAM_SET_TEXOFFSET)command.param;
        AppMain.amDrawSetMaterialTexOffset(paramSetTexoffset.slot, paramSetTexoffset.texoffset.mode, paramSetTexoffset.texoffset.u, paramSetTexoffset.texoffset.v);
    }

    private static void _amDrawSetFog(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.amDrawSetFog(((AppMain.AMS_DRAWSTATE_FOG)command.param).flag);
    }

    private static void _amDrawSetFogColor(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.AMS_DRAWSTATE_FOG_COLOR drawstateFogColor = (AppMain.AMS_DRAWSTATE_FOG_COLOR)command.param;
        AppMain.amDrawSetFogColor(drawstateFogColor.r, drawstateFogColor.g, drawstateFogColor.b);
    }

    private static void _amDrawSetFogRange(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.AMS_DRAWSTATE_FOG_RANGE drawstateFogRange = (AppMain.AMS_DRAWSTATE_FOG_RANGE)command.param;
        AppMain.amDrawSetFogRange(drawstateFogRange.fnear, drawstateFogRange.ffar);
    }

    private static void _amDrawSetZMode(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.AMS_DRAWSTATE_Z_MODE amsDrawstateZMode = (AppMain.AMS_DRAWSTATE_Z_MODE)command.param;
        AppMain.amDrawSetZMode(0U != amsDrawstateZMode.compare, amsDrawstateZMode.func, 0U != amsDrawstateZMode.update);
    }

    private static void _amDrawSetPrimitive3DParam(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive = (AppMain.AMS_PARAM_DRAW_PRIMITIVE)command.param;
        if (paramDrawPrimitive.aTest != (short)0)
            AppMain.nnSetPrimitive3DAlphaFuncGL(516U, 0.5f);
        else
            AppMain.nnSetPrimitive3DAlphaFuncGL(519U, 0.5f);
        if (paramDrawPrimitive.zMask != (short)0)
            AppMain.nnSetPrimitive3DDepthMaskGL(false);
        else
            AppMain.nnSetPrimitive3DDepthMaskGL(true);
        if (paramDrawPrimitive.zTest != (short)0)
            AppMain.nnSetPrimitive3DDepthFuncGL(515U);
        else
            AppMain.nnSetPrimitive3DDepthFuncGL(519U);
        if (paramDrawPrimitive.ablend == 0 || paramDrawPrimitive.bldMode != 32774)
            return;
        switch ((uint)paramDrawPrimitive.bldDst)
        {
            case 1:
                AppMain.nnSetPrimitiveBlend(0);
                break;
            case 771:
                AppMain.nnSetPrimitiveBlend(1);
                break;
            default:
                AppMain.nnSetPrimitiveBlend(1);
                break;
        }
    }

    private static void _amDrawSetPrimitive2DParam(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.AMS_PARAM_DRAW_PRIMITIVE paramDrawPrimitive = (AppMain.AMS_PARAM_DRAW_PRIMITIVE)command.param;
        if (paramDrawPrimitive.aTest != (short)0)
            AppMain.nnSetPrimitive2DAlphaFuncGL(516U, 0.5f);
        else
            AppMain.nnSetPrimitive2DAlphaFuncGL(519U, 0.5f);
        if (paramDrawPrimitive.zMask != (short)0)
            AppMain.nnSetPrimitive3DDepthMaskGL(false);
        else
            AppMain.nnSetPrimitive3DDepthMaskGL(true);
        if (paramDrawPrimitive.zTest != (short)0)
            AppMain.nnSetPrimitive3DDepthFuncGL(515U);
        else
            AppMain.nnSetPrimitive3DDepthFuncGL(519U);
        if (paramDrawPrimitive.ablend == 0 || paramDrawPrimitive.bldMode != 32774)
            return;
        switch (paramDrawPrimitive.bldDst)
        {
            case 1:
                AppMain.nnSetPrimitiveBlend(0);
                break;
            case 771:
                AppMain.nnSetPrimitiveBlend(1);
                break;
            default:
                AppMain.nnSetPrimitiveBlend(1);
                break;
        }
    }

    private static void _amDrawRegistNop(AppMain.AMS_REGISTLIST regist)
    {
        AppMain.mppAssertNotImpl();
        regist.command_id = 0;
    }

    private static void _amDrawLoadTexture(AppMain.AMS_REGISTLIST regist)
    {
        AppMain.AMS_PARAM_LOAD_TEXTURE paramLoadTexture = (AppMain.AMS_PARAM_LOAD_TEXTURE)regist.param;
        OpenGL.glGenTexture(out paramLoadTexture.pTexInfo.TexName);
        OpenGL.glBindTexture(3553U, paramLoadTexture.pTexInfo.TexName);
        OpenGL.mppglTexImage2D(paramLoadTexture.tex);
        AppMain._amSetTextureAttribute(paramLoadTexture);
        paramLoadTexture.buf_delete = (byte[])null;
        regist.command_id = 0;
    }

    private static void _amSetTextureAttribute(AppMain.AMS_PARAM_LOAD_TEXTURE param)
    {
        param.pTexInfo.Bank = param.bank;
        param.pTexInfo.GlobalIndex = param.globalIndex;
        param.pTexInfo.Flag = 0U;
    }

    private static void _amDrawReleaseTexture(AppMain.AMS_REGISTLIST regist)
    {
        AppMain.AMS_PARAM_RELEASE_TEXTURE paramReleaseTexture = (AppMain.AMS_PARAM_RELEASE_TEXTURE)regist.param;
        int nTex = paramReleaseTexture.texlist.nTex;
        AppMain.ArrayPointer<AppMain.NNS_TEXINFO> pTexInfoList = (AppMain.ArrayPointer<AppMain.NNS_TEXINFO>)paramReleaseTexture.texlist.pTexInfoList;
        int num = nTex - 1;
        AppMain.ArrayPointer<AppMain.NNS_TEXINFO> arrayPointer = (AppMain.ArrayPointer<AppMain.NNS_TEXINFO>)(pTexInfoList + (nTex - 1));
        while (num >= 0)
        {
            if (((AppMain.NNS_TEXINFO)~arrayPointer).TexName != 0U)
                OpenGL.glDeleteTextures(1, new uint[1]
                {
          ((AppMain.NNS_TEXINFO) ~arrayPointer).TexName
                });
            --num;
            --arrayPointer;
        }
        paramReleaseTexture.texlist.nTex = -1;
        paramReleaseTexture.texlist.pTexInfoList = (AppMain.NNS_TEXINFO[])null;
        paramReleaseTexture.texlist = (AppMain.NNS_TEXLIST)null;
        regist.command_id = 0;
    }

    private static void _amDrawVertexBufferObject(AppMain.AMS_REGISTLIST regist)
    {
        AppMain.AMS_PARAM_VERTEX_BUFFER_OBJECT vertexBufferObject = (AppMain.AMS_PARAM_VERTEX_BUFFER_OBJECT)regist.param;
        int num = (int)AppMain.nnBindBufferObjectGL(vertexBufferObject.obj, vertexBufferObject.srcobj, vertexBufferObject.bindflag);
        regist.command_id = 0;
    }

    private static void _amDrawDeleteVertexObject(AppMain.AMS_REGISTLIST regist)
    {
        AppMain.nnDeleteBufferObjectGL(((AppMain.AMS_PARAM_DELETE_VERTEX_OBJECT)regist.param).obj);
        regist.command_id = 0;
    }

    private static void _amDrawLoadShaderObject(AppMain.AMS_REGISTLIST regist)
    {
        AppMain.mppAssertNotImpl();
        regist.command_id = 0;
    }

    private static void _amDrawReleaseStdShader(AppMain.AMS_REGISTLIST regist)
    {
        AppMain.mppAssertNotImpl();
        regist.command_id = 0;
    }

    private static void _amDrawLoadShader(AppMain.AMS_REGISTLIST regist)
    {
        AppMain.mppAssertNotImpl();
        regist.command_id = 0;
    }

    private static void _amDrawBuildShader(AppMain.AMS_REGISTLIST regist)
    {
        AppMain.mppAssertNotImpl();
        regist.command_id = 0;
    }

    private static void _amDrawCreateShader(AppMain.AMS_REGISTLIST regist)
    {
        AppMain.mppAssertNotImpl();
        regist.command_id = 0;
    }

    private static void _amDrawReleaseShader(AppMain.AMS_REGISTLIST regist)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void _amDrawLoadTextureImage(AppMain.AMS_REGISTLIST regist)
    {
        AppMain.mppAssertNotImpl();
        AppMain.AMS_PARAM_LOAD_TEXTURE_IMAGE loadTextureImage = (AppMain.AMS_PARAM_LOAD_TEXTURE_IMAGE)regist.param;
        regist.command_id = 0;
    }

    private static void _amDrawReleaseTextureImage(AppMain.AMS_REGISTLIST regist)
    {
        AppMain.mppAssertNotImpl();
        AppMain.AMS_PARAM_RELEASE_TEXTURE_IMAGE releaseTextureImage = (AppMain.AMS_PARAM_RELEASE_TEXTURE_IMAGE)regist.param;
        if (releaseTextureImage.texture != null)
        {
            releaseTextureImage.texture.Dispose();
            releaseTextureImage.texture = (Texture2D)null;
        }
        regist.command_id = 0;
    }

    public static void amMatrixPush(ref AppMain.SNNS_MATRIX mtx)
    {
        AppMain.nnPushMatrix(AppMain._amMatrixGetCurrentStack(), ref mtx);
    }

    public static void amMatrixPush(AppMain.NNS_MATRIX mtx)
    {
        AppMain.nnPushMatrix(AppMain._amMatrixGetCurrentStack(), mtx);
    }

    public static void amMatrixPush()
    {
        AppMain.nnPushMatrix(AppMain._amMatrixGetCurrentStack());
    }

    public static void amMatrixPop()
    {
        AppMain.nnPopMatrix(AppMain._amMatrixGetCurrentStack());
    }

    public static AppMain.NNS_MATRIX amMatrixGetCurrent()
    {
        return AppMain.nnGetCurrentMatrix(AppMain._amMatrixGetCurrentStack());
    }

    public static void amMatrixSetCurrent(AppMain.NNS_MATRIX m)
    {
        AppMain.nnSetCurrentMatrix(AppMain._amMatrixGetCurrentStack(), m);
    }

    public static void amMatrixClearStack()
    {
        AppMain.nnClearMatrixStack(AppMain._amMatrixGetCurrentStack());
    }

    public static void amMatrixCalcPoint(
      ref AppMain.SNNS_VECTOR4D pDst,
      ref AppMain.SNNS_VECTOR4D pSrc)
    {
        AppMain.NNS_MATRIX current = AppMain.amMatrixGetCurrent();
        AppMain.nnTransformVector(ref pDst, current, ref pSrc);
        pDst.w = pSrc.w;
    }

    public static void amMatrixCalcPoint(AppMain.NNS_VECTOR4D pDst, AppMain.NNS_VECTOR4D pSrc)
    {
        AppMain.NNS_MATRIX current = AppMain.amMatrixGetCurrent();
        AppMain.nnTransformVector(pDst, current, pSrc);
        pDst.w = pSrc.w;
    }

    public static void amMatrixCalcPoint(AppMain.NNS_VECTOR pDst, AppMain.NNS_VECTOR pSrc)
    {
        AppMain.NNS_MATRIX current = AppMain.amMatrixGetCurrent();
        AppMain.nnTransformVector(pDst, current, pSrc);
    }

    public static void amMatrixCalcVector(AppMain.NNS_VECTOR pDst, AppMain.NNS_VECTOR pSrc)
    {
        AppMain.NNS_MATRIX current = AppMain.amMatrixGetCurrent();
        AppMain.nnTransformNormalVector(pDst, current, pSrc);
    }

    public static void amMatrixCalcVector(AppMain.NNS_VECTOR4D pDst, AppMain.NNS_VECTOR4D pSrc)
    {
        AppMain.NNS_MATRIX current = AppMain.amMatrixGetCurrent();
        AppMain.nnTransformNormalVector(pDst, current, pSrc);
    }

    public static void amMatrixCalcVector(
      ref AppMain.SNNS_VECTOR4D pDst,
      ref AppMain.SNNS_VECTOR4D pSrc)
    {
        AppMain.NNS_MATRIX current = AppMain.amMatrixGetCurrent();
        AppMain.nnTransformNormalVector(ref pDst, current, ref pSrc);
    }

    public static void amMatrixCalcVector(ref AppMain.SNNS_VECTOR4D pDst, AppMain.NNS_VECTOR4D pSrc)
    {
        AppMain.NNS_MATRIX current = AppMain.amMatrixGetCurrent();
        AppMain.nnTransformNormalVector(ref pDst, current, pSrc);
    }

    public static AppMain.NNS_MATRIXSTACK _amMatrixGetCurrentStack()
    {
        return AppMain._am_default_stack;
    }

    private static void amRenderInit()
    {
        AppMain.AMS_RENDER_MANAGER amsRenderManager = new AppMain.AMS_RENDER_MANAGER();
        AppMain.AMS_RENDER_TARGET amsRenderTarget = new AppMain.AMS_RENDER_TARGET();
        AppMain.AMS_RENDER_TARGET amRenderDefault = AppMain._am_render_default;
        AppMain.AMS_RENDER_MANAGER amRenderManager = AppMain._am_render_manager;
        amRenderManager.targetp = AppMain._am_render_default;
        amRenderManager.target_now = AppMain._am_render_default;
    }

    private static AppMain.AMS_RENDER_TARGET amRenderSetTarget(
      AppMain.AMS_RENDER_TARGET target,
      uint flag,
      AppMain.NNS_RGBA_U8 color)
    {
        return AppMain.amRenderSetTarget(target, flag, color, 1f, 0);
    }

    private static AppMain.AMS_RENDER_TARGET amRenderSetTarget(AppMain.AMS_RENDER_TARGET target)
    {
        return AppMain.amRenderSetTarget(target, 0U, (AppMain.NNS_RGBA_U8)null, 1f, 0);
    }

    private static AppMain.AMS_RENDER_TARGET amRenderSetTarget(
      AppMain.AMS_RENDER_TARGET target,
      uint flag,
      AppMain.NNS_RGBA_U8 color,
      float z,
      int stencil)
    {
        return (AppMain.AMS_RENDER_TARGET)null;
    }

    private static void amRenderSetTexture(int slot, AppMain.AMS_RENDER_TARGET target, int index)
    {
    }

    private static void amRenderCopyTarget(
      AppMain.AMS_RENDER_TARGET target,
      AppMain.NNS_RGBA_U8 color)
    {
    }
}