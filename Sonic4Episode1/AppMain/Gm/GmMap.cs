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

    public static uint GMD_MAP_DRAW_WIDTH
    {
        get
        {
            return AppMain.gm_map_draw_size[0];
        }
    }

    public static uint GMD_MAP_DRAW_HEIGHT
    {
        get
        {
            return AppMain.gm_map_draw_size[1];
        }
    }

    public static bool GMM_MAP_IS_RANGE(int _src, int _min, int _max)
    {
        return _min < _src && _src < _max;
    }

    public static bool GMM_MAP_IS_RANGE(float _src, float _min, float _max)
    {
        return (double)_min < (double)_src && (double)_src < (double)_max;
    }

    private static void GmMapBuildDataInit()
    {
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 5)
            return;
        AppMain.gm_map_reg_obj3d_num = 1;
        AppMain.gmMapBuildDrawMapTvxTexScroll();
        AppMain.gm_map_tex_load_init = true;
    }

    private static bool GmMapBuildDataLoop()
    {
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 5)
            return true;
        if (AppMain._am_displaylist_manager.regist_num >= 192)
            return false;
        if (AppMain.gm_map_tex_load_init)
        {
            AppMain.AoTexBuild(AppMain.gm_map_texture, (AppMain.AMS_AMB_HEADER)AppMain.g_gm_gamedat_map[2]);
            AppMain.AoTexLoad(AppMain.gm_map_texture);
            AppMain.gm_map_tex_load_init = false;
        }
        if (!AppMain.AoTexIsLoaded(AppMain.gm_map_texture))
            return false;
        AppMain.gm_map_tex_draw_count = 1;
        return true;
    }

    private static AppMain.DF_HEADER readDFFile(AppMain.AmbChunk data)
    {
        AppMain.DF_HEADER dfHeader = new AppMain.DF_HEADER()
        {
            block_num = BitConverter.ToUInt32(data.array, data.offset)
        };
        dfHeader.blocks = new AppMain.DF_BLOCK[(int)dfHeader.block_num];
        int num = data.offset + 4;
        for (int index1 = 0; (long)index1 < (long)dfHeader.block_num; ++index1)
        {
            AppMain.DF_BLOCK dfBlock = new AppMain.DF_BLOCK();
            dfHeader.blocks[index1] = dfBlock;
            for (int index2 = 0; index2 < 8; ++index2)
            {
                for (int index3 = 0; index3 < 8; ++index3)
                {
                    dfBlock.df[index2, index3].Data = data.array;
                    dfBlock.df[index2, index3].Offset = num;
                    num += 8;
                }
            }
        }
        return dfHeader;
    }

    private static AppMain.DI_HEADER readDIFile(AppMain.AmbChunk data)
    {
        AppMain.DI_HEADER diHeader = new AppMain.DI_HEADER();
        using (MemoryStream memoryStream = new MemoryStream(data.array, data.offset, data.array.Length - data.offset))
        {
            using (BinaryReader binaryReader = new BinaryReader((Stream)memoryStream))
            {
                diHeader.block_num = binaryReader.ReadUInt32();
                diHeader.blocks = new AppMain.DI_BLOCK[(int)diHeader.block_num];
                for (int index1 = 0; (long)index1 < (long)diHeader.block_num; ++index1)
                {
                    diHeader.blocks[index1] = new AppMain.DI_BLOCK();
                    for (int index2 = 0; index2 < 8; ++index2)
                        diHeader.blocks[index1].di[index2] = binaryReader.ReadBytes(8);
                }
            }
        }
        return diHeader;
    }

    private static AppMain.AT_HEADER readATFile(AppMain.AmbChunk data)
    {
        AppMain.AT_HEADER atHeader = new AppMain.AT_HEADER();
        using (MemoryStream memoryStream = new MemoryStream(data.array, data.offset, data.array.Length - data.offset))
        {
            using (BinaryReader binaryReader = new BinaryReader((Stream)memoryStream))
            {
                atHeader.block_num = binaryReader.ReadUInt32();
                atHeader.blocks = new AppMain.AT_BLOCK[(int)atHeader.block_num];
                for (int index1 = 0; (long)index1 < (long)atHeader.block_num; ++index1)
                {
                    atHeader.blocks[index1] = new AppMain.AT_BLOCK();
                    for (int index2 = 0; index2 < 8; ++index2)
                        atHeader.blocks[index1].at[index2] = binaryReader.ReadBytes(8);
                }
            }
        }
        return atHeader;
    }

    private static AppMain.MP_HEADER readMPFile(AppMain.AmbChunk data)
    {
        if (data.length == 0)
            return (AppMain.MP_HEADER)null;
        AppMain.MP_HEADER mpHeader = new AppMain.MP_HEADER();
        using (MemoryStream memoryStream = new MemoryStream(data.array, data.offset, data.array.Length - data.offset))
        {
            using (BinaryReader binaryReader = new BinaryReader((Stream)memoryStream))
            {
                mpHeader.map_w = binaryReader.ReadUInt16();
                mpHeader.map_h = binaryReader.ReadUInt16();
                int length = (int)mpHeader.map_w * (int)mpHeader.map_h;
                mpHeader.blocks = new AppMain.MP_BLOCK[length];
                for (int index = 0; index < length; ++index)
                    mpHeader.blocks[index] = new AppMain.MP_BLOCK(binaryReader.ReadUInt16());
            }
        }
        return mpHeader;
    }

    private static AppMain.DC_HEADER readDCFile(byte[] data)
    {
        AppMain.DC_HEADER dcHeader = new AppMain.DC_HEADER();
        AppMain.mppAssertNotImpl();
        return dcHeader;
    }

    private static AppMain.MD_HEADER readMDFile(AppMain.AmbChunk data)
    {
        if (data.length == 0)
            return (AppMain.MD_HEADER)null;
        AppMain.MD_HEADER mdHeader = new AppMain.MD_HEADER();
        using (MemoryStream memoryStream = new MemoryStream(data.array, data.offset, data.array.Length - data.offset))
        {
            using (BinaryReader binaryReader = new BinaryReader((Stream)memoryStream))
            {
                mdHeader.map_w = binaryReader.ReadUInt16();
                mdHeader.map_h = binaryReader.ReadUInt16();
                int length = (int)mdHeader.map_w * (int)mdHeader.map_h;
                mdHeader.blocks = new AppMain.MD_BLOCK[length];
                for (int index = 0; index < length; ++index)
                    mdHeader.blocks[index] = new AppMain.MD_BLOCK(binaryReader.ReadSByte());
            }
        }
        return mdHeader;
    }

    private static AppMain.RG_HEADER readRGFile(byte[] data)
    {
        AppMain.RG_HEADER rgHeader = new AppMain.RG_HEADER();
        AppMain.mppAssertNotImpl();
        return rgHeader;
    }

    private static AppMain.EV_HEADER readEVFile(byte[] data)
    {
        AppMain.EV_HEADER evHeader = new AppMain.EV_HEADER();
        AppMain.mppAssertNotImpl();
        return evHeader;
    }

    private static void GmMapBuildColData()
    {
        AppMain.MP_HEADER gGmGamedatMap1 = (AppMain.MP_HEADER)AppMain.g_gm_gamedat_map_set[4];
        AppMain.MP_HEADER gGmGamedatMap2 = (AppMain.MP_HEADER)AppMain.g_gm_gamedat_map_set[5];
        AppMain.g_gm_main_system.map_fcol.map_block_num_x = gGmGamedatMap1.map_w;
        AppMain.g_gm_main_system.map_fcol.map_block_num_y = gGmGamedatMap1.map_h;
        AppMain.g_gm_main_system.map_fcol.block_map_datap[0] = gGmGamedatMap1.blocks;
        AppMain.g_gm_main_system.map_fcol.block_map_datap[1] = gGmGamedatMap2.blocks;
        AppMain.DF_HEADER dfHeader = AppMain.readDFFile((AppMain.AmbChunk)AppMain.g_gm_gamedat_map_attr_set[1]);
        AppMain.DI_HEADER diHeader = AppMain.readDIFile((AppMain.AmbChunk)AppMain.g_gm_gamedat_map_attr_set[2]);
        AppMain.AT_HEADER atHeader = AppMain.readATFile((AppMain.AmbChunk)AppMain.g_gm_gamedat_map_attr_set[0]);
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)9)
        {
            for (int index1 = 0; (long)index1 < (long)atHeader.block_num; ++index1)
            {
                for (int index2 = 0; index2 < 8; ++index2)
                {
                    for (int index3 = 0; index3 < 8; ++index3)
                        atHeader.blocks[index1].at[index2][index3] &= (byte)253;
                }
            }
        }
        AppMain.g_gm_main_system.map_fcol.diff_block_num = dfHeader.block_num;
        AppMain.g_gm_main_system.map_fcol.dir_block_num = diHeader.block_num;
        AppMain.g_gm_main_system.map_fcol.attr_block_num = atHeader.block_num;
        AppMain.g_gm_main_system.map_fcol.cl_diff_datap = dfHeader.blocks;
        AppMain.g_gm_main_system.map_fcol.direc_datap = diHeader.blocks;
        AppMain.g_gm_main_system.map_fcol.char_attr_datap = atHeader.blocks;
        AppMain.g_gm_main_system.map_fcol.left = 0;
        AppMain.g_gm_main_system.map_fcol.top = 0;
        AppMain.g_gm_main_system.map_fcol.right = (int)AppMain.g_gm_main_system.map_fcol.map_block_num_x * 64;
        AppMain.g_gm_main_system.map_fcol.bottom = (int)AppMain.g_gm_main_system.map_fcol.map_block_num_y * 64;
        AppMain.g_gm_main_system.map_size[0] = (int)AppMain.g_gm_main_system.map_fcol.map_block_num_x * 64;
        AppMain.g_gm_main_system.map_size[1] = (int)AppMain.g_gm_main_system.map_fcol.map_block_num_y * 64;
    }

    private static void GmMapFlushData()
    {
        AppMain.gm_map_release_obj3d_num = 1;
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 5)
            return;
        AppMain.AoTexRelease(AppMain.gm_map_texture);
        AppMain.gmMapFlushDrawMapTvxTexScroll();
    }

    private static bool GmMapFlushDataLoop()
    {
        return AppMain.GMM_MAIN_GET_ZONE_TYPE() == 5 || AppMain.AoTexIsReleased(AppMain.gm_map_texture);
    }

    private static void GmMapFlushColData()
    {
    }

    private static void GmMapRelease()
    {
        for (int index = 0; index < 4; ++index)
            AppMain.g_gm_gamedat_map[index] = (object)null;
    }

    private static void GmMapInit()
    {
        AppMain.ObjSetDiffCollision(AppMain.g_gm_main_system.map_fcol);
        AppMain.gmMapTransX = AppMain.g_gs_main_sys_info.stage_id < (ushort)0 || AppMain.g_gs_main_sys_info.stage_id > (ushort)2 ? 0.0f : 1f;
        AppMain.gm_map_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMapMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMapDest), 0U, (ushort)0, 12288U, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_MAP_SYS_WORK()), "GM_MAP_MAIN");
        AppMain.GMS_MAP_SYS_WORK gmsMapSysWork = new AppMain.GMS_MAP_SYS_WORK();
        AppMain.gm_map_tcb.work = (object)gmsMapSysWork;
        AppMain.gm_map_draw_command_state = 0U;
        AppMain.gm_map_draw_margin_adjust = 0U;
        uint stageId = (uint)AppMain.g_gs_main_sys_info.stage_id;
        AppMain.GmMapSetMapDrawSize(1);
        gmsMapSysWork.auto_resize = true;
        switch (stageId)
        {
            case 0:
            case 1:
            case 2:
            case 4:
            case 6:
                AppMain.GmMapSetMapDrawSize(0);
                gmsMapSysWork.auto_resize = false;
                break;
            case 3:
            case 7:
            case 11:
            case 15:
            case 16:
                gmsMapSysWork.auto_resize = false;
                break;
        }
        AppMain.gmMapCreateUsePrimMatrix();
        uint num = 3772834047;
        if (stageId == 0U || stageId == 1U)
            num = uint.MaxValue;
        if (stageId == 2U || stageId == 3U)
            num = 3767175935U;
        else if (stageId == 14U)
            num = 1616929023U;
        AppMain.gm_map_prim_draw_tvx_color = num;
        AppMain.gm_map_prim_draw_tvx_alpha_set = (int[])null;
        if (stageId == 4U || stageId == 5U || (stageId == 6U || stageId == 7U))
            AppMain.gm_map_prim_draw_tvx_alpha_set = AppMain.gm_map_prim_draw_tvx_alpha_set_z2;
        AppMain.gm_map_draw_bgm_timer = AppMain.GMD_MAP_DRAW_BGM_TIMER;
        AppMain.GMS_MAP_OTHER_MAP_STATE[] mapState = gmsMapSysWork.map_state;
        for (int index = 0; index < 5; ++index)
        {
            if (AppMain.g_gm_gamedat_map_set_add[index * 2] != null && AppMain.g_gm_gamedat_map_set_add[index * 2 + 1] != null)
            {
                gmsMapSysWork.flag |= (uint)(1 << index);
                mapState[index].pos_z = AppMain.gm_map_addmap_pos_z_tbl[index];
                AppMain.MP_HEADER mpHeader = (AppMain.MP_HEADER)AppMain.g_gm_gamedat_map_set_add[index * 2];
                mapState[index].map_block_num[0] = (int)mpHeader.map_w;
                mapState[index].map_block_num[1] = (int)mpHeader.map_h;
                mapState[index].map_size[0] = mapState[index].map_block_num[0] * 64;
                mapState[index].map_size[1] = mapState[index].map_block_num[1] * 64;
                mapState[index].scrl_scale[0] = (float)(((double)mapState[index].map_size[0] - (double)AppMain.OBD_LCD_X) / ((double)AppMain.g_gm_main_system.map_size[0] - (double)AppMain.OBD_LCD_X));
                mapState[index].scrl_scale[1] = (float)(((double)mapState[index].map_size[1] - (double)AppMain.OBD_LCD_Y) / ((double)AppMain.g_gm_main_system.map_size[1] - (double)AppMain.OBD_LCD_Y));
                mapState[index].command_state = AppMain.g_gs_main_sys_info.stage_id == (ushort)1 || AppMain.g_gs_main_sys_info.stage_id == (ushort)2 || (AppMain.g_gs_main_sys_info.stage_id == (ushort)8 || AppMain.g_gs_main_sys_info.stage_id == (ushort)10) ? AppMain.gm_map_addmap_command_state_z1_act2_3_z3_act1_3_tbl[index] : (AppMain.g_gs_main_sys_info.stage_id != (ushort)16 ? AppMain.gm_map_addmap_command_state_tbl[index] : AppMain.gm_map_addmap_command_state_zf_tbl[index]);
            }
        }
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)9)
        {
            gmsMapSysWork.map_state[2].pos_z = 160f;
            gmsMapSysWork.map_state[3].pos_z = -96f;
        }
        else
        {
            if (AppMain.g_gs_main_sys_info.stage_id != (ushort)16)
                return;
            gmsMapSysWork.map_state[1].pos_z += -64f;
            gmsMapSysWork.map_state[2].pos_z += -64f;
            gmsMapSysWork.map_state[3].pos_z += -64f;
        }
    }

    private static void GmMapExit()
    {
        if (AppMain.gm_map_tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.gm_map_tcb);
    }

    private static void GmMapSetDrawState(uint command_state)
    {
        AppMain.gm_map_draw_command_state = command_state;
    }

    private static void GmMapSetDrawMarginNormal()
    {
        AppMain.gm_map_draw_margin_adjust = 0U;
    }

    private static void GmMapSetDrawMarginMag()
    {
        AppMain.gm_map_draw_margin_adjust = 1U;
    }

    private static void GmMapDrawMap(
      AppMain.OBS_ACTION3D_NN_WORK obj3d_tbl,
      AppMain.MP_HEADER mp_header,
      AppMain.MD_HEADER md_header,
      float pos_x,
      float pos_y,
      float trans_x,
      float trans_y,
      float trans_z,
      bool loop_h)
    {
        float num1 = 0.0f;
        float num2 = 0.0f;
        int num3 = (int)pos_x >> 6;
        int num4 = (int)pos_y >> 6;
        int num5 = (int)AppMain.GMD_MAP_DRAW_WIDTH + 2 + (int)AppMain.gm_map_draw_margin_adjust * 2;
        int num6 = (int)AppMain.GMD_MAP_DRAW_HEIGHT + 2 + (int)AppMain.gm_map_draw_margin_adjust * 2;
        int block_left = num3 - (num5 >> 1);
        int block_right = num3 + (num5 >> 1);
        int block_top = num4 - (num6 >> 1);
        int block_bottom = num4 + (num6 >> 1);
        if (block_left < 0)
        {
            if (!loop_h)
            {
                block_left = 0;
            }
            else
            {
                block_left += (int)mp_header.map_w;
                num1 = (float)-((int)mp_header.map_w << 6);
            }
        }
        else if (block_left >= (int)mp_header.map_w)
        {
            if (!loop_h)
            {
                block_left = (int)mp_header.map_w - num5;
            }
            else
            {
                block_left -= (int)mp_header.map_w;
                num1 = (float)((int)mp_header.map_w << 6);
            }
        }
        if (block_top < 0)
            block_top = 0;
        if (block_right >= (int)mp_header.map_w)
        {
            if (!loop_h)
            {
                block_right = (int)mp_header.map_w - 1;
            }
            else
            {
                block_right -= (int)mp_header.map_w;
                num2 = (float)((int)mp_header.map_w << 6);
            }
        }
        else if (block_right < 0)
        {
            if (!loop_h)
            {
                block_right = num5;
            }
            else
            {
                block_right += (int)mp_header.map_w;
                num2 = (float)-((int)mp_header.map_w << 6);
            }
        }
        if (block_bottom >= (int)mp_header.map_h)
            block_bottom = (int)mp_header.map_h - 1;
        if (block_left < block_right)
        {
            if (loop_h)
                AppMain.gmMapDrawMapRange(obj3d_tbl, mp_header, md_header, trans_x + num1, trans_y, trans_z, block_left, block_right, block_top, block_bottom);
            else
                AppMain.gmMapDrawMapRange(obj3d_tbl, mp_header, md_header, trans_x, trans_y, trans_z, block_left, block_right, block_top, block_bottom);
        }
        else
        {
            AppMain.gmMapDrawMapRange(obj3d_tbl, mp_header, md_header, trans_x + num1, trans_y, trans_z, block_left, (int)mp_header.map_w - 1, block_top, block_bottom);
            AppMain.gmMapDrawMapRange(obj3d_tbl, mp_header, md_header, trans_x + num2, trans_y, trans_z, 0, block_right, block_top, block_bottom);
        }
    }

    private static void gmMapDrawMapRange(
      AppMain.OBS_ACTION3D_NN_WORK obj3d_tbl,
      AppMain.MP_HEADER mp_header,
      AppMain.MD_HEADER md_header,
      float trans_x,
      float trans_y,
      float trans_z,
      int block_left,
      int block_right,
      int block_top,
      int block_bottom)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void GmMapSetAddMapXLoop()
    {
        if (AppMain.gm_map_tcb == null)
            return;
        AppMain.GMS_MAP_SYS_WORK work = (AppMain.GMS_MAP_SYS_WORK)AppMain.gm_map_tcb.work;
        work.flag |= 2147483648U;
        AppMain.GMS_MAP_OTHER_MAP_STATE[] mapState = work.map_state;
        for (int index = 0; index < 5; ++index)
        {
            if (((int)work.flag & 1 << index) != 0 && mapState[index].map_size[0] != AppMain.g_gm_main_system.map_size[0])
                mapState[index].scrl_scale[0] = (float)mapState[index].map_size[0] / (float)AppMain.g_gm_main_system.map_size[0];
        }
    }

    private static void GmMapEnableAddMapUserScrlX()
    {
        if (AppMain.gm_map_tcb == null)
            return;
        AppMain.GMS_MAP_SYS_WORK work = (AppMain.GMS_MAP_SYS_WORK)AppMain.gm_map_tcb.work;
        if (((int)work.flag & 536870912) != 0)
            return;
        work.flag |= 536870912U;
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
        work.main_cam_user_disp[0] = obsCamera.disp_pos.x;
        work.main_cam_user_disp[1] = obsCamera.disp_pos.y;
        work.main_cam_user_target[0] = obsCamera.disp_pos.x;
        work.main_cam_user_target[1] = obsCamera.disp_pos.y;
        work.main_cam_user_ofst[0] = work.main_cam_user_ofst[1] = 0.0f;
    }

    private static void GmMapDisenableAddMapUserScrlX()
    {
        if (AppMain.gm_map_tcb == null)
            return;
        AppMain.GMS_MAP_SYS_WORK work = (AppMain.GMS_MAP_SYS_WORK)AppMain.gm_map_tcb.work;
        if (((int)work.flag & 536870912) == 0)
            return;
        work.flag &= 3758096383U;
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
        float num = work.main_cam_user_disp[0] + work.main_cam_user_ofst[0] - obsCamera.disp_pos.x;
        for (int index = 0; index < 5; ++index)
        {
            if (index != 1)
                work.map_state[index].cam_ofst[0] += num * work.map_state[index].scrl_scale[0];
        }
    }

    private static void GmMapSetAddMapScrlScaleMagX(int map_type, int mag)
    {
        if (AppMain.gm_map_tcb == null || map_type == 1 || (uint)map_type >= 5U)
            return;
        if (mag == 0)
            mag = 1;
        AppMain.GMS_MAP_SYS_WORK work = (AppMain.GMS_MAP_SYS_WORK)AppMain.gm_map_tcb.work;
        AppMain.GMS_MAP_OTHER_MAP_STATE mapOtherMapState = work.map_state[map_type];
        if ((work.flag & 2147483648U) > 0U)
            mapOtherMapState.scrl_scale[0] = (float)mapOtherMapState.map_size[0] / (float)AppMain.g_gm_main_system.map_size[0] / (float)mag;
        else
            mapOtherMapState.scrl_scale[0] = (float)(((double)mapOtherMapState.map_size[0] - (double)AppMain.OBD_LCD_X) / ((double)AppMain.g_gm_main_system.map_size[0] - (double)AppMain.OBD_LCD_X)) / (float)mag;
    }

    private static void GmMapSetAddMapUserScrlXAddSize(float move_size)
    {
        if (AppMain.gm_map_tcb == null)
            return;
        AppMain.GMS_MAP_SYS_WORK work = (AppMain.GMS_MAP_SYS_WORK)AppMain.gm_map_tcb.work;
        work.main_cam_user_ofst[0] += move_size;
        float num = (float)AppMain.g_gm_main_system.map_size[0];
        if ((double)work.main_cam_user_disp[0] + (double)work.main_cam_user_ofst[0] - (double)AppMain.OBD_LCD_X >= (double)num)
        {
            work.main_cam_user_ofst[0] -= num;
        }
        else
        {
            if ((double)work.main_cam_user_disp[0] + (double)work.main_cam_user_ofst[0] + (double)AppMain.OBD_LCD_X > -(double)num)
                return;
            work.main_cam_user_ofst[0] += num;
        }
    }

    private static void GmMapGetAddMapCameraPos(
      AppMain.NNS_VECTOR main_disp_pos,
      AppMain.NNS_VECTOR main_target_pos,
      AppMain.NNS_VECTOR dest_disp_pos,
      AppMain.NNS_VECTOR dest_target_pos,
      int camera_id)
    {
        if (AppMain.gm_map_tcb == null)
        {
            AppMain.mppAssertNotImpl();
        }
        else
        {
            AppMain.GMS_MAP_SYS_WORK work = (AppMain.GMS_MAP_SYS_WORK)AppMain.gm_map_tcb.work;
            AppMain.main_camera_pos[0].Assign(main_disp_pos);
            AppMain.main_camera_pos[1].Assign(main_target_pos);
            if (((int)work.flag & 536870912) != 0)
            {
                AppMain.main_camera_pos[0].x = work.main_cam_user_disp[0] + work.main_cam_user_ofst[0];
                AppMain.main_camera_pos[1].x = work.main_cam_user_target[0] + work.main_cam_user_ofst[0];
            }
            float num1 = (float)((double)(AppMain.AMD_SCREEN_2D_WIDTH / 2f) * 0.674383342266083 * 1.0);
            float num2 = (float)((double)(AppMain.AMD_SCREEN_2D_HEIGHT / 2f) * 0.674383342266083 * 1.0 * 0.899999976158142);
            float num3 = 0.0f + num1;
            float num4 = (float)AppMain.g_gm_main_system.map_size[0] - num1;
            float num5 = 0.0f + num2;
            float num6 = (float)AppMain.g_gm_main_system.map_size[1] - num2;
            AppMain.GMS_MAP_OTHER_MAP_STATE mapOtherMapState = camera_id != 2 ? (camera_id < 3 ? work.map_state[0] : work.map_state[camera_id - 3 + 2]) : work.map_state[0];
            int index1 = 0;
            dest_disp_pos.x = ((int)work.flag & int.MinValue) != 0 ? AppMain.main_camera_pos[index1].x * mapOtherMapState.scrl_scale[0] : ((double)AppMain.main_camera_pos[index1].x > (double)num3 ? ((double)AppMain.main_camera_pos[index1].x < (double)num4 ? num1 + (AppMain.main_camera_pos[index1].x - num1) * mapOtherMapState.scrl_scale[0] : (float)mapOtherMapState.map_size[0] - num1) : num1);
            dest_disp_pos.x += mapOtherMapState.cam_ofst[0];
            dest_disp_pos.y = -(double)AppMain.main_camera_pos[index1].y > (double)num5 ? (-(double)AppMain.main_camera_pos[index1].y < (double)num6 ? (float)-((double)num2 + (-(double)AppMain.main_camera_pos[index1].y - (double)num2) * (double)mapOtherMapState.scrl_scale[1]) : (float)-((double)mapOtherMapState.map_size[1] - (double)num2)) : -num2;
            dest_disp_pos.z = AppMain.main_camera_pos[index1].z;
            int index2 = 1;
            dest_target_pos.x = ((int)work.flag & int.MinValue) != 0 ? AppMain.main_camera_pos[index2].x * mapOtherMapState.scrl_scale[0] : ((double)AppMain.main_camera_pos[index2].x > (double)num3 ? ((double)AppMain.main_camera_pos[index2].x < (double)num4 ? num1 + (AppMain.main_camera_pos[index2].x - num1) * mapOtherMapState.scrl_scale[0] : (float)mapOtherMapState.map_size[0] - num1) : num1);
            dest_target_pos.x += mapOtherMapState.cam_ofst[0];
            dest_target_pos.y = -(double)AppMain.main_camera_pos[index2].y > (double)num5 ? (-(double)AppMain.main_camera_pos[index2].y < (double)num6 ? (float)-((double)num2 + (-(double)AppMain.main_camera_pos[index2].y - (double)num2) * (double)mapOtherMapState.scrl_scale[1]) : (float)-((double)mapOtherMapState.map_size[1] - (double)num2)) : -num2;
            dest_target_pos.z = AppMain.main_camera_pos[index2].z;
        }
    }

    private static void GmMapSetDispB(bool disp)
    {
        if (AppMain.gm_map_tcb == null)
            return;
        AppMain.GMS_MAP_SYS_WORK work = (AppMain.GMS_MAP_SYS_WORK)AppMain.gm_map_tcb.work;
        if (disp)
            work.flag &= 4026531839U;
        else
            work.flag |= 268435456U;
    }

    private static void GmMapSetDisp(bool disp)
    {
        if (AppMain.gm_map_tcb == null)
            return;
        AppMain.GMS_MAP_SYS_WORK work = (AppMain.GMS_MAP_SYS_WORK)AppMain.gm_map_tcb.work;
        if (disp)
            work.flag &= 4160749567U;
        else
            work.flag |= 134217728U;
    }

    private static bool GmMapIsDrawEnableMMapBack()
    {
        bool flag = true;
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
        float x = obsCamera.disp_pos.x;
        float _src = -obsCamera.disp_pos.y;
        switch (AppMain.g_gs_main_sys_info.stage_id)
        {
            case 0:
                if (AppMain.GMM_MAP_IS_RANGE(x, 5450f, 5725f) && AppMain.GMM_MAP_IS_RANGE(_src, 1010f, 1520f))
                {
                    flag = false;
                    break;
                }
                if (AppMain.GMM_MAP_IS_RANGE(x, 8010f, 8500f) && AppMain.GMM_MAP_IS_RANGE(_src, 1200f, 1650f))
                {
                    flag = false;
                    break;
                }
                if (AppMain.GMM_MAP_IS_RANGE(x, 10055f, 10695f) && AppMain.GMM_MAP_IS_RANGE(_src, 1025f, 1400f))
                {
                    flag = false;
                    break;
                }
                break;
            case 1:
                if (AppMain.GMM_MAP_IS_RANGE(x, 3975f, 4650f) && AppMain.GMM_MAP_IS_RANGE(_src, 1555f, 2200f))
                {
                    flag = false;
                    break;
                }
                if ((double)x > 12415.0)
                {
                    flag = false;
                    break;
                }
                break;
            case 16:
                if ((double)x < 2450.0)
                {
                    flag = false;
                    break;
                }
                if (AppMain.GMM_MAP_IS_RANGE(x, 3020f, 5600f))
                {
                    flag = false;
                    break;
                }
                if (AppMain.GMM_MAP_IS_RANGE(x, 6590f, 9200f))
                {
                    flag = false;
                    break;
                }
                break;
        }
        return flag;
    }

    private static void GmMapSetLight()
    {
        AppMain.NNS_RGBA col = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.NNS_VECTOR nnsVector = new AppMain.NNS_VECTOR(1f, 1f, 1f);
        float intensity = 1f;
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 0)
        {
            nnsVector.x = -1f;
            nnsVector.y = -1f;
            nnsVector.z = -1f;
            col.r = 1f;
            col.g = 1f;
            col.b = 1f;
            col.a = 1f;
            intensity = 1f;
        }
        else if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 3)
        {
            nnsVector.x = -0.2f;
            nnsVector.y = 0.25f;
            nnsVector.z = -1f;
            col.r = 1f;
            col.g = 1f;
            col.b = 1f;
            col.a = 1f;
            intensity = AppMain.g_gs_main_sys_info.stage_id != (ushort)14 ? 1f : 0.4f;
        }
        AppMain.nnNormalizeVector(nnsVector, nnsVector);
        AppMain.ObjDrawSetParallelLight(AppMain.NNE_LIGHT_5, ref col, intensity, nnsVector);
    }

    private static void GmMapSetMapDrawSize(int size)
    {
        AppMain.gm_map_draw_size[0] = AppMain.gm_map_set_draw_size[size, 0];
        AppMain.gm_map_draw_size[1] = AppMain.gm_map_set_draw_size[size, 1];
    }

    private static void gmMapDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gm_map_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static void gmMapMain(AppMain.MTS_TASK_TCB tcb)
    {
        int zoneType = AppMain.GMM_MAIN_GET_ZONE_TYPE();
        if (zoneType == 5)
        {
            if (((int)AppMain.g_gm_main_system.game_flag & 268435456) == 0 || --AppMain.gm_map_draw_bgm_timer > 0)
                return;
            AppMain.g_gm_main_system.game_flag |= 134217728U;
            AppMain.g_gm_main_system.game_flag &= 4026531839U;
        }
        else
        {
            int num1 = 0;
            int num2 = AppMain.gm_map_add_tbl_use_no[zoneType] + 1;
            AppMain.GMS_MAP_SYS_WORK work = (AppMain.GMS_MAP_SYS_WORK)tcb.work;
            if (((int)work.flag & 134217728) != 0)
                return;
            if (AppMain.ObjObjectPauseCheck(0U) == 0U)
                AppMain.gmMapUpdateDrawMapTvxTexScroll();
            if (!AppMain.GmMainIsDrawEnable())
                return;
            if (((int)AppMain.g_gm_main_system.game_flag & 268435456) != 0 && --AppMain.gm_map_draw_bgm_timer <= 0)
            {
                AppMain.g_gm_main_system.game_flag |= 134217728U;
                AppMain.g_gm_main_system.game_flag &= 4026531839U;
            }
            AppMain.TVX_FILE[] gGmGamedat = (AppMain.TVX_FILE[])AppMain.g_gm_gamedat_map[1];
            AppMain.NNS_TEXLIST texList = AppMain.AoTexGetTexList(AppMain.gm_map_texture);
            AppMain.NNS_MATRIX gmMapMainMtx = AppMain.gmMapMain_mtx;
            AppMain.nnMakeUnitMatrix(gmMapMainMtx);
            if (work.auto_resize)
            {
                AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
                if ((obsCamera.roll & 262143) != 0)
                    AppMain.GmMapSetMapDrawSize(0);
                else if ((obsCamera.roll & 16384) != 0)
                    AppMain.GmMapSetMapDrawSize(2);
                else
                    AppMain.GmMapSetMapDrawSize(1);
            }
            AppMain.GMS_MAP_OTHER_MAP_STATE[] mapState = work.map_state;
            for (int index = num2 - 1; index >= num1; --index)
            {
                if (((int)work.flag & 1 << index) != 0 && (index < 2 || AppMain.GmMapIsDrawEnableMMapBack()))
                {
                    bool loop_h = false;
                    if (index != 1 && ((int)work.flag & int.MinValue) != 0)
                        loop_h = true;
                    AppMain.ObjDraw3DNNSetCameraEx(AppMain.gm_map_addmap_camera_tbl[index], 1, mapState[index].command_state);
                    AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(AppMain.gm_map_addmap_camera_tbl[index]);
                    float x = obsCamera.disp_pos.x;
                    float pos_y = -obsCamera.disp_pos.y;
                    AppMain.GmMapSetDrawState(mapState[index].command_state);
                    AppMain.MP_HEADER mp_header = (AppMain.MP_HEADER)AppMain.g_gm_gamedat_map_set_add[index * 2];
                    AppMain.MD_HEADER md_header = (AppMain.MD_HEADER)AppMain.g_gm_gamedat_map_set_add[1 + index * 2];
                    AppMain.gmMapInitDrawMapTvx();
                    AppMain.gmMapSetDrawMapTvx(gGmGamedat, mp_header, md_header, x, pos_y, AppMain.gmMapTransX, 0.0f, mapState[index].pos_z, loop_h, mp_header.blocks[0], md_header.blocks[0]);
                    AppMain.gmMapExecuteDrawMapTvx(gmMapMainMtx, texList);
                }
            }
            AppMain.GmMapSetDrawState(0U);
            AppMain.ObjDraw3DNNSetCameraEx(AppMain.g_obj.glb_camera_id, AppMain.g_obj.glb_camera_type, 0U);
            AppMain.OBS_CAMERA obsCamera1 = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
            float x1 = obsCamera1.disp_pos.x;
            float pos_y1 = -obsCamera1.disp_pos.y;
            AppMain.gmMapInitDrawMapTvx();
            if (((int)work.flag & 268435456) == 0)
            {
                AppMain.MP_HEADER gGmGamedatMap1 = (AppMain.MP_HEADER)AppMain.g_gm_gamedat_map_set[1];
                AppMain.MD_HEADER gGmGamedatMap2 = (AppMain.MD_HEADER)AppMain.g_gm_gamedat_map_set[3];
                AppMain.gmMapSetDrawMapTvx(gGmGamedat, gGmGamedatMap1, gGmGamedatMap2, x1, pos_y1, AppMain.gmMapTransX, 0.0f, (float)sbyte.MinValue, false, gGmGamedatMap1.blocks[0], gGmGamedatMap2.blocks[0]);
            }
            AppMain.MP_HEADER gGmGamedatMap3 = (AppMain.MP_HEADER)AppMain.g_gm_gamedat_map_set[0];
            AppMain.MD_HEADER gGmGamedatMap4 = (AppMain.MD_HEADER)AppMain.g_gm_gamedat_map_set[2];
            AppMain.gmMapSetDrawMapTvx(gGmGamedat, gGmGamedatMap3, gGmGamedatMap4, x1, pos_y1, AppMain.gmMapTransX, 0.0f, 128f, false, gGmGamedatMap3.blocks[0], gGmGamedatMap4.blocks[0]);
            AppMain.gmMapExecuteDrawMapTvx(gmMapMainMtx, texList);
            if (AppMain.gm_map_tex_draw_count <= 0)
                return;
            --AppMain.gm_map_tex_draw_count;
            AppMain.ObjLoadInitDraw();
            if (AppMain.gm_map_tex_draw_count == 0)
                AppMain.ObjLoadClearDraw();
            for (int index = 0; index < AppMain.gm_map_texture.texlist.nTex; ++index)
            {
                AppMain.AMS_PARAM_DRAW_PRIMITIVE prim = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
                prim.Clear();
                prim.type = 0;
                prim.ablend = 1;
                prim.bldSrc = 770;
                prim.bldDst = 771;
                prim.aTest = (short)1;
                prim.zMask = (short)0;
                prim.zTest = (short)1;
                prim.noSort = (short)1;
                prim.uwrap = 1;
                prim.vwrap = 1;
                prim.format3D = 4;
                prim.texlist = texList;
                AppMain.NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(4);
                AppMain.NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
                int offset = nnsPriM3DPctArray.offset;
                prim.vtxPCT3D = nnsPriM3DPctArray;
                prim.count = 4;
                prim.texId = index;
                buffer[offset].Pos.x = buffer[offset + 1].Pos.x = -1f;
                buffer[offset + 2].Pos.x = buffer[offset + 3].Pos.x = 1f;
                buffer[offset].Pos.y = buffer[offset + 2].Pos.y = -1f;
                buffer[offset + 1].Pos.y = buffer[offset + 3].Pos.y = 1f;
                buffer[offset].Pos.z = buffer[offset + 1].Pos.z = buffer[offset + 2].Pos.z = buffer[offset + 3].Pos.z = -1f;
                buffer[offset].Tex.u = buffer[offset + 1].Tex.u = 0.0f;
                buffer[offset + 2].Tex.u = buffer[offset + 3].Tex.u = 1f;
                buffer[offset].Tex.v = buffer[offset + 2].Tex.v = 0.0f;
                buffer[offset + 1].Tex.v = buffer[offset + 3].Tex.v = 1f;
                buffer[offset].Col = buffer[offset + 1].Col = buffer[offset + 2].Col = buffer[offset + 3].Col = uint.MaxValue;
                AppMain.ObjDraw3DNNDrawPrimitive(prim, AppMain.gm_map_draw_command_state, 0, 0);
                AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Release(prim);
            }
        }
    }

    private static void gmMapInitDrawMapTvx()
    {
        AppMain.GMS_MAP_PRIM_DRAW_WORK[] gmMapPrimDrawWork = AppMain.gm_map_prim_draw_work;
        for (int index = 0; index < 16; ++index)
        {
            gmMapPrimDrawWork[index].tex_id = -1;
            gmMapPrimDrawWork[index].all_vtx_num = 0U;
            gmMapPrimDrawWork[index].stack_num = 0U;
        }
    }

    private static void gmMapSetDrawMapTvx(
      AppMain.TVX_FILE[] tvxamb,
      AppMain.MP_HEADER mp_header,
      AppMain.MD_HEADER md_header,
      float pos_x,
      float pos_y,
      float trans_x,
      float trans_y,
      float trans_z,
      bool loop_h,
      AppMain.MP_BLOCK mp_block,
      AppMain.MD_BLOCK md_block)
    {
        float num1 = 0.0f;
        float num2 = 0.0f;
        int num3 = (int)pos_x >> 6;
        int num4 = (int)pos_y >> 6;
        int num5 = (int)AppMain.GMD_MAP_DRAW_WIDTH + 2 + (int)AppMain.gm_map_draw_margin_adjust * 2;
        int num6 = (int)AppMain.GMD_MAP_DRAW_HEIGHT + 2 + (int)AppMain.gm_map_draw_margin_adjust * 2;
        int block_left = num3 - (num5 >> 1);
        int block_right = num3 + (num5 >> 1);
        int block_top = num4 - (num6 >> 1);
        int block_bottom = num4 + (num6 >> 1);
        if (block_left < 0)
        {
            if (!loop_h)
            {
                block_left = 0;
            }
            else
            {
                block_left += (int)mp_header.map_w;
                num1 = (float)-((int)mp_header.map_w << 6);
            }
        }
        else if (block_left >= (int)mp_header.map_w)
        {
            if (!loop_h)
            {
                block_left = (int)mp_header.map_w - num5;
            }
            else
            {
                block_left -= (int)mp_header.map_w;
                num1 = (float)((int)mp_header.map_w << 6);
            }
        }
        if (block_top < 0)
            block_top = 0;
        if (block_right >= (int)mp_header.map_w)
        {
            if (!loop_h)
            {
                block_right = (int)mp_header.map_w - 1;
            }
            else
            {
                block_right -= (int)mp_header.map_w;
                num2 = (float)((int)mp_header.map_w << 6);
            }
        }
        else if (block_right < 0)
        {
            if (!loop_h)
            {
                block_right = num5;
            }
            else
            {
                block_right += (int)mp_header.map_w;
                num2 = (float)-((int)mp_header.map_w << 6);
            }
        }
        if (block_bottom >= (int)mp_header.map_h)
            block_bottom = (int)mp_header.map_h - 1;
        if (block_left < block_right)
        {
            if (loop_h)
                AppMain.gmMapSetDrawMapRangeTvx(tvxamb, mp_header, md_header, trans_x + num1, trans_y, trans_z, block_left, block_right, block_top, block_bottom, mp_block, md_block);
            else
                AppMain.gmMapSetDrawMapRangeTvx(tvxamb, mp_header, md_header, trans_x, trans_y, trans_z, block_left, block_right, block_top, block_bottom, mp_block, md_block);
        }
        else
        {
            AppMain.gmMapSetDrawMapRangeTvx(tvxamb, mp_header, md_header, trans_x + num1, trans_y, trans_z, block_left, (int)mp_header.map_w - 1, block_top, block_bottom, mp_block, md_block);
            AppMain.gmMapSetDrawMapRangeTvx(tvxamb, mp_header, md_header, trans_x + num2, trans_y, trans_z, 0, block_right, block_top, block_bottom, mp_block, md_block);
        }
    }

    private static void gmMapSetDrawMapRangeTvx(
      AppMain.TVX_FILE[] tvxamb,
      AppMain.MP_HEADER mp_header,
      AppMain.MD_HEADER md_header,
      float trans_x,
      float trans_y,
      float trans_z,
      int block_left,
      int block_right,
      int block_top,
      int block_bottom,
      AppMain.MP_BLOCK _mp_block,
      AppMain.MD_BLOCK _md_block)
    {
        for (int index1 = 0; index1 < 24; ++index1)
        {
            for (int index2 = 0; index2 < 24; ++index2)
                AppMain.gm_map_block_check[index1, index2] = (short)-1;
        }
        int gmdMapDrawWidth = (int)AppMain.GMD_MAP_DRAW_WIDTH;
        int gmdMapDrawHeight = (int)AppMain.GMD_MAP_DRAW_HEIGHT;
        AppMain.GMS_MAP_PRIM_DRAW_WORK[] gmMapPrimDrawWork = AppMain.gm_map_prim_draw_work;
        for (int index1 = block_left; index1 <= block_right; ++index1)
        {
            for (int index2 = block_top; index2 <= block_bottom; ++index2)
            {
                int index3 = index2 * (int)mp_header.map_w + index1;
                AppMain.MP_BLOCK block1 = mp_header.blocks[index3];
                float num1 = (float)index1;
                float num2 = (float)index2;
                int id = (int)block1.id;
                if (id == 0)
                {
                    AppMain.MD_BLOCK block2 = md_header.blocks[index3];
                    int ofstX = (int)block2.ofst_x;
                    int ofstY = (int)block2.ofst_y;
                    if ((ofstX | ofstY) != 0)
                    {
                        int index4 = index3 + ((int)mp_header.map_w * ofstY + ofstX);
                        block1 = mp_header.blocks[index4];
                        id = (int)block1.id;
                        num1 += (float)ofstX;
                        num2 += (float)ofstY;
                    }
                    else
                        continue;
                }
                if (id != 0 && AppMain.gm_map_block_check[8 + (int)num1 - block_left, 8 + (int)num2 - block_top] == (short)-1)
                {
                    AppMain.gm_map_block_check[8 + (int)num1 - block_left, 8 + (int)num2 - block_top] = (short)id;
                    int index4 = id - 1;
                    AppMain.TVX_FILE file = tvxamb[index4];
                    uint textureNum = AppMain.AoTvxGetTextureNum(file);
                    for (uint tex_no = 0; tex_no < textureNum; ++tex_no)
                    {
                        uint vertexNum = AppMain.AoTvxGetVertexNum(file, tex_no);
                        int textureId = AppMain.AoTvxGetTextureId(file, tex_no);
                        for (int index5 = 0; index5 < 16; ++index5)
                        {
                            if (gmMapPrimDrawWork[index5].tex_id == -1 || gmMapPrimDrawWork[index5].tex_id == textureId)
                            {
                                gmMapPrimDrawWork[index5].tex_id = textureId;
                                gmMapPrimDrawWork[index5].all_vtx_num += vertexNum;
                                AppMain.GMS_MAP_PRIM_DRAW_STACK mapPrimDrawStack = gmMapPrimDrawWork[index5].stack[(int)gmMapPrimDrawWork[index5].stack_num];
                                mapPrimDrawStack.vtx = AppMain.AoTvxGetVertex(file, tex_no);
                                mapPrimDrawStack.vtx_num = (ushort)vertexNum;
                                mapPrimDrawStack.mp = block1;
                                mapPrimDrawStack.dx = trans_x + (float)(((double)num1 + 0.5) * 64.0);
                                mapPrimDrawStack.dy = (float)(-(double)trans_y + (-(double)num2 - 0.5) * 64.0);
                                mapPrimDrawStack.dz = trans_z;
                                ++gmMapPrimDrawWork[index5].stack_num;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    private static void gmMapExecuteDrawMapTvx(AppMain.NNS_MATRIX mtx, AppMain.NNS_TEXLIST texlist)
    {
        AppMain.AMS_PARAM_DRAW_PRIMITIVE dat = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        AppMain.GMS_MAP_PRIM_DRAW_WORK[] gmMapPrimDrawWork = AppMain.gm_map_prim_draw_work;
        int[] primDrawTvxAlphaSet = AppMain.gm_map_prim_draw_tvx_alpha_set;
        AppMain.GMS_MAP_PRIM_DRAW_WORK[] gmsMapPrimDrawWorkArray = AppMain.amDraw_GMS_MAP_PRIM_DRAW_WORK_Array_Pool.AllocArray(8);
        uint num = 0;
        uint primDrawTvxColor = AppMain.gm_map_prim_draw_tvx_color;
        dat.type = 1;
        dat.ablend = 0;
        dat.bldMode = 32774;
        dat.aTest = (short)1;
        dat.zMask = (short)0;
        dat.zTest = (short)1;
        dat.noSort = (short)1;
        dat.uwrap = 1;
        dat.vwrap = 1;
        dat.format3D = 4;
        dat.texlist = texlist;
        for (uint index = 0; index < 16U && gmMapPrimDrawWork[(int)index].tex_id != -1; ++index)
        {
            if (primDrawTvxAlphaSet != null && primDrawTvxAlphaSet[gmMapPrimDrawWork[(int)index].tex_id] != 0)
            {
                if (num < 8U)
                {
                    gmsMapPrimDrawWorkArray[(int)num] = gmMapPrimDrawWork[(int)index];
                    gmsMapPrimDrawWorkArray[(int)num].op = (uint)primDrawTvxAlphaSet[gmMapPrimDrawWork[(int)index].tex_id];
                    ++num;
                }
                else
                    break;
            }
            else
            {
                dat.count = (int)gmMapPrimDrawWork[(int)index].all_vtx_num + (int)gmMapPrimDrawWork[(int)index].stack_num * 2 - 2;
                AppMain.NNS_PRIM3D_PCT_ARRAY v_tbl_array = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(dat.count);
                AppMain.gmMapExecuteDrawMapTvxCore(mtx, gmMapPrimDrawWork[(int)index], dat, v_tbl_array, primDrawTvxColor);
            }
        }
        if (primDrawTvxAlphaSet != null)
        {
            for (int index = 0; (long)index < (long)num; ++index)
            {
                switch (gmsMapPrimDrawWorkArray[index].op)
                {
                    case 1:
                        dat.bldSrc = 770;
                        dat.bldDst = 771;
                        dat.ablend = 1;
                        dat.aTest = (short)1;
                        break;
                    case 2:
                        dat.bldSrc = 770;
                        dat.bldDst = 1;
                        dat.ablend = 1;
                        dat.aTest = (short)0;
                        break;
                }
                dat.count = (int)gmsMapPrimDrawWorkArray[index].all_vtx_num + (int)gmsMapPrimDrawWorkArray[index].stack_num * 2 - 2;
                AppMain.NNS_PRIM3D_PCT_ARRAY v_tbl_array = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(dat.count);
                AppMain.gmMapExecuteDrawMapTvxCore(mtx, gmsMapPrimDrawWorkArray[index], dat, v_tbl_array, primDrawTvxColor);
            }
        }
        AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Release(dat);
    }

    private static void gmMapExecuteDrawMapTvxCore(
      AppMain.NNS_MATRIX mtx,
      AppMain.GMS_MAP_PRIM_DRAW_WORK work,
      AppMain.AMS_PARAM_DRAW_PRIMITIVE dat,
      AppMain.NNS_PRIM3D_PCT_ARRAY v_tbl_array,
      uint color)
    {
        int offset = v_tbl_array.offset;
        AppMain.NNS_PRIM3D_PCT[] buffer = v_tbl_array.buffer;
        dat.vtxPCT3D = v_tbl_array;
        dat.texId = work.tex_id;
        AppMain.NNS_TEXCOORD scr_uv;
        AppMain.gmMapGetDrawMapTvxTexScrollUV(dat.texId, out scr_uv);
        AppMain.SNNS_VECTOR src = new AppMain.SNNS_VECTOR();
        for (uint index1 = 0; index1 < work.stack_num; ++index1)
        {
            AppMain.GMS_MAP_PRIM_DRAW_STACK mapPrimDrawStack = work.stack[(int)index1];
            int num1 = (int)mapPrimDrawStack.vtx_num / 3;
            float dx = mapPrimDrawStack.dx;
            float dy = mapPrimDrawStack.dy;
            float dz = mapPrimDrawStack.dz;
            AppMain.NNS_MATRIX usePrimMatrix = AppMain.gmMapGetUsePrimMatrix((int)mapPrimDrawStack.mp.rot, (int)mapPrimDrawStack.mp.flip_h | (int)mapPrimDrawStack.mp.flip_v << 1);
            int num2 = offset;
            AppMain.AOS_TVX_VERTEX[] vtx = mapPrimDrawStack.vtx;
            for (int index2 = 0; index2 < (int)mapPrimDrawStack.vtx_num; ++index2)
            {
                src.x = vtx[index2].x;
                src.y = vtx[index2].y;
                src.z = vtx[index2].z;
                int index3 = num2 + index2;
                AppMain.nnTransformVector(ref buffer[index3].Pos, usePrimMatrix, ref src);
                buffer[index3].Pos.x += dx;
                buffer[index3].Pos.y += dy;
                buffer[index3].Pos.z += dz;
                buffer[index3].Tex.u = vtx[index2].u + scr_uv.u;
                buffer[index3].Tex.v = vtx[index2].v + scr_uv.v;
                buffer[index3].Col = vtx[index2].c & color;
            }
            offset += (int)mapPrimDrawStack.vtx_num + 2;
            if (index1 != 0U)
            {
                int index2 = num2 - 1;
                buffer[index2] = buffer[index2 + 1];
            }
            if ((int)index1 != (int)work.stack_num - 1)
            {
                int index2 = num2 + ((int)mapPrimDrawStack.vtx_num - 1);
                buffer[index2 + 1] = buffer[index2];
            }
        }
        AppMain.amMatrixPush(mtx);
        AppMain.ObjDraw3DNNDrawPrimitive(dat, AppMain.gm_map_draw_command_state, 0, 0);
        AppMain.amMatrixPop();
    }

    private static void gmMapBuildDrawMapTvxTexScroll()
    {
        AppMain.GMS_MAP_PRIM_DRAW_TVX_UV_WORK mapPrimDrawUvWork = AppMain.gm_map_prim_draw_uv_work;
        int length1 = 0;
        int length2 = 0;
        int num1 = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        switch (num1)
        {
            case 0:
                return;
            case 1:
                length1 = AppMain.gm_map_prim_draw_tvx_mgr_index_tbl_z2_num;
                length2 = 21;
                break;
            case 2:
                length1 = AppMain.gm_map_prim_draw_tvx_mgr_index_tbl_z3_num;
                length2 = 4;
                break;
            case 3:
                length1 = AppMain.gm_map_prim_draw_tvx_mgr_index_tbl_z4_num;
                length2 = 9;
                break;
            case 4:
                length1 = AppMain.gm_map_prim_draw_tvx_mgr_index_tbl_zf_num;
                length2 = 8;
                break;
            case 5:
                return;
        }
        uint num2 = (uint)((ulong)(uint)(32UL + (ulong)(4 * length1)) + (ulong)(4 * length1));
        AppMain.GMS_MAP_PRIM_DRAW_TVX_UV_WORK primDrawTvxUvWork = new AppMain.GMS_MAP_PRIM_DRAW_TVX_UV_WORK();
        AppMain.gm_map_prim_draw_uv_work = primDrawTvxUvWork;
        primDrawTvxUvWork.mgr_index_tbl_num = length1;
        primDrawTvxUvWork.frame_index_tbl = new uint[length1];
        primDrawTvxUvWork.frame_tbl = new uint[length1];
        primDrawTvxUvWork.tex_uv_index_tbl = new int[length2];
        int index1 = 0;
        for (int length3 = primDrawTvxUvWork.tex_uv_index_tbl.Length; index1 < length3; ++index1)
            primDrawTvxUvWork.tex_uv_index_tbl[index1] = -1;
        switch (num1)
        {
            case 0:
                return;
            case 1:
                primDrawTvxUvWork.mgr_index_tbl_addr = AppMain.gm_map_prim_draw_tvx_mgr_index_tbl_z2;
                primDrawTvxUvWork.mgr_tbl_num = AppMain.gm_map_prim_draw_tvx_mgr_tbl_z2_num;
                primDrawTvxUvWork.mgr_tbl_addr = (DoubleType<uint[], AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR[][]>)AppMain.gm_map_prim_draw_tvx_mgr_tbl_z2;
                primDrawTvxUvWork.uv_mgr_tbl_addr = (DoubleType<uint[], AppMain.NNS_TEXCOORD[][]>)AppMain.gm_map_prim_draw_tvx_uv_mgr_tbl_z2;
                break;
            case 2:
                primDrawTvxUvWork.mgr_index_tbl_addr = AppMain.gm_map_prim_draw_tvx_mgr_index_tbl_z3;
                primDrawTvxUvWork.mgr_tbl_num = AppMain.gm_map_prim_draw_tvx_mgr_tbl_z3_num;
                primDrawTvxUvWork.mgr_tbl_addr = (DoubleType<uint[], AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR[][]>)AppMain.gm_map_prim_draw_tvx_mgr_tbl_z3;
                primDrawTvxUvWork.uv_mgr_tbl_addr = (DoubleType<uint[], AppMain.NNS_TEXCOORD[][]>)AppMain.gm_map_prim_draw_tvx_uv_mgr_tbl_z3;
                break;
            case 3:
                primDrawTvxUvWork.mgr_index_tbl_addr = AppMain.gm_map_prim_draw_tvx_mgr_index_tbl_z4;
                primDrawTvxUvWork.mgr_tbl_num = AppMain.gm_map_prim_draw_tvx_mgr_tbl_z4_num;
                primDrawTvxUvWork.mgr_tbl_addr = (DoubleType<uint[], AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR[][]>)AppMain.gm_map_prim_draw_tvx_mgr_tbl_z4;
                primDrawTvxUvWork.uv_mgr_tbl_addr = (DoubleType<uint[], AppMain.NNS_TEXCOORD[][]>)AppMain.gm_map_prim_draw_tvx_uv_mgr_tbl_z4;
                break;
            case 4:
                primDrawTvxUvWork.mgr_index_tbl_addr = AppMain.gm_map_prim_draw_tvx_mgr_index_tbl_zf;
                primDrawTvxUvWork.mgr_tbl_num = AppMain.gm_map_prim_draw_tvx_mgr_tbl_zf_num;
                primDrawTvxUvWork.mgr_tbl_addr = (DoubleType<uint[], AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR[][]>)AppMain.gm_map_prim_draw_tvx_mgr_tbl_zf;
                primDrawTvxUvWork.uv_mgr_tbl_addr = (DoubleType<uint[], AppMain.NNS_TEXCOORD[][]>)AppMain.gm_map_prim_draw_tvx_uv_mgr_tbl_zf;
                break;
            case 5:
                return;
        }
        DoubleType<uint[], AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR_INDEX[]> mgrIndexTblAddr = primDrawTvxUvWork.mgr_index_tbl_addr;
        int[] texUvIndexTbl = primDrawTvxUvWork.tex_uv_index_tbl;
        for (int index2 = 0; index2 < length1; ++index2)
        {
            ushort texId = ((AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR_INDEX[])mgrIndexTblAddr)[index2].tex_id;
            ushort mgrId = ((AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR_INDEX[])mgrIndexTblAddr)[index2].mgr_id;
            for (int index3 = 0; index3 < length2; ++index3)
            {
                if ((int)texId == index3)
                    texUvIndexTbl[index3] = (int)mgrId;
            }
        }
    }

    private static void gmMapFlushDrawMapTvxTexScroll()
    {
        AppMain.gm_map_prim_draw_uv_work = (AppMain.GMS_MAP_PRIM_DRAW_TVX_UV_WORK)null;
    }

    private static void gmMapUpdateDrawMapTvxTexScroll()
    {
        AppMain.GMS_MAP_PRIM_DRAW_TVX_UV_WORK mapPrimDrawUvWork = AppMain.gm_map_prim_draw_uv_work;
        if (mapPrimDrawUvWork == null)
            return;
        int mgrIndexTblNum = mapPrimDrawUvWork.mgr_index_tbl_num;
        int[] mgrTblNum = mapPrimDrawUvWork.mgr_tbl_num;
        uint[] frameIndexTbl = mapPrimDrawUvWork.frame_index_tbl;
        uint[] frameTbl = mapPrimDrawUvWork.frame_tbl;
        AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR_INDEX[] mgrIndexTblAddr = (AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR_INDEX[])mapPrimDrawUvWork.mgr_index_tbl_addr;
        for (int index = 0; index < mgrIndexTblNum; ++index)
        {
            ushort mgrId = mgrIndexTblAddr[index].mgr_id;
            AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR[] mapPrimDrawTvxMgrArray = ((AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR[][])mapPrimDrawUvWork.mgr_tbl_addr)[(int)mgrId];
            if (++frameTbl[(int)mgrId] >= (uint)mapPrimDrawTvxMgrArray[(int)frameIndexTbl[(int)mgrId]].time)
            {
                frameTbl[(int)mgrId] = 0U;
                frameIndexTbl[(int)mgrId] = (uint)((ulong)(frameIndexTbl[(int)mgrId] + 1U) % (ulong)mgrTblNum[(int)mgrId]);
            }
        }
    }

    private static void gmMapGetDrawMapTvxTexScrollUV(int tex_id, out AppMain.NNS_TEXCOORD scr_uv)
    {
        AppMain.GMS_MAP_PRIM_DRAW_TVX_UV_WORK mapPrimDrawUvWork = AppMain.gm_map_prim_draw_uv_work;
        scr_uv.u = 0.0f;
        scr_uv.v = 0.0f;
        if (mapPrimDrawUvWork == null)
            return;
        int index = mapPrimDrawUvWork.tex_uv_index_tbl[tex_id];
        if (-1 == index)
            return;
        AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR[] mapPrimDrawTvxMgrArray = ((AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR[][])mapPrimDrawUvWork.mgr_tbl_addr)[index];
        AppMain.NNS_TEXCOORD[] nnsTexcoordArray = ((AppMain.NNS_TEXCOORD[][])mapPrimDrawUvWork.uv_mgr_tbl_addr)[index];
        uint num = mapPrimDrawUvWork.frame_index_tbl[index];
        ushort motionId = mapPrimDrawTvxMgrArray[(int)num].motion_id;
        scr_uv.u = nnsTexcoordArray[(int)motionId].u;
        scr_uv.v = nnsTexcoordArray[(int)motionId].v;
    }

    private static void gmMapCreateUsePrimMatrix()
    {
        AppMain.NNS_MATRIX nnsMatrix1 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        for (int index1 = 0; index1 < 4; ++index1)
        {
            AppMain.nnMakeUnitMatrix(nnsMatrix1);
            AppMain.nnRotateZMatrix(nnsMatrix1, nnsMatrix1, index1 * 16384);
            for (int index2 = 0; index2 < 4; ++index2)
            {
                AppMain.NNS_MATRIX nnsMatrix2 = AppMain.gm_map_use_prim_mtx[index1 * 4 + index2];
                AppMain.nnCopyMatrix(nnsMatrix2, nnsMatrix1);
                switch (index2)
                {
                    case 0:
                        AppMain.nnScaleMatrix(nnsMatrix2, nnsMatrix2, 1f, 1f, 1f);
                        break;
                    case 1:
                        AppMain.nnScaleMatrix(nnsMatrix2, nnsMatrix2, -1f, 1f, 1f);
                        break;
                    case 2:
                        AppMain.nnScaleMatrix(nnsMatrix2, nnsMatrix2, 1f, -1f, 1f);
                        break;
                    case 3:
                        AppMain.nnScaleMatrix(nnsMatrix2, nnsMatrix2, -1f, -1f, 1f);
                        break;
                }
                AppMain.nnTranslateMatrix(nnsMatrix2, nnsMatrix2, -32f, -32f, 0.0f);
            }
        }
    }

    private static AppMain.NNS_MATRIX gmMapGetUsePrimMatrix(int rot, int flip)
    {
        return AppMain.gm_map_use_prim_mtx[rot * 4 + flip];
    }

    private static void gmMapFallShaderSettingPrioPreMidMapUserFunc(object data)
    {
        AppMain.NNS_RGBA_U8 color = new AppMain.NNS_RGBA_U8((byte)0, (byte)0, (byte)0, byte.MaxValue);
        AppMain.AMS_RENDER_TARGET target = AppMain._am_render_manager.targetp != AppMain._gm_mapFar_render_work ? AppMain._gm_mapFar_render_work : AppMain._am_draw_target;
        if (target.width == 0)
            return;
        AppMain.amRenderCopyTarget(target, color);
    }

    private static void gmMapDrawFallShaderPrioPreMidMapUserFunc(object data)
    {
        if ((AppMain._am_render_manager.targetp != AppMain._gm_mapFar_render_work ? AppMain._gm_mapFar_render_work : AppMain._am_draw_target).width == 0)
            return;
        AppMain.amDrawGetProjectionMatrix();
    }

}