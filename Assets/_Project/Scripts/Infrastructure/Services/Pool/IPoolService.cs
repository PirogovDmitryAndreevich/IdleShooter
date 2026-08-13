using System;
using UnityEngine;

public interface IPoolService : IService
{
    void Cleanup();
    TPoolObject Get<TPoolObject>() where TPoolObject : Component;
    Component Get(Type type);
    void RegisterFactory<TPoolObject>(Func<TPoolObject> factory) where TPoolObject : Component;
    void Return<TPoolObject>(TPoolObject obj) where TPoolObject : Component;
    void Return(Component obj);
}