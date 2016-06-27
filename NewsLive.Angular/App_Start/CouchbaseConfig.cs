namespace NewsLive.Angular
{
    using Caching;
    using Couchbase;
    using Couchbase.Configuration.Client;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// CouchbaseConfigHelper class is to wrap calls to read the AppSettings section of web.config file.
    /// </summary>
    public static class CouchbaseConfig
    {

        public static void Initialize()
        {
            var config = new ClientConfiguration();
            config.BucketConfigs.Clear();

            config.Servers = new List<Uri>(new Uri[] { new Uri(CacheConfig.Instance.Server) });
            
            config.BucketConfigs.Add(
                CacheConfig.Instance.Bucket,
                new BucketConfiguration
                {
                    BucketName = CacheConfig.Instance.Bucket,
                    Username = CacheConfig.Instance.User,
                    Password = CacheConfig.Instance.Password
                });

            ClusterHelper.Initialize(config);
        }

        public static void Close()
        {
            /*
            The Couchbase Client includes a helper class called ClusterHelper. This class is a singleton that can 
            be shared globally in the application and should be kept alive for the lifetime of the application. 
             
            The benefit of using ClusterHelper is that resources are shared across the the application and thereby 
            setup and tear down is not done unless explicitly needed.

            It is recommended by Couchbase to use ClusterHelper when working with the .NET Client and part of the 
            best practices.
            */
            ClusterHelper.Close();
        }
    }
}