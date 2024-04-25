public partial class AppMain
{
    public class AMS_DRAWSTATE
    {
        public readonly AMS_DRAWSTATE_DIFFUSE diffuse = new AMS_DRAWSTATE_DIFFUSE();
        public readonly AMS_DRAWSTATE_AMBIENT ambient = new AMS_DRAWSTATE_AMBIENT();
        public readonly AMS_DRAWSTATE_SPECULAR specular = new AMS_DRAWSTATE_SPECULAR();
        public readonly AMS_DRAWSTATE_ENVMAP envmap = new AMS_DRAWSTATE_ENVMAP();
        public readonly AMS_DRAWSTATE_ALPHA alpha = new AMS_DRAWSTATE_ALPHA();
        public readonly AMS_DRAWSTATE_BLEND blend = new AMS_DRAWSTATE_BLEND();
        public readonly AMS_DRAWSTATE_TEXOFFSET[] texoffset = New<AMS_DRAWSTATE_TEXOFFSET>(4);
        public readonly AMS_DRAWSTATE_FOG fog = new AMS_DRAWSTATE_FOG();
        public readonly AMS_DRAWSTATE_FOG_COLOR fog_color = new AMS_DRAWSTATE_FOG_COLOR();
        public readonly AMS_DRAWSTATE_FOG_RANGE fog_range = new AMS_DRAWSTATE_FOG_RANGE();
        public readonly AMS_DRAWSTATE_Z_MODE zmode = new AMS_DRAWSTATE_Z_MODE();
        public uint drawflag;

        public AMS_DRAWSTATE()
        {
        }

        public AMS_DRAWSTATE(AMS_DRAWSTATE drawState)
        {
            this.drawflag = drawState.drawflag;
            this.diffuse.Assign(drawState.diffuse);
            this.ambient.Assign(drawState.ambient);
            this.specular.Assign(drawState.specular);
            this.envmap.Assign(drawState.envmap);
            this.alpha.Assign(drawState.alpha);
            this.blend.Assign(drawState.blend);
            for (int index = 0; index < 4; ++index)
                this.texoffset[index].Assign(drawState.texoffset[index]);
            this.fog.flag = drawState.fog.flag;
            this.fog_color.Assign(drawState.fog_color);
            this.fog_range.Assign(drawState.fog_range);
            this.zmode.Assign(drawState.zmode);
        }

        public AMS_DRAWSTATE Assign(AMS_DRAWSTATE drawState)
        {
            if (this != drawState)
            {
                this.drawflag = drawState.drawflag;
                this.diffuse.Assign(drawState.diffuse);
                this.ambient.Assign(drawState.ambient);
                this.specular.Assign(drawState.specular);
                this.envmap.Assign(drawState.envmap);
                this.alpha.Assign(drawState.alpha);
                this.blend.Assign(drawState.blend);
                for (int index = 0; index < 4; ++index)
                    this.texoffset[index].Assign(drawState.texoffset[index]);
                this.fog.flag = drawState.fog.flag;
                this.fog_color.Assign(drawState.fog_color);
                this.fog_range.Assign(drawState.fog_range);
                this.zmode.Assign(drawState.zmode);
            }
            return this;
        }

        public void Clear()
        {
            this.drawflag = 0U;
            this.diffuse.Clear();
            this.ambient.Clear();
            this.specular.Clear();
            this.envmap.Clear();
            this.alpha.Clear();
            this.blend.Clear();
            for (int index = 0; index < 4; ++index)
                this.texoffset[index].Clear();
            this.fog.Clear();
            this.fog_color.Clear();
            this.fog_range.Clear();
            this.zmode.Clear();
        }
    }
}
