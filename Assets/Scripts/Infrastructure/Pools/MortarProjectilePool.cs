using Gameplay.Towers.Cannon;
using Services;
using StaticData.Projectile;
using UnityEngine;
using UnityEngine.Pool;

namespace Infrastructure.Pools
{
    public class MortarProjectilePool
    {
        private readonly ObjectPool<MortarProjectile> _pool;

        public MortarProjectilePool(MortarProjectileType defaultType = MortarProjectileType.Base, int capacity = 10)
        {
            _pool = new ObjectPool<MortarProjectile>(
                createFunc: () =>
                {
                    var data = StaticDataService.GetMortarProjectile(defaultType);
                    var go = Object.Instantiate(data.Prefab.gameObject);
                    var projectile = go.GetComponent<MortarProjectile>();
                    go.SetActive(false);
                    return projectile;
                },
                actionOnGet: p => p.gameObject.SetActive(true),
                actionOnRelease: p => p.gameObject.SetActive(false),
                actionOnDestroy: p => Object.Destroy(p.gameObject),
                collectionCheck: false,
                defaultCapacity: capacity
            );
        }

        public MortarProjectile Get(Vector3 spawnPosition, Quaternion rotation, Vector3 targetPosition, MortarProjectileType type)
        {
            var data = StaticDataService.GetMortarProjectile(type);
            if (data == null) return null;

            var projectile = _pool.Get();
            projectile.transform.SetPositionAndRotation(spawnPosition, rotation);
            projectile.Initialize(targetPosition, data.Speed, data.Damage, data.MaxHeight);

            return projectile;
        }

        public void Release(MortarProjectile projectile) => _pool.Release(projectile);
    }
}