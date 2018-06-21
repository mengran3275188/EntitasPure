using System;
using System.Collections.Generic;

namespace DesperateDevs.Utils
{
	public class ObjectPool<T>
	{
		private readonly Func<T> _factoryMethod;

		private readonly Action<T> _resetMethod;

		private readonly Stack<T> _objectPool;

		public ObjectPool(Func<T> factoryMethod, Action<T> resetMethod = null)
		{
			this._factoryMethod = factoryMethod;
			this._resetMethod = resetMethod;
			this._objectPool = new Stack<T>();
		}

		public T Get()
		{
			if (this._objectPool.Count != 0)
			{
				return this._objectPool.Pop();
			}
			return this._factoryMethod();
		}

		public void Push(T obj)
		{
			if (this._resetMethod != null)
			{
				this._resetMethod(obj);
			}
			this._objectPool.Push(obj);
		}
	}
}
