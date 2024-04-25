public partial class AppMain
{
    public class NNS_MATERIALNAME
    {
        public int iMaterial;
        public string Name;

        public NNS_MATERIALNAME()
        {
        }

        public NNS_MATERIALNAME(NNS_MATERIALNAME materialName)
        {
            this.iMaterial = materialName.iMaterial;
            this.Name = materialName.Name;
        }

        public NNS_MATERIALNAME Assign(NNS_MATERIALNAME materialName)
        {
            this.iMaterial = materialName.iMaterial;
            this.Name = materialName.Name;
            return this;
        }
    }
}
