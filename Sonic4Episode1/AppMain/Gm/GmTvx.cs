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
    private static void GmTvxBuild()
    {
        AppMain.gm_tvx_draw_work = AppMain.New<AppMain.GMS_TVX_DRAW_WORK>(AppMain.GMD_TVX_DRAW_WORK_NUM);
        AppMain.GmTvxInit();
    }

    private static void GmTvxInit()
    {
        AppMain.GMS_TVX_DRAW_WORK[] gmTvxDrawWork = AppMain.gm_tvx_draw_work;
        for (int index = 0; index < AppMain.GMD_TVX_DRAW_WORK_NUM; ++index)
        {
            gmTvxDrawWork[index].Clear();
            gmTvxDrawWork[index].tex_id = -1;
        }
    }

    private static void GmTvxExit()
    {
        AppMain.GmTvxInit();
    }

    private static void GmTvxFlush()
    {
        AppMain.GMS_TVX_DRAW_WORK[] gmTvxDrawWork = AppMain.gm_tvx_draw_work;
        AppMain.gm_tvx_draw_work = (AppMain.GMS_TVX_DRAW_WORK[])null;
    }

    private static void GmTvxSetModel(
      AppMain.TVX_FILE model_tvx,
      AppMain.NNS_TEXLIST model_tex,
      ref AppMain.VecFx32 pos,
      ref AppMain.VecFx32 scale,
      uint flag,
      short rotate_z)
    {
        var work = new AppMain.GMS_TVX_EX_WORK()
        {
            u_wrap = 1,
            v_wrap = 1,
            coord = {
        u = 0.0f,
        v = 0.0f
      },
            color = uint.MaxValue
        };
        AppMain.GmTvxSetModelEx(model_tvx, model_tex, ref pos, ref scale, flag, rotate_z, ref work);
    }

    private static void GmTvxSetModelEx(
      AppMain.TVX_FILE model_tvx,
      AppMain.NNS_TEXLIST model_tex,
      ref AppMain.VecFx32 pos,
      ref AppMain.VecFx32 scale,
      uint flag,
      short rotate_z,
      ref AppMain.GMS_TVX_EX_WORK ex_work)
    {
        if (!AppMain.GmMainIsDrawEnable())
            return;
        AppMain.GMS_TVX_DRAW_WORK[] gmTvxDrawWork = AppMain.gm_tvx_draw_work;
        uint textureNum = AppMain.AoTvxGetTextureNum(model_tvx);
        for (uint tex_no = 0; tex_no < textureNum; ++tex_no)
        {
            uint vertexNum = AppMain.AoTvxGetVertexNum(model_tvx, tex_no);
            int textureId = AppMain.AoTvxGetTextureId(model_tvx, tex_no);
            for (int index = 0; index < AppMain.GMD_TVX_DRAW_WORK_NUM; ++index)
            {
                if (gmTvxDrawWork[index].tex == null && gmTvxDrawWork[index].tex_id == -1 || gmTvxDrawWork[index].tex == model_tex && gmTvxDrawWork[index].tex_id == textureId && (gmTvxDrawWork[index].u_wrap == ex_work.u_wrap && gmTvxDrawWork[index].v_wrap == ex_work.v_wrap))
                {
                    if ((long)gmTvxDrawWork[index].stack_num >= (long)AppMain.GMD_TVX_DRAW_STACK_NUM)
                        return;
                    gmTvxDrawWork[index].tex = model_tex;
                    gmTvxDrawWork[index].tex_id = textureId;
                    gmTvxDrawWork[index].u_wrap = ex_work.u_wrap;
                    gmTvxDrawWork[index].v_wrap = ex_work.v_wrap;
                    gmTvxDrawWork[index].all_vtx_num += vertexNum;
                    AppMain.GMS_TVX_DRAW_STACK gmsTvxDrawStack = gmTvxDrawWork[index].stack[(int)gmTvxDrawWork[index].stack_num];
                    gmsTvxDrawStack.vtx = AppMain.AoTvxGetVertex(model_tvx, tex_no);
                    gmsTvxDrawStack.vtx_num = vertexNum;
                    gmsTvxDrawStack.pos = pos;
                    gmsTvxDrawStack.scale = scale;
                    gmsTvxDrawStack.disp_flag = flag;
                    gmsTvxDrawStack.rotate_z = (int)rotate_z;
                    gmsTvxDrawStack.coord = ex_work.coord;
                    gmsTvxDrawStack.color = ex_work.color;
                    ++gmTvxDrawWork[index].stack_num;
                    break;
                }
            }
        }
    }

    private static void GmTvxExecuteDraw()
    {
        AppMain.GMS_TVX_DRAW_WORK[] gmTvxDrawWork = AppMain.gm_tvx_draw_work;
        if (gmTvxDrawWork == null || gmTvxDrawWork[0].tex == null)
            return;
        uint lightColor = AppMain.GmMainGetLightColor();
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.bldSrc = 770;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.bldDst = 771;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.bldMode = 32774;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.aTest = (short)1;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.zMask = (short)0;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.zTest = (short)1;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.noSort = (short)1;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.format3D = 4;
        for (uint index1 = 0; (long)index1 < (long)AppMain.GMD_TVX_DRAW_WORK_NUM && gmTvxDrawWork[(int)index1].tex_id != -1; ++index1)
        {
            AppMain._AMS_PARAM_DRAW_PRIMITIVE.ablend = 0;
            AppMain._AMS_PARAM_DRAW_PRIMITIVE.texlist = gmTvxDrawWork[(int)index1].tex;
            AppMain._AMS_PARAM_DRAW_PRIMITIVE.uwrap = gmTvxDrawWork[(int)index1].u_wrap;
            AppMain._AMS_PARAM_DRAW_PRIMITIVE.vwrap = gmTvxDrawWork[(int)index1].v_wrap;
            AppMain._AMS_PARAM_DRAW_PRIMITIVE.type = 1;
            AppMain._AMS_PARAM_DRAW_PRIMITIVE.count = (int)gmTvxDrawWork[(int)index1].all_vtx_num + (int)gmTvxDrawWork[(int)index1].stack_num * 2 - 2;
            AppMain.NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(AppMain._AMS_PARAM_DRAW_PRIMITIVE.count);
            AppMain.NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
            int offset = nnsPriM3DPctArray.offset;
            int num1 = 0;
            AppMain._AMS_PARAM_DRAW_PRIMITIVE.vtxPCT3D = nnsPriM3DPctArray;
            AppMain._AMS_PARAM_DRAW_PRIMITIVE.texId = gmTvxDrawWork[(int)index1].tex_id;
            AppMain.SNNS_MATRIX snnsMatrix1 = new AppMain.SNNS_MATRIX();
            AppMain.SNNS_MATRIX snnsMatrix2 = new AppMain.SNNS_MATRIX();
            AppMain.nnMakeUnitMatrix(ref snnsMatrix2);
            for (uint index2 = 0; index2 < gmTvxDrawWork[(int)index1].stack_num; ++index2)
            {
                AppMain.GMS_TVX_DRAW_STACK gmsTvxDrawStack = gmTvxDrawWork[(int)index1].stack[(int)index2];
                if (((int)gmsTvxDrawStack.disp_flag & (int)AppMain.GMD_TVX_DISP_BLEND) != 0)
                    AppMain._AMS_PARAM_DRAW_PRIMITIVE.ablend = 1;
                float num2 = AppMain.FXM_FX32_TO_FLOAT(gmsTvxDrawStack.pos.x);
                float num3 = -AppMain.FXM_FX32_TO_FLOAT(gmsTvxDrawStack.pos.y);
                float num4 = AppMain.FXM_FX32_TO_FLOAT(gmsTvxDrawStack.pos.z);
                AppMain.nnMakeUnitMatrix(ref snnsMatrix1);
                if (((int)gmsTvxDrawStack.disp_flag & (int)AppMain.GMD_TVX_DISP_ROTATE) != 0)
                    AppMain.nnRotateZMatrix(ref snnsMatrix1, ref snnsMatrix1, (int)(ushort)gmsTvxDrawStack.rotate_z);
                if (((int)gmsTvxDrawStack.disp_flag & (int)AppMain.GMD_TVX_DISP_SCALE) != 0)
                    AppMain.nnScaleMatrix(ref snnsMatrix1, ref snnsMatrix1, AppMain.FXM_FX32_TO_FLOAT(gmsTvxDrawStack.scale.x), AppMain.FXM_FX32_TO_FLOAT(gmsTvxDrawStack.scale.y), AppMain.FXM_FX32_TO_FLOAT(gmsTvxDrawStack.scale.z));
                uint num5 = lightColor;
                if (((int)gmsTvxDrawStack.disp_flag & (int)AppMain.GMD_TVX_DISP_LIGHT_DISABLE) != 0)
                    num5 = gmsTvxDrawStack.color;
                AppMain.SNNS_VECTOR src = new AppMain.SNNS_VECTOR();
                int num6 = num1 + offset;
                AppMain.AOS_TVX_VERTEX[] vtx = gmsTvxDrawStack.vtx;
                for (int index3 = 0; (long)index3 < (long)gmsTvxDrawStack.vtx_num; ++index3)
                {
                    src.x = vtx[index3].x;
                    src.y = vtx[index3].y;
                    src.z = vtx[index3].z;
                    int index4 = num6 + index3;
                    if (gmsTvxDrawStack.disp_flag != 0U)
                        AppMain.nnTransformVector(ref buffer[index4].Pos, ref snnsMatrix1, ref src);
                    else
                        buffer[index4].Pos.Assign(src.x, src.y, src.z);
                    buffer[index4].Pos.x += num2;
                    buffer[index4].Pos.y += num3;
                    buffer[index4].Pos.z += num4;
                    buffer[index4].Tex.u = vtx[index3].u + gmsTvxDrawStack.coord.u;
                    buffer[index4].Tex.v = vtx[index3].v + gmsTvxDrawStack.coord.v;
                    buffer[index4].Col = vtx[index3].c & num5;
                }
                num1 += (int)gmsTvxDrawStack.vtx_num + 2;
                if (index2 != 0U)
                {
                    int index3 = num6 - 1;
                    buffer[index3] = buffer[index3 + 1];
                }
                if ((int)index2 != (int)gmTvxDrawWork[(int)index1].stack_num - 1)
                {
                    int index3 = num6 + ((int)gmsTvxDrawStack.vtx_num - 1);
                    buffer[index3 + 1] = buffer[index3];
                }
            }
            AppMain.amMatrixPush(ref snnsMatrix2);
            AppMain.ObjDraw3DNNDrawPrimitive(AppMain._AMS_PARAM_DRAW_PRIMITIVE);
            AppMain.amMatrixPop();
            gmTvxDrawWork[(int)index1].tex = (AppMain.NNS_TEXLIST)null;
            gmTvxDrawWork[(int)index1].tex_id = -1;
            gmTvxDrawWork[(int)index1].stack_num = 0U;
            gmTvxDrawWork[(int)index1].all_vtx_num = 0U;
        }
    }

    public static int gm_map_prim_draw_tvx_mgr_index_tbl_z2_num
    {
        get
        {
            return ((AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR_INDEX[])AppMain.gm_map_prim_draw_tvx_mgr_index_tbl_z2).Length;
        }
    }

    public static int gm_map_prim_draw_tvx_mgr_index_tbl_z3_num
    {
        get
        {
            return ((AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR_INDEX[])AppMain.gm_map_prim_draw_tvx_mgr_index_tbl_z3).Length;
        }
    }

    public static int gm_map_prim_draw_tvx_mgr_index_tbl_z4_num
    {
        get
        {
            return ((AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR_INDEX[])AppMain.gm_map_prim_draw_tvx_mgr_index_tbl_z4).Length;
        }
    }

    public static int gm_map_prim_draw_tvx_mgr_index_tbl_zf_num
    {
        get
        {
            return ((AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR_INDEX[])AppMain.gm_map_prim_draw_tvx_mgr_index_tbl_zf).Length;
        }
    }


}