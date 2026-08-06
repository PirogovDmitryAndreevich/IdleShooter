using UnityEngine;

public interface IGameFactory : IService
{
    void CreatePlayer(Vector3 at);
}