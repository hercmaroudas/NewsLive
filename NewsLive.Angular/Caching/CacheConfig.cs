namespace NewsLive.Angular.Caching
{
    using System.Configuration;

    public class CacheConfig
    {
        private static CacheConfig instance = null;

        private CacheConfig()
        {
        }

        public static CacheConfig Instance
        {
            get { if (instance == null) { instance = new CacheConfig(); } return instance; }
        }

        public string Bucket
        {
            get
            {
                return ConfigurationManager.AppSettings["couchbaseBucketName"];
            }
        }

        public string Server
        {
            get
            {
                return ConfigurationManager.AppSettings["couchbaseServer"];
            }
        }

        public string Password
        {
            get
            {
                return ConfigurationManager.AppSettings["couchbasePassword"];
            }
        }

        public string User
        {
            get
            {
                return ConfigurationManager.AppSettings["couchbaseUser"];
            }
        }
    }
}