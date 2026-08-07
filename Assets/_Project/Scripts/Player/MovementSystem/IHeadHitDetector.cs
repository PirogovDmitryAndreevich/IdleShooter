using UnityEngine;

public interface IHeadHitDetector
{
    bool CheckHeadHit(Transform transform, CharacterController controller);
}
