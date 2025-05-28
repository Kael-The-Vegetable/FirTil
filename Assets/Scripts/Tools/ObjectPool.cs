using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use this class to create a number of instantiated objects at the beginning of the scene loading.
/// </summary>
/// <typeparam name="T">T should be a class that implements the IPoolable interface</typeparam>
[Serializable]
public class ObjectPool<T> where T : Component
{
	#region Fields and Properties
	[field: SerializeField] public string Name { get; private set; }
	[field: SerializeField] public T Prefab { get; private set; }
	[field: SerializeField] public int PoolSize { get; private set; }
	[field: SerializeField] public bool ExpandablePool { get; private set; }

	private Transform _parentTransform;
	private List<T> _pool = new List<T>();
	#endregion

	#region Constructors

	/// <summary>
	/// Use this constructor to create new pools through code.
	/// </summary>
	/// <param name="prefab">A prefab of the object desired to instantiate</param>
	/// <param name="poolSize">The size of the pool to create</param>
	/// <param name="expandablePool">A boolean to determine if we want to be able to increase the pool size at runtime</param>
	public ObjectPool(T prefab, int poolSize, bool expandablePool = false)
	{
		Prefab = prefab;
		Name = prefab.name;
		PoolSize = poolSize;
		ExpandablePool = expandablePool;
	}

	/// <summary>
	/// Use this constructor to create new pools through code with custom name.
	/// </summary>
	/// <param name="name">The custom name for the pool</param>
	/// <param name="prefab">A prefab of the object desired to instantiate</param>
	/// <param name="poolSize">The size of the pool to create</param>
	/// <param name="expandablePool">A boolean to determine if we want to be able to increase the pool size at runtime</param>
	public ObjectPool(string name, T prefab, int poolSize, bool expandablePool = false)
	{
		Name = string.IsNullOrEmpty(name) ? prefab.name : name;
		Prefab = prefab;
		PoolSize = poolSize;
		ExpandablePool = expandablePool;
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// Use this Property to activate an object and gain its <typeparamref name="T"/>
	/// </summary>
	public T GetObject
	{
		get
		{
			for (int i = 0; i < _pool.Count; i++)
			{
				if (!_pool[i].gameObject.activeInHierarchy)
				{
					_pool[i].gameObject.SetActive(true);
					return _pool[i];
				}
			}

			if (ExpandablePool)
			{
				var newObj = UnityEngine.Object.Instantiate(Prefab, _parentTransform);
				_pool.Add(newObj);
				PoolSize = _pool.Count;
				return newObj;
			}

			return null;
		}
	}
	/// <summary>
	/// Use this Property to gain an inactive object and its <typeparamref name="T"/>
	/// </summary>
	public T GetInactiveObject
	{
		get
		{
			for (int i = 0; i < _pool.Count; i++)
			{
				if (!_pool[i].gameObject.activeInHierarchy)
				{
					return _pool[i];
				}
			}

			if (ExpandablePool)
			{
				var newObj = UnityEngine.Object.Instantiate(Prefab, _parentTransform);
				_pool.Add(newObj);
				PoolSize = _pool.Count;
				newObj.gameObject.SetActive(false);
				return newObj;
			}

			return null;
		}
	}
	/// <summary>
	/// Use this method to truly create the pool with a number of instances equal to <see cref="PoolSize"/>.
	/// </summary>
	/// <param name="parent">Supply a transform if you want the objects to be instantiated under a specific object</param>
	public async void CreatePool(Transform parent = null)
	{
		if (Prefab == null)
		{
			Debug.LogError("Pool does not have a Prefab set.");
			return;
		}

		if (parent == null)
		{
			parent = new GameObject($"ObjectPool : {Prefab.name}").transform;
		}
		_parentTransform = parent;

		var objs = await UnityEngine.Object.InstantiateAsync(Prefab, PoolSize, new InstantiateParameters() { parent = _parentTransform });
		_pool = new List<T>(PoolSize);
		foreach (var obj in objs)
		{
			_pool.Add(obj);
			obj.gameObject.SetActive(false);
		}

	}
	public void CreatePoolInstantly(Transform parent = null)
	{
		if (Prefab == null)
		{
			Debug.LogError("Pool does not have a Prefab set.");
			return;
		}

		if (parent == null)
		{
			parent = new GameObject($"ObjectPool : {Prefab.name}").transform;
		}
		_parentTransform = parent;
		_pool = new List<T>(PoolSize);
		for (int i = 0; i < PoolSize; i++)
		{
			var obj = UnityEngine.Object.Instantiate(Prefab, _parentTransform);
			_pool.Add(obj);
			obj.gameObject.SetActive(false);
		}
	}
	#endregion
}
