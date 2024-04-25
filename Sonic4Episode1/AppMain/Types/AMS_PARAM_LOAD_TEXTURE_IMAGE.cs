using Microsoft.Xna.Framework.Graphics;

public partial class AppMain
{
    public struct AMS_PARAM_LOAD_TEXTURE_IMAGE
    {
        public Texture2D texture;
        public int size;
        public short minfilter;
        public short magfilter;
        public short u_wrap;
        public short v_wrap;
    }
}
