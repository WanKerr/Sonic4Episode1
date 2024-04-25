public partial class AppMain
{
    public class AMS_TRAIL_PARAM
    {
        public NNS_RGBA startColor;
        public NNS_RGBA endColor;
        public NNS_RGBA ptclColor;
        public float startSize;
        public float endSize;
        public OBS_OBJECT_WORK trail_obj_work;
        public NNS_TEXLIST texlist;
        public int texId;
        public float life;
        public float vanish_time;
        public float zBias;
        public float ptclSize;
        public short partsNum;
        public short ptclFlag;
        public short ptclTexId;
        public short blendType;
        public short zTest;
        public short zMask;
        public float time;
        public float vanish_rate;
        public short trailId;
        public short trailPartsId;
        public short trailPartsNum;
        public short state;
        public short list_no;

        public VecFx32 trail_pos => this.trail_obj_work.pos;

        public void Clear()
        {
            this.startColor.Clear();
            this.endColor.Clear();
            this.ptclColor.Clear();
            this.startSize = this.endSize = 0.0f;
            this.trail_obj_work = null;
            this.texlist = null;
            this.texId = 0;
            this.life = this.vanish_time = this.zBias = 0.0f;
            this.ptclSize = 0.0f;
            this.partsNum = this.ptclFlag = this.ptclTexId = this.blendType = this.zTest = this.zMask = 0;
            this.time = this.vanish_rate = 0.0f;
            this.trailId = this.trailPartsId = this.trailPartsNum = this.state = 0;
            this.list_no = 0;
        }

        public AMS_TRAIL_PARAM Assign(AMS_TRAIL_PARAM source)
        {
            if (this == source)
                return this;
            this.startColor = source.startColor;
            this.endColor = source.endColor;
            this.ptclColor = source.ptclColor;
            this.startSize = source.startSize;
            this.endSize = source.endSize;
            this.trail_obj_work = source.trail_obj_work;
            this.texlist = source.texlist;
            this.texId = source.texId;
            this.life = source.life;
            this.vanish_time = source.vanish_time;
            this.zBias = source.zBias;
            this.ptclSize = source.ptclSize;
            this.partsNum = source.partsNum;
            this.ptclFlag = source.ptclFlag;
            this.ptclTexId = source.ptclTexId;
            this.blendType = source.blendType;
            this.zTest = source.zTest;
            this.zMask = source.zMask;
            this.time = source.time;
            this.vanish_rate = source.vanish_rate;
            this.trailId = source.trailId;
            this.trailPartsId = source.trailPartsId;
            this.trailPartsNum = source.trailPartsNum;
            this.state = source.state;
            this.list_no = source.list_no;
            return this;
        }
    }
}
