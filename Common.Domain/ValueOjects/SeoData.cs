

namespace Common.Domain.ValueOjects
{
    public class SeoData
    {
        public SeoData(string metaTitle,
                       string metaDescription,
                       string metaKeywords,
                       bool indexPage,
                       string canonical,
                       string shcema)
        {
            MetaTitle = metaTitle;
            MetaDescription = metaDescription;
            MetaKeywords = metaKeywords;
            IndexPage = indexPage;
            Canonical = canonical;
            Shcema = shcema;
        }

        public string MetaTitle { get; }
        public string MetaDescription { get; }
        public string MetaKeywords { get; }

        public bool IndexPage { get; }
        public string Canonical { get; }
        public string Shcema { get; }
    }
}
