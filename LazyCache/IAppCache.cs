using System;
#if NET45 || NET46
using System.Runtime.Caching;
#else
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
#endif
using System.Threading.Tasks;

namespace LazyCache
{

#if NETSTANDARD2_0
	public class ObjectCache : Microsoft.Extensions.Caching.Memory.MemoryCache
	{
		public ObjectCache( IOptions<MemoryCacheOptions> optionsAccessor ) : base( optionsAccessor )
		{

		}

		public object this[ string key ]
		{
			get { return this.Get( key ); }
		}

		//
		// Summary:
		//     When overridden in a derived class, inserts a cache entry into the cache, specifying
		//     a key and a value for the cache entry, and information about how the entry will
		//     be evicted.
		//
		// Parameters:
		//   key:
		//     A unique identifier for the cache entry.
		//
		//   value:
		//     The object to insert.
		//
		//   policy:
		//     An object that contains eviction details for the cache entry. This object provides
		//     more options for eviction than a simple absolute expiration.
		//
		//   regionName:
		//     Optional. A named region in the cache to which the cache entry can be added,
		//     if regions are implemented. The default value for the optional parameter is null.
		//
		// Returns:
		//     If a cache entry with the same key exists, the specified cache entry's value;
		//     otherwise, null.
		public object AddOrGetExisting( string key , object value , CacheItemPolicy policy )
		{
			object result;
			return !this.TryGetValue( key , out result ) ? value : result;
		}


	}

	public class CacheItemPolicy : MemoryCacheEntryOptions
	{
		//public PostEvictionCallbackRegistration RemovedCallback { get { return this.PostEvictionCallbacks[ 0 ]; } }
	}

#endif
	public interface IAppCache
    {
		
        ObjectCache ObjectCache { get; }
        void Add<T>(string key, T item);
        void Add<T>(string key, T item, DateTimeOffset absoluteExpiration);
        void Add<T>(string key, T item, TimeSpan slidingExpiration);
        void Add<T>(string key, T item, CacheItemPolicy policy);

        T Get<T>(string key);

        T GetOrAdd<T>(string key, Func<T> addItemFactory);
        T GetOrAdd<T>(string key, Func<T> addItemFactory, DateTimeOffset absoluteExpiration);
        T GetOrAdd<T>(string key, Func<T> addItemFactory, TimeSpan slidingExpiration);
        T GetOrAdd<T>(string key, Func<T> addItemFactory, CacheItemPolicy policy);

        void Remove(string key);

        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory);
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory, CacheItemPolicy policy);
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory, DateTimeOffset expires);
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory, TimeSpan slidingExpiration);
        Task<T> GetAsync<T>(string key);
    }
}