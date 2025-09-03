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
        public Vector3 InterceptPoint => _interceptPoint;
        public Vector3 ShootPoint => _shootPoint.position;
        public CannonType CannonType => _type;

        private void Update()
        {
            if (_shootPoint == null || _targetsInRange.Count == 0) return;

            MonsterMovementBase monster = GetValidTarget();

            if (monster == null) return;

            float projectileSpeed = GetProjectileSpeed();

            if (monster.CalculateIntercept(_shootPoint.position, GetProjectileSpeed(),
                    out _projectileDirection, out _interceptPoint))
            {
                Debug.DrawRay(_shootPoint.position, _projectileDirection * 30, Color.red, 1f);

                if (_lastShotTime + _shootInterval <= Time.time)
                {
                    Shoot(_projectileDirection);
                    _lastShotTime = Time.time;
                }
            }
        }

        private MonsterMovementBase GetValidTarget()
        {
            _targetsInRange.RemoveAll(m => m == null);

            return _targetsInRange.Count > 0 ? _targetsInRange[0] : null;
        }

        // private MonsterMovement GetValidTarget()
        // {
        //     _targetsInRange.RemoveAll(m => m == null);
        //
        //     if (_targetsInRange.Count == 0)
        //         return null;
        //
        //     MonsterMovement closest = null;
        //     float closestDistanceSqr = (_targetsInRange[0].transform.position - _shootPoint.position).sqrMagnitude;
        //
        //     foreach (var target in _targetsInRange)
        //     {
        //         float distanceSqr = (target.transform.position - _shootPoint.position).sqrMagnitude;
        //         if (distanceSqr < closestDistanceSqr)
        //         {
        //             closestDistanceSqr = distanceSqr;
        //             closest = target;
        //         }
        //     }
        //
        //     return closest;
        // }

        public float GetProjectileSpeed()
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

        public float GetProjectileMaxHeight()
        {
            switch (_type)
            {
                case CannonType.Mortar:
                    var mortarData = StaticDataService.GetMortarProjectile(_mortarProjectileType);

                    return mortarData != null ? mortarData.MaxHeight : 0f;
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
        
    }
}