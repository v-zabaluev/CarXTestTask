using System.Collections.Generic;
using Gameplay.Movement;
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

        [SerializeField] private CannonBarrelRotator _barrelRotator;
        [SerializeField] private MortarBarrelRotator _mortarRotator;

        private Vector3 _projectileDirection;
        private Vector3 _interceptPoint;

        public Vector3 ProjectileDirection => _projectileDirection;
        public Vector3 InterceptPoint => _interceptPoint;
        public Vector3 ShootPoint => _shootPoint.position;
        public CannonType CannonType => _type;

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

        public bool IsCannonAimed(Vector3 targetDirection, Transform barrelTransform, float aimTolerance = 1f)
        {
            Vector3 currentDir = barrelTransform.forward;
            float angle = Vector3.Angle(currentDir, targetDirection);

            return angle <= aimTolerance;
        }

        public bool IsMortarAimed(Vector3 aimDirection, Transform barrel, float aimTolerance = 2f)
        {
            float angle = Vector3.Angle(barrel.forward, aimDirection);

            return angle <= aimTolerance;
        }

        private void Update()
        {
            if (_shootPoint == null || _targetsInRange.Count == 0) return;

            MonsterMovementController monsterMovementController = GetValidTargetMovementController();

            if (monsterMovementController == null) return;

            if (monsterMovementController.GetInterceptInfo(_shootPoint.position, GetProjectileSpeed(),
                    out _projectileDirection, out _interceptPoint))
            {
                Debug.DrawRay(_shootPoint.position, _projectileDirection * 30, Color.red, 1f);

                bool cannonReady = false;

                if (_type == CannonType.Cannon && _barrelRotator != null)
                    cannonReady = IsCannonAimed(_projectileDirection, _barrelRotator.BarrelTransform);

                bool mortarReady = false;

                if (_type == CannonType.Mortar && _mortarRotator != null)
                {
                    mortarReady = IsMortarAimed(_mortarRotator.CurrentAimDirection,
                        _mortarRotator.Barrel);
                }

                if (_lastShotTime + _shootInterval <= Time.time && (cannonReady || mortarReady))
                {
                    Shoot(_projectileDirection);
                    _lastShotTime = Time.time;
                }
            }
        }

        private MonsterMovementController GetValidTargetMovementController()
        {
            _targetsInRange.RemoveAll(m => m == null);

            return _targetsInRange.Count > 0 ? _targetsInRange[0] : null;
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