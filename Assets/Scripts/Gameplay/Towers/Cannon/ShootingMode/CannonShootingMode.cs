using Infrastructure.Factory;
using Services;
using StaticData.Projectile;
using UnityEngine;

namespace Gameplay.Towers.Cannon
{
    public class CannonShootingMode : ITowerShootingMode
    {
        private readonly CannonBarrelRotator _rotator;
        private readonly CannonProjectileType _cannonProjectileType;
        private readonly CannonTower _tower;

        public CannonShootingMode(CannonTower tower, CannonBarrelRotator rotator,
            CannonProjectileType cannonProjectileType)
        {
            _tower = tower;
            _rotator = rotator;
            _cannonProjectileType = cannonProjectileType;
        }

        public void RotateBarrel(Vector3 targetPoint)
        {
            _rotator.RotateTowards(targetPoint, GetProjectileSpeed(), 0f);
        }

        public bool IsAimed()
        {
            return IsCannonAimed(_tower.ProjectileDirection, _rotator.BarrelStand);
        }

        public void Shoot(Vector3 targetPoint)
        {
            GameFactory.Instance.CreateCannonProjectile(_tower.CannonProjectileType,
                _tower.ShootPoint, Quaternion.LookRotation(targetPoint), _tower.InterceptPoint);
        }

        public float GetProjectileSpeed()
        {
            var cannonData = StaticDataService.GetCannonProjectile(_cannonProjectileType);

            return cannonData != null ? cannonData.Speed : 0f;
        }

        public bool IsCannonAimed(Vector3 targetDirection, Transform barrelTransform, float aimTolerance = 1f)
        {
            float angle = Vector3.Angle(barrelTransform.forward, targetDirection);

            return angle <= aimTolerance;
        }
    }
}