public partial class AppMain
{
    private static void GmTvxBuild()
    {
        gm_tvx_draw_work = New<GMS_TVX_DRAW_WORK>(GMD_TVX_DRAW_WORK_NUM);
        GmTvxInit();
    }

    private static void GmTvxInit()
    {
        GMS_TVX_DRAW_WORK[] gmTvxDrawWork = gm_tvx_draw_work;
        for (int index = 0; index < GMD_TVX_DRAW_WORK_NUM; ++index)
        {
            gmTvxDrawWork[index].Clear();
            gmTvxDrawWork[index].tex_id = -1;
        }
    }

    private static void GmTvxExit()
    {
        GmTvxInit();
    }

    private static void GmTvxFlush()
    {
        GMS_TVX_DRAW_WORK[] gmTvxDrawWork = gm_tvx_draw_work;
        gm_tvx_draw_work = null;
    }

    private static void GmTvxSetModel(
      TVX_FILE model_tvx,
      NNS_TEXLIST model_tex,
      ref VecFx32 pos,
      ref VecFx32 scale,
      uint flag,
      short rotate_z)
    {
        var work = new GMS_TVX_EX_WORK()
        {
            u_wrap = 1,
            v_wrap = 1,
            coord = {
        u = 0.0f,
        v = 0.0f
      },
            color = uint.MaxValue
        };
        GmTvxSetModelEx(model_tvx, model_tex, ref pos, ref scale, flag, rotate_z, ref work);
    }

