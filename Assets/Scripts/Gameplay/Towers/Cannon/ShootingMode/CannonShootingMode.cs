using Gameplay.Towers.Cannon.Projectile;
using Gameplay.Towers.Cannon.Rotators;
using Infrastructure.Factory;
using Services;
using StaticData.Projectile;
using UnityEngine;

namespace Gameplay.Towers.Cannon.ShootingMode
{
    public class CannonShootingMode : ITowerShootingMode
    {
        private readonly CannonBarrelRotator _rotator;
        private readonly CannonProjectileType _projectileType;
        private readonly CannonTower _tower;
        
        private readonly CannonProjectileData _data;
        public CannonShootingMode(CannonTower tower, CannonBarrelRotator rotator,
            CannonProjectileType projectileType)
        {
            _tower = tower;
            _rotator = rotator;
            _projectileType = projectileType;
            
            _data = StaticDataService.GetProjectile<CannonProjectileData, CannonProjectileType>(_projectileType);
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

            return _data != null ? _data.Speed : 0f;
        }

        private bool IsCannonAimed(Vector3 targetDirection, Transform barrelTransform, float aimTolerance = 1f)
        {
            float angle = Vector3.Angle(barrelTransform.forward, targetDirection);

            return angle <= aimTolerance;
        }
        
        public float GetProjectileRadius()
        {
            GameObject projectileGO = null;

            if (_data != null)
                projectileGO = GameFactory.Instance.CreateCannonProjectile(_projectileType, Vector3.zero,
                    Quaternion.identity, Vector3.zero);
            if (projectileGO == null) return 0f;

            SphereCollider collider = projectileGO.GetComponent<SphereCollider>();
            projectileGO.SetActive(false);
            projectileGO.GetComponent<CannonProjectile>().DespawnProjectile();


            return collider.radius;
        }
    }
}