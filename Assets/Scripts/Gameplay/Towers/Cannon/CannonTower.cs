using System.Collections.Generic;
using Gameplay.Towers.Cannon;
using Infrastructure.Factory;
using Services;
using StaticData.Projectile;
using UnityEngine;

namespace Gameplay.Towers.Cannon
{
    public class CannonTower : BaseTower
    {
        [SerializeField] private Transform _shootPoint;

        //if you initialize through the factory, then there will be no such nonsense.
        [SerializeField] private CannonType _type;
        [SerializeField] private CannonProjectileType _cannonProjectileType;
        [SerializeField] private MortarProjectileType _mortarProjectileType;

        private Vector3 _projectileDirection;
        private Vector3 _interceptPoint;

        public Vector3 ProjectileDirection => _projectileDirection;

        private void Update()
        {
            if (_shootPoint == null || _targetsInRange.Count == 0) return;

            MonsterMovement monster = GetValidTarget();

            if (monster == null) return;

            float projectileSpeed = GetProjectileSpeed();

            if (CalculateInterceptDirection(_shootPoint.position, monster.transform.position,
                    monster.GetSpeedVector(), projectileSpeed, out _projectileDirection,
                    out _interceptPoint))
            {
                Debug.DrawRay(_shootPoint.position, _projectileDirection * 30, Color.red, 1f);

                if (_lastShotTime + _shootInterval <= Time.time)
                {
                    Shoot(_projectileDirection);
                    _lastShotTime = Time.time;
                }
            }
        }

        private MonsterMovement GetValidTarget()
        {
            _targetsInRange.RemoveAll(m => m == null);

            return _targetsInRange.Count > 0 ? _targetsInRange[0] : null;
        }

        private float GetProjectileSpeed()
        {
            switch (_type)
            {
                case CannonType.Cannon:
                    var cannonData = StaticDataService.GetCannonProjectile(_cannonProjectileType);

                    return cannonData != null ? cannonData.Speed : 0f;
                case CannonType.Mortar:
                    var mortarData = StaticDataService.GetMortarProjectile(_mortarProjectileType);

                    return mortarData != null ? mortarData.Speed : 0f;
                default:
                    return 0f;
            }
        }

        private void Shoot(Vector3 direction)
        {
            Debug.DrawRay(_shootPoint.position, direction * 30, Color.blue, 2f);

            switch (_type)
            {
                case CannonType.Cannon:
                    GameFactory.Instance.CreateCannonProjectile(_cannonProjectileType,
                        _shootPoint.position, Quaternion.LookRotation(direction), _interceptPoint);

                    break;

                case CannonType.Mortar:
                    GameFactory.Instance.CreateMortarProjectile(_mortarProjectileType,
                        _shootPoint.position, Quaternion.LookRotation(direction), _interceptPoint);

                    break;
            }
        }

        private bool CalculateInterceptDirection(Vector3 shooterPos, Vector3 monsterPos, Vector3 monsterVelocity,
            float projectileSpeed, out Vector3 direction, out Vector3 interceptPoint)
        {
            Vector3 displacement = monsterPos - shooterPos;
            float a = Vector3.Dot(monsterVelocity, monsterVelocity) - projectileSpeed * projectileSpeed;
            float b = 2f * Vector3.Dot(monsterVelocity, displacement);
            float c = Vector3.Dot(displacement, displacement);

            float discriminant = b * b - 4 * a * c;

            if (discriminant < 0f)
            {
                direction = Vector3.zero;
                interceptPoint = Vector3.zero;

                return false;
            }

            float sqrtD = Mathf.Sqrt(discriminant);
            float t1 = (-b - sqrtD) / (2 * a);
            float t2 = (-b + sqrtD) / (2 * a);

            float t = Mathf.Min(t1, t2);
            if (t < 0f) t = Mathf.Max(t1, t2);

            if (t < 0f)
            {
                direction = Vector3.zero;
                interceptPoint = Vector3.zero;

                return false;
            }

            interceptPoint = monsterPos + monsterVelocity * t;
            direction = (interceptPoint - shooterPos).normalized;

            return true;
        }
    }
}