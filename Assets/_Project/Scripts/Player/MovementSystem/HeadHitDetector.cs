using UnityEngine;

public class HeadHitDetector : IHeadHitDetector
{
    private PlayerStaticData _data;

    public HeadHitDetector(PlayerStaticData data)
    {
        _data = data;
    }

    public bool CheckHeadHit(Transform transform, CharacterController controller)
    {
        Vector3 ccCenter = transform.TransformPoint(controller.center);
        float hitCalc = controller.height / 2f * _data.HeadHitDistance;

        return Physics.Raycast(ccCenter, Vector3.up, hitCalc);
    }
}