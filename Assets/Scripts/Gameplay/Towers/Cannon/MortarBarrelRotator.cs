using Gameplay.Towers.Cannon;
using UnityEngine;

public class MortarBarrelRotator : MonoBehaviour
{
    [SerializeField] private Transform _basePivot;
    [SerializeField] private Transform _barrel;
    [SerializeField] private CannonTower _tower;

    [SerializeField] private float _turnSpeed = 5f;

    void Update()
    {
        if (_tower == null || _basePivot == null || _barrel == null || _tower.CannonType != CannonType.Mortar)
            return;

      
        Vector3 initialVelocity = CalculateInitialVelocity(
            _tower.ShootPoint,
            _tower.InterceptPoint,
            _tower.GetProjectileSpeed(),
            _tower.GetProjectileMaxHeight()
        );

        if (initialVelocity == Vector3.zero)
            return;

        Vector3 horizontalDir = new Vector3(initialVelocity.x, 0f, initialVelocity.z);

        if (horizontalDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetYaw = Quaternion.LookRotation(horizontalDir);

            _basePivot.rotation = Quaternion.RotateTowards(
                _basePivot.rotation,
                targetYaw,
                _turnSpeed * Time.deltaTime * 100f
            );
        }
        
        Vector3 localDir = _basePivot.InverseTransformDirection(initialVelocity.normalized);
        float angleX = -1* Mathf.Atan2(localDir.y, localDir.z) * Mathf.Rad2Deg;

        Quaternion targetPitch = Quaternion.Euler(angleX, 0f, 0f);

        _barrel.localRotation = Quaternion.RotateTowards(
            _barrel.localRotation,
            targetPitch,
            _turnSpeed * Time.deltaTime * 100f
        );
    }

    private Vector3 CalculateInitialVelocity(Vector3 startPos, Vector3 targetPos, float speed, float maxHeight)
    {
        Vector3 displacement = targetPos - startPos;
        Vector3 horizontalDisplacement = new Vector3(displacement.x, 0, displacement.z);

        if (horizontalDisplacement.magnitude < 0.01f)
            return Vector3.zero;

        float time = horizontalDisplacement.magnitude / speed;

        if (time <= 0.01f)
            return Vector3.zero;

        float startVy = 2 * (maxHeight - startPos.y) / (time / 2);
        float gravity = 2 * (startVy * time - displacement.y) / (time * time);

        Vector3 horizontalVelocity = horizontalDisplacement / time;
        Vector3 initialVelocity = horizontalVelocity + Vector3.up * startVy;

        return initialVelocity;
    }
}