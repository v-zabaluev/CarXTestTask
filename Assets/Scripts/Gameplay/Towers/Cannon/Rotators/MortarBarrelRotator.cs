using UnityEngine;

namespace Gameplay.Towers.Cannon
{
    public class MortarBarrelRotator : BarrelRotatorBase
    {
        public Vector3 CurrentAimDirection { get; private set; }

        public override void RotateTowards(Vector3 targetPoint, float projectileSpeed, float maxHeight)
        {
            Vector3 initialVelocity = CalculateInitialVelocity(_barrelStand.position, targetPoint, projectileSpeed, maxHeight);
            if (initialVelocity == Vector3.zero)
                return;

            CurrentAimDirection = initialVelocity.normalized;

            Vector3 horizontalDir = new Vector3(initialVelocity.x, 0f, initialVelocity.z);
            if (horizontalDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetYaw = Quaternion.LookRotation(horizontalDir);
                _barrelStand.rotation = Quaternion.RotateTowards(_barrelStand.rotation, targetYaw,
                    _turnSpeed * Time.deltaTime * 100f);
            }

            Vector3 localDir = _barrelStand.InverseTransformDirection(initialVelocity.normalized);
            float angleX = -Mathf.Atan2(localDir.y, localDir.z) * Mathf.Rad2Deg;
            Quaternion targetPitch = Quaternion.Euler(angleX, 0f, 0f);
            _barrel.localRotation = Quaternion.RotateTowards(_barrel.localRotation, targetPitch,
                _turnSpeed * Time.deltaTime * 100f);
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
            Vector3 horizontalVelocity = horizontalDisplacement / time;
            Vector3 initialVelocity = horizontalVelocity + Vector3.up * startVy;

            return initialVelocity;
        }
    }
}
