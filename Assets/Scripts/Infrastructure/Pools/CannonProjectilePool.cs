using Gameplay.Towers;
using Gameplay.Towers.Cannon;
using Gameplay.Towers.Cannon.Projectile;
using Services;
using StaticData.Projectile;
using UnityEngine;
using UnityEngine.Pool;

namespace Infrastructure.Pools
{
    public class CannonProjectilePool
    {
        private readonly ObjectPool<CannonProjectile> _pool;

        public CannonProjectilePool(CannonProjectileType type = CannonProjectileType.Base, int capacity = 10)
        {
            _pool = new ObjectPool<CannonProjectile>(
                createFunc: () =>
                {
                    var data = StaticDataService.GetProjectile<CannonProjectileData, CannonProjectileType>(type);
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
            var data = StaticDataService.GetProjectile<CannonProjectileData, CannonProjectileType>(type);

            if (data == null) return null;

            var projectile = _pool.Get();
            projectile.transform.SetPositionAndRotation(spawnPosition, rotation);
            projectile.Initialize( data.Speed, data.Damage);
            projectile.OnDespawn += Release;
            return projectile;
        }

        public void Release(CannonProjectile projectile) => _pool.Release(projectile);
    }
}