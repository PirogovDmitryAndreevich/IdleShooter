using System;
using TMPro;
using UnityEngine;

public class Warp : MonoBehaviour
{
    [SerializeField] private PlayerTriggerObserver _triggerObserver;
    [SerializeField] private TMP_Text _name;

    public event Action <LevelType> WarpToLevel;

    public LevelType LevelType { get; private set; }

    private void OnDestroy() => 
        _triggerObserver.TriggerEnter -= Warping;

    public void Construct(LevelType type)
    {
        LevelType = type;
        _name.text = LevelType.ToString();

        _triggerObserver.TriggerEnter += Warping;
    }

    private void Warping() => 
        WarpToLevel?.Invoke(LevelType);
}
