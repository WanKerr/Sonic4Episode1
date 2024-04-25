using mpp;

public partial class AppMain
{
    public class NNS_MATERIAL_COLOR
    {
        public uint fFlag;
        public NNS_RGBA Ambient;
        public NNS_RGBA Diffuse;
        public NNS_RGBA Specular;
        public NNS_RGBA Emission;
        public float Shininess;
        public uint VtxColMaterial;

        public NNS_MATERIAL_COLOR()
        {
        }

        public NNS_MATERIAL_COLOR(NNS_MATERIAL_COLOR matColor)
        {
            this.fFlag = matColor.fFlag;
            this.Ambient = matColor.Ambient;
            this.Diffuse = matColor.Diffuse;
            this.Specular = matColor.Specular;
            this.Emission = matColor.Emission;
            this.Shininess = matColor.Shininess;
            this.VtxColMaterial = matColor.VtxColMaterial;
        }

        public NNS_MATERIAL_COLOR Assign(NNS_MATERIAL_COLOR matColor)
        {
            this.fFlag = matColor.fFlag;
            this.Ambient = matColor.Ambient;
            this.Diffuse = matColor.Diffuse;
            this.Specular = matColor.Specular;
            this.Emission = matColor.Emission;
            this.Shininess = matColor.Shininess;
            this.VtxColMaterial = matColor.VtxColMaterial;
            return this;
        }

        public static explicit operator NNS_MATERIAL_STDSHADER_COLOR(
          NNS_MATERIAL_COLOR color)
        {
            return new NNS_MATERIAL_STDSHADER_COLOR()
            {
                Ambient = color.Ambient,
                Diffuse = color.Diffuse,
                Emission = color.Emission,
                fFlag = color.fFlag,
                Shininess = color.Shininess,
                Specular = color.Specular,
                SpecularIntensity = MppBitConverter.UInt32ToSingle(color.VtxColMaterial)
            };
        }
    }
}