    private static void GmTvxSetModelEx(
      TVX_FILE model_tvx,
      NNS_TEXLIST model_tex,
      ref VecFx32 pos,
      ref VecFx32 scale,
      uint flag,
      short rotate_z,
      ref GMS_TVX_EX_WORK ex_work)
    {
        if (!GmMainIsDrawEnable())
            return;
        GMS_TVX_DRAW_WORK[] gmTvxDrawWork = gm_tvx_draw_work;
        uint textureNum = AoTvxGetTextureNum(model_tvx);
        for (uint tex_no = 0; tex_no < textureNum; ++tex_no)
        {
            uint vertexNum = AoTvxGetVertexNum(model_tvx, tex_no);
            int textureId = AoTvxGetTextureId(model_tvx, tex_no);
            for (int index = 0; index < GMD_TVX_DRAW_WORK_NUM; ++index)
            {
                if (gmTvxDrawWork[index].tex == null && gmTvxDrawWork[index].tex_id == -1 || gmTvxDrawWork[index].tex == model_tex && gmTvxDrawWork[index].tex_id == textureId && (gmTvxDrawWork[index].u_wrap == ex_work.u_wrap && gmTvxDrawWork[index].v_wrap == ex_work.v_wrap))
                {
                    if (gmTvxDrawWork[index].stack_num >= GMD_TVX_DRAW_STACK_NUM)
                        return;
                    gmTvxDrawWork[index].tex = model_tex;
                    gmTvxDrawWork[index].tex_id = textureId;
                    gmTvxDrawWork[index].u_wrap = ex_work.u_wrap;
                    gmTvxDrawWork[index].v_wrap = ex_work.v_wrap;
                    gmTvxDrawWork[index].all_vtx_num += vertexNum;
                    GMS_TVX_DRAW_STACK gmsTvxDrawStack = gmTvxDrawWork[index].stack[(int)gmTvxDrawWork[index].stack_num];
                    gmsTvxDrawStack.vtx = AoTvxGetVertex(model_tvx, tex_no);
                    gmsTvxDrawStack.vtx_num = vertexNum;
                    gmsTvxDrawStack.pos = pos;
                    gmsTvxDrawStack.scale = scale;
                    gmsTvxDrawStack.disp_flag = flag;
                    gmsTvxDrawStack.rotate_z = rotate_z;
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
        GMS_TVX_DRAW_WORK[] gmTvxDrawWork = gm_tvx_draw_work;
        if (gmTvxDrawWork == null || gmTvxDrawWork[0].tex == null)
            return;
        uint lightColor = GmMainGetLightColor();
        _AMS_PARAM_DRAW_PRIMITIVE.bldSrc = 770;
        _AMS_PARAM_DRAW_PRIMITIVE.bldDst = 771;
        _AMS_PARAM_DRAW_PRIMITIVE.bldMode = 32774;
        _AMS_PARAM_DRAW_PRIMITIVE.aTest = 1;
        _AMS_PARAM_DRAW_PRIMITIVE.zMask = 0;
        _AMS_PARAM_DRAW_PRIMITIVE.zTest = 1;
        _AMS_PARAM_DRAW_PRIMITIVE.noSort = 1;
        _AMS_PARAM_DRAW_PRIMITIVE.format3D = 4;
        for (uint index1 = 0; index1 < GMD_TVX_DRAW_WORK_NUM && gmTvxDrawWork[(int)index1].tex_id != -1; ++index1)
        {
            _AMS_PARAM_DRAW_PRIMITIVE.ablend = 0;
            _AMS_PARAM_DRAW_PRIMITIVE.texlist = gmTvxDrawWork[(int)index1].tex;
            _AMS_PARAM_DRAW_PRIMITIVE.uwrap = gmTvxDrawWork[(int)index1].u_wrap;
            _AMS_PARAM_DRAW_PRIMITIVE.vwrap = gmTvxDrawWork[(int)index1].v_wrap;
            _AMS_PARAM_DRAW_PRIMITIVE.type = 1;
            _AMS_PARAM_DRAW_PRIMITIVE.count = (int)gmTvxDrawWork[(int)index1].all_vtx_num + (int)gmTvxDrawWork[(int)index1].stack_num * 2 - 2;
            NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = amDrawAlloc_NNS_PRIM3D_PCT(_AMS_PARAM_DRAW_PRIMITIVE.count);
            NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
            int offset = nnsPriM3DPctArray.offset;
            int num1 = 0;
            _AMS_PARAM_DRAW_PRIMITIVE.vtxPCT3D = nnsPriM3DPctArray;
            _AMS_PARAM_DRAW_PRIMITIVE.texId = gmTvxDrawWork[(int)index1].tex_id;
            SNNS_MATRIX snnsMatrix1 = new SNNS_MATRIX();
            SNNS_MATRIX snnsMatrix2 = new SNNS_MATRIX();
            nnMakeUnitMatrix(ref snnsMatrix2);
            for (uint index2 = 0; index2 < gmTvxDrawWork[(int)index1].stack_num; ++index2)
            {
                GMS_TVX_DRAW_STACK gmsTvxDrawStack = gmTvxDrawWork[(int)index1].stack[(int)index2];
                if (((int)gmsTvxDrawStack.disp_flag & (int)GMD_TVX_DISP_BLEND) != 0)
                    _AMS_PARAM_DRAW_PRIMITIVE.ablend = 1;
                float num2 = FXM_FX32_TO_FLOAT(gmsTvxDrawStack.pos.x);
                float num3 = -FXM_FX32_TO_FLOAT(gmsTvxDrawStack.pos.y);
                float num4 = FXM_FX32_TO_FLOAT(gmsTvxDrawStack.pos.z);
                nnMakeUnitMatrix(ref snnsMatrix1);
                if (((int)gmsTvxDrawStack.disp_flag & (int)GMD_TVX_DISP_ROTATE) != 0)
                    nnRotateZMatrix(ref snnsMatrix1, ref snnsMatrix1, (ushort)gmsTvxDrawStack.rotate_z);
                if (((int)gmsTvxDrawStack.disp_flag & (int)GMD_TVX_DISP_SCALE) != 0)
                    nnScaleMatrix(ref snnsMatrix1, ref snnsMatrix1, FXM_FX32_TO_FLOAT(gmsTvxDrawStack.scale.x), FXM_FX32_TO_FLOAT(gmsTvxDrawStack.scale.y), FXM_FX32_TO_FLOAT(gmsTvxDrawStack.scale.z));
                uint num5 = lightColor;
                if (((int)gmsTvxDrawStack.disp_flag & (int)GMD_TVX_DISP_LIGHT_DISABLE) != 0)
                    num5 = gmsTvxDrawStack.color;
                SNNS_VECTOR src = new SNNS_VECTOR();
                int num6 = num1 + offset;
                AOS_TVX_VERTEX[] vtx = gmsTvxDrawStack.vtx;
                for (int index3 = 0; index3 < gmsTvxDrawStack.vtx_num; ++index3)
                {
                    src.x = vtx[index3].x;
                    src.y = vtx[index3].y;
                    src.z = vtx[index3].z;
                    int index4 = num6 + index3;
                    if (gmsTvxDrawStack.disp_flag != 0U)
                        nnTransformVector(ref buffer[index4].Pos, ref snnsMatrix1, ref src);
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
            amMatrixPush(ref snnsMatrix2);
            ObjDraw3DNNDrawPrimitive(_AMS_PARAM_DRAW_PRIMITIVE);
            amMatrixPop();
            gmTvxDrawWork[(int)index1].tex = null;
            gmTvxDrawWork[(int)index1].tex_id = -1;
            gmTvxDrawWork[(int)index1].stack_num = 0U;
            gmTvxDrawWork[(int)index1].all_vtx_num = 0U;
        }
    }

    public static int gm_map_prim_draw_tvx_mgr_index_tbl_z2_num => ((GMS_MAP_PRIM_DRAW_TVX_MGR_INDEX[])gm_map_prim_draw_tvx_mgr_index_tbl_z2).Length;

    public static int gm_map_prim_draw_tvx_mgr_index_tbl_z3_num => ((GMS_MAP_PRIM_DRAW_TVX_MGR_INDEX[])gm_map_prim_draw_tvx_mgr_index_tbl_z3).Length;

    public static int gm_map_prim_draw_tvx_mgr_index_tbl_z4_num => ((GMS_MAP_PRIM_DRAW_TVX_MGR_INDEX[])gm_map_prim_draw_tvx_mgr_index_tbl_z4).Length;

    public static int gm_map_prim_draw_tvx_mgr_index_tbl_zf_num => ((GMS_MAP_PRIM_DRAW_TVX_MGR_INDEX[])gm_map_prim_draw_tvx_mgr_index_tbl_zf).Length;


}