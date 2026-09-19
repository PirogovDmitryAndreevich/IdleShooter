using UnityEngine;

public class SphereArena : MonoBehaviour
{
    [SerializeField] private MovingSphereObstacle[] _spheres;
    [SerializeField] private float _knockbackMultiplier;
    [SerializeField] private float _maxKnockbackForce;

    public void Construct()
    {
        for (int i = 0; i < _spheres.Length; i++)        
            _spheres[i].Construct(
                _knockbackMultiplier,
                _maxKnockbackForce);        
    }
}