using UnityEngine;

public abstract class LevelView : MonoBehaviour
{
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private WeaponType _desiredWeapon;

    public abstract LevelType LevelType { get; }
    public WeaponType DesiredWeapon => _desiredWeapon;
    public Transform ExitPoint => _exitPoint;
    public Transition Transition { get; private set; }

    public void Construct(Transition transition) =>        
        Transition = transition;

    public void SetActive(bool isActive) => 
        gameObject.SetActive(isActive);
}
