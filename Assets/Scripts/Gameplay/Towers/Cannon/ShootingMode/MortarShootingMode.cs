using Gameplay.Towers.Cannon.Projectile;
using Gameplay.Towers.Cannon.Rotators;
using Infrastructure.Factory;
using Services;
using StaticData.Projectile;
using UnityEngine;

namespace Gameplay.Towers.Cannon.ShootingMode
{
    public class MortarShootingMode : ITowerShootingMode
    {
        private readonly MortarBarrelRotator _rotator;
        private readonly MortarProjectileType _projectileType;
        private readonly CannonTower _tower;

        private readonly MortarProjectileData _data;

        public MortarShootingMode(CannonTower tower, MortarBarrelRotator rotator, MortarProjectileType projectileType)
        {
            _tower = tower;
            _rotator = rotator;
            _projectileType = projectileType;
            _data = StaticDataService.GetProjectile<MortarProjectileData, MortarProjectileType>(_projectileType);
        }

        public void RotateBarrel(Vector3 targetPoint)
        {
            _rotator.RotateTowards(targetPoint, GetProjectileSpeed(), GetProjectileMaxHeight());
        }

        public bool IsAimed()
        {
            return IsMortarAimed(_rotator.CurrentAimDirection, _rotator.Barrel);
        }

        public float GetProjectileSpeed()
        {
            return _data != null ? _data.Speed : 0f;
        }

        public void Shoot(Vector3 targetPoint)
        {
            GameFactory.Instance.CreateMortarProjectile(_tower.MortarProjectileType,
                _tower.ShootPoint, Quaternion.LookRotation(targetPoint), _tower.InterceptPoint);
        }

        public float GetProjectileRadius()
        {
            GameObject projectileGO = null;

            if (_data != null)
                projectileGO = GameFactory.Instance.CreateMortarProjectile(_projectileType, Vector3.zero,
                    Quaternion.identity, Vector3.zero);

            if (projectileGO == null) return 0f;

            SphereCollider collider = projectileGO.GetComponent<SphereCollider>();
            projectileGO.GetComponent<MortarProjectile>().DespawnProjectile();

            return collider.radius;
        }

        private bool IsMortarAimed(Vector3 aimDirection, Transform barrel, float aimTolerance = 2f)
        {
            float angle = Vector3.Angle(barrel.forward, aimDirection);

            return angle <= aimTolerance;
        }

        private float GetProjectileMaxHeight()
        {
            return _data != null ? _data.MaxHeight : 0f;
        }
    }
}