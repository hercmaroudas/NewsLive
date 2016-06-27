namespace NewsLive.Angular.Caching
{
    using System.Linq;

    using Couchbase;
    using Couchbase.Linq;
    using Couchbase.Core;
    using DataAccess.Models;

    public class CacheService : BucketContext, ICacheService
    {
        public IBucket Bucket { get; private set; }

        public CacheService() : 
            this(ClusterHelper.GetBucket(CacheConfig.Instance.Bucket))
        {
            Bucket = ClusterHelper.GetBucket(CacheConfig.Instance.Bucket);
            
        }

        public CacheService(IBucket bucket) 
            : base(bucket)
        {
        }

        public IQueryable<ArticleModel> Articles
        {
            get
            {
                return Query<ArticleModel>();
            }
        }
        
    }
}