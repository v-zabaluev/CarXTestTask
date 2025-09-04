using UnityEngine;

namespace Gameplay.Towers.Cannon.Rotators
{
    public class CannonBarrelRotator : BarrelRotatorBase
    {
        public override void RotateTowards(Vector3 targetPoint, float projectileSpeed, float maxHeight)
        {
            Vector3 targetDir = targetPoint;

            if (targetDir == Vector3.zero)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            _barrelStand.rotation = Quaternion.RotateTowards(_barrelStand.rotation, targetRotation,
                _turnSpeed * Time.deltaTime * 100f);
            _barrel.localRotation = Quaternion.identity;
        }
    }
}