using System;
using UnityEngine;

public interface IUIPoolService : IService
{
    void Cleanup();
    TPoolObject Get<TPoolObject>() where TPoolObject : Component;
    void RegisterFactory<TPoolObject>(Func<TPoolObject> factory) where TPoolObject : Component;
    void Return<TPoolObject>(TPoolObject obj) where TPoolObject : Component;
    void SetCanvas(Canvas canvas);
}