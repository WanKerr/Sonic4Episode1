public partial class AppMain
{
    private struct CRegionTable
    {
        public string country;
        public GSE_REGION region;

        public CRegionTable(string country, GSE_REGION region)
        {
            this.country = country;
            this.region = region;
        }
    }
}
