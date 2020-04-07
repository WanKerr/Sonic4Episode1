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

    public class TVX_FILE
    {
        public AppMain.TVXS_HEADER header;
        public AppMain.TVXS_TEXTURE[] textures;
        public AppMain.AOS_TVX_VERTEX[][] vertexes;

        public TVX_FILE(AppMain.AmbChunk data)
        {
            this.header = new AppMain.TVXS_HEADER(data.array, data.offset);
            this.textures = new AppMain.TVXS_TEXTURE[(int)this.header.tex_num];
            this.vertexes = new AppMain.AOS_TVX_VERTEX[(int)this.header.tex_num][];
            for (int index1 = 0; index1 < this.textures.Length; ++index1)
            {
                int offset1 = (int)((long)this.header.tex_tbl_ofst + (long)(index1 * AppMain.TVXS_TEXTURE.SizeBytes)) + data.offset;
                this.textures[index1] = new AppMain.TVXS_TEXTURE(data.array, offset1);
                this.vertexes[index1] = new AppMain.AOS_TVX_VERTEX[(int)this.textures[index1].vtx_num];
                for (int index2 = 0; (long)index2 < (long)this.textures[index1].vtx_num; ++index2)
                {
                    int offset2 = (int)((long)this.textures[index1].vtx_tbl_ofst + (long)(index2 * (int)AppMain.AOS_TVX_VERTEX.SizeBytes)) + data.offset;
                    this.vertexes[index1][index2] = new AppMain.AOS_TVX_VERTEX(data.array, offset2);
                }
            }
        }
    }

    private static bool AoTvxIsTvxFile(object file)
    {
        return true;
    }

    private static uint AoTvxGetTextureNum(AppMain.TVX_FILE file)
    {
        return file.header.tex_num;
    }

    private static int AoTvxGetTextureId(AppMain.TVX_FILE file, uint tex_no)
    {
        return file.textures[(int)tex_no].tex_id;
    }

    private static uint AoTvxGetPrimitiveType(AppMain.TVX_FILE file, uint tex_no)
    {
        return file.textures[(int)tex_no].prim_type;
    }

    private static uint AoTvxGetVertexNum(AppMain.TVX_FILE file, uint tex_no)
    {
        return file.textures[(int)tex_no].vtx_num;
    }

    private static AppMain.AOS_TVX_VERTEX[] AoTvxGetVertex(AppMain.TVX_FILE file, uint tex_no)
    {
        return file.vertexes[(int)tex_no];
    }



}