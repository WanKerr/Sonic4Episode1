using Microsoft.Xna.Framework;

public partial class AppMain
{
    public struct RGBA_U8
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;

        public static explicit operator Color(RGBA_U8 c)
        {
            return new Color(c.r, c.g, c.b, (int)c.a);
        }
    }
}
