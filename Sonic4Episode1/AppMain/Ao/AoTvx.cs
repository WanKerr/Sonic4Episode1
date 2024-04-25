public partial class AppMain
{

    public class TVX_FILE
    {
        public TVXS_HEADER header;
        public TVXS_TEXTURE[] textures;
        public AOS_TVX_VERTEX[][] vertexes;

        public TVX_FILE(AmbChunk data)
        {
            this.header = new TVXS_HEADER(data.array, data.offset);
            this.textures = new TVXS_TEXTURE[(int)this.header.tex_num];
            this.vertexes = new AOS_TVX_VERTEX[(int)this.header.tex_num][];
            for (int index1 = 0; index1 < this.textures.Length; ++index1)
            {
                int offset1 = (int)(header.tex_tbl_ofst + index1 * TVXS_TEXTURE.SizeBytes) + data.offset;
                this.textures[index1] = new TVXS_TEXTURE(data.array, offset1);
                this.vertexes[index1] = new AOS_TVX_VERTEX[(int)this.textures[index1].vtx_num];
                for (int index2 = 0; index2 < textures[index1].vtx_num; ++index2)
                {
                    int offset2 = (int)(textures[index1].vtx_tbl_ofst + index2 * (int)AOS_TVX_VERTEX.SizeBytes) + data.offset;
                    this.vertexes[index1][index2] = new AOS_TVX_VERTEX(data.array, offset2);
                }
            }
        }
    }

    private static bool AoTvxIsTvxFile(object file)
    {
        return true;
    }

    private static uint AoTvxGetTextureNum(TVX_FILE file)
    {
        return file.header.tex_num;
    }

    private static int AoTvxGetTextureId(TVX_FILE file, uint tex_no)
    {
        return file.textures[(int)tex_no].tex_id;
    }

    private static uint AoTvxGetPrimitiveType(TVX_FILE file, uint tex_no)
    {
        return file.textures[(int)tex_no].prim_type;
    }

    private static uint AoTvxGetVertexNum(TVX_FILE file, uint tex_no)
    {
        return file.textures[(int)tex_no].vtx_num;
    }

    private static AOS_TVX_VERTEX[] AoTvxGetVertex(TVX_FILE file, uint tex_no)
    {
        return file.vertexes[(int)tex_no];
    }



}