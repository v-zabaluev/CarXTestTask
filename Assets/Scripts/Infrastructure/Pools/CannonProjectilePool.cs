using Gameplay.Towers;
using Gameplay.Towers.Cannon;
using Services;
using StaticData.Projectile;
using UnityEngine;
using UnityEngine.Pool;

namespace Infrastructure.Pools
{
    public class CannonProjectilePool
    {
        private readonly ObjectPool<CannonProjectile> _pool;

        public CannonProjectilePool(CannonProjectileType defaultType = CannonProjectileType.Base, int capacity = 10)
        {
            _pool = new ObjectPool<CannonProjectile>(
                createFunc: () =>
                {
                    var data = StaticDataService.GetCannonProjectile(defaultType);
                    var go = Object.Instantiate(data.Prefab.gameObject);
                    var projectile = go.GetComponent<CannonProjectile>();
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

        public CannonProjectile Get(Vector3 spawnPosition, Quaternion rotation, Vector3 targetPosition,
            CannonProjectileType type)
        {
            var data = StaticDataService.GetCannonProjectile(type);

            if (data == null) return null;

            var projectile = _pool.Get();
            projectile.transform.SetPositionAndRotation(spawnPosition, rotation);
            projectile.Initialize(targetPosition, data.Speed, data.Damage);
            projectile.OnDespawn += Release;
            return projectile;
        }

        public void Release(CannonProjectile projectile) => _pool.Release(projectile);
    }
}