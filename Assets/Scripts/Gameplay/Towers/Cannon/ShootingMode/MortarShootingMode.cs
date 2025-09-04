using Gameplay.Towers.Cannon.Projectile;
using Infrastructure.Factory;
using Services;
using StaticData.Projectile;
using UnityEngine;

namespace Gameplay.Towers.Cannon
{
    public class MortarShootingMode : ITowerShootingMode
    {
        private readonly MortarBarrelRotator _rotator;
        private readonly MortarProjectileType _projectileType;
        private readonly CannonTower _tower;

        public MortarShootingMode(CannonTower tower, MortarBarrelRotator rotator, MortarProjectileType projectileType)
        {
            _tower = tower;
            _rotator = rotator;
            _projectileType = projectileType;
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
            var mortarData = StaticDataService.GetMortarProjectile(_projectileType);

            return mortarData != null ? mortarData.Speed : 0f;
        }

        public void Shoot(Vector3 targetPoint)
        {
            GameFactory.Instance.CreateMortarProjectile(_tower.MortarProjectileType,
                _tower.ShootPoint, Quaternion.LookRotation(targetPoint), _tower.InterceptPoint);
        }

        public float GetProjectileRadius()
        {
            GameObject projectileGO = null;
            var mortarData = StaticDataService.GetMortarProjectile(_projectileType);

            if (mortarData != null)
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
            var mortarData = StaticDataService.GetMortarProjectile(_projectileType);

            return mortarData != null ? mortarData.MaxHeight : 0f;
        }
    }
}