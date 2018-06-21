using System;
using System.Collections.Generic;

namespace DesperateDevs.Utils
{
	public class ObjectCache
	{
		private readonly Dictionary<Type, object> _objectPools;

		public ObjectCache()
		{
			this._objectPools = new Dictionary<Type, object>();
		}

		public ObjectPool<T> GetObjectPool<T>() where T : new()
		{
			Type typeFromHandle = typeof(T);
			object obj = default(object);
			if (!this._objectPools.TryGetValue(typeFromHandle, out obj))
			{
				obj = new ObjectPool<T>((Func<T>)(() => new T()), (Action<T>)null);
				this._objectPools.Add(typeFromHandle, obj);
			}
			return (ObjectPool<T>)obj;
		}

		public T Get<T>() where T : new()
		{
			return this.GetObjectPool<T>().Get();
		}

		public void Push<T>(T obj) where T : new()
		{
			this.GetObjectPool<T>().Push(obj);
		}

		public void RegisterCustomObjectPool<T>(ObjectPool<T> objectPool)
		{
			this._objectPools.Add(typeof(T), (object)objectPool);
		}

		public void Reset()
		{
			this._objectPools.Clear();
		}
	}
}
