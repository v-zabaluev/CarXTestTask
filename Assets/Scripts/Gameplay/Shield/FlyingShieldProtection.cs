using UnityEngine;

namespace Gameplay
{
    using UnityEngine;
    using System;

    namespace Gameplay
    {
        public class FlyingShieldProtection : MonoBehaviour
        {
            [SerializeField] private Collider _shieldCollider;
            [SerializeField] private Transform _shielded;

            public bool WillBlockIntercept(Vector3 shooterPos, Vector3 projectileDir, float projectileSpeed,
                float projectileRadius,
                float currentAngle, float angularSpeed, float radius, Vector3 shieldOffsetPosition, float maxTime = 6f)
            {
                if (_shieldCollider == null)
                    return false;

                float stepTime = 0.05f;
                int steps = Mathf.CeilToInt(maxTime / stepTime);

                for (int i = 0; i <= steps; i++)
                {
                    float t = i * stepTime;

                    Vector3 projectilePos = shooterPos + projectileDir.normalized * projectileSpeed * t;

                    Vector3 shieldPos = CalculatrShieldPos(currentAngle, angularSpeed, radius, shieldOffsetPosition, t);

                    Vector3 dirToShield = (shieldPos - projectilePos).normalized;
                    float distanceToShield = Vector3.Distance(projectilePos, shieldPos);

                    if (Physics.SphereCast(projectilePos, projectileRadius, dirToShield, out RaycastHit hit,
                            distanceToShield))
                    {
                        if (hit.collider == _shieldCollider)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            private Vector3 CalculatrShieldPos(float currentAngle, float angularSpeed, float radius, Vector3 shieldOffsetPosition,
                float t)
            {
                float angle = currentAngle + angularSpeed * Mathf.Deg2Rad * t;
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                Vector3 shieldPos = _shielded.position + shieldOffsetPosition + offset;

                return shieldPos;
            }
        }
    }
}