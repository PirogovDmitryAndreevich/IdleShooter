using System;
using System.Collections.Generic;
using UnityEngine;

public class UIPoolService : IUIPoolService
{
    private Dictionary<Type, Queue<Component>> _pools;
    private Dictionary<Type, Func<Component>> _factories;
    private Dictionary<Type, Transform> _poolParents;

    private Canvas _canvas;

    public UIPoolService()
    {
        _pools = new Dictionary<Type, Queue<Component>>();
        _factories = new Dictionary<Type, Func<Component>>();
        _poolParents = new Dictionary<Type, Transform>();
    }

    public void SetCanvas(Canvas canvas) =>
        _canvas = canvas;

    public void RegisterFactory<TPoolObject>(Func<TPoolObject> factory) where TPoolObject : Component
    {
        Type type = typeof(TPoolObject);

        if (_factories.ContainsKey(type))
            return;

        _factories[type] = () => factory();
    }

    public TPoolObject Get<TPoolObject>() where TPoolObject : Component
    {
        Type type = typeof(TPoolObject);

        if (_pools.TryGetValue(type, out var pool) && pool.Count > 0)
        {
            TPoolObject obj = (TPoolObject)pool.Dequeue();
            obj.gameObject.SetActive(true);

            return obj;
        }

        return CreateNew<TPoolObject>();
    }

    public void Return<TPoolObject>(TPoolObject obj) where TPoolObject : Component
    {
        Type type = typeof(TPoolObject);

        if (!_pools.ContainsKey(type))
            _pools[type] = new Queue<Component>();

        obj.gameObject.SetActive(false);

        _pools[type].Enqueue(obj);
    }

    public void Cleanup()
    {
        _pools.Clear();
        _factories.Clear();
    }

    private TPoolObject CreateNew<TPoolObject>() where TPoolObject : Component
    {
        Type type = typeof(TPoolObject);

        if (!_factories.TryGetValue(type, out var factory))
            throw new Exception($"No factory for {type}");

        TPoolObject obj = (TPoolObject)factory();

        if (!_poolParents.ContainsKey(type))
            CreatePoolParent(type);

        obj.transform.SetParent(_poolParents[type]);

        return obj;
    }

    private void CreatePoolParent(Type type)
    {
        if (_poolParents.ContainsKey(type))
            return;

        GameObject parent = new GameObject($"{type.Name}Pool");
        parent.transform.SetParent(_canvas.transform);

        _poolParents[type] = parent.transform;
    }
}